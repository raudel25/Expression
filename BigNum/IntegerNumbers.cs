namespace BigNum;

public class IntegerNumbers : RealNumbers
{
    public static readonly IntegerNumbers Integer0 = new IntegerNumbers("0",false);

    public static readonly IntegerNumbers Integer1 = new IntegerNumbers("1",false);

    public static readonly IntegerNumbers IntegerN1 = new IntegerNumbers("1", false);

    internal IntegerNumbers(string s,bool check, bool positive = true) : base(s, "0", positive)
    {
        if(!check) return;
        if(!CheckNumber(s)) throw new Exception("El valor introducido no es correcto");
    }
    
    public IntegerNumbers(string s, bool positive = true) : base(s, "0", positive)
    {
        if(!CheckNumber(s)) throw new Exception("El valor introducido no es correcto");
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