namespace BigNum;

public static class BigNumMath
{
    public static Numbers Sum(Numbers x, Numbers y)
    {
        return SumOperations.Sum(x, y);
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
}