using BigNum;

namespace Expression;

public class Sin : UnaryExpression
{
    public Sin(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) => new Cos(value);

    protected override RealNumbers EvaluateUnary(RealNumbers value) => BigNumMath.Sin(value);

    public override string ToString() => "sin(" + this.Value + ")";
}

public class Cos : UnaryExpression
{
    public Cos(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) =>
        new NumberExpression(RealNumbers.RealN1) * new Sin(value);

    protected override RealNumbers EvaluateUnary(RealNumbers value) => BigNumMath.Cos(value);

    public override string ToString() => "cos(" + this.Value + ")";
}

public class Tan : UnaryExpression
{
    public Tan(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) =>
        new Pow(new Sec(value), new NumberExpression(new RealNumbers("2")));

    protected override RealNumbers EvaluateUnary(RealNumbers value) => BigNumMath.Sin(value) / BigNumMath.Cos(value);

    public override string ToString() => "tan(" + this.Value + ")";
}

public class Cot : UnaryExpression
{
    public Cot(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) =>
        new Pow(new Csc(value), new NumberExpression(new RealNumbers("2")));

    protected override RealNumbers EvaluateUnary(RealNumbers value) => BigNumMath.Cos(value) / BigNumMath.Sin(value);

    public override string ToString() => "cot(" + this.Value + ")";
}

public class Sec : UnaryExpression
{
    public Sec(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) => new Sec(value) * new Tan(value);

    protected override RealNumbers EvaluateUnary(RealNumbers value) => RealNumbers.Real1 / BigNumMath.Cos(value);

    public override string ToString() => "sec(" + this.Value + ")";
}

public class Csc : UnaryExpression
{
    public Csc(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) => -new Csc(value) * new Cot(value);

    protected override RealNumbers EvaluateUnary(RealNumbers value) => RealNumbers.Real1 / BigNumMath.Sin(value);

    public override string ToString() => "csc(" + this.Value + ")";
}

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
        get => 5;
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
        get => 5;
    }

    public override string ToString() => this.Value.ToString();
}