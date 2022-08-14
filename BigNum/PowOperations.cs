namespace BigNum;

internal static class PowOperations
{
    internal static RealNumbers Pow(RealNumbers x, RealNumbers y)
    {
        int cant = y.PartDecimal.Length;
        IntegerNumbers denominator = new IntegerNumbers(AuxOperations.AddZerosRight("1", cant));
        IntegerNumbers numerator = new IntegerNumbers(y.PartNumber + y.PartDecimal);
        IntegerNumbers mcd = BigNumMath.Mcd(numerator, denominator);

        RealNumbers pow = SqrtOperations.Sqrt(x, denominator / mcd);
        pow = Pow(pow, numerator / mcd);

        return y.Positive() ? pow : RealNumbers.Real1 / pow;
    }

    internal static RealNumbers Pow(RealNumbers x, IntegerNumbers y)
    {
        RealNumbers result = RealNumbers.Real1;
        int pow = int.Parse(y.PartNumber);

        for (int i = 0; i < Math.Abs(pow); i++) result *= x;

        if (pow < 0) result = RealNumbers.Real1 / result;

        return result;
    }

    /// <summary>
    /// Potencia entre una fraccion real y un numero entero(C#)
    /// </summary>
    /// <param name="x">Fraccion real</param>
    /// <param name="pow">Numero entero(C#)</param>
    /// <returns>Resultado fraccion real</returns>
    internal static Fraction Pow(Fraction x, int pow)
    {
        Fraction result = new Fraction(RealNumbers.Real1, RealNumbers.Real1);

        for (int i = 0; i < Math.Abs(pow); i++) result *= x;

        if (pow < 0) result = new Fraction(result.Denominator, result.Numerator);

        return result;
    }
}