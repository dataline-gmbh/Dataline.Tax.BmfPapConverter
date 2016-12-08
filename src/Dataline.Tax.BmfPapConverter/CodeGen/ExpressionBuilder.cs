// <copyright file="ExpressionBuilder.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public abstract class ExpressionBuilder : ICodeGen
    {
        public abstract void CodeGen(CodeBuilder builder);
    }

    public class ConstantExpressionBuilder : ExpressionBuilder
    {
        public ConstantExpressionBuilder(System.Type type, string textConstant)
        {
            Type = type;
            TextConstant = textConstant;
        }

        public System.Type Type { get; set; }

        public string TextConstant { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            builder.AppendToken(ConstantExpressionConverter.ToConstantString(Type, TextConstant));
        }
    }

    public class MemberExpressionBuilder : ExpressionBuilder
    {
        public MemberExpressionBuilder(params string[] memberPath)
        {
            MemberPath = memberPath.ToList();
        }

        public MemberExpressionBuilder(List<string> memberPath)
        {
            MemberPath = memberPath;
        }

        public List<string> MemberPath { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            builder.AppendToken(string.Join(".", MemberPath));
        }

        public bool IsEquivalent(string str)
        {
            return string.Join(".", MemberPath) == str;
        }
    }

    public class KeywordExpressionBuilder : ExpressionBuilder
    {
        public enum Keywords
        {
            This,
            Value
        }

        public KeywordExpressionBuilder(Keywords keyword)
        {
            Keyword = keyword;
        }

        public Keywords Keyword { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            switch (Keyword)
            {
                case Keywords.This:
                    builder.AppendToken("this");
                    break;
                case Keywords.Value:
                    builder.AppendToken("value");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class UnaryExpressionBuilder : ExpressionBuilder
    {
        public UnaryExpressionBuilder(string @operator, ExpressionBuilder expression)
        {
            Operator = @operator;
            Expression = expression;
        }

        public string Operator { get; set; }

        public ExpressionBuilder Expression { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            builder.AppendToken("(");
            builder.ForceNoWhitespace();

            builder.AppendToken(Operator);
            builder.ForceNoWhitespace();
            Expression.CodeGen(builder);

            builder.AppendToken(")");
        }
    }

    public class BinaryExpressionBuilder : ExpressionBuilder
    {
        public BinaryExpressionBuilder(string @operator, ExpressionBuilder left, ExpressionBuilder right)
        {
            Operator = @operator;
            Left = left;
            Right = right;
        }

        public string Operator { get; set; }

        public ExpressionBuilder Left { get; set; }

        public ExpressionBuilder Right { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            builder.AppendToken("(");
            builder.ForceNoWhitespace();
            Left.CodeGen(builder);
            
            builder.AppendToken(Operator);

            Right.CodeGen(builder);
            builder.AppendToken(")");
        }
    }

    public class ArrayExpressionBuilder : ExpressionBuilder
    {
        public List<ExpressionBuilder> Elements { get; } = new List<ExpressionBuilder>();
        
        public override void CodeGen(CodeBuilder builder)
        {
            builder.AppendToken("{");
            builder.BeginSeparatedList(",");

            foreach (var element in Elements)
            {
                element.CodeGen(builder);
                builder.EndOfSeparatedListItem();
            }

            builder.EndOfSeparatedList();
            builder.AppendToken("}");
        }
    }

    public class ElementAccessExpressionBuilder : ExpressionBuilder
    {
        public ElementAccessExpressionBuilder(ExpressionBuilder baseExpression, ExpressionBuilder elementExpression)
        {
            BaseExpression = baseExpression;
            ElementExpression = elementExpression;
        }

        public ExpressionBuilder BaseExpression { get; }

        public ExpressionBuilder ElementExpression { get; }

        public override void CodeGen(CodeBuilder builder)
        {
            BaseExpression.CodeGen(builder);

            builder.AppendToken("[");
            builder.ForceNoWhitespace();

            ElementExpression.CodeGen(builder);

            builder.AppendToken("]");
        }
    }

    public class InvocationExpressionBuilder : ExpressionBuilder
    {
        public InvocationExpressionBuilder(string function, ExpressionBuilder baseExpression = null, ExpressionBuilder argument = null)
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
        }
    }

    public class CastExpressionBuilder : ExpressionBuilder
    {
        public CastExpressionBuilder(Type type, ExpressionBuilder expression)
        {
            Type = type;
            Expression = expression;
        }

        public Type Type { get; set; }

        public ExpressionBuilder Expression { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            builder.AppendToken("(");
            builder.ForceNoWhitespace();
            builder.AppendToken("(");
            builder.ForceNoWhitespace();

            Type.CodeGen(builder);

            builder.AppendToken(")");
            builder.ForceNoWhitespace();

            Expression.CodeGen(builder);

            builder.AppendToken(")");
        }
    }

    public class NewExpressionBuilder : ExpressionBuilder
    {
        public NewExpressionBuilder(Type type)
        {
            Type = type;
        }

        public Type Type { get; set; }

        public override void CodeGen(CodeBuilder builder)
        {
            builder.AppendToken("new");

            Type.CodeGen(builder);
            
            builder.ForceNoWhitespace();
            builder.AppendToken("()");
        }
    }
}
