using BigNum;

namespace Expression.Arithmetics;

public class BigNumExp : IArithmetic<RealNumbers>
{
    private readonly BigNumMath _big;

    public BigNumExp(BigNumMath big)
    {
        _big = big;
        E = _big.E;
        PI = _big.PI;
        RealN1 = _big.RealN1;
        Real0 = _big.Real0;
        Real1 = _big.Real1;
    }

    public RealNumbers Real1 { get; }
    public RealNumbers Real0 { get; }
    public RealNumbers RealN1 { get; }

    public RealNumbers StringToNumber(string s, bool positive = true)
    {
        return _big.Real(s, positive);
    }

    public bool IsInteger(RealNumbers n)
    {
        return BigNumMath.IsInteger(n);
    }

    public bool Positive(RealNumbers n)
    {
        return n.Positive();
    }

    public RealNumbers Rest(RealNumbers x, RealNumbers y)
    {
        if (!IsInteger(x) || !IsInteger(y))
            throw new Exception("The parameters are not integer");

        return BigNumMath.RealToInteger(x) % BigNumMath.RealToInteger(y);
    }

    public RealNumbers Sum(RealNumbers x, RealNumbers y)
    {
        return x + y;
    }

    public RealNumbers Subtraction(RealNumbers x, RealNumbers y)
    {
        return x - y;
    }

    public RealNumbers Multiply(RealNumbers x, RealNumbers y)
    {
        return x * y;
    }

    public RealNumbers Division(RealNumbers x, RealNumbers y)
    {
        return x / y;
    }

    public RealNumbers E { get; }
    public RealNumbers PI { get; }

    public RealNumbers Factorial(RealNumbers x)
    {
        if (!IsInteger(x))
            throw new Exception("The parameters are not integer");

        return BigNumMath.Factorial(BigNumMath.RealToInteger(x));
    }

    public RealNumbers Log(RealNumbers x, RealNumbers y)
    {
        return BigNumMath.Log(x, y);
    }

    public RealNumbers Ln(RealNumbers x)
    {
        return BigNumMath.Ln(x);
    }

    public RealNumbers Pow(RealNumbers x, RealNumbers y)
    {
        return BigNumMath.Pow(x, y);
    }

    public RealNumbers Sin(RealNumbers x)
    {
        return BigNumMath.Sin(x);
    }

    public RealNumbers Cos(RealNumbers x)
    {
        return BigNumMath.Cos(x);
    }

    public RealNumbers Tan(RealNumbers x)
    {
        return BigNumMath.Sin(x) / BigNumMath.Cos(x);
    }

    public RealNumbers Cot(RealNumbers x)
    {
        return BigNumMath.Cos(x) / BigNumMath.Sin(x);
    }

    public RealNumbers Asin(RealNumbers x)
    {
        return BigNumMath.Asin(x);
    }

    public RealNumbers Acos(RealNumbers x)
    {
        return BigNumMath.Acos(x);
    }

    public RealNumbers Atan(RealNumbers x)
    {
        return BigNumMath.Atan(x);
    }

    public RealNumbers Acot(RealNumbers x)
    {
        return BigNumMath.Acot(x);
    }
}