using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dataline.Tax.BmfPapConverter.CodeGen;
using Xunit;

namespace Dataline.Tax.BmfPapConverter.Test
{
    public class CodeGenTests
    {
        [Fact]
        public void TestGenerateClass()
        {
            var @class = new ModuleBuilder(ModuleBuilder.ModuleTypes.Class, Visibilities.Public, "TestClass");
            @class.Inheritations.Add(new ArbitraryType("SomeTypeXY"));
            @class.Inheritations.Add(typeof(string));

            var codeBuilder = new CodeBuilder();
            @class.CodeGen(codeBuilder);

            Assert.Equal("public class TestClass : SomeTypeXY, string\r\n{\r\n}", codeBuilder.ToString());
        }

        [Fact]
        public void TestGenerateProperty()
        {
            var property = new AutoPropertyBuilder(Visibilities.Public, typeof(string), "Test");

            var codeBuilder = new CodeBuilder();
            property.CodeGen(codeBuilder);

            Assert.Equal("public string Test { get; set; }", codeBuilder.ToString());
        }

        [Fact]
        public void TestGenerateClassWithProperties()
        {
            var property1 = new AutoPropertyBuilder(Visibilities.Public, typeof(string), "Test");
            var property2 = new AutoPropertyBuilder(Visibilities.Public, typeof(int), "Test2");

            var @class = new ModuleBuilder(ModuleBuilder.ModuleTypes.Class, Visibilities.Public, "TestClass");
            @class.Inheritations.Add(new ArbitraryType("SomeTypeXY"));
            @class.Inheritations.Add(typeof(string));

            @class.Properties.Add(property1);
            @class.Properties.Add(property2);

            var codeBuilder = new CodeBuilder();
            @class.CodeGen(codeBuilder);

            const string expected =
                "public class TestClass : SomeTypeXY, string\r\n" +
                "{\r\n" +
                "    public string Test { get; set; }\r\n" +
                "\r\n" +
                "    public int Test2 { get; set; }" +
                "\r\n" +
                "}";
            Assert.Equal(expected, codeBuilder.ToString());
        }
    }
}
