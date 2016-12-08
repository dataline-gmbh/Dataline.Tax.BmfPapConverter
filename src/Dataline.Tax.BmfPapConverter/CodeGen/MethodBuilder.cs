// <copyright file="MethodBuilder.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public class MethodBuilder : ICodeGen
    {
        public MethodBuilder(Visibilities visibility, string name, StatementBuilder body, Type returnType = null)
        {
            Visibility = visibility;
            ReturnType = returnType;
            Name = name;
            Body = body;
        }

        public Visibilities Visibility { get; }

        public Type ReturnType { get; }

        public string Name { get; }

        public StatementBuilder Body { get; set; }

        public string Summary { get; set; }

        public void CodeGen(CodeBuilder builder)
        {
            if (Summary != null)
                builder.AppendSummary(Summary);

            Visibility.CodeGen(builder);

            if (ReturnType != null)
                ReturnType.CodeGen(builder);
            else
                builder.AppendToken("void");

            builder.AppendToken(Name);
            builder.ForceNoWhitespace();

            builder.AppendToken("()");
            builder.BeginBlock();

            Body.CodeGen(builder);

            builder.EndBlock();
        }
    }
}
