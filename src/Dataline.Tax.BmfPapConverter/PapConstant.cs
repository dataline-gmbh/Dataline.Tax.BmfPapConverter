using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dataline.Tax.BmfPapConverter
{
    public class PapConstant
    {
        public PapConstant(XElement element)
        {
            Name = element.Attribute("name")?.Value;
            Type = element.Attribute("type")?.Value;
            Value = element.Attribute("value")?.Value;
            Documentation = element.GetPreviousComment();

            if (string.IsNullOrEmpty(Name))
                throw new InvalidPapException("Der name einer Konstante muss gesetzt sein.");
            if (string.IsNullOrEmpty(Type))
                throw new InvalidPapException("Der type einer Konstante muss gesetzt sein.");
            if (string.IsNullOrEmpty(Value))
                throw new InvalidPapException("Der Wert einer Konstante muss gesetzt sein.");
        }

        public PapConstant()
        {
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public string Documentation { get; set; }
    }
}
