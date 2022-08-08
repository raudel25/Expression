namespace BigNum;

internal static class ProductOperations
{
    internal static Numbers Product(Numbers x, Numbers y)
    {
        bool positive = x.Sign == y.Sign;
        int cantDecimal = x.PartDecimal.Length + y.PartDecimal.Length;

        IntegerNumbers m = new IntegerNumbers(x.PartNumber + x.PartDecimal);
        IntegerNumbers n = new IntegerNumbers(y.PartNumber + y.PartDecimal);

        string result = KaratsubaAlgorithm(m, n).PartNumber;

        return new Numbers(result.Substring(0, result.Length - cantDecimal),
            result.Substring(result.Length - cantDecimal, cantDecimal), positive);
    }

    private static IntegerNumbers KaratsubaAlgorithm(IntegerNumbers x, IntegerNumbers y)
    {
        string xValor = x.PartNumber;
        string yValor = y.PartNumber;
        Console.WriteLine(xValor);
        Console.WriteLine(yValor);

        if (xValor == "0" || yValor == "0") return new IntegerNumbers("0");
        if (xValor.Length == 1 && yValor.Length == 1)
            return BigNumMath.ConvertToIntegerNumbers(int.Parse(xValor) * int.Parse(yValor));

        int max = Math.Max(xValor.Length, yValor.Length);

        xValor = AuxOperations.AddZerosLeft(xValor, max - xValor.Length);
        yValor = AuxOperations.AddZerosLeft(yValor, max - yValor.Length);

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
        IntegerNumbers z1 = BigNumMath.NumbersToInteger(BigNumMath.Sum(z11, z12));
        IntegerNumbers z0 = new IntegerNumbers(KaratsubaAlgorithm(x0, y0).PartNumber);

        return BigNumMath.NumbersToInteger(BigNumMath.Sum(BigNumMath.Sum(z2, z1), z0));
    }
}