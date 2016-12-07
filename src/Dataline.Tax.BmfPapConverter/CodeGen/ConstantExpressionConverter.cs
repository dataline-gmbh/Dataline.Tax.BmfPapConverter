using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public static class ConstantExpressionConverter
    {
        public static string ToConstantString(System.Type type, string objText)
        {
            if (type == typeof(uint))
            {
                return $"{objText}U";
            }
            if (type == typeof(ulong))
            {
                return $"{objText}UL";
            }
            if (type == typeof(float))
            {
                return $"{objText}f";
            }
            if (type == typeof(decimal))
            {
                return $"{objText}m";
            }

            return objText;
        }
    }
}
