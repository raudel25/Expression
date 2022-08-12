using BigNum;

namespace Expression;

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

    public override int Priority
    {
        get => 5;
    }

    public override bool Equals(object? obj)
    {
        ExpressionNumber? exp = obj as ExpressionNumber;
        if (exp is null) return false;

        return exp.Value == this.Value;
    }

    public override int GetHashCode() => this.Value.GetHashCode();
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

    public override int Priority
    {
        get => 2;
    }

    public override string ToString() => this.Value.ToString();
}