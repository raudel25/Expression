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
        if (y == x.Real0) return x.Real1;

        string[] s = y.ToString().Split('.');

        if (s.Length == 1) return Pow(x, int.Parse(y.ToString()));

        (string partNumber, string partDecimal) = (s[0], s[1]);

        int cant = partDecimal.Length;
        IntegerNumbers denominator = new IntegerNumbers(AuxOperations.AddZerosRight("1", cant), x.Base10, x.IndBase10);
        IntegerNumbers numerator = new IntegerNumbers(partNumber + partDecimal, x.Base10, x.IndBase10);
        IntegerNumbers mcd = BigNumMath.Mcd(numerator, denominator);

        RealNumbers pow = SqrtOperations.Sqrt(x, int.Parse((denominator / mcd).ToString()));
        pow = Pow(pow, numerator / mcd);

        return y.Positive() ? pow : y.Real1 / pow;
    }

    /// <summary>
    /// Potencia entre un numero real y uno entero
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="y">Exponente</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Pow(RealNumbers x, int y)
    {
        RealNumbers result = x.Real1;

        for (int i = 0; i < Math.Abs(y); i++) result *= x;

        if (y < 0) result = x.Real1 / result;

        return result;
    }

    /// <summary>
    /// Potencia entre una fraccion real y un numero entero
    /// </summary>
    /// <param name="x">Fraccion real</param>
    /// <param name="pow">Numero entero(C#)</param>
    /// <returns>Resultado fraccion real</returns>
    internal static Fraction Pow(Fraction x, int pow)
    {
        Fraction result = x.Fraction1;

        for (int i = 0; i < Math.Abs(pow); i++) result *= x;

        if (pow < 0) result = new Fraction(result.Denominator, result.Numerator);

        return result;
    }
}