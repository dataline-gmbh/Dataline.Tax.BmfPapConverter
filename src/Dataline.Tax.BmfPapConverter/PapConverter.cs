// <copyright file="PapConverter.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Dataline.Tax.BmfPapConverter.CodeGen;
using Dataline.Tax.BmfPapConverter.Mappings;

namespace Dataline.Tax.BmfPapConverter
{
    public class PapConverter
    {
        /// <summary>
        /// Der Name der Klasse für die Eingangsparameter.
        /// </summary>
        public string InputClassName { get; set; }

        /// <summary>
        /// Der Name der Klasse für die Ausgangsparameter.
        /// </summary>
        public string OutputClassName { get; set; }

        /// <summary>
        /// Der Name der Berechnungs-Klasse.
        /// </summary>
        public string OperationClassName { get; set; }

        /// <summary>
        /// Der Name der Haupt-Berechnungsmethode in der Berechnungs-Klasse.
        /// </summary>
        public string OperationMainMethodName { get; set; }

        /// <summary>
        /// Der Namespace für das Projekt.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Der Kommentar, der als Header für jede Quelldatei verwendet wird.
        /// </summary>
        public string FileHeader { get; set; } =
            $"Automatisch generiert mit Dataline.Tax.BmfPapConverter\nam {DateTime.Now}";

        /// <summary>
        /// Die gewünschten Erweiterungen der Berechnungsklasse.
        /// Eine Erweiterung ist ein Codebaustein, der in die Berechnungsklasse eingesetzt wird.
        /// Verfügbare Erweiterungen befinden sich im Resources-Verzeichnis mit Präfix "Extension_".
        /// </summary>
        public string[] Extensions { get; set; }

        /// <summary>
        /// Das optionale Zieljahr. Dieses wird verwendet, um spezifische Versionen der <see cref="Extensions"/>
        /// auszuwählen. Hierbei wird zuerst nach "Extension_(Name)_(Jahr).cs" und dann nach "Extension_(Name).cs"
        /// gesucht.
        /// </summary>
        public string Year { get; set; }

        public ProjectBuilder GenerateProject(PapDocument document)
        {
            var inputClass = new ModuleBuilder(ModuleBuilder.ModuleTypes.Class, Visibilities.Public, InputClassName);
            var outputClass = new ModuleBuilder(ModuleBuilder.ModuleTypes.Class, Visibilities.Public, OutputClassName);
            var operationClass = new ModuleBuilder(ModuleBuilder.ModuleTypes.Class, Visibilities.Public, OperationClassName);

            var modules = new[] { inputClass, outputClass, operationClass };

            // Eingangsparameter erzeugen
            GenerateProperties(inputClass, Visibilities.Public, document.Inputs);
            // Ausgangsparameter erzeugen
            GenerateProperties(outputClass, Visibilities.Public, document.Outputs);

            // Eingangs- und Ausgangsparameter in Berechnungsklasse erzeugen
            GenerateOperationalInputOutputProperties(operationClass);
            GenerateOperationalInputAccessors(operationClass, document.Inputs);
            GenerateOperationalOutputAccessors(operationClass, document.Outputs);

            // Interne Berechnungsparameter erzeugen
            GenerateProperties(operationClass, Visibilities.Private, document.Internals);
            // Konstanten erzeugen
            GenerateConstants(operationClass, document.Constants);

            // Berechnungs-Code erzeugen
            GenerateCalculationCode(operationClass, document);

            // Erweiterungen erzeugen
            GenerateExtensions(operationClass);

            // Statische Berechnungsmethoden in Berechnungsklasse erzeugen
            operationClass.StaticCode.Add(StaticCodeLoader.Load(StaticCodeLoader.PapOperationalClassStaticCodeName));

            // Projekt erzeugen
            var project = new ProjectBuilder();
            project.Sources.AddRange(modules.Select(m => new SourceBuilder(m)
            {
                Header = FileHeader,
                Namespace = Namespace
            }));

            return project;
        }

        private void GenerateCalculationCode(ModuleBuilder target, PapDocument document)
        {
            // Main-Methode erzeugen
            target.Methods.Add(new MethodBuilder(Visibilities.Public,
                                                 OperationMainMethodName,
                                                 PapSyntaxTreeConverter.Convert(document.MainMethod.Statements)));

            // Weitere Methoden erzeugen
            target.Methods.AddRange(document.Methods.Select(m => new MethodBuilder(Visibilities.Private,
                                                                                   m.Name,
                                                                                   PapSyntaxTreeConverter.Convert(m.Statements))
            {
                Summary = m.Documentation
            }));
        }

        private void GenerateProperties(ModuleBuilder target,
                                        Visibilities defaultVisibility,
                                        IEnumerable<PapVariable> variables)
        {
            foreach (var variable in variables)
            {
                var defaultValueExpression = variable.Default != null
                    ? PapEvalCodeParser.ConvertToExpression(variable.Default)
                    : null;

                target.Properties.Add(new AutoPropertyBuilder(defaultVisibility,
                                                              TypeMapping.Map(variable.Type),
                                                              variable.Name,
                                                              defaultValueExpression)
                {
                    Summary = variable.Documentation
                });
            }
        }

        private void GenerateOperationalInputOutputProperties(ModuleBuilder target)
        {
            // Property für die Eingangsparameter erzeugen
            target.Properties.Add(new AutoPropertyBuilder(Visibilities.Public,
                                                          new ArbitraryType(InputClassName),
                                                          InputClassName)
            {
                SetInConstructor = true
            });

            // Property für die Ausgangsparameter erzeugen
            var outType = new ArbitraryType(OutputClassName);
            target.Properties.Add(new AutoPropertyBuilder(Visibilities.Public,
                                                          outType,
                                                          OutputClassName)
            {
                DefaultExpression = new NewExpressionBuilder(outType)
            });
        }

        private void GenerateOperationalAccessors(ModuleBuilder target,
                                                  string targetName,
                                                  IEnumerable<PapVariable> variables)
        {
            // Generiert Properties, die Eingabe- oder Ausgabeparameter in die Berechnungsklasse
            // weiterleiten.
            foreach (var variable in variables)
            {
                var memberExpr = new MemberExpressionBuilder(targetName, variable.Name);

                var getter = new ReturnStatementBuilder(memberExpr);
                var setter = new AssignmentStatementBuilder(memberExpr,
                                                            new KeywordExpressionBuilder(KeywordExpressionBuilder.Keywords.Value));

                target.Properties.Add(new FullPropertyBuilder(Visibilities.Private,
                                                              TypeMapping.Map(variable.Type),
                                                              variable.Name,
                                                              getter,
                                                              setter));
            }
        }

        private void GenerateOperationalInputAccessors(ModuleBuilder target, IEnumerable<PapVariable> variables)
        {
            // Accessors für die Eingangsparameter erzeugen
            // Der PAP beschreibt auch Eingangsparameter, deshalb leider keine Getter-Only Properties möglich
            GenerateOperationalAccessors(target, InputClassName, variables);
        }

        private void GenerateOperationalOutputAccessors(ModuleBuilder target, IEnumerable<PapVariable> variables)
        {
            // Accessors für die Ausgangsparameter erzeugen
            GenerateOperationalAccessors(target, OutputClassName, variables);
        }

        private void GenerateConstants(ModuleBuilder target, IEnumerable<PapConstant> constants)
        {
            foreach (var constant in constants)
            {
                var valueExpression = PapEvalCodeParser.ConvertToExpression(constant.Value);
                var type = TypeMapping.Map(constant.Type);

                target.Constants.Add(new ConstantBuilder(Visibilities.Private,
                                                         type,
                                                         constant.Name,
                                                         type.CanBeCompileTimeConstant,
                                                         valueExpression)
                {
                    Summary = constant.Documentation
                });
            }
        }

        private void GenerateExtensions(ModuleBuilder target)
        {
            foreach (string extension in Extensions ?? Array.Empty<string>())
            {
                string extCode = StaticCodeLoader.Load(StaticCodeLoader.ExtensionStaticCodeName(extension, Year));
                target.StaticCode.Add(extCode);
            }
        }
    }
}
