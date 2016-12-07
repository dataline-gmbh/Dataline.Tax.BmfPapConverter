using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dataline.Tax.BmfPapConverter.CodeGen;
using Xunit;

namespace Dataline.Tax.BmfPapConverter.Test
{
    public class PapConverterTests
    {
        private readonly PapConverter _converter = new PapConverter
        {
            InputClassName = "Eingangsparameter",
            OutputClassName = "Ausgangsparameter",
            OperationClassName = "Berechnung",
            OperationMainMethodName = "Main",
            Namespace = "Dataline.Tax.PapTest",
            FileHeader = "Test"
        };

        private readonly PapDocument _papDocument = new PapDocument
        {
            Inputs = new List<PapVariable>
            {
                new PapVariable
                {
                    Name = "Input1",
                    Type = "int"
                },
                new PapVariable
                {
                    Name = "Input2",
                    Type = "BigDecimal",
                    Default = "new BigDecimal(1234.5)"
                }
            },
            Outputs = new List<PapVariable>
            {
                new PapVariable
                {
                    Name = "Output1",
                    Type = "double"
                },
                new PapVariable
                {
                    Name = "Output2",
                    Type = "BigDecimal"
                }
            },
            Internals = new List<PapVariable>
            {
                new PapVariable
                {
                    Name = "Internal1",
                    Type = "int",
                    Default = "BigDecimal.ONE"
                },
                new PapVariable
                {
                    Name = "Internal2",
                    Type = "BigDecimal"
                }
            },
            Constants = new List<PapConstant>
            {
                new PapConstant
                {
                    Name = "Constant1",
                    Type = "int",
                    Value = "1234"
                },
                new PapConstant
                {
                    Name = "Constant2",
                    Type = "BigDecimal[]",
                    Value = "{BigDecimal.valueOf(1), BigDecimal.valueOf(1.234)}"
                }
            },
            MainMethod = new PapMethod
            {
                Statements = new PapSyntaxTreeNodeStatementList
                {
                    Nodes = new List<IPapSyntaxTreeNode>
                    {
                        new PapSyntaxTreeNodeExecute
                        {
                            MethodName = "METHODABC"
                        }
                    }
                }
            },
            Methods = new List<PapMethod>
            {
                new PapMethod
                {
                    Name = "METHODABC",
                    Statements = new PapSyntaxTreeNodeStatementList
                    {
                        Nodes = new List<IPapSyntaxTreeNode>
                        {
                            new PapSyntaxTreeNodeIf
                            {
                                Condition = "Input2 == BigDecimal. valueOf(1234.5)",
                                ThenStatements = new PapSyntaxTreeNodeStatementList
                                {
                                    Nodes = new List<IPapSyntaxTreeNode>
                                    {
                                        new PapSyntaxTreeNodeEval
                                        {
                                            Expression = "Output2 = BigDecimal.ZERO"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        [Fact]
        public void TestGeneratePapInputOutput()
        {
            var project = _converter.GenerateProject(_papDocument);

            string inputSrc = MakeSource(project.Sources.Single(s => s.Name == "Eingangsparameter.cs"));
            string outputSrc = MakeSource(project.Sources.Single(s => s.Name == "Ausgangsparameter.cs"));

            const string expectedInputSrc =
                "/*\r\n" +
                " * Test\r\n" +
                " */\r\n" +
                "\r\n" +
                "using System;\r\n" +
                "using System.Collections.Generic;\r\n" +
                "using System.Linq;\r\n" +
                "\r\n" +
                "namespace Dataline.Tax.PapTest\r\n" +
                "{\r\n" +
                "    public class Eingangsparameter\r\n" +
                "    {\r\n" +
                "        public int Input1 { get; set; }\r\n" +
                "\r\n" +
                "        public decimal Input2 { get; set; } = 1234.5m;\r\n" +
                "    }\r\n" +
                "}";
            const string expectedOutputSrc =
                "/*\r\n" +
                " * Test\r\n" +
                " */\r\n" +
                "\r\n" +
                "using System;\r\n" +
                "using System.Collections.Generic;\r\n" +
                "using System.Linq;\r\n" +
                "\r\n" +
                "namespace Dataline.Tax.PapTest\r\n" +
                "{\r\n" +
                "    public class Ausgangsparameter\r\n" +
                "    {\r\n" +
                "        public double Output1 { get; set; }\r\n" +
                "\r\n" +
                "        public decimal Output2 { get; set; }\r\n" +
                "    }\r\n" +
                "}";

            Assert.Equal(expectedInputSrc, inputSrc);
            Assert.Equal(expectedOutputSrc, outputSrc);
        }

        public static string MakeSource(ICodeGen codeGen)
        {
            var builder = new CodeBuilder();
            codeGen.CodeGen(builder);
            return builder.ToString();
        }
    }
}

