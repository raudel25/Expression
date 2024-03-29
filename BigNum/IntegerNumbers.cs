namespace BigNum;

public class IntegerNumbers : RealNumbers
{
    internal IntegerNumbers(string partNumber, long base10, int indBase10, bool positive = true) : base(
        $"{partNumber}.0",
        1, base10, indBase10, positive)
    {
    }

    internal IntegerNumbers(List<long> number, long base10, int indBase10, bool positive = true) : base(number, 1,
        base10, indBase10, positive)
    {
    }

    public IntegerNumbers Integer0 => BigNumMath.RealToInteger(Real0);

    public IntegerNumbers Integer1 => BigNumMath.RealToInteger(Real1);

    public IntegerNumbers IntegerN1 => BigNumMath.RealToInteger(RealN1);

    #region operadores

    public static IntegerNumbers operator +(IntegerNumbers a, IntegerNumbers b)
    {
        return BigNumMath.RealToInteger(BigNumMath.Sum(a, b));
    }

    public static IntegerNumbers operator -(IntegerNumbers a)
    {
        return BigNumMath.RealToInteger(BigNumMath.Opposite(a));
    }

    public static IntegerNumbers operator -(IntegerNumbers a, IntegerNumbers b)
    {
        return BigNumMath.RealToInteger(BigNumMath.Sum(a, BigNumMath.Opposite(b)));
    }

    public static IntegerNumbers operator *(IntegerNumbers a, IntegerNumbers b)
    {
        return BigNumMath.RealToInteger(BigNumMath.Product(a, b));
    }

    public static IntegerNumbers operator /(IntegerNumbers a, IntegerNumbers b)
    {
        return BigNumMath.RealToInteger(BigNumMath.Division(a, b, true));
    }

    public static IntegerNumbers operator %(IntegerNumbers a, IntegerNumbers b)
    {
        return BigNumMath.RealToInteger(BigNumMath.Rest(a, b));
    }

    public static IntegerNumbers operator ++(IntegerNumbers a)
    {
        return a + a.Integer1;
    }

    public static IntegerNumbers operator --(IntegerNumbers a)
    {
        return a - a.Integer1;
    }

    #endregion
}