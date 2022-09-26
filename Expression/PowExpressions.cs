using BigNum;

namespace Expression;

public class Pow : BinaryExpression
{
    public static Pow DeterminatePow(ExpressionType left, ExpressionType right)
    {
        NumberExpression? number = right as NumberExpression;
        if (number != null) return new PowExponentNumber(left, number);

        ConstantE? e = left as ConstantE;
        if (e != null) return new PowE(right);

        return new Pow(left, right);
    }

    public Pow(ExpressionType left, ExpressionType right) : base(left, right)
    {
    }

    protected override ExpressionType Derivative(char variable, ExpressionType left, ExpressionType right) =>
        new PowE(right * new Ln(left)) * (right * new Ln(left)).Derivative(variable);

    protected override RealNumbers Evaluate(RealNumbers left, RealNumbers right) => BigNumMath.Pow(left, right);

    protected override ExpressionType EvaluateExpression(ExpressionType left, ExpressionType right) =>
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

        if (this.Right is Pow) right = Aux.Colocated(right);

        (bool leftOpposite, bool rightOpposite) = (left[0] == '-', right[0] == '-');

        if (leftOpposite) return $"{Aux.Colocated(left)} ^ {right}";
        if (rightOpposite) return $"{left} ^ {Aux.Colocated(right)}";

        return $"{left} ^ {right}";
    }

    public override bool Equals(object? obj)
    {
        Pow? binary = obj as Pow;
        if (binary is null) return false;

        return this.Left.Equals(binary.Left) && this.Right.Equals(binary.Right);
    }

    public override int GetHashCode() => 6 * this.Left.GetHashCode() * this.Right.GetHashCode();
}

public class PowExponentNumber : Pow
{
    public PowExponentNumber(ExpressionType exp, NumberExpression number) : base(exp, number)
    {
    }

    protected override ExpressionType Derivative(char variable, ExpressionType left, ExpressionType right) => right *
        new PowExponentNumber(left, new NumberExpression(((NumberExpression) right).Value - RealNumbers.Real1)) *
        left.Derivative(variable);
}

public class PowE : Pow
{
    public PowE(ExpressionType pow) : base(new ConstantE(), pow)
    {
    }

    protected override ExpressionType Derivative(char variable, ExpressionType left, ExpressionType right) =>
        this * right.Derivative(variable);
}