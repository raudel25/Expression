using BigNum;

namespace Expression;

public class Pow : BinaryExpression
{
    public static Pow DeterminatePow(ExpressionType left, ExpressionType right)
    {
        (VariableExpression? variable, NumberExpression? number) =
            (left as VariableExpression, right as NumberExpression);
        if (variable != null && number != null) return new PowVariable(variable, number);

        ConstantE? e = left as ConstantE;
        if (e != null) return new PowE(right);

        return new Pow(left, right);
    }

    public Pow(ExpressionType left, ExpressionType right) : base(left, right)
    {
    }

    protected override ExpressionType Derivative(ExpressionType left, ExpressionType right) =>
        new PowE(right * new Ln(left)) * (right * new Ln(left)).Derivative();

    protected override RealNumbers EvaluateBinary(RealNumbers left, RealNumbers right) => BigNumMath.Pow(left, right);

    protected override bool IsBinaryImplement() =>
        !(this.Left.ToString() == "0" || this.Right.ToString() == "0" || this.Left.ToString() == "1");

    public override int Priority
    {
        get => 4;
    }

    public override string ToString()
    {
        if (this.Left.ToString() == "0") return "0";
        if (this.Left.ToString() == "1" || this.Right.ToString() == "0") return "1";

        (string left, string right) = this.DeterminatePriority();

        // if (this.Left.IsBinary()) left = Aux.Colocated(left);
        // if (this.Right.IsBinary()) right = Aux.Colocated(right);

        (bool leftOpposite, bool rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite) return $"{Aux.Colocated(left)} ^ {right}";
        if (rightOpposite) return $"{left} ^ {Aux.Colocated(right)}";

        return $"{left} ^ {right}";
    }
}

public class PowVariable : Pow
{
    public PowVariable(VariableExpression variable, NumberExpression number) : base(variable, number)
    {
    }

    protected override ExpressionType Derivative(ExpressionType left, ExpressionType right) =>
        right * new PowVariable((VariableExpression) left,
            new NumberExpression(right.Evaluate(RealNumbers.Real0) - RealNumbers.Real1));

    public override int Priority
    {
        get => 7;
    }
}

public class PowE : Pow
{
    public PowE(ExpressionType pow) : base(new ConstantE(), pow)
    {
    }

    protected override ExpressionType Derivative(ExpressionType left, ExpressionType right) =>
        this * right.Derivative();
}