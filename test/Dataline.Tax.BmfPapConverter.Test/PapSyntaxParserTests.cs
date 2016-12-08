// <copyright file="PapSyntaxParserTests.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Dataline.Tax.BmfPapConverter.Test
{
    public class PapSyntaxParserTests
    {
        private readonly CSharpParseOptions _options = new CSharpParseOptions(kind: SourceCodeKind.Script);

        [Fact]
        public void Test1()
        {
            var tree = CSharpSyntaxTree.ParseText(
                "ZVE= (ZRE4.subtract (ZTABFB).subtract (VSP).subtract ((VMT).divide " +
                "(ZAHL100)).subtract ((VKAPA).divide (ZAHL100))).setScale (2, " +
                "BigDecimal.ROUND_DOWN)", _options);

            var result = PapConverterTests.MakeSource(PapEvalCodeParser.ConvertToStatement(tree.GetRoot()));

            const string expected = "ZVE = Floor(((((ZRE4 - ZTABFB) - VSP) - (VMT / ZAHL100)) - (VKAPA / ZAHL100)), 2);";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test2()
        {
            var tree = CSharpSyntaxTree.ParseText(
                "FVBZ = BigDecimal.valueOf(ZVBEZ.longValue())", _options);

            var result = PapConverterTests.MakeSource(PapEvalCodeParser.ConvertToStatement(tree.GetRoot()));

            const string expected = "FVBZ = ((decimal)((long)ZVBEZ));";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test3()
        {
            var tree = CSharpSyntaxTree.ParseText(
                "PVSATZAN = PVSATZAN.add(BigDecimal.valueOf(0.2))", _options);

            var result = PapConverterTests.MakeSource(PapEvalCodeParser.ConvertToStatement(tree.GetRoot()));

            const string expected = "PVSATZAN = (PVSATZAN + 0.2m);";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test4()
        {
            var tree = CSharpSyntaxTree.ParseText(
                "ZRE4J= RE4.divide (ZAHL100, 2, BigDecimal.ROUND_UP)", _options);

            var result = PapConverterTests.MakeSource(PapEvalCodeParser.ConvertToStatement(tree.GetRoot()));

            const string expected = "ZRE4J = Ceiling((RE4 / ZAHL100), 2);";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test5()
        {
            var tree = CSharpSyntaxTree.ParseText(
                "FVB= ((VBEZB.multiply (TAB1[J]))).divide (ZAHL100).setScale (2, BigDecimal.ROUND_UP)", _options);

            var result = PapConverterTests.MakeSource(PapEvalCodeParser.ConvertToStatement(tree.GetRoot()));

            const string expected = "FVB = Ceiling(((VBEZB * TAB1[J]) / ZAHL100), 2);";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test6()
        {
            var tree = CSharpSyntaxTree.ParseText(
                "{BigDecimal.valueOf (0.0), BigDecimal.valueOf (0.4), BigDecimal.valueOf (0.384), BigDecimal.valueOf (0.368)}", _options);

            var result = PapConverterTests.MakeSource(PapEvalCodeParser.ConvertToExpression(tree.GetRoot()));

            const string expected = "{ 0.0m, 0.4m, 0.384m, 0.368m }";

            Assert.Equal(expected, result);
        }
        
        [Fact]
        public void Test7()
        {
            var tree = CSharpSyntaxTree.ParseText(
                "new BigDecimal(2)", _options);

            var result = PapConverterTests.MakeSource(PapEvalCodeParser.ConvertToExpression(tree.GetRoot()));

            const string expected = "2m";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test8()
        {
            var tree = CSharpSyntaxTree.ParseText(
                "AJAHR < 2006", _options);

            var result = PapConverterTests.MakeSource(PapEvalCodeParser.ConvertToExpression(tree.GetRoot()));

            const string expected = "(AJAHR < 2006)";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test9()
        {
            var tree = CSharpSyntaxTree.ParseText(
                "ZVBEZ.compareTo(BigDecimal.ZERO) == -1", _options);

            var result = PapConverterTests.MakeSource(PapEvalCodeParser.ConvertToExpression(tree.GetRoot()));

            const string expected = "(CompareTo(ZVBEZ, 0m) == (-1))";

            Assert.Equal(expected, result);
        }
    }
}
