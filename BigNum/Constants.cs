namespace BigNum;

internal static class Constants
{
    private static int _precisionE = 20;

    private static int _precisionPI = 10;

    internal static RealNumbers ConstantE()
    {
        RealNumbers e = RealNumbers.Real0;
        RealNumbers fact = RealNumbers.Real1;
        RealNumbers index = RealNumbers.Real0;

        for (int i = 0; i < _precisionE; i++)
        {
            if (i != 0) fact *= index;
            e += RealNumbers.Real1 / fact;
            index++;
        }

        return e;
    }

    internal static RealNumbers ConstantPI()
    {
        throw new NotImplementedException();
    }
}