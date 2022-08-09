namespace BigNum;

public static class BigNumMath
{
    public static Numbers Sum(Numbers x, Numbers y) => SumOperations.Sum(x, y);

    public static Numbers Product(Numbers x, Numbers y) => ProductOperations.Product(x, y);

    public static Numbers Division(Numbers x, Numbers y) => DivisionOperations.Division(x, y);

    public static Numbers Max(Numbers x, Numbers y)
    {
        if (x.CompareTo(y) == 1) return x;

        return y;
    }

    public static Numbers Min(Numbers x, Numbers y)
    {
        if (x.CompareTo(y) == -1) return x;

        return y;
    }

    public static Numbers Abs(Numbers x) => x.Abs;

    public static Numbers Opposite(Numbers x) => new Numbers(x.PartNumber, x.PartDecimal, !x.Positive());

    public static Numbers ConvertToNumbers(double n) => new Numbers(n + "", n >= 0);

    public static IntegerNumbers ConvertToIntegerNumbers(int n) => new IntegerNumbers(n + "", n >= 0);

    public static IntegerNumbers NumbersToInteger(Numbers n) => new IntegerNumbers(n.PartNumber, n.Positive());
}