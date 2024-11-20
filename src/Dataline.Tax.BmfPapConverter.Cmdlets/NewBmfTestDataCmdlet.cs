// <copyright file="NewBmfTestDataCmdlet.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace Dataline.Tax.BmfPapConverter.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "BmfTestData")]
    public class NewBmfTestDataCmdlet : PSCmdlet
    {
        public enum StandardTestDataTypes
        {
            Allgemein,
            Besonders
        }

        [Parameter(Mandatory = true, HelpMessage = "Lohnsteuer-Prüftabelle")]
        public decimal[][] Table { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Einkommensbezogener Zusatzbeitragssatz")]
        public decimal Kvz { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Krankenversicherung", ParameterSetName = "Custom"), ValidateRange(0, 2)]
        public decimal Pkv { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Merker für die Vorsorgepauschale", ParameterSetName = "Custom"), ValidateRange(0, 2)]
        public decimal Krv { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Typ der Prüftabelle", ParameterSetName = "Standard")]
        public StandardTestDataTypes Type { get; set; }

        [Parameter(Mandatory = true)]
        public string OutputPath { get; set; }

        protected override void ProcessRecord()
        {
            const string header =
                "lfd. Nr.;STKL;AF;F;ZKF;AJAHR;ALTER1;RE4;VBEZ;LZZ;KRV;KVZ;PKPV;PKV;PVS;PVZ;R;" +
                "LZZFREIB;LZZHINZU;VJAHR;VBEZM;VBEZS;ZMVB;JRE4;JVBEZ;JFREIB;JHINZU;JRE4ENT;" +
                "SONSTB;STERBE;VBS;SONSTENT;LSTLZZ";
            var culture = new CultureInfo("de-DE"); // Deutsche Dezimaltrenner in CSV
            
            if (ParameterSetName == "Standard")
            {
                // Standard-Prüftabelle
                switch (Type)
                {
                    case StandardTestDataTypes.Allgemein:
                        Pkv = 0m;
                        Krv = 0m;
                        break;
                    case StandardTestDataTypes.Besonders:
                        Pkv = 1m;
                        Krv = 1m; // ab 2025
                        //Krv = 2m; // bis 2024
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            int lfdnr = 1;

            using (var stream = new FileStream(SessionState.Path.GetUnresolvedProviderPathFromPSPath(OutputPath), FileMode.Create))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine(header);

                foreach (var row in Table)
                {
                    if (row.Length != 7)
                        throw new InvalidOperationException("Eine Zeile muss exakt 7 Spalten haben.");

                    decimal jahresbruttolohn = row[0];

                    for (int steuerklasse = 1; steuerklasse != 7; steuerklasse++)
                    {
                        decimal jahreslohnsteuer = row[steuerklasse];

                        decimal lzz = 1m; // Jahr
                        decimal stkl = steuerklasse;
                        decimal pvz = steuerklasse != 2 ? 1 : 0;
                        decimal re4 = jahresbruttolohn * 100;
                        decimal jre4 = jahresbruttolohn * 100;
                        decimal ajahr = 2040; // Geburt 1975
                        decimal alter1 = 0; // 64. LJ endet nach Ende des LZZ

                        // Spalten gemäß des Headers
                        // Nicht gesetzte Felder werden auf ihre Default-Werte gesetzt
                        decimal[] csvRow =
                        {
                            lfdnr++, // lfd. Nr.
                            stkl, // STKL
                            1m, // AF
                            1m, // F
                            0m, // ZKF
                            ajahr, // AJAHR
                            alter1, // ALTER1
                            re4, // RE4
                            0m, // VBEZ
                            lzz, // LZZ
                            Krv, // KRV
                            Kvz, // KVZ
                            0, // PKPV
                            Pkv, // PKV
                            0m, // PVS
                            pvz, // PVZ
                            0m, // R
                            0m, // LZZFREIB
                            0m, // LZZHINZU
                            0m, // VJAHR
                            0m, // VBEZM
                            0m, // VBEZS
                            0m, // ZMVB
                            jre4, // JRE4
                            0m, // JVBEZ
                            0m, // JFREIB
                            0m, // JHINZU
                            0m, // JRE4ENT
                            0m, // SONSTB
                            0m, // STERBE
                            0m, // VBS
                            0m, // SONSTENT
                            jahreslohnsteuer * 100 // LSTLZZ
                        };

                        writer.WriteLine(string.Join(";", csvRow.Select(r => r.ToString(culture))));
                    }
                }
            }
        }
    }
}
