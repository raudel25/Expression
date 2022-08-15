using BigNum;

namespace Expression;

public class Log : BinaryExpression
{
    public Log(ExpressionType left, ExpressionType right) : base(left, right)
    {
    }

    protected override ExpressionType Derivative(ExpressionType left, ExpressionType right) =>
        (new Ln(right) / new Ln(left)).Derivative();

    protected override RealNumbers EvaluateBinary(RealNumbers left, RealNumbers right)
    {
        throw new NotImplementedException();
    }

    protected override bool IsBinaryImplement() => this.Right.ToString() != "1";

    public override int Priority
    {
        get => 2;
    }

    public override string ToString()
    {
        (string left, string right) = (this.Left.ToString()!, this.Right.ToString()!);

        if (right == "1") return "0";

        return $"log({left})({right})";
    }
}

public class Ln : Log
{
    public Ln(ExpressionType value) : base(new ConstantE(), value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType left, ExpressionType right) =>
        new NumberExpression(RealNumbers.Real1) / right * right.Derivative();

    public override string ToString()
    {
        if (this.Right.ToString() == "1") return "0";
        return $"ln({this.Right})";   
    }
}