namespace Expression;

public class Log<T> : BinaryExpression<T>
{
    /// <summary>
    /// Determinar el tio de logaritmo
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <returns>Expresion logaritmica</returns>
    public static Log<T> DeterminateLog(ExpressionType<T> left, ExpressionType<T> right)
    {
        ConstantE<T>? e = left as ConstantE<T>;
        if (e != null) return new Ln<T>(right);

        return new Log<T>(left, right);
    }

    public Log(ExpressionType<T> left, ExpressionType<T> right) : base(left, right)
    {
    }

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right) =>
        (new Ln<T>(right) / new Ln<T>(left)).Derivative(variable);

    protected override T Evaluate(T left, T right) => Arithmetic.Log(left, right);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right) =>
        DeterminateLog(left, right);

    protected override bool IsBinaryImplement() => false;

    public override int Priority => 4;

    public override string ToString()
    {
        (string left, string right) = (this.Left.ToString()!, this.Right.ToString()!);

        if (right == "1") return "0";

        return $"log({left})({right})";
    }

    public override bool Equals(object? obj)
    {
        Log<T>? binary = obj as Log<T>;
        if (binary is null) return false;

        return this.Left.Equals(binary.Left) && this.Right.Equals(binary.Right);
    }

    public override int GetHashCode() => 5 * this.Left.GetHashCode() * this.Right.GetHashCode();
}

public class Ln<T> : Log<T>
{
    public Ln(ExpressionType<T> value) : base(new ConstantE<T>(value.Arithmetic), value)
    {
    }

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right) =>
        new NumberExpression<T>(Arithmetic.Real1, Arithmetic) / right * right.Derivative(variable);

    protected override T Evaluate(T left, T right) => Arithmetic.Ln(right);

    public override string ToString()
    {
        if (this.Right.ToString() == "1") return "0";
        return $"ln({this.Right})";
    }
}