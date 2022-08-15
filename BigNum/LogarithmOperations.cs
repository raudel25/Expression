namespace BigNum;

public static class LogarithmOperations
{
    private static int _precision = 100;

    internal static RealNumbers Ln(RealNumbers x)
    {
        if (x == BigNumMath.E) return RealNumbers.Real1;

        bool positive = x.Abs > RealNumbers.Real1;
        x = positive ? RealNumbers.Real1 / x : x;
        x = RealNumbers.Real1 - x;

        RealNumbers pow = x;
        RealNumbers index = RealNumbers.Real1;
        RealNumbers ln = RealNumbers.Real0;

        for (int i = 1; i <= _precision; i++)
        {
            ln += pow / index;
            pow *= x;
            index++;
        }

        return positive ? ln : -ln;
    }

    internal static RealNumbers Log(RealNumbers x, RealNumbers y)
    {
        RealNumbers pow = RealNumbers.Real1;
        int index = 0;

        while (pow <= y)
        {
            if (pow == y) return BigNumMath.ConvertToRealNumbers(index);
            pow *= x;
            index++;
        }

        return Ln(y) / Ln(x);
    }
}