namespace BigNum.Operations;

internal static class ProductOperations
{
    /// <summary>
    ///     Multiplicar dos numeros
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Product(RealNumbers x, RealNumbers y)
    {
        AuxOperations.EqualPrecision(x, y);

        var positive = x.Sign == y.Sign;

        if (x.Abs == x.Real1)
            return new RealNumbers(y.NumberValue, x.Precision, x.Base10, x.IndBase10, positive);
        if (y.Abs == y.Real1)
            return new RealNumbers(x.NumberValue, x.Precision, x.Base10, x.IndBase10, positive);

        var (lx, ly) = AuxOperations.EqualZerosLeftValue(x.NumberValue, y.NumberValue);

        return new RealNumbers(KaratsubaAlgorithm(lx, ly, x.Base10).Skip(x.Precision).ToList(), x.Precision, x.Base10,
            x.IndBase10, positive);
    }

    /// <summary>
    ///     Algoritmo de Karatsuba
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <param name="base10">Base</param>
    /// <returns>Resultado</returns>
    private static List<long> KaratsubaAlgorithm(List<long> x, List<long> y, long base10)
    {
        (x, y) = AuxOperations.EqualZerosLeftValue(x, y);

        if (x.Count == 1) return new List<long> { x[0] * y[0] % base10, x[0] * y[0] / base10 };

        //  Algortimo de Karatsuba
        //  https: // es.wikipedia.org/wiki/Algoritmo_de_Karatsuba

        var n = x.Count / 2;

        var x0 = x.Take(n).ToList();
        var x1 = x.Skip(n).ToList();
        var y0 = y.Take(n).ToList();
        var y1 = y.Skip(n).ToList();

        var z2 = AuxOperations.AddZerosRightValue(KaratsubaAlgorithm(x1, y1, base10), 2 * n);
        var z11 = AuxOperations.AddZerosRightValue(KaratsubaAlgorithm(x1, y0, base10), n);
        var z12 = AuxOperations.AddZerosRightValue(KaratsubaAlgorithm(y1, x0, base10), n);
        var z1 = SumOperations.Sum(z11, z12, base10);
        var z0 = KaratsubaAlgorithm(x0, y0, base10);

        return SumOperations.Sum(z2, SumOperations.Sum(z1, z0, base10), base10);
    }

    internal static List<long> SimpleMultiplication(List<long> x, long y, long base10)
    {
        long drag = 0;
        var result = new List<long>(x.Count);

        foreach (var t in x)
        {
            var n = t * y + drag;
            drag = n / base10;
            result.Add(n % base10);
        }

        if (drag != 0) result.Add(drag);

        return result;
    }
}