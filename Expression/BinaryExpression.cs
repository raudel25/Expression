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
    public Subtraction(ExpressionValue left, ExpressionValue right) : base(left, right)
    {
    }

    protected override ExpressionValue Derivative(ExpressionValue left, ExpressionValue right) =>
        left.Derivative() - right.Derivative();

    protected override Numbers Evaluate(Numbers left, Numbers right) => left - right;

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
    public Multiply(ExpressionValue left, ExpressionValue right) : base(left, right)
    {
    }

    protected override ExpressionValue Derivative(ExpressionValue left, ExpressionValue right) =>
        left.Derivative() * right + left * right.Derivative();

    protected override Numbers Evaluate(Numbers left, Numbers right) => left * right;

    public override int Priority
    {
        get => 3;
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
    public Division(ExpressionValue left, ExpressionValue right) : base(left, right)
    {
    }

    protected override ExpressionValue Derivative(ExpressionValue left, ExpressionValue right) =>
        left.Derivative() * right - left * right.Derivative();

    protected override Numbers Evaluate(Numbers left, Numbers right) => left * right;

    public override int Priority
    {
        get => 2;
    }

    public override string ToString()
    {
        if (this.Left.ToString() == "0") return "0";
        if (this.Right.ToString() == "1") return this.Left.ToString()!;

        (string left, string right) = this.DeterminatePriority();

        Aux.IsBinary(this.Left, ref left);
        Aux.IsBinary(this.Right, ref right);

        (bool leftOpposite, bool rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite && rightOpposite)
            return left.Substring(1, left.Length - 1) + " / " + right.Substring(1, right.Length - 1);
        if (rightOpposite) return left + " / " + Aux.Colocated(right);

        return left + " / " + right;
    }
}

public class Pow : BinaryExpression
{
    public Pow(ExpressionValue left, ExpressionValue right) : base(left, right)
    {
    }

    protected override ExpressionValue Derivative(ExpressionValue left, ExpressionValue right) =>
        throw new NotImplementedException();

    protected override Numbers Evaluate(Numbers left, Numbers right) => throw new NotImplementedException();

    public override int Priority
    {
        get => 2;
    }

    public override string ToString()
    {
        if (this.Left.ToString() == "0") return "0";
        if (this.Left.ToString() == "1" || this.Right.ToString() == "0") return "1";

        (string left, string right) = this.DeterminatePriority();

        Aux.IsBinary(this.Left, ref left);
        Aux.IsBinary(this.Right, ref right);

        (bool leftOpposite, bool rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite) return Aux.Colocated(left) + " ^ " + right;
        if (rightOpposite) return left + " ^ " + Aux.Colocated(right);

        return left + " ^ " + right;
    }
}

public class Logarithm : BinaryExpression
{
    public Logarithm(ExpressionValue left, ExpressionValue right) : base(left, right)
    {
    }

    protected override ExpressionValue Derivative(ExpressionValue left, ExpressionValue right)
    {
        throw new NotImplementedException();
    }

    protected override Numbers Evaluate(Numbers left, Numbers right)
    {
        throw new NotImplementedException();
    }

    public override int Priority
    {
        get => 2;
    }
}