namespace BigNum;

internal static class Constants
{
    private const int PrecisionE = 40;

    /// <summary>
    ///     Aproximacion de E
    /// </summary>
    /// <returns>Numero E</returns>
    internal static RealNumbers ConstantE(RealNumbers real0, RealNumbers real1)
    {
        var e = real0;
        var fact = real1;
        var index = real0;

        //Formula de taylor e^x
        //https://es.wikipedia.org/wiki/Serie_de_Taylor
        for (var i = 0; i < PrecisionE; i++)
        {
            if (i != 0) fact *= index;
            e += real1 / fact;
            index++;
        }

        return e;
    }

    /// <summary>
    ///     Aproximacion de PI
    /// </summary>
    /// <returns>Resultado Real</returns>
    internal static RealNumbers ConstantPI(int precision, long base10, int indBase10)
    {
        return BigNumMath.Asin(new RealNumbers(
                   new long[precision - 1].Concat(new long[] { 5 * base10 / 10, 0 }).ToList(), precision,
                   base10, indBase10)) *
               new RealNumbers(new long[precision].Concat(new long[] { 6 }).ToList(), precision, base10, indBase10);
    }
}