namespace BigNum;

internal static class PowOperations
{
    /// <summary>
    /// Potencia entre 2 numeros reales
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="y">Exponente</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Pow(RealNumbers x, RealNumbers y)
    {
        if (y == RealNumbers.Real0) return RealNumbers.Real1;

        string[] s = y.ToString().Split('.');
        (string partNumber, string partDecimal) = (s[0], s[1]);

        int cant = partDecimal.Length;
        IntegerNumbers denominator = new IntegerNumbers(AuxOperations.AddZerosRight("1", cant));
        IntegerNumbers numerator = new IntegerNumbers(partNumber + partDecimal);
        IntegerNumbers mcd = BigNumMath.Mcd(numerator, denominator);

        RealNumbers pow = SqrtOperations.Sqrt(x, denominator / mcd);
        pow = Pow(pow, numerator / mcd);

        return y.Positive() ? pow : RealNumbers.Real1 / pow;
    }

    /// <summary>
    /// Potencia entre un numero real y uno entero
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="y">Exponente</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Pow(RealNumbers x, IntegerNumbers y)
    {
        RealNumbers result = RealNumbers.Real1;
        int pow = int.Parse(y.ToString());

        for (int i = 0; i < Math.Abs(pow); i++) result *= x;

        if (pow < 0) result = RealNumbers.Real1 / result;

        return result;
    }

    /// <summary>
    /// Potencia entre una fraccion real y un numero entero
    /// </summary>
    /// <param name="x">Fraccion real</param>
    /// <param name="pow">Numero entero(C#)</param>
    /// <returns>Resultado fraccion real</returns>
    internal static Fraction Pow(Fraction x, IntegerNumbers pow)
    {
        int powInt = int.Parse(pow.ToString());

        Fraction result = new Fraction(RealNumbers.Real1, RealNumbers.Real1);

        for (int i = 0; i < Math.Abs(powInt); i++) result *= x;

        if (powInt < 0) result = new Fraction(result.Denominator, result.Numerator);

        return result;
    }
}