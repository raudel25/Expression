namespace Expression;

public class Sin<T> : UnaryExpression<T>
{
    public Sin(ExpressionType<T> value) : base(value)
    {
    }

    protected override ExpressionType<T> Derivative(ExpressionType<T> value) => new Cos<T>(value);

    protected override T Evaluate(T value) => Arithmetic.Sin(value);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> value) => new Sin<T>(value);

    public override string ToString() => $"sin({this.Value})";

    public override bool Equals(object? obj)
    {
        Sin<T>? unary = obj as Sin<T>;
        if (unary is null) return false;

        return this.Value.Equals(unary.Value);
    }

    public override int GetHashCode() => this.Value.GetHashCode();
}

public class Cos<T> : UnaryExpression<T>
{
    public Cos(ExpressionType<T> value) : base(value)
    {
    }

    protected override ExpressionType<T> Derivative(ExpressionType<T> value) =>
        new NumberExpression<T>(Arithmetic.RealN1, Arithmetic) * new Sin<T>(value);

    protected override T Evaluate(T value) => Arithmetic.Cos(value);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> value) => new Cos<T>(value);

    public override string ToString() => $"cos({this.Value})";

    public override bool Equals(object? obj)
    {
        Cos<T>? unary = obj as Cos<T>;
        if (unary is null) return false;

        return this.Value.Equals(unary.Value);
    }

    public override int GetHashCode() => 2 * this.Value.GetHashCode();
}

public class Tan<T> : UnaryExpression<T>
{
    public Tan(ExpressionType<T> value) : base(value)
    {
    }

    protected override ExpressionType<T> Derivative(ExpressionType<T> value) =>
        Pow<T>.DeterminatePow(new Sec<T>(value), new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic));

    protected override T Evaluate(T value) => Arithmetic.Division(Arithmetic.Sin(value), Arithmetic.Cos(value));

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> value) => new Tan<T>(value);

    public override string ToString() => $"tan({this.Value})";

    public override bool Equals(object? obj)
    {
        Tan<T>? unary = obj as Tan<T>;
        if (unary is null) return false;

        return this.Value.Equals(unary.Value);
    }

    public override int GetHashCode() => 3 * this.Value.GetHashCode();
}

public class Cot<T> : UnaryExpression<T>
{
    public Cot(ExpressionType<T> value) : base(value)
    {
    }

    protected override ExpressionType<T> Derivative(ExpressionType<T> value) =>
        Pow<T>.DeterminatePow(new Csc<T>(value), new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic));

    protected override T Evaluate(T value) => Arithmetic.Division(Arithmetic.Cos(value), Arithmetic.Sin(value));

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> value) => new Cot<T>(value);

    public override string ToString() => $"cot({this.Value})";

    public override bool Equals(object? obj)
    {
        Cot<T>? unary = obj as Cot<T>;
        if (unary is null) return false;

        return this.Value.Equals(unary.Value);
    }

    public override int GetHashCode() => 4 * this.Value.GetHashCode();
}

public class Sec<T> : UnaryExpression<T>
{
    public Sec(ExpressionType<T> value) : base(value)
    {
    }

    protected override ExpressionType<T> Derivative(ExpressionType<T> value) => new Sec<T>(value) * new Tan<T>(value);

    protected override T Evaluate(T value) => Arithmetic.Division(Arithmetic.Real1, Arithmetic.Cos(value));

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> value) => new Sec<T>(value);

    public override string ToString() => $"sec({this.Value})";

    public override bool Equals(object? obj)
    {
        Sec<T>? unary = obj as Sec<T>;
        if (unary is null) return false;

        return this.Value.Equals(unary.Value);
    }

    public override int GetHashCode() => 5 * this.Value.GetHashCode();
}

public class Csc<T> : UnaryExpression<T>
{
    public Csc(ExpressionType<T> value) : base(value)
    {
    }

    protected override ExpressionType<T> Derivative(ExpressionType<T> value) => -new Csc<T>(value) * new Cot<T>(value);

    protected override T Evaluate(T value) => Arithmetic.Division(Arithmetic.Real1, Arithmetic.Sin(value));

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> value) => new Csc<T>(value);

    public override string ToString() => $"csc({this.Value})";

    public override bool Equals(object? obj)
    {
        Csc<T>? unary = obj as Csc<T>;
        if (unary is null) return false;

        return this.Value.Equals(unary.Value);
    }

    public override int GetHashCode() => 6 * this.Value.GetHashCode();
}

public class Asin<T> : UnaryExpression<T>
{
    public Asin(ExpressionType<T> value) : base(value)
    {
    }

    protected override ExpressionType<T> Derivative(ExpressionType<T> value)
        => new NumberExpression<T>(Arithmetic.Real1, Arithmetic) /
           Pow<T>.DeterminatePow(
               new NumberExpression<T>(Arithmetic.Real1, Arithmetic) -
               Pow<T>.DeterminatePow(value, new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic)),
               new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic));

    protected override T Evaluate(T value) => Arithmetic.Asin(value);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> value) => new Asin<T>(value);

    public override string ToString() => $"arcsin({this.Value})";

    public override bool Equals(object? obj)
    {
        Asin<T>? unary = obj as Asin<T>;
        if (unary is null) return false;

        return this.Value.Equals(unary.Value);
    }

    public override int GetHashCode() => 7 * this.Value.GetHashCode();
}

public class Acos<T> : UnaryExpression<T>
{
    public Acos(ExpressionType<T> value) : base(value)
    {
    }

    protected override ExpressionType<T> Derivative(ExpressionType<T> value)
        => -new NumberExpression<T>(Arithmetic.Real1, Arithmetic) /
           Pow<T>.DeterminatePow(
               new NumberExpression<T>(Arithmetic.Real1, Arithmetic) -
               Pow<T>.DeterminatePow(value, new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic)),
               new NumberExpression<T>(Arithmetic.StringToNumber("0.5"), Arithmetic));

    protected override T Evaluate(T value) => Arithmetic.Acos(value);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> value) => new Acos<T>(value);

    public override string ToString() => $"arccos({this.Value})";

    public override bool Equals(object? obj)
    {
        Acos<T>? unary = obj as Acos<T>;
        if (unary is null) return false;

        return this.Value.Equals(unary.Value);
    }

    public override int GetHashCode() => 8 * this.Value.GetHashCode();
}

public class Atan<T> : UnaryExpression<T>
{
    public Atan(ExpressionType<T> value) : base(value)
    {
    }

    protected override ExpressionType<T> Derivative(ExpressionType<T> value)
        => new NumberExpression<T>(Arithmetic.Real1, Arithmetic) /
           (new NumberExpression<T>(Arithmetic.Real1, Arithmetic) + Pow<T>.DeterminatePow(value,
               new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic)));

    protected override T Evaluate(T value) => Arithmetic.Atan(value);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> value) => new Atan<T>(value);

    public override string ToString() => $"arctan({this.Value})";

    public override bool Equals(object? obj)
    {
        Atan<T>? unary = obj as Atan<T>;
        if (unary is null) return false;

        return this.Value.Equals(unary.Value);
    }

    public override int GetHashCode() => 9 * this.Value.GetHashCode();
}

public class Acot<T> : UnaryExpression<T>
{
    public Acot(ExpressionType<T> value) : base(value)
    {
    }

    protected override ExpressionType<T> Derivative(ExpressionType<T> value)
        => -new NumberExpression<T>(Arithmetic.Real1, Arithmetic) /
           (new NumberExpression<T>(Arithmetic.Real1, Arithmetic) + Pow<T>.DeterminatePow(value,
               new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic)));

    protected override T Evaluate(T value) => Arithmetic.Acot(value);

    protected override ExpressionType<T> EvaluateExpression(ExpressionType<T> value) => new Acot<T>(value);

    public override string ToString() => $"arccot({this.Value})";

    public override bool Equals(object? obj)
    {
        Acot<T>? unary = obj as Acot<T>;
        if (unary is null) return false;

        return this.Value.Equals(unary.Value);
    }

    public override int GetHashCode() => 10 * this.Value.GetHashCode();
}