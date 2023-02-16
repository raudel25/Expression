namespace Expression;

public class Sum<T> : BinaryExpression<T>
{
    public Sum(ExpressionType<T> left, ExpressionType<T> right) : base(left, right)
    {
    }

    public override int Priority => 1;

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right)
    {
        return left.Derivative(variable) + right.Derivative(variable);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Sum(left, right);
    }

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right)
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

    public override string ToString()
    {
        if (Left.ToString() == "0") return Right.ToString()!;
        if (Right.ToString() == "0") return Left.ToString()!;

        var (left, right) = DeterminatePriority();

        var rightOpposite = right[0] == '-';
        if (rightOpposite) return $"{left} - {right.Substring(1, right.Length - 1)}";

        return $"{left} + {right}";
    }
}

public class Subtraction<T> : BinaryExpression<T>
{
    public Subtraction(ExpressionType<T> left, ExpressionType<T> right) : base(left, right)
    {
    }

    public override int Priority => 1;

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right)
    {
        return left.Derivative(variable) - right.Derivative(variable);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Subtraction(left, right);
    }

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right)
    {
        return left - right;
    }

    protected override bool IsBinaryImplement()
    {
        return !(Left.ToString() == "0" || Right.ToString() == "0");
    }

    public override string ToString()
    {
        if (Left.ToString() == "0") return Aux<T>.Opposite(Right);
        if (Right.ToString() == "0") return Left.ToString()!;

        var (left, right) = DeterminatePriority();

        var rightOpposite = right[0] == '-';
        if (rightOpposite) return $"{left} + {right.Substring(1, right.Length - 1)}";

        return $"{left} - {right}";
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
}

public class Multiply<T> : BinaryExpression<T>
{
    public Multiply(ExpressionType<T> left, ExpressionType<T> right) : base(left, right)
    {
    }

    public override int Priority => 2;

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right)
    {
        return left.Derivative(variable) * right + left * right.Derivative(variable);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Multiply(left, right);
    }

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right)
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

    public override string ToString()
    {
        if (Left.ToString() == "0" || Right.ToString() == "0") return "0";
        if (Left.ToString() == "1") return Right.ToString()!;
        if (Right.ToString() == "1") return Left.ToString()!;
        if (Left.ToString() == "-1" && Right.ToString() == "-1") return "1";
        if (Left.ToString() == "-1") return Aux<T>.Opposite(Right);
        if (Right.ToString() == "-1") return Aux<T>.Opposite(Left);

        var (left, right) = DeterminatePriority();

        var (leftOpposite, rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite && rightOpposite)
            return $"{left.Substring(1, left.Length - 1)} * {right.Substring(1, right.Length - 1)}";
        if (rightOpposite) return $"{left} * {Aux<T>.Colocated(right)}";

        return $"{left} * {right}";
    }
}

public class Division<T> : BinaryExpression<T>
{
    public Division(ExpressionType<T> left, ExpressionType<T> right) : base(left, right)
    {
    }

    public override int Priority => 2;

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right)
    {
        return (left.Derivative(variable) * right - left * right.Derivative(variable)) / (right * right);
    }

    protected override T Evaluate(T left, T right)
    {
        return Arithmetic.Division(left, right);
    }

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right)
    {
        return left / right;
    }

    protected override bool IsBinaryImplement()
    {
        return !(Left.ToString() == "0" || Right.ToString() == "1");
    }

    public override string ToString()
    {
        if (Left.ToString() == "0") return "0";
        if (Right.ToString() == "1") return Left.ToString()!;

        var (left, right) = DeterminatePriority();

        if (Right is Division<T>) right = Aux<T>.Colocated(right);

        var (leftOpposite, rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite && rightOpposite)
            return $"{left.Substring(1, left.Length - 1)} / {right.Substring(1, right.Length - 1)}";
        if (rightOpposite) return $"{left} / {Aux<T>.Colocated(right)}";

        return $"{left} / {right}";
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
}