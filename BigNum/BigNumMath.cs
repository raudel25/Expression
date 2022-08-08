namespace BigNum;

public static class BigNumMath
{
    public static Numbers Sum(Numbers x, Numbers y)
    {
        return SumOperations.Sum(x, y);
    }

    public static Numbers Product(Numbers x, Numbers y)
    {
        return ProductOperations.Product(x, y);
    }

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

    public static Numbers Abs(Numbers x)
    {
        return x.Abs;
    }

    public static Numbers ConvertToNumbers(double n)
    {
        return new Numbers(n + "", n >= 0);
    }

    public static IntegerNumbers ConvertToIntegerNumbers(int n)
    {
        return new IntegerNumbers(n + "", n >= 0);
    }

    public static IntegerNumbers NumbersToInteger(Numbers n)
    {
        return new IntegerNumbers(n.PartNumber, n.Positive());
    }
}