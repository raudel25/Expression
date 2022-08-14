namespace BigNum;

internal static class TrigonometryOperations
{
    internal delegate bool Condition(int x);

    private static int _precision = 30;

    internal static RealNumbers SinCos(RealNumbers x, Condition filter1, Condition filter2)
    {
        RealNumbers result = RealNumbers.Real0;
        RealNumbers pow = RealNumbers.Real1;
        RealNumbers fact = RealNumbers.Real1;
        RealNumbers index = RealNumbers.Real0;

        for (int i = 0; i < _precision; i++)
        {
            if (i != 0) fact *= index;
            if (i != 0) pow *= x;
            index++;
            if (filter1(i)) continue;
            result = filter2(i) ? result + pow / fact : result - pow / fact;
        }

        return result;
    }
}