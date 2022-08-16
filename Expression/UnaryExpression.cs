using BigNum;

namespace Expression;

public class NumberExpression : ExpressionType
{
    public readonly RealNumbers Value;

    public NumberExpression(RealNumbers value)
    {
        this.Value = value;
    }

    public override ExpressionType Derivative() => new NumberExpression(RealNumbers.Real0);

    public override RealNumbers Evaluate(RealNumbers x) => this.Value;

    public override string ToString() => this.Value.ToString();

    public override int Priority
    {
        get => 9;
    }

    public override bool Equals(object? obj)
    {
        NumberExpression? exp = obj as NumberExpression;
        if (exp is null) return false;

        return exp.Value == this.Value;
    }

    public override int GetHashCode() => this.Value.GetHashCode();
}

public class VariableExpression : ExpressionType
{
    public readonly char Value;

    public VariableExpression(char value)
    {
        this.Value = value;
    }

    public override ExpressionType Derivative() => new NumberExpression(RealNumbers.Real1);

    public override RealNumbers Evaluate(RealNumbers x) => x;

    public override int Priority
    {
        get => 8;
    }

    public override string ToString() => this.Value.ToString();
}

public class ConstantE : NumberExpression
{
    public ConstantE() : base(BigNumMath.E)
    {
    }

    public override string ToString() => "e";
}

public class ConstantPI : NumberExpression
{
    public ConstantPI() : base(BigNumMath.PI)
    {
    }

    public override string ToString() => "pi";
}