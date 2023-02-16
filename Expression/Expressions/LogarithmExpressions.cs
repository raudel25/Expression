namespace Expression.Expressions;

public class Log<T> : BinaryExpression<T>
{
    public Log(ExpressionType<T> left, ExpressionType<T> right) : base(left, right)
    {
    }

    public override int Priority => 4;

    /// <summary>
    ///     Determinar el tio de logaritmo
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <returns>Expresion logaritmica</returns>
    public static Log<T> DeterminateLog(ExpressionType<T> left, ExpressionType<T> right)
    {
        if (left is ConstantE<T>) return new Ln<T>(right);

        return new Log<T>(left, right);
    }

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right)
    {
        return (new Ln<T>(right) / new Ln<T>(left)).Derivative(variable);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Log(left, right);
    }

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right)
    {
        return DeterminateLog(left, right);
    }

    protected override bool IsBinaryImplement()
    {
        return false;
    }

    public override string ToString()
    {
        var (left, right) = (Left.ToString()!, Right.ToString()!);

        if (right == "1") return "0";

        return $"log[{left}]({right})";
    }

    public override bool Equals(object? obj)
    {
        var binary = obj as Log<T>;
        if (binary is null) return false;

        return Left.Equals(binary.Left) && Right.Equals(binary.Right);
    }

    public override int GetHashCode()
    {
        return 5 * Left.GetHashCode() * Right.GetHashCode();
    }
}

public class Ln<T> : Log<T>
{
    public Ln(ExpressionType<T> value) : base(new ConstantE<T>(value.Arithmetic), value)
    {
    }

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right)
    {
        return new NumberExpression<T>(Arithmetic.Real1, Arithmetic) / right * right.Derivative(variable);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Ln(right);
    }

    public override string ToString()
    {
        if (Right.ToString() == "1") return "0";
        return $"ln({Right})";
    }
}