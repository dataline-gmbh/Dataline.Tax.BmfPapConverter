using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Dataline.Tax.LSt2014.Test
{
    public class Test
    {
        private static readonly CultureInfo _culture = new CultureInfo("de-DE"); // Deutsche Dezimaltrenner in der CSV

        [Fact]
        public void RunTest()
        {
            var testdataPaths = Directory.EnumerateFiles(Path.GetDirectoryName(typeof(Test).GetTypeInfo().Assembly.Location))
                .Where(path => Path.GetFileName(path).StartsWith("testdata-") && Path.GetExtension(path) == ".csv");

            int files = 0;
            int tests = 0;

            foreach (string path in testdataPaths)
            {
                foreach (var line in ReadCsvTable(path))
                {
                    var e = new Eingabeparameter
                    {
                        STKL = int.Parse(line["STKL"], _culture),
                        af = int.Parse(line["AF"], _culture),
                        f = double.Parse(line["F"], _culture),
                        ZKF = decimal.Parse(line["ZKF"], _culture),
                        AJAHR = int.Parse(line["AJAHR"], _culture),
                        ALTER1 = int.Parse(line["ALTER1"], _culture),
                        RE4 = decimal.Parse(line["RE4"], _culture),
                        VBEZ = decimal.Parse(line["VBEZ"], _culture),
                        LZZ = int.Parse(line["LZZ"], _culture),
                        KRV = int.Parse(line["KRV"], _culture),
                        PKPV = decimal.Parse(line["PKPV"], _culture),
                        PKV = int.Parse(line["PKV"], _culture),
                        PVS = int.Parse(line["PVS"], _culture),
                        PVZ = int.Parse(line["PVZ"], _culture),
                        R = int.Parse(line["R"], _culture),
                        LZZFREIB = decimal.Parse(line["LZZFREIB"], _culture),
                        LZZHINZU = decimal.Parse(line["LZZHINZU"], _culture),
                        VJAHR = int.Parse(line["VJAHR"], _culture),
                        VBEZM = decimal.Parse(line["VBEZM"], _culture),
                        VBEZS = decimal.Parse(line["VBEZS"], _culture),
                        ZMVB = int.Parse(line["ZMVB"], _culture),
                        JRE4 = decimal.Parse(line["JRE4"], _culture),
                        JVBEZ = decimal.Parse(line["JVBEZ"], _culture),
                        JFREIB = decimal.Parse(line["JFREIB"], _culture),
                        JHINZU = decimal.Parse(line["JHINZU"], _culture),
                        JRE4ENT = decimal.Parse(line["JRE4ENT"], _culture),
                        SONSTB = decimal.Parse(line["SONSTB"], _culture),
                        STERBE = decimal.Parse(line["STERBE"], _culture),
                        VBS = decimal.Parse(line["VBS"], _culture),
                        SONSTENT = decimal.Parse(line["SONSTENT"], _culture),
                        VKAPA = decimal.Parse(line["VKAPA"], _culture),
                        VMT = decimal.Parse(line["VMT"], _culture),
                        ENTSCH = decimal.Parse(line["ENTSCH"], _culture)
                    };

                    var berechnung = new Berechnung(e);
                    berechnung.Lohnsteuer();

                    var a = berechnung.Ausgabeparameter;

                    Assert.Equal(decimal.Parse(line["LSTLZZ"], _culture), a.LSTLZZ);

                    // Optionale Tests je nach Existenz des Feldes in der CSV
                    OptionalTest(line, "SOLZLZZ", a.SOLZLZZ);
                    OptionalTest(line, "BK", a.BK);
                    OptionalTest(line, "STS", a.STS);
                    OptionalTest(line, "SOLZS", a.SOLZS);
                    OptionalTest(line, "BKS", a.BKS);
                    OptionalTest(line, "STV", a.STV);
                    OptionalTest(line, "SOLZV", a.SOLZV);
                    OptionalTest(line, "BKV", a.BKV);
                    OptionalTest(line, "VKVLZZ", a.VKVLZZ);
                    OptionalTest(line, "VKVSONST", a.VKVSONST);

                    tests++;
                }

                files++;
            }

            Assert.NotEqual(0, tests);

            Console.WriteLine($"{tests} Berechnungstests aus {files} Testdateien durchgeführt");
        }

        private static void OptionalTest(Dictionary<string, string> line, string name, decimal current)
        {
            string expected;
            if (line.TryGetValue(name, out expected))
                Assert.Equal(decimal.Parse(expected, _culture), current);
        }

        private static IEnumerable<Dictionary<string, string>> ReadCsvTable(string path)
        {
            const char sep = ';';

            using (var stream = new FileStream(path, FileMode.Open))
            using (var input = new StreamReader(stream))
            {
                // Erste Zeile ist Header-Info
                string[] headers = input.ReadLine().Split(sep);

                int line = 1;
                while (!input.EndOfStream)
                {
                    string lineText = input.ReadLine().Trim();
                    if (string.IsNullOrEmpty(lineText))
                        continue;

                    string[] fields = lineText.Split(sep);
                    if (fields.Length != headers.Length)
                        throw new InvalidOperationException($"Zeile {line}: {headers.Length} Kopfzeilen, {fields.Length} Felder");

                    var dict = new Dictionary<string, string>();
                    for (int i = 0; i < headers.Length; i++)
                        dict[headers[i]] = fields[i];

                    yield return dict;

                    line++;
                }
            }
        }
    }
}
