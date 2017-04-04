/*
 * Automatisch generiert mit Dataline.Tax.BmfPapConverter
 * am 04.04.2017 15:19:39
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataline.Tax.LSt201512
{
    public class Eingabeparameter
    {
        /// <summary>
        /// 1, wenn die Anwendung des Faktorverfahrens gewählt wurden (nur in Steuerklasse IV)
        /// </summary>
        public int af { get; set; } = 1;

        /// <summary>
        /// Auf die Vollendung des 64. Lebensjahres folgende
        /// Kalenderjahr (erforderlich, wenn ALTER1=1)
        /// </summary>
        public int AJAHR { get; set; }

        /// <summary>
        /// 1, wenn das 64. Lebensjahr zu Beginn des Kalenderjahres vollendet wurde, in dem
        /// der Lohnzahlungszeitraum endet (§ 24 a EStG), sonst = 0
        /// </summary>
        public int ALTER1 { get; set; }

        /// <summary>
        /// in VKAPA und VMT enthaltene Entschädigungen nach §24 Nummer 1 EStG in Cent
        /// </summary>
        public decimal ENTSCH { get; set; } = 0m;

        /// <summary>
        /// eingetragener Faktor mit drei Nachkommastellen
        /// </summary>
        public double f { get; set; } = 1.0;

        /// <summary>
        /// Jahresfreibetrag nach Maßgabe der Eintragungen auf der
        /// Lohnsteuerkarte in Cents (ggf. 0)
        /// </summary>
        public decimal JFREIB { get; set; }

        /// <summary>
        /// Jahreshinzurechnungsbetrag in Cents (ggf. 0)
        /// </summary>
        public decimal JHINZU { get; set; }

        /// <summary>
        /// Voraussichtlicher Jahresarbeitslohn ohne sonstige Bezüge und ohne Vergütung für mehrjährige Tätigkeit in Cent.
        /// Anmerkung: Die Eingabe dieses Feldes (ggf. 0) ist erforderlich bei Eingabe „sonsti-ger Bezüge“ (Feld SONSTB)
        /// oder bei Eingabe der „Vergütung für mehrjährige Tätigkeit“ (Feld VMT).
        /// Sind in einem vorangegangenen Abrechnungszeitraum bereits sonstige Bezüge gezahlt worden, so sind sie dem
        /// voraussichtlichen Jahresarbeitslohn hinzuzurechnen. Vergütungen für mehrere Jahres aus einem vorangegangenen
        /// Abrechnungszeitraum sind in voller Höhe hinzuzurechnen.
        /// </summary>
        public decimal JRE4 { get; set; }

        /// <summary>
        /// In JRE4 enthaltene Versorgungsbezuege in Cents (ggf. 0)
        /// </summary>
        public decimal JVBEZ { get; set; }

        /// <summary>
        /// Merker für die Vorsorgepauschale
        /// 2 = der Arbeitnehmer ist NICCHT in der gesetzlichen Rentenversicherung versichert.
        /// 
        /// 1 = der Arbeitnehmer ist in der gesetzlichen Rentenversicherung versichert, es gilt die
        /// Beitragsbemessungsgrenze OST.
        /// 
        /// 0 = der Arbeitnehmer ist in der gesetzlichen Rentenversicherung versichert, es gilt die
        /// Beitragsbemessungsgrenze WEST.
        /// </summary>
        public int KRV { get; set; }

        /// <summary>
        /// Einkommensbezogener Zusatzbeitragssatz eines gesetzlich krankenversicherten Arbeitnehmers,
        /// auf dessen Basis der an die Kran-kenkasse zu zahlende Zusatzbeitrag berechnet wird,
        /// in Prozent (bspw. 0,90 für 0,90 %) mit 2 Dezimalstellen.
        /// Der von der Kranken-kasse festgesetzte Zusatzbeitragssatz ist bei Abweichungen unmaßgeblich.
        /// </summary>
        public decimal KVZ { get; set; }

        /// <summary>
        /// Lohnzahlungszeitraum:
        /// 1 = Jahr
        /// 2 = Monat
        /// 3 = Woche
        /// 4 = Tag
        /// </summary>
        public int LZZ { get; set; }

        /// <summary>
        /// In der Lohnsteuerkarte des Arbeitnehmers eingetragener Freibetrag für
        /// den Lohnzahlungszeitraum in Cent
        /// </summary>
        public decimal LZZFREIB { get; set; }

        /// <summary>
        /// In der Lohnsteuerkarte des Arbeitnehmers eingetragener Hinzurechnungsbetrag
        /// für den Lohnzahlungszeitraum in Cent
        /// </summary>
        public decimal LZZHINZU { get; set; }

        /// <summary>
        /// Dem Arbeitgeber mitgeteilte Zahlungen des Arbeitnehmers zur privaten
        /// Kranken- bzw. Pflegeversicherung im Sinne des §10 Abs. 1 Nr. 3 EStG 2010
        /// als Monatsbetrag in Cent (der Wert ist inabhängig vom Lohnzahlungszeitraum immer
        /// als Monatsbetrag anzugeben).
        /// </summary>
        public decimal PKPV { get; set; } = 0m;

        /// <summary>
        /// Krankenversicherung:
        /// 0 = gesetzlich krankenversicherte Arbeitnehmer
        /// 1 = ausschließlich privat krankenversicherte Arbeitnehmer OHNE Arbeitgeberzuschuss
        /// 2 = ausschließlich privat krankenversicherte Arbeitnehmer MIT Arbeitgeberzuschuss
        /// </summary>
        public int PKV { get; set; } = 0;

        /// <summary>
        /// 1, wenn bei der sozialen Pflegeversicherung die Besonderheiten in Sachsen zu berücksichtigen sind bzw.
        /// zu berücksichtigen wären, sonst 0.
        /// </summary>
        public int PVS { get; set; } = 0;

        /// <summary>
        /// 1, wenn er der Arbeitnehmer den Zuschlag zur sozialen Pflegeversicherung
        /// zu zahlen hat, sonst 0.
        /// </summary>
        public int PVZ { get; set; } = 0;

        /// <summary>
        /// Religionsgemeinschaft des Arbeitnehmers lt. Lohnsteuerkarte (bei
        /// keiner Religionszugehoerigkeit = 0)
        /// </summary>
        public int R { get; set; }

        /// <summary>
        /// Steuerpflichtiger Arbeitslohn vor Beruecksichtigung der Freibetraege
        /// fuer Versorgungsbezuege, des Altersentlastungsbetrags und des auf
        /// der Lohnsteuerkarte fuer den Lohnzahlungszeitraum eingetragenen
        /// Freibetrags in Cents.
        /// </summary>
        public decimal RE4 { get; set; }

        /// <summary>
        /// Sonstige Bezuege (ohne Verguetung aus mehrjaehriger Taetigkeit) einschliesslich
        /// Sterbegeld bei Versorgungsbezuegen sowie Kapitalauszahlungen/Abfindungen,
        /// soweit es sich nicht um Bezuege fuer mehrere Jahre handelt in Cents (ggf. 0)
        /// </summary>
        public decimal SONSTB { get; set; }

        /// <summary>
        /// Sterbegeld bei Versorgungsbezuegen sowie Kapitalauszahlungen/Abfindungen,
        /// soweit es sich nicht um Bezuege fuer mehrere Jahre handelt
        /// (in SONSTB enthalten) in Cents
        /// </summary>
        public decimal STERBE { get; set; }

        /// <summary>
        /// Steuerklasse:
        /// 1 = I
        /// 2 = II
        /// 3 = III
        /// 4 = IV
        /// 5 = V
        /// 6 = VI
        /// </summary>
        public int STKL { get; set; }

        /// <summary>
        /// In RE4 enthaltene Versorgungsbezuege in Cents (ggf. 0)
        /// </summary>
        public decimal VBEZ { get; set; }

        /// <summary>
        /// Vorsorgungsbezug im Januar 2005 bzw. fuer den ersten vollen Monat
        /// in Cents
        /// </summary>
        public decimal VBEZM { get; set; }

        /// <summary>
        /// Voraussichtliche Sonderzahlungen im Kalenderjahr des Versorgungsbeginns
        /// bei Versorgungsempfaengern ohne Sterbegeld, Kapitalauszahlungen/Abfindungen
        /// bei Versorgungsbezuegen in Cents
        /// </summary>
        public decimal VBEZS { get; set; }

        /// <summary>
        /// In SONSTB enthaltene Versorgungsbezuege einschliesslich Sterbegeld
        /// in Cents (ggf. 0)
        /// </summary>
        public decimal VBS { get; set; }

        /// <summary>
        /// Jahr, in dem der Versorgungsbezug erstmalig gewaehrt wurde; werden
        /// mehrere Versorgungsbezuege gezahlt, so gilt der aelteste erstmalige Bezug
        /// </summary>
        public int VJAHR { get; set; }

        /// <summary>
        /// Kapitalauszahlungen / Abfindungen / Nachzahlungen bei Versorgungsbezügen
        /// für mehrere Jahre in Cent (ggf. 0)
        /// </summary>
        public decimal VKAPA { get; set; }

        /// <summary>
        /// Vergütung für mehrjährige Tätigkeit ohne Kapitalauszahlungen und ohne Abfindungen
        /// bei Versorgungsbezügen in Cent (ggf. 0)
        /// </summary>
        public decimal VMT { get; set; }

        /// <summary>
        /// Zahl der Freibetraege fuer Kinder (eine Dezimalstelle, nur bei Steuerklassen
        /// I, II, III und IV)
        /// </summary>
        public decimal ZKF { get; set; }

        /// <summary>
        /// Zahl der Monate, fuer die Versorgungsbezuege gezahlt werden (nur
        /// erforderlich bei Jahresberechnung (LZZ = 1)
        /// </summary>
        public int ZMVB { get; set; }

        /// <summary>
        /// In JRE4 enthaltene Entschädigungen nach § 24 Nummer 1 EStG in Cent
        /// </summary>
        public decimal JRE4ENT { get; set; } = 0m;

        /// <summary>
        /// In SONSTB enthaltene Entschädigungen nach § 24 Nummer 1 EStG in Cent
        /// </summary>
        public decimal SONSTENT { get; set; } = 0m;
    }
}