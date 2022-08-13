namespace BigNum;

public static class SqrtOperations
{
    private static int _precision = 50;

    internal static RealNumbers Sqrt(RealNumbers x, IntegerNumbers y)
    {
        if (y == IntegerNumbers.Integer1) return x;
        if (x == RealNumbers.Real0) return RealNumbers.Real0;
        if (y == RealNumbers.Real1) return RealNumbers.Real1;

        bool parity = y % new IntegerNumbers("2") == IntegerNumbers.Integer0;
        bool positive = parity || x.Positive();

        if (parity && !x.Positive()) throw new Exception("Operacion Invalida (el resultado no es real)");

        RealNumbers sqrt = AlgorithmSqrt(x.Abs, BigNumMath.Abs(y));

        return y.Positive()
            ? new RealNumbers(sqrt.PartNumber, sqrt.PartDecimal, positive)
            : RealNumbers.Real1 / new RealNumbers(sqrt.PartNumber, sqrt.PartDecimal, positive);
    }

    private static RealNumbers AlgorithmSqrt(RealNumbers x, IntegerNumbers y)
    {
        (RealNumbers value, bool find) = ApproximateInteger(x, y);
        if (find) return value;

        IntegerNumbers aux = y - IntegerNumbers.Integer1;

        for (int i = 0; i < _precision; i++)
        {
            value = RealNumbers.Real1 / y * (aux * value + x / BigNumMath.Pow(value, aux));
        }

        return value;
    }

    private static (RealNumbers, bool) ApproximateInteger(RealNumbers x, IntegerNumbers y)
    {
        int sqrt = int.Parse(y.PartNumber);
        int cant = x.PartNumber.Length - 1;

        string s = "1";
        s = AuxOperations.AddZerosRight(s, cant / sqrt);
        RealNumbers value = new RealNumbers(s, "0");

        RealNumbers pow = BigNumMath.Pow(value, y);

        while (pow < x)
        {
            value++;
            pow = BigNumMath.Pow(value, y);
        }

        return pow == x ? (value, true) : (value - RealNumbers.Real1, false);
    }
}