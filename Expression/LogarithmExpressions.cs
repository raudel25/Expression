using BigNum;

namespace Expression;

public class Log : BinaryExpression
{
    /// <summary>
    /// Determinar el tio de logaritmo
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <returns>Expresion logaritmica</returns>
    public static Log DeterminateLog(ExpressionType left, ExpressionType right)
    {
        ConstantE? e = left as ConstantE;
        if (e != null) return new Ln(right);

        return new Log(left, right);
    }

    public Log(ExpressionType left, ExpressionType right) : base(left, right)
    {
    }

    protected override ExpressionType Derivative(char variable, ExpressionType left, ExpressionType right) =>
        (new Ln(right) / new Ln(left)).Derivative(variable);

    protected override RealNumbers Evaluate(RealNumbers left, RealNumbers right) => BigNumMath.Log(left, right);

    protected override ExpressionType EvaluateExpression(ExpressionType left, ExpressionType right) =>
        DeterminateLog(left, right);

    protected override bool IsBinaryImplement() => false;

    public override int Priority
    {
        get => 5;
    }

    public override string ToString()
    {
        (string left, string right) = (this.Left.ToString()!, this.Right.ToString()!);

        if (right == "1") return "0";

        return $"log({left})({right})";
    }

    public override bool Equals(object? obj)
    {
        Log? binary = obj as Log;
        if (binary is null) return false;

        return this.Left.Equals(binary.Left) && this.Right.Equals(binary.Right);
    }

    public override int GetHashCode() => 5 * this.Left.GetHashCode() * this.Right.GetHashCode();
}

public class Ln : Log
{
    public Ln(ExpressionType value) : base(new ConstantE(), value)
    {
    }

    protected override ExpressionType Derivative(char variable, ExpressionType left, ExpressionType right) =>
        new NumberExpression(RealNumbers.Real1) / right * right.Derivative(variable);

    protected override RealNumbers Evaluate(RealNumbers left, RealNumbers right) => BigNumMath.Ln(right);

    public override string ToString()
    {
        if (this.Right.ToString() == "1") return "0";
        return $"ln({this.Right})";
    }
}