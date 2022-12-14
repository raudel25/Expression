namespace BigNum;

public class IntegerNumbers : RealNumbers
{
    public static readonly IntegerNumbers Integer0 = new IntegerNumbers("0");

    public static readonly IntegerNumbers Integer1 = new IntegerNumbers("1");

    public static readonly IntegerNumbers IntegerN1 = new IntegerNumbers("1");

    public IntegerNumbers(string partNumber, bool positive = true) : base($"{partNumber}.0", positive,1)
    {
    }
    
    internal IntegerNumbers(List<long> number, bool positive=true) : base(number,positive,1)
    {
    }

    #region operadores

    public static IntegerNumbers operator +(IntegerNumbers a, IntegerNumbers b) =>
        BigNumMath.RealToInteger(BigNumMath.Sum(a, b));

    public static IntegerNumbers operator -(IntegerNumbers a) => BigNumMath.RealToInteger(BigNumMath.Opposite(a));

    public static IntegerNumbers operator -(IntegerNumbers a, IntegerNumbers b) =>
        BigNumMath.RealToInteger(BigNumMath.Sum(a, BigNumMath.Opposite(b)));

    public static IntegerNumbers operator *(IntegerNumbers a, IntegerNumbers b) =>
        BigNumMath.RealToInteger(BigNumMath.Product(a, b));

    public static IntegerNumbers operator /(IntegerNumbers a, IntegerNumbers b) =>
        BigNumMath.RealToInteger(BigNumMath.Division(a, b, true));

    public static IntegerNumbers operator %(IntegerNumbers a, IntegerNumbers b) =>
        BigNumMath.RealToInteger(BigNumMath.Rest(a, b));

    public static IntegerNumbers operator ++(IntegerNumbers a) => a + Integer1;

    public static IntegerNumbers operator --(IntegerNumbers a) => a - Integer1;

    #endregion
}