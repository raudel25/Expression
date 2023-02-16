namespace Expression.Arithmetics;

public class ArithmeticExp<T>
{
    public IArithmetic<T> Arithmetic { get; private set; }

    public ArithmeticExp(IArithmetic<T> arithmetic)
    {
        this.Arithmetic = arithmetic;
    }

    public NumberExpression<T> NumberExpression(T value) => new(value, Arithmetic);

    public VariableExpression<T> VariableExpression(char variable) => new(variable, Arithmetic);

    public Factorial<T> Factorial(T value) => new(value, Arithmetic);

    public ConstantE<T> ConstantE => new(Arithmetic);

    public ConstantPI<T> ConstantPI => new(Arithmetic);

    public ExpressionType<T>? Parsing(string s) => ConvertExpression<T>.Parsing(s, Arithmetic);
}