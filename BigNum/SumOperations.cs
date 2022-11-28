namespace BigNum;

internal static class SumOperations
{
    /// <summary>
    /// Sumar dos numeros reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Sum(RealNumbers x, RealNumbers y)
    {
        if (x.Precision != y.Precision) throw new Exception("Numeros de representacion diferente");
        if (x == RealNumbers.Real0) return y;
        if (y == RealNumbers.Real0) return x;

        if (x.Sign == y.Sign)
            return new RealNumbers(Sum(x.NumberValue, y.NumberValue, x.Base10), x.Positive(), x.Precision);

        int compare = x.Abs.CompareTo(y.Abs);
        if (compare == 0) return RealNumbers.Real0;
        if (compare == 1)
            return new RealNumbers(Subtraction(x.NumberValue, y.NumberValue, x.Base10), x.Positive(), x.Precision);

        return new RealNumbers(Subtraction(x.NumberValue, y.NumberValue, x.Base10), y.Positive(), x.Precision);
    }

    /// <summary>
    /// Sumar dos cadenas
    /// </summary>
    /// <param name="x">Cadena</param>
    /// <param name="y">Cadena</param>
    /// <param name="base10">Base</param>
    /// <returns>Resultado</returns>
    internal static List<long> Sum(List<long> x, List<long> y, long base10)
    {
        (x, y) = AuxOperations.EqualZerosLeftValue(x, y);
        bool drag = false;
        int len = x.Count;
        List<long> sum = new List<long>(len);

        for (int i = 0; i < len; i++)
        {
            long n = x[i] + y[i];

            n = drag ? n + 1 : n;
            drag = n >= base10;
            sum.Add(n % base10);
        }

        if (drag) sum.Add(1);

        return sum;
    }

    /// <summary>
    /// Restar dos cadenas
    /// </summary>
    /// <param name="x">Cadena</param>
    /// <param name="y">Cadena</param>
    /// <param name="base10">Base</param>
    /// <returns>Resultado</returns>
    internal static List<long> Subtraction(List<long> x, List<long> y, long base10)
    {
        (x, y) = AuxOperations.EqualZerosLeftValue(x, y);
        bool drag = false;
        int len = x.Count;
        List<long> sub = new List<long>(len);

        for (int i = 0; i < len; i++)
        {
            long n = x[i] - y[i];

            n = drag ? n - 1 : n;
            drag = n < 0;
            n = n < 0 ? n + base10 : n;
            sub.Add(n);
        }

        return sub;
    }
}