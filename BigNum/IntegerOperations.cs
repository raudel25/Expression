namespace BigNum;

internal static class IntegerOperations
{
    internal static IntegerNumbers Mcd(IntegerNumbers x, IntegerNumbers y)
    {
        if (x == IntegerNumbers.Integer0 || y == IntegerNumbers.Integer0)
            throw new Exception("Operacion Invalida (division por 0)");

        (IntegerNumbers a, IntegerNumbers b) = (BigNumMath.Max(x, y), BigNumMath.Min(x, y));
        IntegerNumbers rest = IntegerNumbers.Integer1;

        while (rest != IntegerNumbers.Integer0)
        {
            rest = a % b;
            (a, b) = (b, rest);
        }

        return a;
    }

    internal static IntegerNumbers Factorial(IntegerNumbers x)
    {
        IntegerNumbers fact = new IntegerNumbers("1");
        int xx = int.Parse(x.PartNumber);
        IntegerNumbers index = IntegerNumbers.Integer1;

        for (int i = 1; i <= xx; i++) fact *= index++;

        return fact;
    }

    internal static IntegerNumbers Combinations(IntegerNumbers x, IntegerNumbers y)
    {
        IntegerNumbers min = BigNumMath.Min(x - y, y);

        IntegerNumbers index = x;
        IntegerNumbers fact = IntegerNumbers.Integer1;
        int minInt = int.Parse(min.PartNumber);

        for (int i = 0; i < minInt; i++) fact *= index--;

        return fact / Factorial(min);
    }
}