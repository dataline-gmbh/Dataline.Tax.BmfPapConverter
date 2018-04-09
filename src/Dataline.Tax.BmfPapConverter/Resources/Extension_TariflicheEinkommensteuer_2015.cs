/// <summary>
/// Tarifliche Einkommensteuer §32a EStG, PAP Seite 32
///
/// Fix DATALINE für 2015: Im Original-PAP existiert nur UPTAB14 (für TE 2014), UPTAB15 wurde ergänzt aus PAP 201512
/// </summary>
private void UPTAB15()
{
    if ((CompareTo(X, 8473m) == (-1)))
    {
        ST = 0m;
    }
    else
    {
        if ((CompareTo(X, 13470m) == (-1)))
        {
            Y = Floor(((X - 8472m) / 10000m), 6);
            RW = (Y * 997.60m);
            RW = (RW + 1400m);
            ST = Floor((RW * Y), 0);
        }
        else
        {
            if ((CompareTo(X, 52882m) == (-1)))
            {
                Y = Floor(((X - 13469m) / 10000m), 6);
                RW = (Y * 228.74m);
                RW = (RW + 2397m);
                RW = (RW * Y);
                ST = Floor((RW + 948.68m), 0);
            }
            else
            {
                if ((CompareTo(X, 250731m) == (-1)))
                {
                    ST = Floor(((X * 0.42m) - 8261.29m), 0);
                }
                else
                {
                    ST = Floor(((X * 0.45m) - 15783.19m), 0);
                }
            }
        }
    }

    ST = (ST * ((decimal)KZTAB));
}

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
    MPARA();
    KZTAB = kztab;
    X = Floor((zve / ((decimal)KZTAB)), 0);
    UPTAB15();
    return ST;
}