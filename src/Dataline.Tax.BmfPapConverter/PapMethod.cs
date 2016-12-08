// <copyright file="PapMethod.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

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
