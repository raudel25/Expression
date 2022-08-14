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
        int cantDecimal = x.PartDecimal.Length + y.PartDecimal.Length;

        if (x.Abs == RealNumbers.Real1) return new RealNumbers(y.PartNumber, y.PartDecimal, positive);
        if (y.Abs == RealNumbers.Real1) return new RealNumbers(x.PartNumber, x.PartDecimal, positive);

        IntegerNumbers m = new IntegerNumbers(x.PartNumber + x.PartDecimal);
        IntegerNumbers n = new IntegerNumbers(y.PartNumber + y.PartDecimal);

        string result = KaratsubaAlgorithm(m, n).PartNumber;

        if (result == "0") return RealNumbers.Real0;

        if (result.Length < cantDecimal) result = AuxOperations.AddZerosLeft(result, cantDecimal - result.Length);

        return new RealNumbers(result.Substring(0, result.Length - cantDecimal),
            result.Substring(result.Length - cantDecimal, cantDecimal), positive);
    }

    /// <summary>
    /// Algoritmo de Karatsuba
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado</returns>
    private static IntegerNumbers KaratsubaAlgorithm(IntegerNumbers x, IntegerNumbers y)
    {
        string xValor = x.PartNumber;
        string yValor = y.PartNumber;

        if (xValor == "0" || yValor == "0") return new IntegerNumbers("0");
        if (xValor == "1") return y;
        if (yValor == "1") return x;
        if (xValor.Length == 1 && yValor.Length == 1)
            return BigNumMath.ConvertToIntegerNumbers(int.Parse(xValor) * int.Parse(yValor));

        AuxOperations.EqualZerosLeft(ref xValor, ref yValor);

        int n = xValor.Length / 2;
        int m = xValor.Length;

        IntegerNumbers x1 = new IntegerNumbers(xValor.Substring(0, n));
        IntegerNumbers x0 = new IntegerNumbers(xValor.Substring(n, xValor.Length - n));
        IntegerNumbers y1 = new IntegerNumbers(yValor.Substring(0, n));
        IntegerNumbers y0 = new IntegerNumbers(yValor.Substring(n, yValor.Length - n));

        IntegerNumbers z2 =
            new IntegerNumbers(AuxOperations.AddZerosRight(KaratsubaAlgorithm(x1, y1).PartNumber, 2 * (m - n)));
        IntegerNumbers z11 =
            new IntegerNumbers(AuxOperations.AddZerosRight(KaratsubaAlgorithm(x1, y0).PartNumber, m - n));
        IntegerNumbers z12 =
            new IntegerNumbers(AuxOperations.AddZerosRight(KaratsubaAlgorithm(y1, x0).PartNumber, m - n));
        IntegerNumbers z1 = z11 + z12;
        IntegerNumbers z0 = new IntegerNumbers(KaratsubaAlgorithm(x0, y0).PartNumber);

        return z2 + z1 + z0;
    }
}