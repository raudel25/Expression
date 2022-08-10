namespace BigNum;

internal static class DivisionOperations
{
    private static int _presition = 1000;

    internal static Numbers Division(Numbers x, Numbers y)
    {
        bool positive = x.Sign == y.Sign;

        if (y.Abs.Equals(new Numbers("1"))) return new Numbers(x.PartNumber, x.PartDecimal, positive);

        (string xPartDecimal, string yPartDecimal) = (x.PartDecimal, y.PartDecimal);
        AuxOperations.EqualZerosRight(ref xPartDecimal, ref yPartDecimal);

        IntegerNumbers m = new IntegerNumbers(x.PartNumber + xPartDecimal);
        IntegerNumbers n = new IntegerNumbers(y.PartNumber + yPartDecimal);

        int cantDecimal = 0;

        string result = DivisionAlgorithm(m, n, ref cantDecimal);

        if (cantDecimal != 0)
            return new Numbers(result.Substring(0, result.Length - cantDecimal),
                result.Substring(result.Length - cantDecimal, cantDecimal), positive);
        return new Numbers(result, positive);
    }

    private static string DivisionAlgorithm(IntegerNumbers x, IntegerNumbers y, ref int cantDecimal)
    {
        string result = "";
        IntegerNumbers rest = new IntegerNumbers("0");

        foreach (var t in x.PartNumber)
        {
            IntegerNumbers div = new IntegerNumbers(rest.PartNumber + t);

            rest = DivisionImmediate(div, y, ref result);
        }

        while (rest != new IntegerNumbers("0"))
        {
            IntegerNumbers div = new IntegerNumbers(rest.PartNumber + "0");

            rest = DivisionImmediate(div, y, ref result);

            cantDecimal++;

            if (cantDecimal == _presition) break;
        }

        return result;
    }

    private static IntegerNumbers DivisionImmediate(IntegerNumbers div, IntegerNumbers divisor, ref string result)
    {
        IntegerNumbers aux = new IntegerNumbers("0");
        for (int j = 9; j >= 0; j--)
        {
            aux = BigNumMath.NumbersToInteger(divisor * BigNumMath.ConvertToNumbers(j));

            if (aux <= div)
            {
                result = result + j;
                break;
            }
        }

        return BigNumMath.NumbersToInteger(div - aux);
    }
}