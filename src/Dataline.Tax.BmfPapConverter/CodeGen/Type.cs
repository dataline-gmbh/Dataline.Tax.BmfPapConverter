// <copyright file="Type.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using System.Reflection;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public abstract class Type : ICodeGen
    {
        public abstract Type MakeArrayType();

        public abstract bool CanBeCompileTimeConstant { get; }

        public abstract void CodeGen(CodeBuilder builder);

        public static implicit operator Type(System.Type type)
        {
            return new ClrType(type);
        }
    }

    public class ClrType : Type
    {
        public ClrType(System.Type type)
        {
            Type = type;
        }

        public System.Type Type { get; set; }

        public override Type MakeArrayType()
        {
            return Type.MakeArrayType();
        }

        public override bool CanBeCompileTimeConstant => Type.GetTypeInfo().IsValueType;

        public override void CodeGen(CodeBuilder builder)
        {
            string resultType;
            switch (Type.FullName)
            {
                case "System.Double":
                    resultType = "double";
                    break;
                case "System.Int32":
                    resultType = "int";
                    break;
                case "System.String":
                    resultType = "string";
                    break;
                case "System.Decimal":
                    resultType = "decimal";
                    break;
                case "System.Decimal[]":
                    resultType = "decimal[]";
                    break;
                case "System.Int64":
                    resultType = "long";
                    break;
                default:
                    resultType = Type.FullName;
                    break;
            }
            builder.AppendToken(resultType);
        }
    }

    public class ArbitraryType : Type
    {
        public ArbitraryType(string type)
        {
            Type = type;
        }

        public string Type { get; set; }

        public override Type MakeArrayType()
        {
            return new ArbitraryType($"{Type}[]");
        }

        public override bool CanBeCompileTimeConstant => false;

        public override void CodeGen(CodeBuilder builder)
        {
            builder.AppendToken(Type);
        }
    }
}
