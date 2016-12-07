/// <summary>
/// Berechnet die tarifliche Einkommensteuer.
/// </summary>
/// <param name="x">
/// Zu versteuerndes Einkommen gem. § 32a Abs. 1 und 2 EStG €, C
/// (2 Dezimalstellen)
/// </param>
/// <param name="kztab">
/// Kennzahl fuer die Einkommensteuer-Tabellenart:
/// 1 = Grundtabelle
/// 2 = Splittingtabelle
/// </param>
/// <param name="gfb">
/// Grundfreibetrag in Euro
/// </param>
/// <returns>Die tarifliche Einkommensteuer.</returns>
public decimal TariflicheEinkommensteuer(decimal x, int kztab, decimal gfb)
{
    X = x;
    KZTAB = kztab;
    GFB = gfb;
    UPTAB17();
    return ST;
}