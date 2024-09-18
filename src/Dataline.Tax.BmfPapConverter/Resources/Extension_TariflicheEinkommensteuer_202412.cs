/// <summary>
/// Berechnet die tarifliche Einkommensteuer.
/// </summary>
/// <param name="zve">
/// Zu versteuerndes Einkommen gem. § 32a Abs. 1 und 2 EStG €, C
/// (2 Dezimalstellen)
/// </param>
/// <param name="kztab">
/// Kennzahl fuer die Einkommensteuer-Tabellenart:
/// 1 = Grundtabelle
/// 2 = Splittingtabelle
/// </param>
/// <returns>Die tarifliche Einkommensteuer.</returns>
public decimal TariflicheEinkommensteuer(decimal zve, int kztab)
{
    KZTAB = kztab;
    X = Floor((zve / ((decimal)KZTAB)), 0);
    UPTAB24A();
    return ST;
}