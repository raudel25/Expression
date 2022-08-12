using BigNum;

namespace Expression;

public abstract class ExpressionValue
{
    public abstract ExpressionValue Derivative();

    public abstract Numbers Evaluate(Numbers x);

    public abstract int Priority { get; }

    public ExpressionValue Derivative(int n)
    {
        ExpressionValue expression = this.Derivative();

        for (int i = 1; i < n; i++) expression = expression.Derivative();

        return expression;
    }

    public static ExpressionValue operator +(ExpressionValue left, ExpressionValue right) => new Sum(left, right);

    public static ExpressionValue operator -(ExpressionValue left, ExpressionValue right) =>
        new Subtraction(left, right);

    public static ExpressionValue operator -(ExpressionValue value) =>
        new Subtraction(new ExpressionNumber(new Numbers("0")), value);

    public static ExpressionValue operator *(ExpressionValue left, ExpressionValue right) => new Multiply(left, right);

    public static ExpressionValue operator /(ExpressionValue left, ExpressionValue right) => new Division(left, right);
}

public abstract class BinaryExpression : ExpressionValue
{
    public readonly ExpressionValue Left;

    public readonly ExpressionValue Right;

    public BinaryExpression(ExpressionValue left, ExpressionValue right)
    {
        this.Left = left;
        this.Right = right;
    }

    public override ExpressionValue Derivative() => this.Derivative(this.Left, this.Right);

    public override Numbers Evaluate(Numbers x) => Evaluate(this.Left.Evaluate(x), this.Right.Evaluate(x));

    protected abstract ExpressionValue Derivative(ExpressionValue left, ExpressionValue right);

    protected abstract Numbers Evaluate(Numbers left, Numbers right);

    protected (string, string) DeterminatePriority() => (
        this.Left.Priority < this.Priority ? Aux.Colocated(this.Left.ToString()!) : this.Left.ToString()!,
        this.Right.Priority < this.Priority ? Aux.Colocated(this.Right.ToString()!) : this.Right.ToString()!);
}

public abstract class UnaryExpression : ExpressionValue
{
    public readonly ExpressionValue Value;

    public UnaryExpression(ExpressionValue value)
    {
        this.Value = value;
    }

    public override ExpressionValue Derivative() => this.Derivative(this.Value);

    protected abstract ExpressionValue Derivative(ExpressionValue value);

    public override int Priority
    {
        get => 4;
    }
}