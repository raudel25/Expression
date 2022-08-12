namespace BigNum;

public class IntegerNumbers : Numbers
{
    public IntegerNumbers(string s, bool positive = true) : base(s, "0", positive)
    {
    }

    #region operadores

    public static IntegerNumbers operator +(IntegerNumbers a, IntegerNumbers b) =>
        BigNumMath.NumbersToInteger(BigNumMath.Sum(a, b));

    public static IntegerNumbers operator -(IntegerNumbers a) => BigNumMath.NumbersToInteger(BigNumMath.Opposite(a));

    public static IntegerNumbers operator -(IntegerNumbers a, IntegerNumbers b) =>
        BigNumMath.NumbersToInteger(BigNumMath.Sum(a, BigNumMath.Opposite(b)));

    public static IntegerNumbers operator *(IntegerNumbers a, IntegerNumbers b) =>
        BigNumMath.NumbersToInteger(BigNumMath.Product(a, b));

    public static IntegerNumbers operator /(IntegerNumbers a, IntegerNumbers b) =>
        BigNumMath.NumbersToInteger(BigNumMath.Division(a, b, true));

    public static IntegerNumbers operator %(IntegerNumbers a, IntegerNumbers b) =>
        BigNumMath.NumbersToInteger(BigNumMath.Rest(a, b));

    public static IntegerNumbers operator ++(IntegerNumbers a) => a + new IntegerNumbers("1");
    
    public static IntegerNumbers operator --(IntegerNumbers a) => a - new IntegerNumbers("1");

    #endregion
}