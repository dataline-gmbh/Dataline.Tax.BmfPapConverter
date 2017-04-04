/*
 * Automatisch generiert mit Dataline.Tax.BmfPapConverter
 * am 04.04.2017 15:19:39
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataline.Tax.LSt2016
{
    public class Berechnung
    {
        /// <summary>
        /// Tabelle fuer die Vomhundertsaetze des Versorgungsfreibetrags
        /// </summary>
        private static readonly decimal[] TAB1 = { 0.0m, 0.4m, 0.384m, 0.368m, 0.352m, 0.336m, 0.32m, 0.304m, 0.288m, 0.272m, 0.256m, 0.24m, 0.224m, 0.208m, 0.192m, 0.176m, 0.16m, 0.152m, 0.144m, 0.136m, 0.128m, 0.12m, 0.112m, 0.104m, 0.096m, 0.088m, 0.08m, 0.072m, 0.064m, 0.056m, 0.048m, 0.04m, 0.032m, 0.024m, 0.016m, 0.008m, 0.0m };

        /// <summary>
        /// Tabelle fuer die Hoechstbetrage des Versorgungsfreibetrags
        /// </summary>
        private static readonly decimal[] TAB2 = { 0m, 3000m, 2880m, 2760m, 2640m, 2520m, 2400m, 2280m, 2160m, 2040m, 1920m, 1800m, 1680m, 1560m, 1440m, 1320m, 1200m, 1140m, 1080m, 1020m, 960m, 900m, 840m, 780m, 720m, 660m, 600m, 540m, 480m, 420m, 360m, 300m, 240m, 180m, 120m, 60m, 0m };

        /// <summary>
        /// Tabelle fuer die Zuschlaege zum Versorgungsfreibetrag
        /// </summary>
        private static readonly decimal[] TAB3 = { 0m, 900m, 864m, 828m, 792m, 756m, 720m, 684m, 648m, 612m, 576m, 540m, 504m, 468m, 432m, 396m, 360m, 342m, 324m, 306m, 288m, 270m, 252m, 234m, 216m, 198m, 180m, 162m, 144m, 126m, 108m, 90m, 72m, 54m, 36m, 18m, 0m };

        /// <summary>
        /// Tabelle fuer die Vomhundertsaetze des Altersentlastungsbetrags
        /// </summary>
        private static readonly decimal[] TAB4 = { 0.0m, 0.4m, 0.384m, 0.368m, 0.352m, 0.336m, 0.32m, 0.304m, 0.288m, 0.272m, 0.256m, 0.24m, 0.224m, 0.208m, 0.192m, 0.176m, 0.16m, 0.152m, 0.144m, 0.136m, 0.128m, 0.12m, 0.112m, 0.104m, 0.096m, 0.088m, 0.08m, 0.072m, 0.064m, 0.056m, 0.048m, 0.04m, 0.032m, 0.024m, 0.016m, 0.008m, 0.0m };

        /// <summary>
        /// Tabelle fuer die Hoechstbetraege des Altersentlastungsbetrags
        /// </summary>
        private static readonly decimal[] TAB5 = { 0m, 1900m, 1824m, 1748m, 1672m, 1596m, 1520m, 1444m, 1368m, 1292m, 1216m, 1140m, 1064m, 988m, 912m, 836m, 760m, 722m, 684m, 646m, 608m, 570m, 532m, 494m, 456m, 418m, 380m, 342m, 304m, 266m, 228m, 190m, 152m, 114m, 76m, 38m, 0m };

        /// <summary>
        /// Zahlenkonstanten fuer im Plan oft genutzte BigDecimal Werte
        /// </summary>
        private const decimal ZAHL1 = 1m;
        private const decimal ZAHL2 = 2m;
        private const decimal ZAHL5 = 5m;
        private const decimal ZAHL7 = 7m;
        private const decimal ZAHL12 = 12m;
        private const decimal ZAHL100 = 100m;
        private const decimal ZAHL360 = 360m;
        private const decimal ZAHL500 = 500m;
        private const decimal ZAHL700 = 700m;
        private const decimal ZAHL1000 = 1000m;
        private const decimal ZAHL10000 = 10000m;

        public Berechnung(Eingabeparameter eingabeparameter)
        {
            Eingabeparameter = eingabeparameter;
        }

        public Eingabeparameter Eingabeparameter { get; set; }

        public Ausgabeparameter Ausgabeparameter { get; set; } = new Ausgabeparameter();

        private int af
        {
            get
            {
                return Eingabeparameter.af;
            }
            set
            {
                Eingabeparameter.af = value;
            }
        }

        private int AJAHR
        {
            get
            {
                return Eingabeparameter.AJAHR;
            }
            set
            {
                Eingabeparameter.AJAHR = value;
            }
        }

        private int ALTER1
        {
            get
            {
                return Eingabeparameter.ALTER1;
            }
            set
            {
                Eingabeparameter.ALTER1 = value;
            }
        }

        private decimal ENTSCH
        {
            get
            {
                return Eingabeparameter.ENTSCH;
            }
            set
            {
                Eingabeparameter.ENTSCH = value;
            }
        }

        private double f
        {
            get
            {
                return Eingabeparameter.f;
            }
            set
            {
                Eingabeparameter.f = value;
            }
        }

        private decimal JFREIB
        {
            get
            {
                return Eingabeparameter.JFREIB;
            }
            set
            {
                Eingabeparameter.JFREIB = value;
            }
        }

        private decimal JHINZU
        {
            get
            {
                return Eingabeparameter.JHINZU;
            }
            set
            {
                Eingabeparameter.JHINZU = value;
            }
        }

        private decimal JRE4
        {
            get
            {
                return Eingabeparameter.JRE4;
            }
            set
            {
                Eingabeparameter.JRE4 = value;
            }
        }

        private decimal JVBEZ
        {
            get
            {
                return Eingabeparameter.JVBEZ;
            }
            set
            {
                Eingabeparameter.JVBEZ = value;
            }
        }

        private int KRV
        {
            get
            {
                return Eingabeparameter.KRV;
            }
            set
            {
                Eingabeparameter.KRV = value;
            }
        }

        private decimal KVZ
        {
            get
            {
                return Eingabeparameter.KVZ;
            }
            set
            {
                Eingabeparameter.KVZ = value;
            }
        }

        private int LZZ
        {
            get
            {
                return Eingabeparameter.LZZ;
            }
            set
            {
                Eingabeparameter.LZZ = value;
            }
        }

        private decimal LZZFREIB
        {
            get
            {
                return Eingabeparameter.LZZFREIB;
            }
            set
            {
                Eingabeparameter.LZZFREIB = value;
            }
        }

        private decimal LZZHINZU
        {
            get
            {
                return Eingabeparameter.LZZHINZU;
            }
            set
            {
                Eingabeparameter.LZZHINZU = value;
            }
        }

        private decimal PKPV
        {
            get
            {
                return Eingabeparameter.PKPV;
            }
            set
            {
                Eingabeparameter.PKPV = value;
            }
        }

        private int PKV
        {
            get
            {
                return Eingabeparameter.PKV;
            }
            set
            {
                Eingabeparameter.PKV = value;
            }
        }

        private int PVS
        {
            get
            {
                return Eingabeparameter.PVS;
            }
            set
            {
                Eingabeparameter.PVS = value;
            }
        }

        private int PVZ
        {
            get
            {
                return Eingabeparameter.PVZ;
            }
            set
            {
                Eingabeparameter.PVZ = value;
            }
        }

        private int R
        {
            get
            {
                return Eingabeparameter.R;
            }
            set
            {
                Eingabeparameter.R = value;
            }
        }

        private decimal RE4
        {
            get
            {
                return Eingabeparameter.RE4;
            }
            set
            {
                Eingabeparameter.RE4 = value;
            }
        }

        private decimal SONSTB
        {
            get
            {
                return Eingabeparameter.SONSTB;
            }
            set
            {
                Eingabeparameter.SONSTB = value;
            }
        }

        private decimal STERBE
        {
            get
            {
                return Eingabeparameter.STERBE;
            }
            set
            {
                Eingabeparameter.STERBE = value;
            }
        }

        private int STKL
        {
            get
            {
                return Eingabeparameter.STKL;
            }
            set
            {
                Eingabeparameter.STKL = value;
            }
        }

        private decimal VBEZ
        {
            get
            {
                return Eingabeparameter.VBEZ;
            }
            set
            {
                Eingabeparameter.VBEZ = value;
            }
        }

        private decimal VBEZM
        {
            get
            {
                return Eingabeparameter.VBEZM;
            }
            set
            {
                Eingabeparameter.VBEZM = value;
            }
        }

        private decimal VBEZS
        {
            get
            {
                return Eingabeparameter.VBEZS;
            }
            set
            {
                Eingabeparameter.VBEZS = value;
            }
        }

        private decimal VBS
        {
            get
            {
                return Eingabeparameter.VBS;
            }
            set
            {
                Eingabeparameter.VBS = value;
            }
        }

        private int VJAHR
        {
            get
            {
                return Eingabeparameter.VJAHR;
            }
            set
            {
                Eingabeparameter.VJAHR = value;
            }
        }

        private decimal VKAPA
        {
            get
            {
                return Eingabeparameter.VKAPA;
            }
            set
            {
                Eingabeparameter.VKAPA = value;
            }
        }

        private decimal VMT
        {
            get
            {
                return Eingabeparameter.VMT;
            }
            set
            {
                Eingabeparameter.VMT = value;
            }
        }

        private decimal ZKF
        {
            get
            {
                return Eingabeparameter.ZKF;
            }
            set
            {
                Eingabeparameter.ZKF = value;
            }
        }

        private int ZMVB
        {
            get
            {
                return Eingabeparameter.ZMVB;
            }
            set
            {
                Eingabeparameter.ZMVB = value;
            }
        }

        private decimal JRE4ENT
        {
            get
            {
                return Eingabeparameter.JRE4ENT;
            }
            set
            {
                Eingabeparameter.JRE4ENT = value;
            }
        }

        private decimal SONSTENT
        {
            get
            {
                return Eingabeparameter.SONSTENT;
            }
            set
            {
                Eingabeparameter.SONSTENT = value;
            }
        }

        private decimal BK
        {
            get
            {
                return Ausgabeparameter.BK;
            }
            set
            {
                Ausgabeparameter.BK = value;
            }
        }

        private decimal BKS
        {
            get
            {
                return Ausgabeparameter.BKS;
            }
            set
            {
                Ausgabeparameter.BKS = value;
            }
        }

        private decimal BKV
        {
            get
            {
                return Ausgabeparameter.BKV;
            }
            set
            {
                Ausgabeparameter.BKV = value;
            }
        }

        private decimal LSTLZZ
        {
            get
            {
                return Ausgabeparameter.LSTLZZ;
            }
            set
            {
                Ausgabeparameter.LSTLZZ = value;
            }
        }

        private decimal SOLZLZZ
        {
            get
            {
                return Ausgabeparameter.SOLZLZZ;
            }
            set
            {
                Ausgabeparameter.SOLZLZZ = value;
            }
        }

        private decimal SOLZS
        {
            get
            {
                return Ausgabeparameter.SOLZS;
            }
            set
            {
                Ausgabeparameter.SOLZS = value;
            }
        }

        private decimal SOLZV
        {
            get
            {
                return Ausgabeparameter.SOLZV;
            }
            set
            {
                Ausgabeparameter.SOLZV = value;
            }
        }

        private decimal STS
        {
            get
            {
                return Ausgabeparameter.STS;
            }
            set
            {
                Ausgabeparameter.STS = value;
            }
        }

        private decimal STV
        {
            get
            {
                return Ausgabeparameter.STV;
            }
            set
            {
                Ausgabeparameter.STV = value;
            }
        }

        private decimal VKVLZZ
        {
            get
            {
                return Ausgabeparameter.VKVLZZ;
            }
            set
            {
                Ausgabeparameter.VKVLZZ = value;
            }
        }

        private decimal VKVSONST
        {
            get
            {
                return Ausgabeparameter.VKVSONST;
            }
            set
            {
                Ausgabeparameter.VKVSONST = value;
            }
        }

        private decimal VFRB
        {
            get
            {
                return Ausgabeparameter.VFRB;
            }
            set
            {
                Ausgabeparameter.VFRB = value;
            }
        }

        private decimal VFRBS1
        {
            get
            {
                return Ausgabeparameter.VFRBS1;
            }
            set
            {
                Ausgabeparameter.VFRBS1 = value;
            }
        }

        private decimal VFRBS2
        {
            get
            {
                return Ausgabeparameter.VFRBS2;
            }
            set
            {
                Ausgabeparameter.VFRBS2 = value;
            }
        }

        private decimal WVFRB
        {
            get
            {
                return Ausgabeparameter.WVFRB;
            }
            set
            {
                Ausgabeparameter.WVFRB = value;
            }
        }

        private decimal WVFRBO
        {
            get
            {
                return Ausgabeparameter.WVFRBO;
            }
            set
            {
                Ausgabeparameter.WVFRBO = value;
            }
        }

        private decimal WVFRBM
        {
            get
            {
                return Ausgabeparameter.WVFRBM;
            }
            set
            {
                Ausgabeparameter.WVFRBM = value;
            }
        }

        /// <summary>
        /// Altersentlastungsbetrag nach Alterseinkünftegesetz in €,
        /// Cent (2 Dezimalstellen)
        /// </summary>
        private decimal ALTE { get; set; } = 0m;

        /// <summary>
        /// Arbeitnehmer-Pauschbetrag in EURO
        /// </summary>
        private decimal ANP { get; set; } = 0m;

        /// <summary>
        /// Auf den Lohnzahlungszeitraum entfallender Anteil von Jahreswerten
        /// auf ganze Cents abgerundet
        /// </summary>
        private decimal ANTEIL1 { get; set; } = 0m;

        /// <summary>
        /// Bemessungsgrundlage für Altersentlastungsbetrag in €, Cent
        /// (2 Dezimalstellen)
        /// </summary>
        private decimal BMG { get; set; } = 0m;

        /// <summary>
        /// Beitragsbemessungsgrenze in der gesetzlichen Krankenversicherung
        /// und der sozialen Pflegeversicherung in Euro
        /// </summary>
        private decimal BBGKVPV { get; set; } = 0m;

        /// <summary>
        /// allgemeine Beitragsbemessungsgrenze in der allgemeinen Renten-versicherung in Euro
        /// </summary>
        private decimal BBGRV { get; set; } = 0m;

        /// <summary>
        /// Differenz zwischen ST1 und ST2 in EURO
        /// </summary>
        private decimal DIFF { get; set; } = 0m;

        /// <summary>
        /// Entlastungsbetrag fuer Alleinerziehende in EURO
        /// </summary>
        private decimal EFA { get; set; } = 0m;

        /// <summary>
        /// Versorgungsfreibetrag in €, Cent (2 Dezimalstellen)
        /// </summary>
        private decimal FVB { get; set; } = 0m;

        /// <summary>
        /// Versorgungsfreibetrag in €, Cent (2 Dezimalstellen) für die Berechnung
        /// der Lohnsteuer für den sonstigen Bezug
        /// </summary>
        private decimal FVBSO { get; set; } = 0m;

        /// <summary>
        /// Zuschlag zum Versorgungsfreibetrag in EURO
        /// </summary>
        private decimal FVBZ { get; set; } = 0m;

        /// <summary>
        /// Zuschlag zum Versorgungsfreibetrag in EURO fuer die Berechnung
        /// der Lohnsteuer beim sonstigen Bezug
        /// </summary>
        private decimal FVBZSO { get; set; } = 0m;

        /// <summary>
        /// Grundfreibetrag in Euro
        /// </summary>
        private decimal GFB { get; set; } = 0m;

        /// <summary>
        /// Maximaler Altersentlastungsbetrag in €
        /// </summary>
        private decimal HBALTE { get; set; } = 0m;

        /// <summary>
        /// Massgeblicher maximaler Versorgungsfreibetrag in €
        /// </summary>
        private decimal HFVB { get; set; } = 0m;

        /// <summary>
        /// Massgeblicher maximaler Zuschlag zum Versorgungsfreibetrag in €,Cent
        /// (2 Dezimalstellen)
        /// </summary>
        private decimal HFVBZ { get; set; } = 0m;

        /// <summary>
        /// Massgeblicher maximaler Zuschlag zum Versorgungsfreibetrag in €, Cent
        /// (2 Dezimalstellen) für die Berechnung der Lohnsteuer für den
        /// sonstigen Bezug
        /// </summary>
        private decimal HFVBZSO { get; set; } = 0m;

        /// <summary>
        /// Nummer der Tabellenwerte fuer Versorgungsparameter
        /// </summary>
        private int J { get; set; } = 0;

        /// <summary>
        /// Jahressteuer nach § 51a EStG, aus der Solidaritaetszuschlag und
        /// Bemessungsgrundlage fuer die Kirchenlohnsteuer ermittelt werden in EURO
        /// </summary>
        private decimal JBMG { get; set; } = 0m;

        /// <summary>
        /// Auf einen Jahreslohn hochgerechneter LZZFREIB in €, Cent
        /// (2 Dezimalstellen)
        /// </summary>
        private decimal JLFREIB { get; set; } = 0m;

        /// <summary>
        /// Auf einen Jahreslohn hochgerechnete LZZHINZU in €, Cent
        /// (2 Dezimalstellen)
        /// </summary>
        private decimal JLHINZU { get; set; } = 0m;

        /// <summary>
        /// Jahreswert, dessen Anteil fuer einen Lohnzahlungszeitraum in
        /// UPANTEIL errechnet werden soll in Cents
        /// </summary>
        private decimal JW { get; set; } = 0m;

        /// <summary>
        /// Nummer der Tabellenwerte fuer Parameter bei Altersentlastungsbetrag
        /// </summary>
        private int K { get; set; } = 0;

        /// <summary>
        /// Merker für Berechnung Lohnsteuer für mehrjährige Tätigkeit.
        /// 0 = normale Steuerberechnung
        /// 1 = Steuerberechnung für mehrjährige Tätigkeit
        /// 2 = entfällt
        /// </summary>
        private int KENNVMT { get; set; } = 0;

        /// <summary>
        /// Summe der Freibetraege fuer Kinder in EURO
        /// </summary>
        private decimal KFB { get; set; } = 0m;

        /// <summary>
        /// Beitragssatz des Arbeitgebers zur Krankenversicherung
        /// </summary>
        private decimal KVSATZAG { get; set; } = 0m;

        /// <summary>
        /// Beitragssatz des Arbeitnehmers zur Krankenversicherung
        /// </summary>
        private decimal KVSATZAN { get; set; } = 0m;

        /// <summary>
        /// Kennzahl fuer die Einkommensteuer-Tabellenart:
        /// 1 = Grundtabelle
        /// 2 = Splittingtabelle
        /// </summary>
        private int KZTAB { get; set; } = 0;

        /// <summary>
        /// Jahreslohnsteuer in EURO
        /// </summary>
        private decimal LSTJAHR { get; set; } = 0m;

        /// <summary>
        /// Zwischenfelder der Jahreslohnsteuer in Cent
        /// </summary>
        private decimal LST1 { get; set; } = 0m;

        private decimal LST2 { get; set; } = 0m;

        private decimal LST3 { get; set; } = 0m;

        private decimal LSTOSO { get; set; } = 0m;

        private decimal LSTSO { get; set; } = 0m;

        /// <summary>
        /// Mindeststeuer fuer die Steuerklassen V und VI in EURO
        /// </summary>
        private decimal MIST { get; set; } = 0m;

        /// <summary>
        /// Beitragssatz des Arbeitgebers zur Pflegeversicherung
        /// </summary>
        private decimal PVSATZAG { get; set; } = 0m;

        /// <summary>
        /// Beitragssatz des Arbeitnehmers zur Pflegeversicherung
        /// </summary>
        private decimal PVSATZAN { get; set; } = 0m;

        /// <summary>
        /// Beitragssatz des Arbeitnehmers in der allgemeinen gesetzlichen Rentenversicherung (4 Dezimalstellen)
        /// </summary>
        private decimal RVSATZAN { get; set; } = 0m;

        /// <summary>
        /// Rechenwert in Gleitkommadarstellung
        /// </summary>
        private decimal RW { get; set; } = 0m;

        /// <summary>
        /// Sonderausgaben-Pauschbetrag in EURO
        /// </summary>
        private decimal SAP { get; set; } = 0m;

        /// <summary>
        /// Freigrenze fuer den Solidaritaetszuschlag in EURO
        /// </summary>
        private decimal SOLZFREI { get; set; } = 0m;

        /// <summary>
        /// Solidaritaetszuschlag auf die Jahreslohnsteuer in EURO, C (2 Dezimalstellen)
        /// </summary>
        private decimal SOLZJ { get; set; } = 0m;

        /// <summary>
        /// Zwischenwert fuer den Solidaritaetszuschlag auf die Jahreslohnsteuer
        /// in EURO, C (2 Dezimalstellen)
        /// </summary>
        private decimal SOLZMIN { get; set; } = 0m;

        /// <summary>
        /// Tarifliche Einkommensteuer in EURO
        /// </summary>
        private decimal ST { get; set; } = 0m;

        /// <summary>
        /// Tarifliche Einkommensteuer auf das 1,25-fache ZX in EURO
        /// </summary>
        private decimal ST1 { get; set; } = 0m;

        /// <summary>
        /// Tarifliche Einkommensteuer auf das 0,75-fache ZX in EURO
        /// </summary>
        private decimal ST2 { get; set; } = 0m;

        /// <summary>
        /// Zwischenfeld zur Ermittlung der Steuer auf Vergütungen für mehrjährige Tätigkeit
        /// </summary>
        private decimal STOVMT { get; set; } = 0m;

        /// <summary>
        /// Teilbetragssatz der Vorsorgepauschale für die Rentenversicherung (2 Dezimalstellen)
        /// </summary>
        private decimal TBSVORV { get; set; } = 0m;

        /// <summary>
        /// Bemessungsgrundlage fuer den Versorgungsfreibetrag in Cents
        /// </summary>
        private decimal VBEZB { get; set; } = 0m;

        /// <summary>
        /// Bemessungsgrundlage für den Versorgungsfreibetrag in Cent für
        /// den sonstigen Bezug
        /// </summary>
        private decimal VBEZBSO { get; set; } = 0m;

        /// <summary>
        /// Hoechstbetrag der Vorsorgepauschale nach Alterseinkuenftegesetz in EURO, C
        /// </summary>
        private decimal VHB { get; set; } = 0m;

        /// <summary>
        /// Vorsorgepauschale in EURO, C (2 Dezimalstellen)
        /// </summary>
        private decimal VSP { get; set; } = 0m;

        /// <summary>
        /// Vorsorgepauschale nach Alterseinkuenftegesetz in EURO, C
        /// </summary>
        private decimal VSPN { get; set; } = 0m;

        /// <summary>
        /// Zwischenwert 1 bei der Berechnung der Vorsorgepauschale nach
        /// dem Alterseinkuenftegesetz in EURO, C (2 Dezimalstellen)
        /// </summary>
        private decimal VSP1 { get; set; } = 0m;

        /// <summary>
        /// Zwischenwert 2 bei der Berechnung der Vorsorgepauschale nach
        /// dem Alterseinkuenftegesetz in EURO, C (2 Dezimalstellen)
        /// </summary>
        private decimal VSP2 { get; set; } = 0m;

        /// <summary>
        /// Vorsorgepauschale mit Teilbeträgen für die gesetzliche Kranken- und
        /// soziale Pflegeversicherung nach fiktiven Beträgen oder ggf. für die
        /// private Basiskrankenversicherung und private Pflege-Pflichtversicherung
        /// in Euro, Cent (2 Dezimalstellen)
        /// </summary>
        private decimal VSP3 { get; set; } = 0m;

        /// <summary>
        /// Erster Grenzwert in Steuerklasse V/VI in Euro
        /// </summary>
        private decimal W1STKL5 { get; set; } = 0m;

        /// <summary>
        /// Zweiter Grenzwert in Steuerklasse V/VI in Euro
        /// </summary>
        private decimal W2STKL5 { get; set; } = 0m;

        /// <summary>
        /// Dritter Grenzwert in Steuerklasse V/VI in Euro
        /// </summary>
        private decimal W3STKL5 { get; set; } = 0m;

        /// <summary>
        /// Hoechstbetrag der Vorsorgepauschale nach § 10c Abs. 2 Nr. 2 EStG in EURO
        /// </summary>
        private decimal VSPMAX1 { get; set; } = 0m;

        /// <summary>
        /// Hoechstbetrag der Vorsorgepauschale nach § 10c Abs. 2 Nr. 3 EStG in EURO
        /// </summary>
        private decimal VSPMAX2 { get; set; } = 0m;

        /// <summary>
        /// Vorsorgepauschale nach § 10c Abs. 2 Satz 2 EStG vor der Hoechstbetragsberechnung
        /// in EURO, C (2 Dezimalstellen)
        /// </summary>
        private decimal VSPO { get; set; } = 0m;

        /// <summary>
        /// Fuer den Abzug nach § 10c Abs. 2 Nrn. 2 und 3 EStG verbleibender
        /// Rest von VSPO in EURO, C (2 Dezimalstellen)
        /// </summary>
        private decimal VSPREST { get; set; } = 0m;

        /// <summary>
        /// Hoechstbetrag der Vorsorgepauschale nach § 10c Abs. 2 Nr. 1 EStG
        /// in EURO, C (2 Dezimalstellen)
        /// </summary>
        private decimal VSPVOR { get; set; } = 0m;

        /// <summary>
        /// Zu versteuerndes Einkommen gem. § 32a Abs. 1 und 2 EStG €, C
        /// (2 Dezimalstellen)
        /// </summary>
        private decimal X { get; set; } = 0m;

        /// <summary>
        /// gem. § 32a Abs. 1 EStG (6 Dezimalstellen)
        /// </summary>
        private decimal Y { get; set; } = 0m;

        /// <summary>
        /// Auf einen Jahreslohn hochgerechnetes RE4 in €, C (2 Dezimalstellen)
        /// nach Abzug der Freibeträge nach § 39 b Abs. 2 Satz 3 und 4.
        /// </summary>
        private decimal ZRE4 { get; set; } = 0m;

        /// <summary>
        /// Auf einen Jahreslohn hochgerechnetes RE4 in €, C (2 Dezimalstellen)
        /// </summary>
        private decimal ZRE4J { get; set; } = 0m;

        /// <summary>
        /// Auf einen Jahreslohn hochgerechnetes RE4 in €, C (2 Dezimalstellen)
        /// nach Abzug des Versorgungsfreibetrags und des Alterentlastungsbetrags
        /// zur Berechnung der Vorsorgepauschale in €, Cent (2 Dezimalstellen)
        /// </summary>
        private decimal ZRE4VP { get; set; } = 0m;

        /// <summary>
        /// Feste Tabellenfreibeträge (ohne Vorsorgepauschale) in €, Cent
        /// (2 Dezimalstellen)
        /// </summary>
        private decimal ZTABFB { get; set; } = 0m;

        /// <summary>
        /// Auf einen Jahreslohn hochgerechnetes (VBEZ abzueglich FVB) in
        /// EURO, C (2 Dezimalstellen)
        /// </summary>
        private decimal ZVBEZ { get; set; } = 0m;

        /// <summary>
        /// Auf einen Jahreslohn hochgerechnetes VBEZ in €, C (2 Dezimalstellen)
        /// </summary>
        private decimal ZVBEZJ { get; set; } = 0m;

        /// <summary>
        /// Zu versteuerndes Einkommen in €, C (2 Dezimalstellen)
        /// </summary>
        private decimal ZVE { get; set; } = 0m;

        /// <summary>
        /// Zwischenfelder zu X fuer die Berechnung der Steuer nach § 39b
        /// Abs. 2 Satz 7 EStG in €
        /// </summary>
        private decimal ZX { get; set; } = 0m;

        private decimal ZZX { get; set; } = 0m;

        private decimal HOCH { get; set; } = 0m;

        private decimal VERGL { get; set; } = 0m;

        /// <summary>
        /// Jahreswert der berücksichtigten Beiträge zur privaten Basis-Krankenversicherung und
        /// privaten Pflege-Pflichtversicherung (ggf. auch die Mindestvorsorgepauschale) in Cent.
        /// </summary>
        private decimal VKV { get; set; } = 0m;

        public void Lohnsteuer()
        {
            MPARA();
            MRE4JL();
            VBEZBSO = 0m;
            KENNVMT = 0;
            MRE4();
            MRE4ABZ();
            MBERECH();
            MSONST();
            MVMT();
        }

        /// <summary>
        /// Zuweisung von Werten für bestimmte Sozialversicherungsparameter  PAP Seite 14
        /// </summary>
        private void MPARA()
        {
            if ((KRV < 2))
            {
                if ((KRV == 0))
                {
                    BBGRV = 74400m;
                }
                else
                {
                    BBGRV = 64800m;
                }

                RVSATZAN = 0.0935m;
                TBSVORV = 0.64m;
            }
            else
            {
            }

            BBGKVPV = 50850m;
            KVSATZAN = ((KVZ / ZAHL100) + 0.07m);
            KVSATZAG = 0.07m;
            if ((PVS == 1))
            {
                PVSATZAN = 0.01675m;
                PVSATZAG = 0.00675m;
            }
            else
            {
                PVSATZAN = 0.01175m;
                PVSATZAG = 0.01175m;
            }

            if ((PVZ == 1))
            {
                PVSATZAN = (PVSATZAN + 0.0025m);
            }

            W1STKL5 = 10070m;
            W2STKL5 = 26832m;
            W3STKL5 = 203557m;
            GFB = 8652m;
            SOLZFREI = 972m;
        }

        /// <summary>
        /// Ermittlung des Jahresarbeitslohns nach § 39 b Abs. 2 Satz 2 EStG, PAP Seite 15
        /// </summary>
        private void MRE4JL()
        {
            if ((LZZ == 1))
            {
                ZRE4J = Floor((RE4 / ZAHL100), 2);
                ZVBEZJ = Floor((VBEZ / ZAHL100), 2);
                JLFREIB = Floor((LZZFREIB / ZAHL100), 2);
                JLHINZU = Floor((LZZHINZU / ZAHL100), 2);
            }
            else
            {
                if ((LZZ == 2))
                {
                    ZRE4J = Floor(((RE4 * ZAHL12) / ZAHL100), 2);
                    ZVBEZJ = Floor(((VBEZ * ZAHL12) / ZAHL100), 2);
                    JLFREIB = Floor(((LZZFREIB * ZAHL12) / ZAHL100), 2);
                    JLHINZU = Floor(((LZZHINZU * ZAHL12) / ZAHL100), 2);
                }
                else
                {
                    if ((LZZ == 3))
                    {
                        ZRE4J = Floor(((RE4 * ZAHL360) / ZAHL700), 2);
                        ZVBEZJ = Floor(((VBEZ * ZAHL360) / ZAHL700), 2);
                        JLFREIB = Floor(((LZZFREIB * ZAHL360) / ZAHL700), 2);
                        JLHINZU = Floor(((LZZHINZU * ZAHL360) / ZAHL700), 2);
                    }
                    else
                    {
                        ZRE4J = Floor(((RE4 * ZAHL360) / ZAHL100), 2);
                        ZVBEZJ = Floor(((VBEZ * ZAHL360) / ZAHL100), 2);
                        JLFREIB = Floor(((LZZFREIB * ZAHL360) / ZAHL100), 2);
                        JLHINZU = Floor(((LZZHINZU * ZAHL360) / ZAHL100), 2);
                    }
                }
            }

            if ((af == 0))
            {
                f = 1;
            }
        }

        /// <summary>
        /// Freibeträge für Versorgungsbezüge, Altersentlastungsbetrag (§ 39b Abs. 2 Satz 3 EStG), PAP Seite 16
        /// </summary>
        private void MRE4()
        {
            if ((CompareTo(ZVBEZJ, 0m) == 0))
            {
                FVBZ = 0m;
                FVB = 0m;
                FVBZSO = 0m;
                FVBSO = 0m;
            }
            else
            {
                if ((VJAHR < 2006))
                {
                    J = 1;
                }
                else
                {
                    if ((VJAHR < 2040))
                    {
                        J = (VJAHR - 2004);
                    }
                    else
                    {
                        J = 36;
                    }
                }

                if ((LZZ == 1))
                {
                    VBEZB = ((VBEZM * ((decimal)ZMVB)) + VBEZS);
                    HFVB = ((TAB2[J] / ZAHL12) * ((decimal)ZMVB));
                    FVBZ = Ceiling(((TAB3[J] / ZAHL12) * ((decimal)ZMVB)), 0);
                }
                else
                {
                    VBEZB = Floor(((VBEZM * ZAHL12) + VBEZS), 2);
                    HFVB = TAB2[J];
                    FVBZ = TAB3[J];
                }

                FVB = Ceiling(((VBEZB * TAB1[J]) / ZAHL100), 2);
                if ((CompareTo(FVB, HFVB) == 1))
                {
                    FVB = HFVB;
                }

                if ((CompareTo(FVB, ZVBEZJ) == 1))
                {
                    FVB = ZVBEZJ;
                }

                FVBSO = Ceiling((FVB + ((VBEZBSO * TAB1[J]) / ZAHL100)), 2);
                if ((CompareTo(FVBSO, TAB2[J]) == 1))
                {
                    FVBSO = TAB2[J];
                }

                HFVBZSO = Floor((((VBEZB + VBEZBSO) / ZAHL100) - FVBSO), 2);
                FVBZSO = Ceiling((FVBZ + (VBEZBSO / ZAHL100)), 0);
                if ((CompareTo(FVBZSO, HFVBZSO) == 1))
                {
                    FVBZSO = Ceiling(HFVBZSO, 0);
                }

                if ((CompareTo(FVBZSO, TAB3[J]) == 1))
                {
                    FVBZSO = TAB3[J];
                }

                HFVBZ = Floor(((VBEZB / ZAHL100) - FVB), 2);
                if ((CompareTo(FVBZ, HFVBZ) == 1))
                {
                    FVBZ = Ceiling(HFVBZ, 0);
                }
            }

            MRE4ALTE();
        }

        /// <summary>
        /// Altersentlastungsbetrag (§ 39b Abs. 2 Satz 3 EStG), PAP Seite 17
        /// </summary>
        private void MRE4ALTE()
        {
            if ((ALTER1 == 0))
            {
                ALTE = 0m;
            }
            else
            {
                if ((AJAHR < 2006))
                {
                    K = 1;
                }
                else
                {
                    if ((AJAHR < 2040))
                    {
                        K = (AJAHR - 2004);
                    }
                    else
                    {
                        K = 36;
                    }
                }

                BMG = (ZRE4J - ZVBEZJ);
                ALTE = Ceiling((BMG * TAB4[K]), 0);
                HBALTE = TAB5[K];
                if ((CompareTo(ALTE, HBALTE) == 1))
                {
                    ALTE = HBALTE;
                }
            }
        }

        /// <summary>
        /// Ermittlung des Jahresarbeitslohns nach Abzug der Freibeträge nach § 39 b Abs. 2 Satz 3 und 4 EStG, PAP Seite 19
        /// </summary>
        private void MRE4ABZ()
        {
            ZRE4 = Floor(((((ZRE4J - FVB) - ALTE) - JLFREIB) + JLHINZU), 2);
            if ((CompareTo(ZRE4, 0m) == (-1)))
            {
                ZRE4 = 0m;
            }

            ZRE4VP = ZRE4J;
            if ((KENNVMT == 2))
            {
                ZRE4VP = Floor((ZRE4VP - (ENTSCH / ZAHL100)), 2);
            }

            ZVBEZ = Floor((ZVBEZJ - FVB), 2);
            if ((CompareTo(ZVBEZ, 0m) == (-1)))
            {
                ZVBEZ = 0m;
            }
        }

        /// <summary>
        /// Berechnung fuer laufende Lohnzahlungszeitraueme Seite 20
        /// </summary>
        private void MBERECH()
        {
            MZTABFB();
            VFRB = Floor(((ANP + (FVB + FVBZ)) * ZAHL100), 0);
            MLSTJAHR();
            WVFRB = Floor(((ZVE - GFB) * ZAHL100), 0);
            if ((CompareTo(WVFRB, 0m) == (-1)))
            {
                WVFRB = 0m;
            }

            LSTJAHR = Floor((ST * ((decimal)f)), 0);
            UPLSTLZZ();
            UPVKVLZZ();
            if ((CompareTo(ZKF, 0m) == 1))
            {
                ZTABFB = (ZTABFB + KFB);
                MRE4ABZ();
                MLSTJAHR();
                JBMG = Floor((ST * ((decimal)f)), 0);
            }
            else
            {
                JBMG = LSTJAHR;
            }

            MSOLZ();
        }

        /// <summary>
        /// Ermittlung der festen Tabellenfreibeträge (ohne Vorsorgepauschale), PAP Seite 21
        /// </summary>
        private void MZTABFB()
        {
            ANP = 0m;
            if (((CompareTo(ZVBEZ, 0m) >= 0) && (CompareTo(ZVBEZ, FVBZ) == (-1))))
            {
                FVBZ = ((decimal)((long)ZVBEZ));
            }

            if ((STKL < 6))
            {
                if ((CompareTo(ZVBEZ, 0m) == 1))
                {
                    if ((CompareTo((ZVBEZ - FVBZ), 102m) == (-1)))
                    {
                        ANP = Ceiling((ZVBEZ - FVBZ), 0);
                    }
                    else
                    {
                        ANP = 102m;
                    }
                }
            }
            else
            {
                FVBZ = 0m;
                FVBZSO = 0m;
            }

            if ((STKL < 6))
            {
                if ((CompareTo(ZRE4, ZVBEZ) == 1))
                {
                    if ((CompareTo((ZRE4 - ZVBEZ), ZAHL1000) == (-1)))
                    {
                        ANP = Ceiling(((ANP + ZRE4) - ZVBEZ), 0);
                    }
                    else
                    {
                        ANP = (ANP + ZAHL1000);
                    }
                }
            }

            KZTAB = 1;
            if ((STKL == 1))
            {
                SAP = 36m;
                KFB = Floor((ZKF * 7248m), 0);
            }
            else
            {
                if ((STKL == 2))
                {
                    EFA = 1908m;
                    SAP = 36m;
                    KFB = Floor((ZKF * 7248m), 0);
                }
                else
                {
                    if ((STKL == 3))
                    {
                        KZTAB = 2;
                        SAP = 36m;
                        KFB = Floor((ZKF * 7248m), 0);
                    }
                    else
                    {
                        if ((STKL == 4))
                        {
                            SAP = 36m;
                            KFB = Floor((ZKF * 3624m), 0);
                        }
                        else
                        {
                            if ((STKL == 5))
                            {
                                SAP = 36m;
                                KFB = 0m;
                            }
                            else
                            {
                                KFB = 0m;
                            }
                        }
                    }
                }
            }

            ZTABFB = Floor((((EFA + ANP) + SAP) + FVBZ), 2);
        }

        /// <summary>
        /// Ermittlung Jahreslohnsteuer, PAP Seite 22
        /// </summary>
        private void MLSTJAHR()
        {
            UPEVP();
            if ((KENNVMT!= 1))
            {
                ZVE = Floor(((ZRE4 - ZTABFB) - VSP), 2);
                UPMLST();
            }
            else
            {
                ZVE = Floor(((((ZRE4 - ZTABFB) - VSP) - (VMT / ZAHL100)) - (VKAPA / ZAHL100)), 2);
                if ((CompareTo(ZVE, 0m) == (-1)))
                {
                    ZVE = Floor((((ZVE + (VMT / ZAHL100)) + (VKAPA / ZAHL100)) / ZAHL5), 2);
                    UPMLST();
                    ST = Floor((ST * ZAHL5), 0);
                }
                else
                {
                    UPMLST();
                    STOVMT = ST;
                    ZVE = Floor((ZVE + ((VMT + VKAPA) / ZAHL500)), 2);
                    UPMLST();
                    ST = Floor((((ST - STOVMT) * ZAHL5) + STOVMT), 0);
                }
            }
        }

        /// <summary>
        /// PAP Seite 23
        /// </summary>
        private void UPVKVLZZ()
        {
            UPVKV();
            JW = VKV;
            UPANTEIL();
            VKVLZZ = ANTEIL1;
        }

        /// <summary>
        /// PAP Seite 23
        /// </summary>
        private void UPVKV()
        {
            if ((PKV > 0))
            {
                if ((CompareTo(VSP2, VSP3) == 1))
                {
                    VKV = (VSP2 * ZAHL100);
                }
                else
                {
                    VKV = (VSP3 * ZAHL100);
                }
            }
            else
            {
                VKV = 0m;
            }
        }

        /// <summary>
        /// Neu 2016
        /// </summary>
        private void UPLSTLZZ()
        {
            JW = (LSTJAHR * ZAHL100);
            UPANTEIL();
            LSTLZZ = ANTEIL1;
        }

        /// <summary>
        /// Ermittlung der Jahreslohnsteuer aus dem Einkommensteuertarif. PAP Seite 25
        /// </summary>
        private void UPMLST()
        {
            if ((CompareTo(ZVE, ZAHL1) == (-1)))
            {
                ZVE = 0m;
                X = 0m;
            }
            else
            {
                X = Floor((ZVE / ((decimal)KZTAB)), 0);
            }

            if ((STKL < 5))
            {
                UPTAB16();
            }
            else
            {
                MST5_6();
            }
        }

        /// <summary>
        /// Vorsorgepauschale (§ 39b Absatz 2 Satz 5 Nummer 3 und Absatz 4 EStG)
        /// Achtung: Es wird davon ausgegangen, dass
        /// a) Es wird davon ausge-gangen, dassa) für die BBG (Ost) 60.000 Euro und für die BBG (West) 71.400 Euro festgelegt wird sowie
        /// b) der Beitragssatz zur Rentenversicherung auf 18,9 % gesenkt wird.
        /// 
        /// PAP Seite 26
        /// </summary>
        private void UPEVP()
        {
            if ((KRV > 1))
            {
                VSP1 = 0m;
            }
            else
            {
                if ((CompareTo(ZRE4VP, BBGRV) == 1))
                {
                    ZRE4VP = BBGRV;
                }

                VSP1 = Floor((TBSVORV * ZRE4VP), 2);
                VSP1 = Floor((VSP1 * RVSATZAN), 2);
            }

            VSP2 = Floor((ZRE4VP * 0.12m), 2);
            if ((STKL == 3))
            {
                VHB = 3000m;
            }
            else
            {
                VHB = 1900m;
            }

            if ((CompareTo(VSP2, VHB) == 1))
            {
                VSP2 = VHB;
            }

            VSPN = Ceiling((VSP1 + VSP2), 0);
            MVSP();
            if ((CompareTo(VSPN, VSP) == 1))
            {
                VSP = Floor(VSPN, 2);
            }
        }

        /// <summary>
        /// Vorsorgepauschale (§39b Abs. 2 Satz 5 Nr 3 EStG) Vergleichsberechnung fuer Guenstigerpruefung, PAP Seite 27
        /// </summary>
        private void MVSP()
        {
            if ((CompareTo(ZRE4VP, BBGKVPV) == 1))
            {
                ZRE4VP = BBGKVPV;
            }

            if ((PKV > 0))
            {
                if ((STKL == 6))
                {
                    VSP3 = 0m;
                }
                else
                {
                    VSP3 = ((PKPV * ZAHL12) / ZAHL100);
                    if ((PKV == 2))
                    {
                        VSP3 = Floor((VSP3 - (ZRE4VP * (KVSATZAG + PVSATZAG))), 2);
                    }
                }
            }
            else
            {
                VSP3 = Floor((ZRE4VP * (KVSATZAN + PVSATZAN)), 2);
            }

            VSP = Ceiling((VSP3 + VSP1), 0);
        }

        /// <summary>
        /// Lohnsteuer fuer die Steuerklassen V und VI (§ 39b Abs. 2 Satz 7 EStG), PAP Seite 28
        /// </summary>
        private void MST5_6()
        {
            ZZX = X;
            if ((CompareTo(ZZX, W2STKL5) == 1))
            {
                ZX = W2STKL5;
                UP5_6();
                if ((CompareTo(ZZX, W3STKL5) == 1))
                {
                    ST = Floor((ST + ((W3STKL5 - W2STKL5) * 0.42m)), 0);
                    ST = Floor((ST + ((ZZX - W3STKL5) * 0.45m)), 0);
                }
                else
                {
                    ST = Floor((ST + ((ZZX - W2STKL5) * 0.42m)), 0);
                }
            }
            else
            {
                ZX = ZZX;
                UP5_6();
                if ((CompareTo(ZZX, W1STKL5) == 1))
                {
                    VERGL = ST;
                    ZX = W1STKL5;
                    UP5_6();
                    HOCH = Floor((ST + ((ZZX - W1STKL5) * 0.42m)), 0);
                    if ((CompareTo(HOCH, VERGL) == (-1)))
                    {
                        ST = HOCH;
                    }
                    else
                    {
                        ST = VERGL;
                    }
                }
            }
        }

        /// <summary>
        /// Unterprogramm zur Lohnsteuer fuer die Steuerklassen V und VI (§ 39b Abs. 2 Satz 7 EStG), PAP Seite 29
        /// </summary>
        private void UP5_6()
        {
            X = Floor((ZX * 1.25m), 2);
            UPTAB16();
            ST1 = ST;
            X = Floor((ZX * 0.75m), 2);
            UPTAB16();
            ST2 = ST;
            DIFF = ((ST1 - ST2) * ZAHL2);
            MIST = Floor((ZX * 0.14m), 0);
            if ((CompareTo(MIST, DIFF) == 1))
            {
                ST = MIST;
            }
            else
            {
                ST = DIFF;
            }
        }

        /// <summary>
        /// Solidaritaetszuschlag, PAP Seite 30
        /// </summary>
        private void MSOLZ()
        {
            SOLZFREI = (SOLZFREI * ((decimal)KZTAB));
            if ((CompareTo(JBMG, SOLZFREI) == 1))
            {
                SOLZJ = Floor(((JBMG * 5.5m) / ZAHL100), 2);
                SOLZMIN = Floor((((JBMG - SOLZFREI) * 20m) / ZAHL100), 2);
                if ((CompareTo(SOLZMIN, SOLZJ) == (-1)))
                {
                    SOLZJ = SOLZMIN;
                }

                JW = Floor((SOLZJ * ZAHL100), 0);
                UPANTEIL();
                SOLZLZZ = ANTEIL1;
            }
            else
            {
                SOLZLZZ = 0m;
            }

            if ((R > 0))
            {
                JW = (JBMG * ZAHL100);
                UPANTEIL();
                BK = ANTEIL1;
            }
            else
            {
                BK = 0m;
            }
        }

        /// <summary>
        /// Anteil von Jahresbetraegen fuer einen LZZ (§ 39b Abs. 2 Satz 9 EStG), PAP Seite 31
        /// </summary>
        private void UPANTEIL()
        {
            if ((LZZ == 1))
            {
                ANTEIL1 = JW;
            }
            else
            {
                if ((LZZ == 2))
                {
                    ANTEIL1 = Floor((JW / ZAHL12), 0);
                }
                else
                {
                    if ((LZZ == 3))
                    {
                        ANTEIL1 = Floor(((JW * ZAHL7) / ZAHL360), 0);
                    }
                    else
                    {
                        ANTEIL1 = Floor((JW / ZAHL360), 0);
                    }
                }
            }
        }

        /// <summary>
        /// Berechnung sonstiger Bezuege nach § 39b Abs. 3 Saetze 1 bis 8 EStG), PAP Seite 32
        /// </summary>
        private void MSONST()
        {
            LZZ = 1;
            if ((ZMVB == 0))
            {
                ZMVB = 12;
            }

            if ((CompareTo(SONSTB, 0m) == 0))
            {
                VKVSONST = 0m;
                LSTSO = 0m;
                STS = 0m;
                SOLZS = 0m;
                BKS = 0m;
            }
            else
            {
                MOSONST();
                UPVKV();
                VKVSONST = VKV;
                ZRE4J = Floor(((JRE4 + SONSTB) / ZAHL100), 2);
                ZVBEZJ = Floor(((JVBEZ + VBS) / ZAHL100), 2);
                VBEZBSO = STERBE;
                MRE4SONST();
                MLSTJAHR();
                WVFRBM = Floor(((ZVE - GFB) * ZAHL100), 2);
                if ((CompareTo(WVFRBM, 0m) == (-1)))
                {
                    WVFRBM = 0m;
                }

                UPVKV();
                VKVSONST = (VKV - VKVSONST);
                LSTSO = (ST * ZAHL100);
                STS = (Floor((((LSTSO - LSTOSO) * ((decimal)f)) / ZAHL100), 0) * ZAHL100);
                if ((CompareTo(STS, 0m) == (-1)))
                {
                    STS = 0m;
                }

                SOLZS = Floor(((STS * 5.5m) / ZAHL100), 0);
                if ((R > 0))
                {
                    BKS = STS;
                }
                else
                {
                    BKS = 0m;
                }
            }
        }

        /// <summary>
        /// Berechnung der Verguetung fuer mehrjaehrige Taetigkeit nach § 39b Abs. 3 Satz 9 und 10 EStG), PAP Seite 33
        /// </summary>
        private void MVMT()
        {
            if ((CompareTo(VKAPA, 0m) == (-1)))
            {
                VKAPA = 0m;
            }

            if ((CompareTo((VMT + VKAPA), 0m) == 1))
            {
                if ((CompareTo(LSTSO, 0m) == 0))
                {
                    MOSONST();
                    LST1 = LSTOSO;
                }
                else
                {
                    LST1 = LSTSO;
                }

                VBEZBSO = (STERBE + VKAPA);
                ZRE4J = Floor(((((JRE4 + SONSTB) + VMT) + VKAPA) / ZAHL100), 2);
                ZVBEZJ = Floor((((JVBEZ + VBS) + VKAPA) / ZAHL100), 2);
                KENNVMT = 2;
                MRE4SONST();
                MLSTJAHR();
                LST3 = (ST * ZAHL100);
                MRE4ABZ();
                ZRE4VP = ((ZRE4VP - (JRE4ENT / ZAHL100)) - (SONSTENT / ZAHL100));
                KENNVMT = 1;
                MLSTJAHR();
                LST2 = (ST * ZAHL100);
                STV = (LST2 - LST1);
                LST3 = (LST3 - LST1);
                if ((CompareTo(LST3, STV) == (-1)))
                {
                    STV = LST3;
                }

                if ((CompareTo(STV, 0m) == (-1)))
                {
                    STV = 0m;
                }
                else
                {
                    STV = (Floor(((STV * ((decimal)f)) / ZAHL100), 0) * ZAHL100);
                }

                SOLZV = Floor(((STV * 5.5m) / ZAHL100), 0);
                if ((R > 0))
                {
                    BKV = STV;
                }
                else
                {
                    BKV = 0m;
                }
            }
            else
            {
                STV = 0m;
                SOLZV = 0m;
                BKV = 0m;
            }
        }

        /// <summary>
        /// Sonderberechnung ohne sonstige Bezüge für Berechnung bei sonstigen Bezügen oder Vergütung für mehrjährige Tätigkeit, PAP Seite 34
        /// </summary>
        private void MOSONST()
        {
            ZRE4J = Floor((JRE4 / ZAHL100), 2);
            ZVBEZJ = Floor((JVBEZ / ZAHL100), 2);
            JLFREIB = Floor((JFREIB / ZAHL100), 2);
            JLHINZU = Floor((JHINZU / ZAHL100), 2);
            MRE4();
            MRE4ABZ();
            ZRE4VP = (ZRE4VP - (JRE4ENT / ZAHL100));
            MZTABFB();
            VFRBS1 = Floor(((ANP + (FVB + FVBZ)) * ZAHL100), 2);
            MLSTJAHR();
            WVFRBO = Floor(((ZVE - GFB) * ZAHL100), 2);
            if ((CompareTo(WVFRBO, 0m) == (-1)))
            {
                WVFRBO = 0m;
            }

            LSTOSO = (ST * ZAHL100);
        }

        /// <summary>
        /// Sonderberechnung mit sonstige Bezüge für Berechnung bei sonstigen Bezügen oder Vergütung für mehrjährige Tätigkeit, PAP Seite 35
        /// </summary>
        private void MRE4SONST()
        {
            MRE4();
            FVB = FVBSO;
            MRE4ABZ();
            ZRE4VP = ((ZRE4VP - (JRE4ENT / ZAHL100)) - (SONSTENT / ZAHL100));
            FVBZ = FVBZSO;
            MZTABFB();
            VFRBS2 = ((((ANP + FVB) + FVBZ) * ZAHL100) - VFRBS1);
        }

        /// <summary>
        /// Tarifliche Einkommensteuer §32a EStG, PAP Seite 36
        /// </summary>
        private void UPTAB16()
        {
            if ((CompareTo(X, (GFB + ZAHL1)) == (-1)))
            {
                ST = 0m;
            }
            else
            {
                if ((CompareTo(X, 13670m) == (-1)))
                {
                    Y = Floor(((X - GFB) / ZAHL10000), 6);
                    RW = (Y * 993.62m);
                    RW = (RW + 1400m);
                    ST = Floor((RW * Y), 0);
                }
                else
                {
                    if ((CompareTo(X, 53666m) == (-1)))
                    {
                        Y = Floor(((X - 13669m) / ZAHL10000), 6);
                        RW = (Y * 225.40m);
                        RW = (RW + 2397m);
                        RW = (RW * Y);
                        ST = Floor((RW + 952.48m), 0);
                    }
                    else
                    {
                        if ((CompareTo(X, 254447m) == (-1)))
                        {
                            ST = Floor(((X * 0.42m) - 8394.14m), 0);
                        }
                        else
                        {
                            ST = Floor(((X * 0.45m) - 16027.52m), 0);
                        }
                    }
                }
            }

            ST = (ST * ((decimal)KZTAB));
        }

        #region Berechnungs-Helper

        // Rundungsfaktoren für Rundung auf Nachkommastellen
        private static readonly decimal[] RoundingFactorsPositive = Enumerable.Range(0, 7).Select(x =>
        {
            var f = 1m;
            while (x-- != 0)
                f *= 10m;
            return f;
        }).ToArray();

        // Rundungsfaktoren für Rundung auf Vorkommastellen
        private static readonly decimal[] RoundingFactorsNegative = Enumerable.Range(0, 3).Select(x =>
        {
            var f = 1m;
            while (x-- != 0)
                f /= 10m;
            return f;
        }).ToArray();

        private static decimal Calc(decimal value, int decimals, Func<decimal, decimal> fn)
        {
            var factor = decimals >= 0 ? RoundingFactorsPositive[decimals] : RoundingFactorsNegative[-decimals];
            value *= factor;
            value = fn(value);
            value /= factor;
            return value;
        }

        private static decimal Ceiling(decimal value, int decimals)
        {
            return Calc(value, decimals, decimal.Ceiling);
        }

        private static decimal Floor(decimal value, int decimals)
        {
            return Calc(value, decimals, decimal.Floor);
        }

        private static int CompareTo<T>(T left, T right) where T : IComparable<T>
        {
            int result = left.CompareTo(right);
            if (result < 0)
                return -1;
            if (result > 0)
                return 1;
            return 0;
        }

        #endregion
    }
}