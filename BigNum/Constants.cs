namespace BigNum;

internal static class Constants
{
    private static int _precisionE = 40;

    /// <summary>
    /// Aproximacion de E
    /// </summary>
    /// <returns>Numero E</returns>
    internal static RealNumbers ConstantE()
    {
        RealNumbers e = RealNumbers.Real0;
        RealNumbers fact = RealNumbers.Real1;
        RealNumbers index = RealNumbers.Real0;

        //Formula de taylor e^x
        //https://es.wikipedia.org/wiki/Serie_de_Taylor
        for (int i = 0; i < _precisionE; i++)
        {
            if (i != 0) fact *= index;
            e += RealNumbers.Real1 / fact;
            index++;
        }

        return e;
    }

    /// <summary>
    /// Aproximacion de PI
    /// </summary>
    /// <returns>Resultado Real</returns>
    internal static RealNumbers ConstantPI() => BigNumMath.Asin(new RealNumbers("0.5")) * new RealNumbers("6.0");
}