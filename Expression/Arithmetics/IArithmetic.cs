namespace Expression;

public interface IArithmetic<T>
{
    public T Real1 { get; }
    public T Real0 { get; }

    public T RealN1 { get; }

    public T E { get; }

    public T PI { get; }

    public T StringToNumber(string s, bool positive = true);

    public bool IsInteger(T n);

    public bool Positive(T n);

    public T Rest(T x, T y);

    public T Sum(T x, T y);
    public T Subtraction(T x, T y);

    public T Multiply(T x, T y);

    public T Division(T x, T y);

    public T Factorial(T x);

    public T Log(T x, T y);

    public T Ln(T x);

    public T Pow(T x, T y);

    public T Sin(T x);

    public T Cos(T x);
    public T Tan(T x);
    public T Cot(T x);

    public T Asin(T x);

    public T Acos(T x);

    public T Atan(T x);
    public T Acot(T x);
}