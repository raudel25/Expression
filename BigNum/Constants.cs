namespace BigNum;

internal static class Constants
{
    private static int _precision = 20;

    internal static RealNumbers ConstantE()
    {
        RealNumbers e = RealNumbers.Real0;
        RealNumbers fact = RealNumbers.Real1;
        RealNumbers index = RealNumbers.Real0;

        for (int i = 0; i < _precision; i++)
        {
            if (i != 0) fact *= index;
            e += RealNumbers.Real1 / fact;
            index++;
        }

        return e;
    }
}