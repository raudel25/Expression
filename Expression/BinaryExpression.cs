namespace Expression;

public class Sum<T> : BinaryExpression<T>
{
    public Sum(ExpressionType<T> left, ExpressionType<T> right) : base(left, right)
    {
    }

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right) =>
        left.Derivative(variable) + right.Derivative(variable);

    protected override T Evaluate(T left, T right) => Arithmetic.Sum(left, right);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right) =>
        left + right;

    protected override bool IsBinaryImplement() => !(this.Left.ToString() == "0" || this.Right.ToString() == "0");

    public override int Priority => 1;

    public override bool Equals(object? obj)
    {
        Sum<T>? binary = obj as Sum<T>;
        if (binary is null) return false;

        return (this.Left.Equals(binary.Left) && this.Right.Equals(binary.Right)) ||
               (this.Left.Equals(binary.Right) && this.Right.Equals(binary.Left));
    }

    public override int GetHashCode() => this.Left.GetHashCode() * this.Right.GetHashCode();

    public override string ToString()
    {
        if (this.Left.ToString() == "0") return this.Right.ToString()!;
        if (this.Right.ToString() == "0") return this.Left.ToString()!;

        (string left, string right) = this.DeterminatePriority();

        bool rightOpposite = right[0] == '-';
        if (rightOpposite) return $"{left} - {right.Substring(1, right.Length - 1)}";

        return $"{left} + {right}";
    }
}

public class Subtraction<T> : BinaryExpression<T>
{
    public Subtraction(ExpressionType<T> left, ExpressionType<T> right) : base(left, right)
    {
    }

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right) =>
        left.Derivative(variable) - right.Derivative(variable);

    protected override T Evaluate(T left, T right) => Arithmetic.Subtraction(left, right);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right) =>
        left - right;

    protected override bool IsBinaryImplement() => !(this.Left.ToString() == "0" || this.Right.ToString() == "0");

    public override int Priority => 1;

    public override string ToString()
    {
        if (this.Left.ToString() == "0") return Aux<T>.Opposite(this.Right);
        if (this.Right.ToString() == "0") return this.Left.ToString()!;

        (string left, string right) = this.DeterminatePriority();

        bool rightOpposite = right[0] == '-';
        if (rightOpposite) return $"{left} + {right.Substring(1, right.Length - 1)}";

        return $"{left} - {right}";
    }

    public override bool Equals(object? obj)
    {
        Subtraction<T>? binary = obj as Subtraction<T>;
        if (binary is null) return false;

        return this.Left.Equals(binary.Left) && this.Right.Equals(binary.Right);
    }

    public override int GetHashCode() => 2 * this.Left.GetHashCode() * this.Right.GetHashCode();
}

public class Multiply<T> : BinaryExpression<T>
{
    public Multiply(ExpressionType<T> left, ExpressionType<T> right) : base(left, right)
    {
    }

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right) =>
        left.Derivative(variable) * right + left * right.Derivative(variable);

    protected override T Evaluate(T left, T right) => Arithmetic.Multiply(left, right);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right) =>
        left * right;

    protected override bool IsBinaryImplement()
    {
        if (this.Left.ToString() == "0" || this.Right.ToString() == "0") return false;
        if (this.Left.ToString() == "1" || this.Right.ToString() == "1") return false;
        if (this.Left.ToString() == "-1" || this.Right.ToString() == "-1") return false;

        return true;
    }

    public override int Priority => 2;

    public override bool Equals(object? obj)
    {
        Multiply<T>? binary = obj as Multiply<T>;
        if (binary is null) return false;

        return (this.Left.Equals(binary.Left) && this.Right.Equals(binary.Right)) ||
               (this.Left.Equals(binary.Right) && this.Right.Equals(binary.Left));
    }

    public override int GetHashCode() => 3 * this.Left.GetHashCode() * this.Right.GetHashCode();

    public override string ToString()
    {
        if (this.Left.ToString() == "0" || this.Right.ToString() == "0") return "0";
        if (this.Left.ToString() == "1") return this.Right.ToString()!;
        if (this.Right.ToString() == "1") return this.Left.ToString()!;
        if (this.Left.ToString() == "-1" && this.Right.ToString() == "-1") return "1";
        if (this.Left.ToString() == "-1") return Aux<T>.Opposite(this.Right);
        if (this.Right.ToString() == "-1") return Aux<T>.Opposite(this.Left);

        (string left, string right) = this.DeterminatePriority();

        (bool leftOpposite, bool rightOpposite) = (left[0] == '-', right[0] == '-');

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

    protected override ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right) =>
        (left.Derivative(variable) * right - left * right.Derivative(variable)) / (right * right);

    protected override T Evaluate(T left, T right) => Arithmetic.Division(left, right);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right) =>
        left / right;

    protected override bool IsBinaryImplement() => !(this.Left.ToString() == "0" || this.Right.ToString() == "1");

    public override int Priority => 2;

    public override string ToString()
    {
        if (this.Left.ToString() == "0") return "0";
        if (this.Right.ToString() == "1") return this.Left.ToString()!;

        (string left, string right) = this.DeterminatePriority();

        if (this.Right is Division<T>) right = Aux<T>.Colocated(right);

        (bool leftOpposite, bool rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite && rightOpposite)
            return $"{left.Substring(1, left.Length - 1)} / {right.Substring(1, right.Length - 1)}";
        if (rightOpposite) return $"{left} / {Aux<T>.Colocated(right)}";

        return $"{left} / {right}";
    }

    public override bool Equals(object? obj)
    {
        Division<T>? binary = obj as Division<T>;
        if (binary is null) return false;

        return this.Left.Equals(binary.Left) && this.Right.Equals(binary.Right);
    }

    public override int GetHashCode() => 4 * this.Left.GetHashCode() * this.Right.GetHashCode();
}