using Exception = System.Exception;

namespace Expression.Arithmetics;

public class NativeExp : IArithmetic<double>
{
    public NativeExp()
    {
        this.Real0 = 0;
        this.Real1 = 1;
        this.RealN1 = -1;
        this.E = Math.E;
        this.PI = Math.PI;
    }

    public double Real1 { get; }
    public double Real0 { get; }
    public double RealN1 { get; }
    public double E { get; }
    public double PI { get; }

    public double StringToNumber(string s, bool positive = true)
    {
        if (double.TryParse(s, out double n)) return positive ? n : -n;
        throw new Exception("The string is not number");
    }

    public bool IsInteger(double n) => n - (int)n == 0;

    public bool Positive(double n) => n >= 0;

    public double Rest(double x, double y)
    {
        if (!IsInteger(x) || !IsInteger(y))
            throw new Exception("The parameters are not integer");

        return (int)x % (int)y;
    }

    public double Sum(double x, double y) => x + y;

    public double Subtraction(double x, double y)
    {
        return x - y;
    }

    public double Multiply(double x, double y)
    {
        return x * y;
    }

    public double Division(double x, double y)
    {
        return x / y;
    }

    public double Factorial(double x)
    {
        if (!IsInteger(x))
            throw new Exception("The parameters are not integer");

        double fact = 1;

        for (int i = 1; i <= x; i++) fact *= i;

        return fact;
    }

    public double Log(double x, double y)
    {
        return Math.Log(y, x);
    }

    public double Ln(double x)
    {
        return Math.Log(x);
    }

    public double Pow(double x, double y)
    {
        return Math.Pow(x, y);
    }

    public double Sin(double x)
    {
        return Math.Sin(x);
    }

    public double Cos(double x)
    {
        return Math.Cos(x);
    }

    public double Tan(double x)
    {
        return Math.Tan(x);
    }

    public double Cot(double x)
    {
        return Math.Cos(x) / Math.Sin(x);
    }

    public double Asin(double x)
    {
        return Math.Asin(x);
    }

    public double Acos(double x)
    {
        return Math.Acos(x);
    }

    public double Atan(double x)
    {
        return Math.Atan(x);
    }

    public double Acot(double x)
    {
        return Math.PI / 2 - Math.Atan(x);
    }

    public double Sqrt(double x, double y)
    {
        return Math.Pow(x, 1 / y);
    }
}