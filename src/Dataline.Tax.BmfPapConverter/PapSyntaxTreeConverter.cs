// <copyright file="PapSyntaxTreeConverter.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using System.Linq;
using Dataline.Tax.BmfPapConverter.CodeGen;

namespace Dataline.Tax.BmfPapConverter
{
    public class PapSyntaxTreeConverter : PapSyntaxTreeVisitor<StatementBuilder>
    {
        public static StatementBuilder Convert(IPapSyntaxTreeNode node)
        {
            var visitor = new PapSyntaxTreeConverter();
            return visitor.Visit(node);
        }

        public override StatementBuilder VisitStatementList(PapSyntaxTreeNodeStatementList node)
        {
            return new MultipleStatementBuilder(node.Nodes.Select(Visit).ToList());
        }

        public override StatementBuilder VisitIf(PapSyntaxTreeNodeIf node)
        {
            return new IfStatementBuilder(PapEvalCodeParser.ConvertToExpression(node.Condition),
                                          Visit(node.ThenStatements),
                                          node.ElseStatements != null ? Visit(node.ElseStatements) : null);
        }

        public override StatementBuilder VisitEval(PapSyntaxTreeNodeEval node)
        {
            return PapEvalCodeParser.ConvertToStatement(node.Expression);
        }

        public override StatementBuilder VisitExecute(PapSyntaxTreeNodeExecute node)
        {
            return new InvocationStatementBuilder(node.MethodName);
        }
    }
}
