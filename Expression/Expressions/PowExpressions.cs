namespace Expression.Expressions;

public class Pow<T> : BinaryExpression<T>
{
    public Pow(Function<T> left, Function<T> right) : base(left, right)
    {
    }

    public override int Priority => 3;

    public static Pow<T> DeterminatePow(Function<T> left, Function<T> right)
    {
        if (right is NumberExpression<T> number) return new PowExponentNumber<T>(left, number);

        return left is ConstantE<T> ? new PowE<T>(right, right.Arithmetic) : new Pow<T>(left, right);
    }

    protected override Function<T> Derivative(char variable, Function<T> left, Function<T> right)
    {
        return new PowE<T>(right * new Ln<T>(left), right.Arithmetic) *
               (right * new Ln<T>(left)).Derivative(variable);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Pow(left, right);
    }

    protected override Function<T> EvaluateExpression(Function<T> left, Function<T> right)
    {
        return DeterminatePow(left, right);
    }

    protected override bool IsBinaryImplement()
    {
        return !(Left.ToString() == "0" || Right.ToString() == "0" || Left.ToString() == "1");
    }

    public override string ToString()
    {
        if (Left.ToString() == "0") return "0";
        if (Left.ToString() == "1" || Right.ToString() == "0") return "1";
        if (Right.ToString() == "1") return Left.ToString()!;

        var (left, right) = DeterminatePriority();

        if (Right is Pow<T>) right = Aux<T>.Colocated(right);

        var (leftOpposite, rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite) return $"{Aux<T>.Colocated(left)} ^ {right}";
        if (rightOpposite) return $"{left} ^ {Aux<T>.Colocated(right)}";

        return $"{left} ^ {right}";
    }

    public override bool Equals(object? obj)
    {
        var binary = obj as Pow<T>;
        if (binary is null) return false;

        return Left.Equals(binary.Left) && Right.Equals(binary.Right);
    }

    public override int GetHashCode()
    {
        return 6 * Left.GetHashCode() * Right.GetHashCode();
    }
}

public class PowExponentNumber<T> : Pow<T>
{
    public PowExponentNumber(Function<T> exp, NumberExpression<T> number) : base(exp, number)
    {
    }

    protected override Function<T> Derivative(char variable, Function<T> left, Function<T> right)
    {
        return right *
               new PowExponentNumber<T>(left, new NumberExpression<T>(
                   Arithmetic.Subtraction(((NumberExpression<T>)right).Value, Arithmetic.Real1),
                   left.Arithmetic)) * left.Derivative(variable);
    }
}

public class PowE<T> : Pow<T>
{
    public PowE(Function<T> pow, IArithmetic<T> arithmetic) : base(new ConstantE<T>(arithmetic), pow)
    {
    }

    protected override Function<T> Derivative(char variable, Function<T> left, Function<T> right)
    {
        return this * right.Derivative(variable);
    }
}