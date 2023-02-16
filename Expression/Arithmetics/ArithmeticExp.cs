namespace Expression.Arithmetics;

public class ArithmeticExp<T>
{
    public ArithmeticExp(IArithmetic<T> arithmetic)
    {
        Arithmetic = arithmetic;
    }

    public IArithmetic<T> Arithmetic { get; }

    public ConstantE<T> ConstantE => new(Arithmetic);

    public ConstantPI<T> ConstantPI => new(Arithmetic);

    public NumberExpression<T> NumberExpression(T value)
    {
        return new(value, Arithmetic);
    }

    public VariableExpression<T> VariableExpression(char variable)
    {
        return new(variable, Arithmetic);
    }

    public Factorial<T> Factorial(T value)
    {
        return new(value, Arithmetic);
    }

    public ExpressionType<T>? Parsing(string s)
    {
        return ConvertExpression<T>.Parsing(s, Arithmetic);
    }
}