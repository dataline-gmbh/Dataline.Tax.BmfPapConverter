// <copyright file="Visibilities.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using System;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public enum Visibilities
    {
        Public,
        Protected,
        Internal,
        ProtectedInternal,
        Private
    }

    public static class VisibilitiesCodeGen
    {
        public static void CodeGen(this Visibilities visibility, CodeBuilder builder)
        {
            switch (visibility)
            {
                case Visibilities.Public:
                    builder.AppendToken("public");
                    break;
                case Visibilities.Protected:
                    builder.AppendToken("protected");
                    break;
                case Visibilities.Internal:
                    builder.AppendToken("internal");
                    break;
                case Visibilities.ProtectedInternal:
                    builder.AppendToken("protected internal");
                    break;
                case Visibilities.Private:
                    builder.AppendToken("private");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null);
            }
        }
    }
}
