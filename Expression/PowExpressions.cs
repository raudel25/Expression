namespace Expression;

public class Pow<T> : BinaryExpression<T>
{
    public static Pow<T> DeterminatePow(ExpressionType<T> left, ExpressionType<T> right)
    {
        NumberExpression<T>? number = right as NumberExpression<T>;
        if (number != null) return new PowExponentNumber<T>(left, number);

        ConstantE<T>? e = left as ConstantE<T>;
        return e != null ? new PowE<T>(right, right.Arithmetic) : new Pow<T>(left, right);
    }

    public Pow(ExpressionType<T> left, ExpressionType<T> right) : base(left, right)
    {
    }

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right) =>
        new PowE<T>(right * new Ln<T>(left), right.Arithmetic) *
        (right * new Ln<T>(left)).Derivative(variable);

    protected override T Evaluate(T left, T right) => Arithmetic.Pow(left, right);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right) =>
        DeterminatePow(left, right);

    protected override bool IsBinaryImplement() =>
        !(this.Left.ToString() == "0" || this.Right.ToString() == "0" || this.Left.ToString() == "1");

    public override int Priority => 3;

    public override string ToString()
    {
        if (this.Left.ToString() == "0") return "0";
        if (this.Left.ToString() == "1" || this.Right.ToString() == "0") return "1";
        if (this.Right.ToString() == "1") return this.Left.ToString()!;

        (string left, string right) = this.DeterminatePriority();

        if (this.Right is Pow<T>) right = Aux<T>.Colocated(right);

        (bool leftOpposite, bool rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite) return $"{Aux<T>.Colocated(left)} ^ {right}";
        if (rightOpposite) return $"{left} ^ {Aux<T>.Colocated(right)}";

        return $"{left} ^ {right}";
    }

    public override bool Equals(object? obj)
    {
        Pow<T>? binary = obj as Pow<T>;
        if (binary is null) return false;

        return this.Left.Equals(binary.Left) && this.Right.Equals(binary.Right);
    }

    public override int GetHashCode() => 6 * this.Left.GetHashCode() * this.Right.GetHashCode();
}

public class PowExponentNumber<T> : Pow<T>
{
    public PowExponentNumber(ExpressionType<T> exp, NumberExpression<T> number) : base(exp, number)
    {
    }

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right) =>
        right *
        new PowExponentNumber<T>(left, new NumberExpression<T>(
            Arithmetic.Subtraction(((NumberExpression<T>)right).Value, Arithmetic.Real1),
            left.Arithmetic)) * left.Derivative(variable);
}

public class PowE<T> : Pow<T>
{
    public PowE(ExpressionType<T> pow, IArithmetic<T> arithmetic) : base(new ConstantE<T>(arithmetic), pow)
    {
    }

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right) =>
        this * right.Derivative(variable);
}