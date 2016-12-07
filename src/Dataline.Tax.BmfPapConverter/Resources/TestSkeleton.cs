using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace %projectname%.Test
{
    public class Test
    {
        [Fact]
        public void RunTest()
        {
            string data = Path.Combine(Path.GetDirectoryName(typeof(Test).GetTypeInfo().Assembly.Location), "testdata.csv");
            Assert.True(File.Exists(data), $"Test-Daten existieren nicht: {data}");

            var culture = new CultureInfo("de-DE"); // Deutsche Dezimaltrenner in der CSV
        
            foreach (var line in ReadCsvTable(data))
            {
                var e = new Eingabeparameter
                {
                    STKL = int.Parse(line["STKL"], culture),
                    af = int.Parse(line["AF"], culture),
                    f = double.Parse(line["F"], culture),
                    ZKF = decimal.Parse(line["ZKF"], culture),
                    AJAHR = int.Parse(line["AJAHR"], culture),
                    ALTER1 = int.Parse(line["ALTER1"], culture),
                    RE4 = decimal.Parse(line["RE4"], culture),
                    VBEZ = decimal.Parse(line["VBEZ"], culture),
                    LZZ = int.Parse(line["LZZ"], culture),
                    KRV = int.Parse(line["KRV"], culture),
                    KVZ = decimal.Parse(line["KVZ"], culture),
                    PKPV = decimal.Parse(line["PKPV"], culture),
                    PKV = int.Parse(line["PKV"], culture),
                    PVS = int.Parse(line["PVS"], culture),
                    PVZ = int.Parse(line["PVZ"], culture),
                    R = int.Parse(line["R"], culture),
                    LZZFREIB = decimal.Parse(line["LZZFREIB"], culture),
                    LZZHINZU = decimal.Parse(line["LZZHINZU"], culture),
                    VJAHR = int.Parse(line["VJAHR"], culture),
                    VBEZM = decimal.Parse(line["VBEZM"], culture),
                    VBEZS = decimal.Parse(line["VBEZS"], culture),
                    ZMVB = int.Parse(line["ZMVB"], culture),
                    JRE4 = decimal.Parse(line["JRE4"], culture),
                    JVBEZ = decimal.Parse(line["JVBEZ"], culture),
                    JFREIB = decimal.Parse(line["JFREIB"], culture),
                    JHINZU = decimal.Parse(line["JHINZU"], culture),
                    JRE4ENT = decimal.Parse(line["JRE4ENT"], culture),
                    SONSTB = decimal.Parse(line["SONSTB"], culture),
                    STERBE = decimal.Parse(line["STERBE"], culture),
                    VBS = decimal.Parse(line["VBS"], culture),
                    SONSTENT = decimal.Parse(line["SONSTENT"], culture),
                    VKAPA = decimal.Parse(line["VKAPA"], culture),
                    VMT = decimal.Parse(line["VMT"], culture),
                    ENTSCH = decimal.Parse(line["ENTSCH"], culture)
                };

                var berechnung = new Berechnung(e);
                berechnung.Lohnsteuer();

                var a = berechnung.Ausgabeparameter;

                Assert.Equal(decimal.Parse(line["LSTLZZ"], culture), a.LSTLZZ);
                Assert.Equal(decimal.Parse(line["SOLZLZZ"], culture), a.SOLZLZZ);
                Assert.Equal(decimal.Parse(line["BK"], culture), a.BK);
                Assert.Equal(decimal.Parse(line["STS"], culture), a.STS);
                Assert.Equal(decimal.Parse(line["SOLZS"], culture), a.SOLZS);
                Assert.Equal(decimal.Parse(line["BKS"], culture), a.BKS);
                Assert.Equal(decimal.Parse(line["STV"], culture), a.STV);
                Assert.Equal(decimal.Parse(line["SOLZV"], culture), a.SOLZV);
                Assert.Equal(decimal.Parse(line["BKV"], culture), a.BKV);
                Assert.Equal(decimal.Parse(line["VKVLZZ"], culture), a.VKVLZZ);
                Assert.Equal(decimal.Parse(line["VKVSONST"], culture), a.VKVSONST);
            }
        }

        private IEnumerable<Dictionary<string, string>> ReadCsvTable(string path)
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
