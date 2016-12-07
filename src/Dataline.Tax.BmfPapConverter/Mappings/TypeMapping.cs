using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Type = Dataline.Tax.BmfPapConverter.CodeGen.Type;

namespace Dataline.Tax.BmfPapConverter.Mappings
{
    public static class TypeMapping
    {
        private static readonly Dictionary<string, Type> Mappings = new Dictionary<string, Type>
        {
            { "int", typeof(int) },
            { "BigDecimal", typeof(decimal) },
            { "double", typeof(double) },
        };

        public static Type Map(string javaType)
        {
            Type result;
            bool isArray = javaType.EndsWith("[]");
            string typeName = isArray ? javaType.Substring(0, javaType.Length - 2) : javaType;

            try
            {
                result = Mappings[typeName];
            }
            catch (KeyNotFoundException)
            {
                throw new NotSupportedException();
            }

            return isArray ? result.MakeArrayType() : result;
        }
    }
}

