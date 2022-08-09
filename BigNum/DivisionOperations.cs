namespace BigNum;

internal static class DivisionOperations
{
    internal static Numbers Division(Numbers x, Numbers y)
    {
        bool positive = x.Sign == y.Sign;
        
        if (y.Abs.Equals(new Numbers("1"))) return new Numbers(x.PartNumber, x.PartDecimal, positive);

        (string xPartDecimal, string yPartDecimal) = (x.PartDecimal, y.PartDecimal);
        int cantDecimal = AuxOperations.EqualZerosRight(ref xPartDecimal, ref yPartDecimal);

        IntegerNumbers m = new IntegerNumbers(x.PartNumber + xPartDecimal);
        IntegerNumbers n = new IntegerNumbers(y.PartNumber + yPartDecimal);

        return DivisionAlgorithm(m, n);
    }

    private static IntegerNumbers DivisionAlgorithm(IntegerNumbers x, IntegerNumbers y)
    {
        Console.WriteLine(y+"**");

        int compare = x.CompareTo(y);
        if (compare == 0) return new IntegerNumbers("1");
        if (compare == -1) return new IntegerNumbers("0");

        IntegerNumbers recursive =
            BigNumMath.NumbersToInteger(BigNumMath.Sum(x, BigNumMath.Opposite(y)));

        return BigNumMath.NumbersToInteger(BigNumMath.Sum(DivisionAlgorithm(recursive,y), new Numbers("1")));
    }
}