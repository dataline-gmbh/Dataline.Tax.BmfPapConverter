using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public abstract class StatementBuilder : ICodeGen
    {
        public abstract void CodeGen(CodeBuilder builder);
    }

    public class MultipleStatementBuilder : StatementBuilder
    {
        public MultipleStatementBuilder(List<StatementBuilder> statements)
        {
            Statements = statements;
        }

        public List<StatementBuilder> Statements { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            foreach (var st in Statements)
                st.CodeGen(builder);
        }
    }

    public class AssignmentStatementBuilder : StatementBuilder
    {
        public AssignmentStatementBuilder(ExpressionBuilder targetExpression, ExpressionBuilder expression)
        {
            TargetExpression = targetExpression;
            Expression = expression;
        }

        public ExpressionBuilder TargetExpression { get; set; }

        public ExpressionBuilder Expression { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            TargetExpression.CodeGen(builder);

            builder.AppendToken("=");

            Expression.CodeGen(builder);

            builder.EndOfStatement();
        }
    }

    public class ReturnStatementBuilder : StatementBuilder
    {
        public ReturnStatementBuilder(ExpressionBuilder expression)
        {
            Expression = expression;
        }

        public ExpressionBuilder Expression { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            builder.AppendToken("return");

            Expression.CodeGen(builder);

            builder.EndOfStatement();
        }
    }

    public class IfStatementBuilder : StatementBuilder
    {
        public IfStatementBuilder(ExpressionBuilder conditionalExpression, StatementBuilder thenStatement, StatementBuilder elseStatement)
        {
            ConditionalExpression = conditionalExpression;
            ThenStatement = thenStatement;
            ElseStatement = elseStatement;
        }

        public ExpressionBuilder ConditionalExpression { get; set; }

        public StatementBuilder ThenStatement { get; set; }

        public StatementBuilder ElseStatement { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            builder.AppendToken("if (");
            builder.ForceNoWhitespace();

            ConditionalExpression.CodeGen(builder);

            builder.AppendToken(")");
            builder.BeginBlock();

            ThenStatement.CodeGen(builder);

            builder.EndBlock();

            if (ElseStatement != null)
            {
                builder.EndOfLine(); // Nur einfachen Zeilenumbruch erzwingen

                builder.AppendToken("else");
                builder.BeginBlock();

                ElseStatement.CodeGen(builder);

                builder.EndBlock();
            }
        }
    }

    public class InvocationStatementBuilder : StatementBuilder
    {
        public InvocationStatementBuilder(string function, ExpressionBuilder baseExpression = null, ExpressionBuilder argument = null)
        {
            Function = function;
            BaseExpression = baseExpression;

            if (argument != null)
                Arguments.Add(argument);
        }

        public ExpressionBuilder BaseExpression { get; set; }

        public string Function { get; set; }

        public List<ExpressionBuilder> Arguments { get; set; } = new List<ExpressionBuilder>();

        public override void CodeGen(CodeBuilder builder)
        {
            if (BaseExpression != null)
            {
                BaseExpression.CodeGen(builder);
                builder.AppendToken(".");
                builder.ForceNoWhitespace();
            }

            builder.AppendToken(Function);
            builder.ForceNoWhitespace();
            builder.AppendToken("(");
            builder.ForceNoWhitespace();
            builder.BeginSeparatedList(",");

            foreach (var argument in Arguments)
            {
                argument.CodeGen(builder);
                builder.EndOfSeparatedListItem();
            }

            builder.EndOfSeparatedList();
            builder.AppendToken(")");

            builder.EndOfStatement();
        }
    }
}
