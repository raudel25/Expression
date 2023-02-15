namespace Expression;

public interface IArithmetic<T>
{
    public T Real1 { get; }
    public T Real0 { get; }

    public T Sum(T x, T y);
    public T Subtraction(T x, T y);

    public T Multiply(T x, T y);

    public T Division(T x, T y);

    public T E { get; }

    public T PI { get; }

    public T Factorial(T x);
}