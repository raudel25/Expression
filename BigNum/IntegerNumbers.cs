namespace BigNum;

public class IntegerNumbers : RealNumbers
{
    public static readonly IntegerNumbers Integer0 = new IntegerNumbers("0");
    
    public static readonly IntegerNumbers Integer1 = new IntegerNumbers("1");
    
    public static readonly IntegerNumbers IntegerN1 = new IntegerNumbers("1",false);

    public IntegerNumbers(string s, bool positive = true) : base(s, "0", positive)
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

    public static IntegerNumbers operator ++(IntegerNumbers a) => a + new IntegerNumbers("1");
    
    public static IntegerNumbers operator --(IntegerNumbers a) => a - new IntegerNumbers("1");

    #endregion
}