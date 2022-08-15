namespace BigNum;

internal static class TrigonometryOperations
{
    internal delegate bool Condition(int x);

    private static int _precisionSinCos = 30;

    private static int _precisionArctan = 100;

    private static int _precisionArcsin = 100;

    internal static RealNumbers SinCos(RealNumbers x,bool sin)
    {
        Condition filter1 = sin ? (a) => (a & 1) == 0 : (a) => (a & 1) != 0;
        Condition filter2 = sin ? (a) => a % 4 == 1 : (a) => a % 4 == 0;
        
        RealNumbers result = RealNumbers.Real0;
        RealNumbers pow = RealNumbers.Real1;
        RealNumbers fact = RealNumbers.Real1;
        RealNumbers index = RealNumbers.Real0;

        for (int i = 0; i < _precisionSinCos; i++)
        {
            if (i != 0) fact *= index;
            if (i != 0) pow *= x;
            index++;
            if (filter1(i)) continue;
            result = filter2(i) ? result + pow / fact : result - pow / fact;
        }

        return result;
    }

    //Deficiente
    internal static RealNumbers Arctan(RealNumbers x)
    {
        RealNumbers pow=x;
        RealNumbers index=RealNumbers.Real1;
        RealNumbers arctan=RealNumbers.Real0;
        RealNumbers xx = BigNumMath.Pow(x, new IntegerNumbers("2"));

        for (int i = 1; i < 2 * _precisionArctan; i += 2)
        {
            arctan = i % 4 == 1 ? arctan + pow / index : arctan - pow / index;
            pow *= xx;
            index+=new RealNumbers("2");
        }

        return arctan;
    }

    //Deficiente
    internal static RealNumbers Arcsin(RealNumbers x)
    {
        RealNumbers index=RealNumbers.Real1;
        RealNumbers even=RealNumbers.Real1;
        RealNumbers odd = RealNumbers.Real1;
        RealNumbers pow=RealNumbers.Real1;
        RealNumbers arcsin=RealNumbers.Real0;

        for (int i = 1; i <= _precisionArcsin; i++)
        {
            pow *= x;
            if ((i & 1) == 0) even *= index;
            else odd *= index;

            if ((i & 1) == 1) arcsin += (odd * pow) / (even * index);

            index++;
        }

        return arcsin;
    }
}