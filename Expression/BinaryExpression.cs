using BigNum;

namespace Expression;

public class Sum : BinaryExpression
{
    public Sum(ExpressionType left, ExpressionType right) : base(left, right)
    {
    }

    protected override ExpressionType Derivative(ExpressionType left, ExpressionType right) =>
        left.Derivative() + right.Derivative();

    protected override RealNumbers EvaluateBinary(RealNumbers left, RealNumbers right) => left + right;

    protected override bool IsBinaryImplement() => !(this.Left.ToString() == "0" || this.Right.ToString() == "0");

    public override int Priority
    {
        get => 1;
    }

    public override string ToString()
    {
        if (this.Left.ToString() == "0") return this.Right.ToString()!;
        if (this.Right.ToString() == "0") return this.Left.ToString()!;

        (string left, string right) = this.DeterminatePriority();

        bool rightOpposite = right[0] == '-';
        if (rightOpposite) return left + " - " + right.Substring(1, right.Length - 1);

        return left + " + " + right;
    }
}

public class Subtraction : BinaryExpression
{
    public Subtraction(ExpressionType left, ExpressionType right) : base(left, right)
    {
    }

    protected override ExpressionType Derivative(ExpressionType left, ExpressionType right) =>
        left.Derivative() - right.Derivative();

    protected override RealNumbers EvaluateBinary(RealNumbers left, RealNumbers right) => left - right;

    protected override bool IsBinaryImplement() => !(this.Left.ToString() == "0" || this.Right.ToString() == "0");

    public override int Priority
    {
        get => 1;
    }

    public override string ToString()
    {
        if (this.Left.ToString() == "0") return Aux.Opposite(this.Right);
        if (this.Right.ToString() == "0") return this.Left.ToString()!;

        (string left, string right) = this.DeterminatePriority();

        bool rightOpposite = right[0] == '-';
        if (rightOpposite) return left + " + " + right.Substring(1, right.Length - 1);

        return left + " - " + right;
    }
}

public class Multiply : BinaryExpression
{
    public Multiply(ExpressionType left, ExpressionType right) : base(left, right)
    {
    }

    protected override ExpressionType Derivative(ExpressionType left, ExpressionType right) =>
        left.Derivative() * right + left * right.Derivative();

    protected override RealNumbers EvaluateBinary(RealNumbers left, RealNumbers right) => left * right;

    protected override bool IsBinaryImplement()
    {
        if (this.Left.ToString() == "0" || this.Right.ToString() == "0") return false;
        if (this.Left.ToString() == "1" || this.Right.ToString() == "1") return false;
        if (this.Left.ToString() == "-1" || this.Right.ToString() == "-1") return false;

        return true;
    }

    public override int Priority
    {
        get => 2;
    }

    public override string ToString()
    {
        if (this.Left.ToString() == "0" || this.Right.ToString() == "0") return "0";
        if (this.Left.ToString() == "1") return this.Right.ToString()!;
        if (this.Right.ToString() == "1") return this.Left.ToString()!;
        if (this.Left.ToString() == "-1" && this.Right.ToString() == "-1") return "1";
        if (this.Left.ToString() == "-1") return Aux.Opposite(this.Right);
        if (this.Right.ToString() == "-1") return Aux.Opposite(this.Left);

        (string left, string right) = this.DeterminatePriority();

        (bool leftOpposite, bool rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite && rightOpposite)
            return left.Substring(1, left.Length - 1) + " * " + right.Substring(1, right.Length - 1);
        if (rightOpposite) return left + " * " + Aux.Colocated(right);

        return left + " * " + right;
    }
}

public class Division : BinaryExpression
{
    public Division(ExpressionType left, ExpressionType right) : base(left, right)
    {
    }

    protected override ExpressionType Derivative(ExpressionType left, ExpressionType right) =>
        left.Derivative() * right - left * right.Derivative();

    protected override RealNumbers EvaluateBinary(RealNumbers left, RealNumbers right) => left / right;

    protected override bool IsBinaryImplement() => !(this.Left.ToString() == "0" || this.Right.ToString() == "1");

    public override int Priority
    {
        get => 2;
    }

    public override string ToString()
    {
        if (this.Left.ToString() == "0") return "0";
        if (this.Right.ToString() == "1") return this.Left.ToString()!;

        (string left, string right) = this.DeterminatePriority();

        if (this.Left.IsBinary()) left = Aux.Colocated(left);
        if (this.Right.IsBinary()) right = Aux.Colocated(right);

        (bool leftOpposite, bool rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite && rightOpposite)
            return left.Substring(1, left.Length - 1) + " / " + right.Substring(1, right.Length - 1);
        if (rightOpposite) return left + " / " + Aux.Colocated(right);

        return left + " / " + right;
    }
}

public class Logarithm : BinaryExpression
{
    public Logarithm(ExpressionType left, ExpressionType right) : base(left, right)
    {
    }

    protected override ExpressionType Derivative(ExpressionType left, ExpressionType right)
    {
        throw new NotImplementedException();
    }

    protected override RealNumbers EvaluateBinary(RealNumbers left, RealNumbers right)
    {
        throw new NotImplementedException();
    }

    protected override bool IsBinaryImplement() => throw new NotImplementedException();

    public override int Priority
    {
        get => 2;
    }
}