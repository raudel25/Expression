namespace BigNum;

internal static class DivisionOperations
{
    private static int _precision = 100;

    internal static (Numbers, IntegerNumbers) Division(Numbers x, Numbers y, bool integer = false)
    {
        bool positive = x.Sign == y.Sign;

        if (y.Abs.Equals(new Numbers("1")))
            return (new Numbers(x.PartNumber, x.PartDecimal, positive), new IntegerNumbers("0"));

        (string xPartDecimal, string yPartDecimal) = (x.PartDecimal, y.PartDecimal);
        AuxOperations.EqualZerosRight(ref xPartDecimal, ref yPartDecimal);
        if (integer) (xPartDecimal, yPartDecimal) = ("", "");

        IntegerNumbers m = new IntegerNumbers(x.PartNumber + xPartDecimal);
        IntegerNumbers n = new IntegerNumbers(y.PartNumber + yPartDecimal);

        int cantDecimal = 0;

        (string result, IntegerNumbers rest) = DivisionAlgorithm(m, n, integer, ref cantDecimal);

        if (cantDecimal != 0)
            return (new Numbers(result.Substring(0, result.Length - cantDecimal),
                result.Substring(result.Length - cantDecimal, cantDecimal), positive), rest);
        return (new Numbers(result, positive), rest);
    }

    private static (string, IntegerNumbers) DivisionAlgorithm(IntegerNumbers x, IntegerNumbers y, bool integer,
        ref int cantDecimal)
    {
        string result = "";
        IntegerNumbers rest = new IntegerNumbers("0");

        foreach (var t in x.PartNumber)
        {
            IntegerNumbers div = new IntegerNumbers(rest.PartNumber + t);

            rest = DivisionImmediate(div, y, ref result);
        }

        while (rest != new IntegerNumbers("0") && !integer)
        {
            IntegerNumbers div = new IntegerNumbers(rest.PartNumber + "0");

            rest = DivisionImmediate(div, y, ref result);

            cantDecimal++;

            if (cantDecimal == _precision) break;
        }

        return (result, rest);
    }

    private static IntegerNumbers DivisionImmediate(IntegerNumbers div, IntegerNumbers divisor, ref string result)
    {
        IntegerNumbers aux = new IntegerNumbers("0");
        for (int j = 9; j >= 0; j--)
        {
            aux = divisor * BigNumMath.ConvertToIntegerNumbers(j);

            if (aux <= div)
            {
                result = result + j;
                break;
            }
        }

        return div - aux;
    }
}