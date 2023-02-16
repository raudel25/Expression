namespace BigNum.Operations;

internal static class SumOperations
{
    /// <summary>
    ///     Sumar dos numeros reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Sum(RealNumbers x, RealNumbers y)
    {
        AuxOperations.EqualPrecision(x, y);

        if (x == x.Real0) return y;
        if (y == y.Real0) return x;

        if (x.Sign == y.Sign)
            return new RealNumbers(Sum(x.NumberValue, y.NumberValue, x.Base10), x.Precision, x.Base10, x.IndBase10,
                x.Positive());

        var compare = x.Abs.CompareTo(y.Abs);
        if (compare == 0) return x.Real0;
        if (compare == 1)
            return new RealNumbers(Subtraction(x.NumberValue, y.NumberValue, x.Base10), x.Precision, x.Base10,
                x.IndBase10, x.Positive());

        return new RealNumbers(Subtraction(y.NumberValue, x.NumberValue, x.Base10), x.Precision, x.Base10, x.IndBase10,
            y.Positive());
    }

    /// <summary>
    ///     Sumar dos cadenas
    /// </summary>
    /// <param name="x">Cadena</param>
    /// <param name="y">Cadena</param>
    /// <param name="base10">Base</param>
    /// <returns>Resultado</returns>
    internal static List<long> Sum(List<long> x, List<long> y, long base10)
    {
        (x, y) = AuxOperations.EqualZerosLeftValue(x, y);
        var drag = false;
        var len = x.Count;
        var sum = new List<long>(len);

        for (var i = 0; i < len; i++)
        {
            var n = x[i] + y[i];

            n = drag ? n + 1 : n;
            drag = n >= base10;
            sum.Add(n % base10);
        }

        if (drag) sum.Add(1);

        return sum;
    }

    /// <summary>
    ///     Restar dos cadenas
    /// </summary>
    /// <param name="x">Cadena</param>
    /// <param name="y">Cadena</param>
    /// <param name="base10">Base</param>
    /// <returns>Resultado</returns>
    internal static List<long> Subtraction(List<long> x, List<long> y, long base10)
    {
        (x, y) = AuxOperations.EqualZerosLeftValue(x, y);
        var drag = false;
        var len = x.Count;
        var sub = new List<long>(len);

        for (var i = 0; i < len; i++)
        {
            var n = x[i] - y[i];

            n = drag ? n - 1 : n;
            drag = n < 0;
            n = n < 0 ? n + base10 : n;
            sub.Add(n);
        }

        return sub;
    }
}