namespace Expression.Expressions;

public class Sum<T> : BinaryExpression<T>
{
    public Sum(Function<T> left, Function<T> right) : base(left, right)
    {
    }

    public override int Priority => 1;

    protected override Function<T> Derivative(char variable, Function<T> left, Function<T> right)
    {
        return left.Derivative(variable) + right.Derivative(variable);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Sum(left, right);
    }

    protected override Function<T> EvaluateExpression(Function<T> left, Function<T> right)
    {
        return left + right;
    }

    protected override bool IsBinaryImplement()
    {
        return !(Left.ToString() == "0" || Right.ToString() == "0");
    }

    public override bool Equals(object? obj)
    {
        var binary = obj as Sum<T>;
        if (binary is null) return false;

        return (Left.Equals(binary.Left) && Right.Equals(binary.Right)) ||
               (Left.Equals(binary.Right) && Right.Equals(binary.Left));
    }

    public override int GetHashCode()
    {
        return Left.GetHashCode() * Right.GetHashCode();
    }

    private string AuxString(bool latex)
    {
        var (left, right) = latex ? (Left.ToLatex(), Right.ToLatex()) : (Left.ToString()!, Right.ToString()!);
        (left, right) = DeterminatePriority(left, right, latex);

        var rightOpposite = right[0] == '-';
        return rightOpposite ? $"{left} - {right.Substring(1, right.Length - 1)}" : $"{left} + {right}";
    }

    public override string ToString()
    {
        return AuxString(false);
    }

    public override string ToLatex()
    {
        return AuxString(true);
    }
}

public class Subtraction<T> : BinaryExpression<T>
{
    public Subtraction(Function<T> left, Function<T> right) : base(left, right)
    {
    }

    public override int Priority => 1;

    protected override Function<T> Derivative(char variable, Function<T> left, Function<T> right)
    {
        return left.Derivative(variable) - right.Derivative(variable);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Subtraction(left, right);
    }

    protected override Function<T> EvaluateExpression(Function<T> left, Function<T> right)
    {
        return left - right;
    }

    protected override bool IsBinaryImplement()
    {
        return !(Left.ToString() == "0" || Right.ToString() == "0");
    }

    public override bool Equals(object? obj)
    {
        var binary = obj as Subtraction<T>;
        if (binary is null) return false;

        return Left.Equals(binary.Left) && Right.Equals(binary.Right);
    }

    public override int GetHashCode()
    {
        return 2 * Left.GetHashCode() * Right.GetHashCode();
    }

    private string AuxString(bool latex)
    {
        var (left, right) = latex ? (Left.ToLatex(), Right.ToLatex()) : (Left.ToString()!, Right.ToString()!);
        (left, right) = DeterminatePriority(left, right, latex);

        var rightOpposite = right[0] == '-';
        return rightOpposite ? $"{left} + {right.Substring(1, right.Length - 1)}" : $"{left} - {right}";
    }

    public override string ToString()
    {
        return AuxString(false);
    }

    public override string ToLatex()
    {
        return AuxString(true);
    }
}

public class Multiply<T> : BinaryExpression<T>
{
    public Multiply(Function<T> left, Function<T> right) : base(left, right)
    {
    }

    public override int Priority => 2;

    protected override Function<T> Derivative(char variable, Function<T> left, Function<T> right)
    {
        return left.Derivative(variable) * right + left * right.Derivative(variable);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Multiply(left, right);
    }

    protected override Function<T> EvaluateExpression(Function<T> left, Function<T> right)
    {
        return left * right;
    }

    protected override bool IsBinaryImplement()
    {
        if (Left.ToString() == "0" || Right.ToString() == "0") return false;
        if (Left.ToString() == "1" || Right.ToString() == "1") return false;
        if (Left.ToString() == "-1" || Right.ToString() == "-1") return false;

        return true;
    }

    public override bool Equals(object? obj)
    {
        var binary = obj as Multiply<T>;
        if (binary is null) return false;

        return (Left.Equals(binary.Left) && Right.Equals(binary.Right)) ||
               (Left.Equals(binary.Right) && Right.Equals(binary.Left));
    }

    public override int GetHashCode()
    {
        return 3 * Left.GetHashCode() * Right.GetHashCode();
    }

    private string AuxString(bool latex)
    {
        var (left, right) = latex ? (Left.ToLatex(), Right.ToLatex()) : (Left.ToString()!, Right.ToString()!);
        var symbol = latex ? "\\cdot" : " * ";

        if (left == "-1") return Aux<T>.Opposite(right, Right.Priority, latex);
        if (right == "-1") return Aux<T>.Opposite(left, Left.Priority, latex);

        (left, right) = DeterminatePriority(left, right, latex);

        var (leftOpposite, rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite && rightOpposite)
            return $"{left.Substring(1, left.Length - 1)} * {right.Substring(1, right.Length - 1)}";
        if (rightOpposite) return $"{left}{symbol}{Aux<T>.Colocated(right, latex)}";

        return this.Left is not NumberExpression<T> || this.Right is not NumberExpression<T>
            ? this.Right is NumberExpression<T> ? $"{right}{left}" : $"{left}{right}"
            : $"{left}{symbol}{right}";
    }

    public override string ToString()
    {
        return AuxString(false);
    }

    public override string ToLatex()
    {
        return AuxString(true);
    }
}

public class Division<T> : BinaryExpression<T>
{
    public Division(Function<T> left, Function<T> right) : base(left, right)
    {
    }

    public override int Priority => 2;

    protected override Function<T> Derivative(char variable, Function<T> left, Function<T> right)
    {
        return (left.Derivative(variable) * right - left * right.Derivative(variable)) / (right * right);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Division(left, right);
    }

    protected override Function<T> EvaluateExpression(Function<T> left, Function<T> right)
    {
        return left / right;
    }

    protected override bool IsBinaryImplement()
    {
        return !(Left.ToString() == "0" || Right.ToString() == "1");
    }

    public override bool Equals(object? obj)
    {
        var binary = obj as Division<T>;
        if (binary is null) return false;

        return Left.Equals(binary.Left) && Right.Equals(binary.Right);
    }

    public override int GetHashCode()
    {
        return 4 * Left.GetHashCode() * Right.GetHashCode();
    }

    public override string ToString()
    {
        var (left, right) = DeterminatePriority(Left.ToString()!, Right.ToString()!);

        if (Right is Division<T>) right = Aux<T>.Colocated(right);

        var (leftOpposite, rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite && rightOpposite)
            return $"{left.Substring(1, left.Length - 1)} / {right.Substring(1, right.Length - 1)}";
        return rightOpposite ? $"{left} / {Aux<T>.Colocated(right)}" : $"{left} / {right}";
    }

    public override string ToLatex()
    {
        var (l, r) = ("{", "}");
        return $"{l}{Left.ToLatex()}\\over {Right.ToLatex()}{r}";
    }
}