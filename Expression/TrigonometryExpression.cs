using BigNum;

namespace Expression;

public class Sin : UnaryExpression
{
    public Sin(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) => new Cos(value);

    protected override RealNumbers Evaluate(RealNumbers value) => BigNumMath.Sin(value);

    protected override ExpressionType EvaluateExpression(ExpressionType value) => new Sin(value);

    public override string ToString() => $"sin({this.Value})";
}

public class Cos : UnaryExpression
{
    public Cos(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) =>
        new NumberExpression(RealNumbers.RealN1) * new Sin(value);

    protected override RealNumbers Evaluate(RealNumbers value) => BigNumMath.Cos(value);

    protected override ExpressionType EvaluateExpression(ExpressionType value) => new Cos(value);

    public override string ToString() => $"cos({this.Value})";
}

public class Tan : UnaryExpression
{
    public Tan(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) =>
        Pow.DeterminatePow(new Sec(value), new NumberExpression(new RealNumbers("2")));

    protected override RealNumbers Evaluate(RealNumbers value) => BigNumMath.Sin(value) / BigNumMath.Cos(value);

    protected override ExpressionType EvaluateExpression(ExpressionType value) => new Tan(value);

    public override string ToString() => $"tan({this.Value})";
}

public class Cot : UnaryExpression
{
    public Cot(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) =>
        Pow.DeterminatePow(new Csc(value), new NumberExpression(new RealNumbers("2")));

    protected override RealNumbers Evaluate(RealNumbers value) => BigNumMath.Cos(value) / BigNumMath.Sin(value);

    protected override ExpressionType EvaluateExpression(ExpressionType value) => new Cot(value);

    public override string ToString() => $"cot({this.Value})";
}

public class Sec : UnaryExpression
{
    public Sec(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) => new Sec(value) * new Tan(value);

    protected override RealNumbers Evaluate(RealNumbers value) => RealNumbers.Real1 / BigNumMath.Cos(value);

    protected override ExpressionType EvaluateExpression(ExpressionType value) => new Sec(value);

    public override string ToString() => $"sec({this.Value})";
}

public class Csc : UnaryExpression
{
    public Csc(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value) => -new Csc(value) * new Cot(value);

    protected override RealNumbers Evaluate(RealNumbers value) => RealNumbers.Real1 / BigNumMath.Sin(value);

    protected override ExpressionType EvaluateExpression(ExpressionType value) => new Csc(value);

    public override string ToString() => $"csc({this.Value})";
}

public class Asin : UnaryExpression
{
    public Asin(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value)
        => new NumberExpression(RealNumbers.Real1) /
           Pow.DeterminatePow(new NumberExpression(RealNumbers.Real1) - value,
               new NumberExpression(new RealNumbers("0.5")));

    protected override RealNumbers Evaluate(RealNumbers value) => BigNumMath.Asin(value);

    protected override ExpressionType EvaluateExpression(ExpressionType value) => new Asin(value);

    public override string ToString() => $"arcsin({this.Value})";
}

public class Acos : UnaryExpression
{
    public Acos(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value)
        => -new NumberExpression(RealNumbers.Real1) /
           Pow.DeterminatePow(new NumberExpression(RealNumbers.Real1) - value,
               new NumberExpression(new RealNumbers("0.5")));

    protected override RealNumbers Evaluate(RealNumbers value) => BigNumMath.Acos(value);

    protected override ExpressionType EvaluateExpression(ExpressionType value) => new Acos(value);

    public override string ToString() => $"arccos({this.Value})";
}

public class Atan : UnaryExpression
{
    public Atan(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value)
        => new NumberExpression(RealNumbers.Real1) /
           (new NumberExpression(RealNumbers.Real1) + Pow.DeterminatePow(value,
               new NumberExpression(new IntegerNumbers("2"))));

    protected override RealNumbers Evaluate(RealNumbers value) => BigNumMath.Atan(value);

    protected override ExpressionType EvaluateExpression(ExpressionType value) => new Atan(value);

    public override string ToString() => $"arctan({this.Value})";
}

public class Acot : UnaryExpression
{
    public Acot(ExpressionType value) : base(value)
    {
    }

    protected override ExpressionType Derivative(ExpressionType value)
        => -new NumberExpression(RealNumbers.Real1) /
           (new NumberExpression(RealNumbers.Real1) + Pow.DeterminatePow(value,
               new NumberExpression(new IntegerNumbers("2"))));

    protected override RealNumbers Evaluate(RealNumbers value) => BigNumMath.Acot(value);

    protected override ExpressionType EvaluateExpression(ExpressionType value) => new Acot(value);

    public override string ToString() => $"arccot({this.Value})";
}