using BigNum;

namespace Expression;

public class Sum : BinaryExpression
{
    public Sum(ExpressionValue left, ExpressionValue right) : base(left, right)
    {
    }

    protected override ExpressionValue Derivative(ExpressionValue left, ExpressionValue right) =>
        left.Derivative() + right.Derivative();

    protected override Numbers Evaluate(Numbers left, Numbers right) => left + right;
}

public class Subtraction : BinaryExpression
{
    public Subtraction(ExpressionValue left, ExpressionValue right) : base(left, right)
    {
    }

    protected override ExpressionValue Derivative(ExpressionValue left, ExpressionValue right) =>
        left.Derivative() - right.Derivative();

    protected override Numbers Evaluate(Numbers left, Numbers right) => left - right;
}

public class Multiply : BinaryExpression
{
    public Multiply(ExpressionValue left, ExpressionValue right) : base(left, right)
    {
    }

    protected override ExpressionValue Derivative(ExpressionValue left, ExpressionValue right) =>
        left.Derivative() * right + left * right.Derivative();

    protected override Numbers Evaluate(Numbers left, Numbers right) => left * right;
}

public class Division : BinaryExpression
{
    public Division(ExpressionValue left, ExpressionValue right) : base(left, right)
    {
    }

    protected override ExpressionValue Derivative(ExpressionValue left, ExpressionValue right) =>
        left.Derivative() * right - left * right.Derivative();

    protected override Numbers Evaluate(Numbers left, Numbers right) => left * right;
}

public class Sin : UnaryExpression
{
    public Sin(ExpressionValue value) : base(value)
    {
    }

    protected override ExpressionValue Derivative(ExpressionValue value) => new Cos(value) * value.Derivative();

    protected override Numbers Evaluate(Numbers value)
    {
        throw new NotImplementedException();
    }

    public override string ToString() => "sin(" + this.Value + ")";
}

public class Cos : UnaryExpression
{
    public Cos(ExpressionValue value) : base(value)
    {
    }

    protected override ExpressionValue Derivative(ExpressionValue value) =>
        new ExpressionNumber(new Numbers("1", false)) * new Sin(value) * value.Derivative();

    protected override Numbers Evaluate(Numbers value)
    {
        throw new NotImplementedException();
    }

    public override string ToString() => "cos(" + this.Value + ")";
}

public class ExpressionNumber : ExpressionValue
{
    public readonly Numbers Value;

    public ExpressionNumber(Numbers value)
    {
        this.Value = value;
    }

    public override ExpressionValue Derivative() => new ExpressionNumber(new Numbers("0"));

    public override Numbers Evaluate() => this.Value;

    public override string ToString() => this.Value.ToString();
}

public class ExpressionVariable : ExpressionValue
{
    public readonly char Value;

    public ExpressionVariable(char value)
    {
        this.Value = value;
    }

    public override ExpressionValue Derivative() => new ExpressionNumber(new Numbers("1"));

    public override Numbers Evaluate()
    {
        throw new NotImplementedException();
    }

    public override string ToString() => this.Value.ToString();
}