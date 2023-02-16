using BigNum;

namespace Expression.Arithmetics;

public class BigNum : IArithmetic<RealNumbers>
{
    public readonly BigNumMath Big;

    public BigNum(BigNumMath big)
    {
        this.Big = big;
        E = Big.E;
        PI = Big.PI;
        this.RealN1 = Big.RealN1;
        this.Real0 = Big.Real0;
        this.Real1 = Big.Real1;
    }

    public RealNumbers Real1 { get; }
    public RealNumbers Real0 { get; }
    public RealNumbers RealN1 { get; }

    public RealNumbers StringToNumber(string s, bool positive = true) => Big.Real(s, positive);

    public bool IsInteger(RealNumbers n) => BigNumMath.IsInteger(n);

    public bool Positive(RealNumbers n) => n.Positive();

    public RealNumbers Rest(RealNumbers x, RealNumbers y)
    {
        if (!IsInteger(x) || !IsInteger(y))
            throw new Exception("The parameters are not integer");

        return BigNumMath.RealToInteger(x) % BigNumMath.RealToInteger(y);
    }

    public RealNumbers Sum(RealNumbers x, RealNumbers y) => x + y;

    public RealNumbers Subtraction(RealNumbers x, RealNumbers y) => x - y;

    public RealNumbers Multiply(RealNumbers x, RealNumbers y) => x * y;

    public RealNumbers Division(RealNumbers x, RealNumbers y) => x / y;

    public RealNumbers E { get; }
    public RealNumbers PI { get; }

    public RealNumbers Factorial(RealNumbers x)
    {
        if (!IsInteger(x))
            throw new Exception("The parameters are not integer");

        return BigNumMath.Factorial(BigNumMath.RealToInteger(x));
    }

    public RealNumbers Log(RealNumbers x, RealNumbers y) => BigNumMath.Log(x, y);

    public RealNumbers Ln(RealNumbers x) => BigNumMath.Ln(x);

    public RealNumbers Pow(RealNumbers x, RealNumbers y) => BigNumMath.Pow(x, y);

    public RealNumbers Sin(RealNumbers x) => BigNumMath.Sin(x);

    public RealNumbers Cos(RealNumbers x) => BigNumMath.Cos(x);

    public RealNumbers Tan(RealNumbers x) => BigNumMath.Sin(x) / BigNumMath.Cos(x);

    public RealNumbers Cot(RealNumbers x) => BigNumMath.Cos(x) / BigNumMath.Sin(x);

    public RealNumbers Asin(RealNumbers x) => BigNumMath.Asin(x);

    public RealNumbers Acos(RealNumbers x) => BigNumMath.Acos(x);

    public RealNumbers Atan(RealNumbers x) => BigNumMath.Atan(x);

    public RealNumbers Acot(RealNumbers x) => BigNumMath.Acot(x);
}