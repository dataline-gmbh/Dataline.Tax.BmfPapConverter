using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
