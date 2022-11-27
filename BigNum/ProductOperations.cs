namespace BigNum;

internal static class ProductOperations
{
    /// <summary>
    /// Multiplicar dos numeros
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Product(RealNumbers x, RealNumbers y)
    {
        bool positive = x.Sign == y.Sign;

        if (x.Abs == RealNumbers.Real1) return new RealNumbers(y.NumberValue, positive, x.Precision);
        if (y.Abs == RealNumbers.Real1) return new RealNumbers(x.NumberValue, positive, x.Precision);

        (List<long> lx, List<long> ly) = AuxOperations.EqualZerosLeftValue(x.NumberValue, y.NumberValue);

        return new RealNumbers(KaratsubaAlgorithm(lx, ly, x.Base10).Skip(x.Precision).ToList(), positive, x.Precision);
    }

    /// <summary>
    /// Algoritmo de Karatsuba
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <param name="base10">Base</param>
    /// <returns>Resultado</returns>
    private static List<long> KaratsubaAlgorithm(List<long> x, List<long> y, long base10)
    {
        (x, y) = AuxOperations.EqualZerosLeftValue(x, y);

        if (x.Count == 1) return new List<long>() { x[0] * y[0] % base10, x[0] * y[0] / base10 };

        //  Algortimo de Karatsuba
        //  https: // es.wikipedia.org/wiki/Algoritmo_de_Karatsuba

        int n = x.Count / 2;

        List<long> x0 = x.Take(n).ToList();
        List<long> x1 = x.Skip(n).ToList();
        List<long> y0 = y.Take(n).ToList();
        List<long> y1 = y.Skip(n).ToList();

        List<long> z2 = AuxOperations.AddZerosRightValue(KaratsubaAlgorithm(x1, y1, base10), 2 * n);
        List<long> z11 = AuxOperations.AddZerosRightValue(KaratsubaAlgorithm(x1, y0, base10), n);
        List<long> z12 = AuxOperations.AddZerosRightValue(KaratsubaAlgorithm(y1, x0, base10), n);
        List<long> z1 = SumOperations.Sum(z11, z12, base10);
        List<long> z0 = KaratsubaAlgorithm(x0, y0, base10);

        return SumOperations.Sum(z2, SumOperations.Sum(z1, z0, base10), base10);
    }

    internal static List<long> SimpleMultiplication(List<long> x, long y, long base10)
    {
        long drag = 0;
        List<long> result = new List<long>(x.Count);

        for (int i = 0; i < x.Count; i++)
        {
            long n = x[i] * y + drag;
            drag = n / base10;
            result.Add(n % base10);
        }

        if (drag != 0) result.Add(drag);

        return result;
    }
}