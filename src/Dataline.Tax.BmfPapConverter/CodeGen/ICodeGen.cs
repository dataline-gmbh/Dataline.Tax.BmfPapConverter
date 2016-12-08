// <copyright file="ICodeGen.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public interface ICodeGen
    {
        void CodeGen(CodeBuilder builder);
    }
}
