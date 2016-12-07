using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dataline.Tax.BmfPapConverter
{
    public class PapDocument
    {
        public PapDocument(XDocument papDocument)
        {
            ParseDocument(papDocument);
        }

        public PapDocument()
        {
        }

        public string Name { get; set; }

        public ICollection<PapVariable> Inputs { get; set; } = new List<PapVariable>();

        public ICollection<PapVariable> Outputs { get; set; } = new List<PapVariable>();

        public ICollection<PapVariable> Internals { get; set; } = new List<PapVariable>();

        public ICollection<PapConstant> Constants { get; set; } = new List<PapConstant>();

        public ICollection<PapMethod> Methods { get; set; } = new List<PapMethod>();

        public PapMethod MainMethod { get; set; }


        private void ParseDocument(XDocument papDocument)
        {
            // Parsen des Root-Dokuments
            var root = papDocument.Element("PAP");
            if (root == null)
                throw new InvalidPapException("Root-Element PAP nicht gefunden.");

            Name = root.Attribute("name")?.Value;
            if (string.IsNullOrEmpty(Name))
                throw new InvalidPapException("PAP.name muss gesetzt sein.");

            // Parsen der Variablendeklarationen
            var variables = root.Element("VARIABLES");
            if (variables == null)
                throw new InvalidPapException("Deklaration der Variablen nicht gefunden.");

            var inputs = variables.Element("INPUTS");
            if (inputs == null)
                throw new InvalidPapException("Deklaration der Inputs-Variablen nicht gefunden.");
            var outputs = variables.Elements("OUTPUTS").ToList();
            if (!outputs.Any())
                throw new InvalidPapException("Deklaration der Outputs-Variablen nicht gefunden.");
            var internals = variables.Element("INTERNALS");
            if (internals == null)
                throw new InvalidPapException("Deklaration der Internals-Variablen nicht gefunden.");

            var constants = root.Element("CONSTANTS");
            if (constants == null)
                throw new InvalidPapException("Deklaration der Konstanten nicht gefunden.");

            Inputs = inputs.Elements().Select(x => new PapVariable(x)).ToList();
            Outputs = outputs.Elements().Select(x => new PapVariable(x)).ToList();
            Internals = internals.Elements().Select(x => new PapVariable(x)).ToList();
            Constants = constants.Elements().Select(x => new PapConstant(x)).ToList();

            // Parsen der Methodendeklarationen
            var methods = root.Element("METHODS");
            if (methods == null)
                throw new InvalidPapException("Deklaration der Methoden nicht gefunden.");

            Methods = methods.Elements("METHOD").Select(x => new PapMethod(x)).ToList();

            var main = methods.Element("MAIN");
            if (main == null)
                throw new InvalidPapException("Deklaration der Main-Methode nicht gefunden.");

            MainMethod = new PapMethod(main, "Main");
        }
    }
}
