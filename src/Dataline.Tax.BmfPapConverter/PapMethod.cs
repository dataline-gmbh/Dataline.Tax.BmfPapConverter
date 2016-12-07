using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dataline.Tax.BmfPapConverter
{
    public class PapMethod
    {
        public PapMethod(XElement element, string overrideName = null)
        {
            if (overrideName != null)
            {
                Name = overrideName;
            }
            else
            {
                Name = element.Attribute("name")?.Value;
                if (Name == null)
                    throw new InvalidPapException("Methode enthält kein Namensattribut");
            }

            Statements = new PapSyntaxTreeNodeStatementList(element);
            Documentation = element.GetPreviousComment();
        }

        public PapMethod()
        {
        }

        public string Name { get; set; }

        public PapSyntaxTreeNodeStatementList Statements { get; set; }

        public string Documentation { get; set; }
    }
}
