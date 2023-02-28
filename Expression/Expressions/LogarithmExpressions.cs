namespace Expression.Expressions;

public class Log<T> : BinaryExpression<T>
{
    public Log(Function<T> left, Function<T> right) : base(left, right)
    {
    }

    public override int Priority => 4;

    /// <summary>
    ///     Determinar el tio de logaritmo
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <returns>Expresion logaritmica</returns>
    public static Log<T> DeterminateLog(Function<T> left, Function<T> right)
    {
        return left is ConstantE<T> ? new Ln<T>(right) : new Log<T>(left, right);
    }

    protected override Function<T> Derivative(char variable, Function<T> left, Function<T> right)
    {
        return (new Ln<T>(right) / new Ln<T>(left)).Derivative(variable);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Log(left, right);
    }

    protected override Function<T> EvaluateExpression(Function<T> left, Function<T> right)
    {
        return DeterminateLog(left, right);
    }

    protected override bool IsBinaryImplement()
    {
        return false;
    }

    public override string ToString()
    {
        return $"log[{Left}]({Right})";
    }
    
    public override string ToLatex()
    {
        var (l, r) = ("{", "}");
        return $"\\log_{l}{Left.ToLatex()}{r}\\left({Right.ToLatex()}\\right)";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Log<T> binary) return false;

        return Left.Equals(binary.Left) && Right.Equals(binary.Right);
    }

    public override int GetHashCode()
    {
        return 5 * Left.GetHashCode() * Right.GetHashCode();
    }
}

public class Ln<T> : Log<T>
{
    public Ln(Function<T> value) : base(new ConstantE<T>(value.Arithmetic), value)
    {
    }

    protected override Function<T> Derivative(char variable, Function<T> left, Function<T> right)
    {
        return new NumberExpression<T>(Arithmetic.Real1, Arithmetic) / right * right.Derivative(variable);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Ln(right);
    }

    public override string ToString()
    {
        return $"ln({Right})";
    }
    
    public override string ToLatex()
    {
        return $"\\ln\\left({Right.ToLatex()}\\right)";
    }
}