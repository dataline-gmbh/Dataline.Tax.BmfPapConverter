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