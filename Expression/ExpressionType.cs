using BigNum;

namespace Expression;

public abstract class ExpressionType
{
    public abstract ExpressionType Derivative();

    public abstract RealNumbers Evaluate(RealNumbers x);

    public abstract int Priority { get; }

    public virtual bool IsBinary() => this is BinaryExpression;

    public ExpressionType Derivative(int n)
    {
        ExpressionType expression = this.Derivative();

        for (int i = 1; i < n; i++) expression = expression.Derivative();

        return expression;
    }

    public static ExpressionType operator +(ExpressionType left, ExpressionType right) => new Sum(left, right);

    public static ExpressionType operator -(ExpressionType left, ExpressionType right) =>
        new Subtraction(left, right);

    public static ExpressionType operator -(ExpressionType value) =>
        new Subtraction(new NumberExpression(new RealNumbers("0")), value);

    public static ExpressionType operator *(ExpressionType left, ExpressionType right) => new Multiply(left, right);

    public static ExpressionType operator /(ExpressionType left, ExpressionType right) => new Division(left, right);
}

public abstract class BinaryExpression : ExpressionType
{
    public readonly ExpressionType Left;

    public readonly ExpressionType Right;

    public BinaryExpression(ExpressionType left, ExpressionType right)
    {
        this.Left = left;
        this.Right = right;
    }

    public override ExpressionType Derivative() => this.Derivative(this.Left, this.Right);

    public override RealNumbers Evaluate(RealNumbers x) => Evaluate(this.Left.Evaluate(x), this.Right.Evaluate(x));

    protected abstract ExpressionType Derivative(ExpressionType left, ExpressionType right);

    protected abstract RealNumbers Evaluate(RealNumbers left, RealNumbers right);

    protected abstract bool IsBinaryImplement();

    public override bool IsBinary()
    {
        if (this.Left.IsBinary() || this.Right.IsBinary()) return true;

        return IsBinaryImplement();
    }

    protected (string, string) DeterminatePriority() => (
        this.Left.Priority < this.Priority && this.Left.IsBinary()
            ? Aux.Colocated(this.Left.ToString()!)
            : this.Left.ToString()!,
        this.Right.Priority < this.Priority && this.Right.IsBinary()
            ? Aux.Colocated(this.Right.ToString()!)
            : this.Right.ToString()!);
}

public abstract class UnaryExpression : ExpressionType
{
    public readonly ExpressionType Value;

    public UnaryExpression(ExpressionType value)
    {
        this.Value = value;
    }

    public override ExpressionType Derivative() => this.Derivative(this.Value);

    protected abstract ExpressionType Derivative(ExpressionType value);

    public override int Priority
    {
        get => 4;
    }
}