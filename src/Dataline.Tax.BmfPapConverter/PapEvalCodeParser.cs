// <copyright file="PapEvalCodeParser.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using System;
using System.Linq;
using Dataline.Tax.BmfPapConverter.CodeGen;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Dataline.Tax.BmfPapConverter
{
    public class PapEvalCodeParser : CSharpSyntaxWalker
    {
        private static readonly CSharpParseOptions _parseOptions = new CSharpParseOptions(kind: SourceCodeKind.Script);

        public object Result { get; private set; }

        public static object Convert(SyntaxNode node)
        {
            var parser = new PapEvalCodeParser();
            parser.Visit(node);

            var result = parser.Result;
            if (result == null)
                throw new InvalidPapException("Ausdruck ergab kein Resultat");

            return result;
        }

        public static ICodeGen ConvertToCodeGen(SyntaxNode node)
        {
            var result = Convert(node);
            
            // Spezialbehandlung für MemberAccessNode
            var memberAccessNode = result as MemberAccessNode;
            if (memberAccessNode != null)
                return memberAccessNode.ToExpression();

            var codeGen = result as ICodeGen;
            if (codeGen == null)
                throw new InvalidPapException("Ein gültiges Code-Element erwartet");

            return codeGen;
        }

        public static StatementBuilder ConvertToStatement(SyntaxNode node)
        {
            var statement = ConvertToCodeGen(node) as StatementBuilder;
            if (statement == null)
                throw new InvalidPapException("Ein Statement erwartet");

            return statement;
        }

        public static ExpressionBuilder ConvertToExpression(SyntaxNode node)
        {
            var expression = ConvertToCodeGen(node) as ExpressionBuilder;
            if (expression == null)
                throw new InvalidPapException("Einen Ausdruck erwartet");

            return expression;
        }

        public static StatementBuilder ConvertToStatement(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code, _parseOptions);
            return ConvertToStatement(tree.GetRoot());
        }

        public static ExpressionBuilder ConvertToExpression(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code, _parseOptions);
            return ConvertToExpression(tree.GetRoot());
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            if (node.OperatorToken.Text != "=")
                throw new NotSupportedException();

            var leftExpression = ConvertToExpression(node.Left);
            var rightExpression = ConvertToExpression(node.Right);

            Result = new AssignmentStatementBuilder(leftExpression, rightExpression);
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            Result = new MemberExpressionBuilder(node.Identifier.Text);
        }

        public override void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            if (!node.IsKind(SyntaxKind.NumericLiteralExpression))
                throw new NotSupportedException();

            Result = new ConstantExpressionBuilder(node.Token.Value.GetType(), node.Token.Text);
        }

        public override void VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            var arguments = node.ArgumentList.Arguments.ToArray();

            if (arguments.Length != 1)
                throw new NotSupportedException();

            Result = new ElementAccessExpressionBuilder(ConvertToExpression(node.Expression), ConvertToExpression(arguments[0]));
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var memberAccessNode = (MemberAccessNode)Convert(node.Expression);

            var arguments = node.ArgumentList.Arguments.ToArray();

            switch (memberAccessNode.MemberName)
            {
                case "setScale":
                    {
                        // Runden; erstes Argument ist Nachkommastelle, zweites Argument
                        // ist Rundungsrichtung
                        if (arguments.Length < 1 || arguments.Length > 2)
                            throw new NotSupportedException();

                        var decimals = (ConstantExpressionBuilder)ConvertToExpression(arguments[0]);
                        var direction = arguments.Length == 2
                            ? (MemberExpressionBuilder)((MemberAccessNode)Convert(arguments[1])).ToExpression()
                            : null;

                        Result = MakeRoundingExpression(memberAccessNode.BaseExpression, decimals, direction);
                        break;
                    }
                case "multiply":
                    if (arguments.Length != 1)
                        throw new NotSupportedException();

                    Result = new BinaryExpressionBuilder("*", memberAccessNode.BaseExpression, ConvertToExpression(arguments[0]));
                    break;
                case "divide":
                    if (arguments.Length == 3)
                    {
                        // divide(operand, nachkommastellen, rundungsrichtung)
                        var operand = ConvertToExpression(arguments[0]);
                        var decimals = (ConstantExpressionBuilder)ConvertToExpression(arguments[1]);
                        var direction = (MemberExpressionBuilder)((MemberAccessNode)Convert(arguments[2])).ToExpression();

                        var division = new BinaryExpressionBuilder("/", memberAccessNode.BaseExpression, operand);
                        Result = MakeRoundingExpression(division, decimals, direction);
                    }
                    else if (arguments.Length == 1)
                    {
                        Result = new BinaryExpressionBuilder("/", memberAccessNode.BaseExpression, ConvertToExpression(arguments[0]));
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                    break;
                case "add":
                    if (arguments.Length != 1)
                        throw new NotSupportedException();

                    Result = new BinaryExpressionBuilder("+", memberAccessNode.BaseExpression, ConvertToExpression(arguments[0]));
                    break;
                case "subtract":
                    if (arguments.Length != 1)
                        throw new NotSupportedException();

                    Result = new BinaryExpressionBuilder("-", memberAccessNode.BaseExpression, ConvertToExpression(arguments[0]));
                    break;
                case "compareTo":
                    {
                        if (arguments.Length != 1)
                            throw new NotSupportedException();

                        var invocation = new InvocationExpressionBuilder("CompareTo");

                        invocation.Arguments.Add(memberAccessNode.BaseExpression);
                        invocation.Arguments.Add(ConvertToExpression(arguments[0]));

                        Result = invocation;
                        break;
                    }
                case "valueOf":
                    // BigDecimal.valueOf
                    if (arguments.Length != 1)
                        throw new NotSupportedException();

                    if (!((MemberExpressionBuilder)memberAccessNode.BaseExpression).IsEquivalent("BigDecimal"))
                        throw new NotSupportedException();

                    var innerExpression = ConvertToExpression(arguments[0]);
                    var innerConst = innerExpression as ConstantExpressionBuilder;
                    if (innerConst != null)
                    {
                        // Der innere Ausdruck ist eine Konstante, wir ändern den gesamten Ausdruck in
                        // eine decimal-Konstante
                        Result = new ConstantExpressionBuilder(typeof(decimal), innerConst.TextConstant);
                    }
                    else
                    {
                        // Der innere Ausdruck ist irgendetwas anderes, wir casten ihn nach decimal
                        Result = new CastExpressionBuilder(typeof(decimal), innerExpression);
                    }

                    break;
                case "longValue":
                    if (arguments.Length != 0)
                        throw new NotSupportedException();

                    Result = new CastExpressionBuilder(typeof(long), memberAccessNode.BaseExpression);
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        public override void VisitBlock(BlockSyntax node)
        {
            // Ein Block ist bei uns immer ein Array aus Werten
            var arrayExpr = new ArrayExpressionBuilder();

            arrayExpr.Elements.AddRange(node.Statements.Select(ConvertToExpression));

            Result = arrayExpr;
        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            Result = new MemberAccessNode
            {
                MemberName = node.Name.Identifier.Text,
                BaseExpression = ConvertToExpression(node.Expression)
            };
        }

        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            // Hier wird nur new BigDecimal(x) unterstützt
            var typeExpression = ConvertToExpression(node.Type) as MemberExpressionBuilder;
            if (typeExpression == null || !typeExpression.IsEquivalent("BigDecimal"))
                throw new NotSupportedException();

            var arguments = node.ArgumentList.Arguments.ToArray();
            if (arguments.Length != 1)
                throw new NotSupportedException();

            var value = ConvertToExpression(arguments[0]);
            var constantValue = value as ConstantExpressionBuilder;
            if (constantValue != null)
            {
                // Wertkonstante
                Result = new ConstantExpressionBuilder(typeof(decimal), constantValue.TextConstant);
            }
            else
            {
                // Anderer Ausdruck, wir casten nach double
                Result = new CastExpressionBuilder(typeof(double), value);
            }
        }

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            Result = new BinaryExpressionBuilder(node.OperatorToken.Text,
                                                 ConvertToExpression(node.Left),
                                                 ConvertToExpression(node.Right));
        }

        public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            Result = new UnaryExpressionBuilder(node.OperatorToken.Text, ConvertToExpression(node.Operand));
        }

        private InvocationExpressionBuilder MakeRoundingExpression(ExpressionBuilder inner,
                                                                   ConstantExpressionBuilder decimals,
                                                                   MemberExpressionBuilder direction)
        {
            if (direction != null)
            {
                string roundingFunction;

                if (direction.IsEquivalent("BigDecimal.ROUND_UP"))
                    roundingFunction = "Ceiling";
                else if (direction.IsEquivalent("BigDecimal.ROUND_DOWN"))
                    roundingFunction = "Floor";
                else
                    throw new NotSupportedException();

                var invocation = new InvocationExpressionBuilder(roundingFunction);
                invocation.Arguments.Add(inner);
                invocation.Arguments.Add(decimals);

                return invocation;
            }
            else
            {
                var invocation = new InvocationExpressionBuilder("System.Math.Round");
                invocation.Arguments.Add(inner);
                invocation.Arguments.Add(decimals);
                invocation.Arguments.Add(new MemberExpressionBuilder("System", "MidpointRounding", "AwayFromZero"));

                return invocation;
            }
        }

        public override void VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitAccessorList(AccessorListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
        {
            throw new NotSupportedException();
        }
        
        public override void VisitArgumentList(ArgumentListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitArrayType(ArrayTypeSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitAttribute(AttributeSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitAttributeArgument(AttributeArgumentSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitAttributeArgumentList(AttributeArgumentListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitAttributeList(AttributeListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitAttributeTargetSpecifier(AttributeTargetSpecifierSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitAwaitExpression(AwaitExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitBadDirectiveTrivia(BadDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitBaseExpression(BaseExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitBaseList(BaseListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitBracketedArgumentList(BracketedArgumentListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitBracketedParameterList(BracketedParameterListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitBreakStatement(BreakStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitCastExpression(CastExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitCatchClause(CatchClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitCatchDeclaration(CatchDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitCatchFilterClause(CatchFilterClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitCheckedExpression(CheckedExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitCheckedStatement(CheckedStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitConstructorConstraint(ConstructorConstraintSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitConstructorInitializer(ConstructorInitializerSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitContinueStatement(ContinueStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitConversionOperatorMemberCref(ConversionOperatorMemberCrefSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitCrefBracketedParameterList(CrefBracketedParameterListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitCrefParameter(CrefParameterSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitCrefParameterList(CrefParameterListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitDefaultExpression(DefaultExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitDefaultSwitchLabel(DefaultSwitchLabelSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitDefineDirectiveTrivia(DefineDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitDestructorDeclaration(DestructorDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitDoStatement(DoStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitElementBindingExpression(ElementBindingExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitElseClause(ElseClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitEmptyStatement(EmptyStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitErrorDirectiveTrivia(ErrorDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitEventDeclaration(EventDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitFinallyClause(FinallyClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitFixedStatement(FixedStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitFromClause(FromClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitGenericName(GenericNameSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitGotoStatement(GotoStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitGroupClause(GroupClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitImplicitElementAccess(ImplicitElementAccessSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitIncompleteMember(IncompleteMemberSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitIndexerMemberCref(IndexerMemberCrefSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitInterpolatedStringText(InterpolatedStringTextSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitInterpolation(InterpolationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitInterpolationFormatClause(InterpolationFormatClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitJoinClause(JoinClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitJoinIntoClause(JoinIntoClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitLabeledStatement(LabeledStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitLeadingTrivia(SyntaxToken token)
        {
            throw new NotSupportedException();
        }

        public override void VisitLetClause(LetClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitLineDirectiveTrivia(LineDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitLoadDirectiveTrivia(LoadDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitLockStatement(LockStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitMakeRefExpression(MakeRefExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitMemberBindingExpression(MemberBindingExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitNameColon(NameColonSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitNameEquals(NameEqualsSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitNameMemberCref(NameMemberCrefSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitNullableType(NullableTypeSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitOperatorMemberCref(OperatorMemberCrefSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitOrderByClause(OrderByClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitOrdering(OrderingSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitParameter(ParameterSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitParameterList(ParameterListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitPointerType(PointerTypeSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitPragmaChecksumDirectiveTrivia(PragmaChecksumDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitPredefinedType(PredefinedTypeSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitQualifiedCref(QualifiedCrefSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitQualifiedName(QualifiedNameSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitQueryBody(QueryBodySyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitQueryContinuation(QueryContinuationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitQueryExpression(QueryExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitRefTypeExpression(RefTypeExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitRefValueExpression(RefValueExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitReturnStatement(ReturnStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitSelectClause(SelectClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitShebangDirectiveTrivia(ShebangDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitSimpleBaseType(SimpleBaseTypeSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitSizeOfExpression(SizeOfExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitSkippedTokensTrivia(SkippedTokensTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitSwitchSection(SwitchSectionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitThisExpression(ThisExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitThrowStatement(ThrowStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitToken(SyntaxToken token)
        {
            throw new NotSupportedException();
        }

        public override void VisitTrailingTrivia(SyntaxToken token)
        {
            throw new NotSupportedException();
        }

        public override void VisitTrivia(SyntaxTrivia trivia)
        {
            throw new NotSupportedException();
        }

        public override void VisitTryStatement(TryStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitTypeArgumentList(TypeArgumentListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitTypeConstraint(TypeConstraintSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitTypeCref(TypeCrefSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitTypeParameter(TypeParameterSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitTypeParameterList(TypeParameterListSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitUndefDirectiveTrivia(UndefDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitUnsafeStatement(UnsafeStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitUsingStatement(UsingStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitWarningDirectiveTrivia(WarningDirectiveTriviaSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitWhereClause(WhereClauseSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlCDataSection(XmlCDataSectionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlComment(XmlCommentSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlCrefAttribute(XmlCrefAttributeSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlElement(XmlElementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlElementEndTag(XmlElementEndTagSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlElementStartTag(XmlElementStartTagSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlEmptyElement(XmlEmptyElementSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlName(XmlNameSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlNameAttribute(XmlNameAttributeSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlPrefix(XmlPrefixSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlProcessingInstruction(XmlProcessingInstructionSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlText(XmlTextSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitXmlTextAttribute(XmlTextAttributeSyntax node)
        {
            throw new NotSupportedException();
        }

        public override void VisitYieldStatement(YieldStatementSyntax node)
        {
            throw new NotSupportedException();
        }

        private class MemberAccessNode
        {
            public string MemberName { get; set; }
            public ExpressionBuilder BaseExpression { get; set; }

            public ExpressionBuilder ToExpression()
            {
                // Dies wird nur aufgerufen, wenn dieser Node alleinstehend (also nicht zusammenhängend mit einer
                // invocation) ist. Dies ist nur erlaubt, wenn der Basisausdruck ein MemberExpressionBuilder ist.
                var memberExpressionBuilder = BaseExpression as MemberExpressionBuilder;
                if (memberExpressionBuilder == null)
                    throw new NotSupportedException();

                if (memberExpressionBuilder.MemberPath.Count == 1 &&
                    memberExpressionBuilder.MemberPath[0] == "BigDecimal")
                {
                    // BigDecimal-Konstanten
                    if (MemberName == "ZERO")
                        return new ConstantExpressionBuilder(typeof(decimal), "0");
                    if (MemberName == "ONE")
                        return new ConstantExpressionBuilder(typeof(decimal), "1");
                }

                return new MemberExpressionBuilder(memberExpressionBuilder.MemberPath.Concat(new []{MemberName}).ToList());
            }
        }
    }
}
