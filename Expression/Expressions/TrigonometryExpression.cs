namespace Expression.Expressions;

public class Sin<T> : UnaryExpression<T>
{
    public Sin(Function<T> value) : base(value)
    {
    }

    protected override Function<T> Derivative(Function<T> value)
    {
        return new Cos<T>(value);
    }

    protected override T Evaluate(T value)
    {
        return Arithmetic.Sin(value);
    }

    protected override Function<T> EvaluateExpression(Function<T> value)
    {
        return new Sin<T>(value);
    }

    public override string ToString()
    {
        return $"sin({Value})";
    }

    public override bool Equals(object? obj)
    {
        var unary = obj as Sin<T>;
        if (unary is null) return false;

        return Value.Equals(unary.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}

public class Cos<T> : UnaryExpression<T>
{
    public Cos(Function<T> value) : base(value)
    {
    }

    protected override Function<T> Derivative(Function<T> value)
    {
        return new NumberExpression<T>(Arithmetic.RealN1, Arithmetic) * new Sin<T>(value);
    }

    protected override T Evaluate(T value)
    {
        return Arithmetic.Cos(value);
    }

    protected override Function<T> EvaluateExpression(Function<T> value)
    {
        return new Cos<T>(value);
    }

    public override string ToString()
    {
        return $"cos({Value})";
    }

    public override bool Equals(object? obj)
    {
        var unary = obj as Cos<T>;
        if (unary is null) return false;

        return Value.Equals(unary.Value);
    }

    public override int GetHashCode()
    {
        return 2 * Value.GetHashCode();
    }
}

public class Tan<T> : UnaryExpression<T>
{
    public Tan(Function<T> value) : base(value)
    {
    }

    protected override Function<T> Derivative(Function<T> value)
    {
        return Pow<T>.DeterminatePow(new Sec<T>(value),
            new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic));
    }

    protected override T Evaluate(T value)
    {
        return Arithmetic.Division(Arithmetic.Sin(value), Arithmetic.Cos(value));
    }

    protected override Function<T> EvaluateExpression(Function<T> value)
    {
        return new Tan<T>(value);
    }

    public override string ToString()
    {
        return $"tan({Value})";
    }

    public override bool Equals(object? obj)
    {
        var unary = obj as Tan<T>;
        if (unary is null) return false;

        return Value.Equals(unary.Value);
    }

    public override int GetHashCode()
    {
        return 3 * Value.GetHashCode();
    }
}

public class Cot<T> : UnaryExpression<T>
{
    public Cot(Function<T> value) : base(value)
    {
    }

    protected override Function<T> Derivative(Function<T> value)
    {
        return Pow<T>.DeterminatePow(new Csc<T>(value),
            new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic));
    }

    protected override T Evaluate(T value)
    {
        return Arithmetic.Division(Arithmetic.Cos(value), Arithmetic.Sin(value));
    }

    protected override Function<T> EvaluateExpression(Function<T> value)
    {
        return new Cot<T>(value);
    }

    public override string ToString()
    {
        return $"cot({Value})";
    }

    public override bool Equals(object? obj)
    {
        var unary = obj as Cot<T>;
        if (unary is null) return false;

        return Value.Equals(unary.Value);
    }

    public override int GetHashCode()
    {
        return 4 * Value.GetHashCode();
    }
}

public class Sec<T> : UnaryExpression<T>
{
    public Sec(Function<T> value) : base(value)
    {
    }

    protected override Function<T> Derivative(Function<T> value)
    {
        return new Sec<T>(value) * new Tan<T>(value);
    }

    protected override T Evaluate(T value)
    {
        return Arithmetic.Division(Arithmetic.Real1, Arithmetic.Cos(value));
    }

    protected override Function<T> EvaluateExpression(Function<T> value)
    {
        return new Sec<T>(value);
    }

    public override string ToString()
    {
        return $"sec({Value})";
    }

    public override bool Equals(object? obj)
    {
        var unary = obj as Sec<T>;
        if (unary is null) return false;

        return Value.Equals(unary.Value);
    }

    public override int GetHashCode()
    {
        return 5 * Value.GetHashCode();
    }
}

public class Csc<T> : UnaryExpression<T>
{
    public Csc(Function<T> value) : base(value)
    {
    }

    protected override Function<T> Derivative(Function<T> value)
    {
        return -new Csc<T>(value) * new Cot<T>(value);
    }

    protected override T Evaluate(T value)
    {
        return Arithmetic.Division(Arithmetic.Real1, Arithmetic.Sin(value));
    }

    protected override Function<T> EvaluateExpression(Function<T> value)
    {
        return new Csc<T>(value);
    }

    public override string ToString()
    {
        return $"csc({Value})";
    }

    public override bool Equals(object? obj)
    {
        var unary = obj as Csc<T>;
        if (unary is null) return false;

        return Value.Equals(unary.Value);
    }

    public override int GetHashCode()
    {
        return 6 * Value.GetHashCode();
    }
}

public class Asin<T> : UnaryExpression<T>
{
    public Asin(Function<T> value) : base(value)
    {
    }

    protected override Function<T> Derivative(Function<T> value)
    {
        return new NumberExpression<T>(Arithmetic.Real1, Arithmetic) /
               Pow<T>.DeterminatePow(
                   new NumberExpression<T>(Arithmetic.Real1, Arithmetic) -
                   Pow<T>.DeterminatePow(value, new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic)),
                   new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic));
    }

    protected override T Evaluate(T value)
    {
        return Arithmetic.Asin(value);
    }

    protected override Function<T> EvaluateExpression(Function<T> value)
    {
        return new Asin<T>(value);
    }

    public override string ToString()
    {
        return $"arcsin({Value})";
    }

    public override bool Equals(object? obj)
    {
        var unary = obj as Asin<T>;
        if (unary is null) return false;

        return Value.Equals(unary.Value);
    }

    public override int GetHashCode()
    {
        return 7 * Value.GetHashCode();
    }
}

public class Acos<T> : UnaryExpression<T>
{
    public Acos(Function<T> value) : base(value)
    {
    }

    protected override Function<T> Derivative(Function<T> value)
    {
        return -new NumberExpression<T>(Arithmetic.Real1, Arithmetic) /
               Pow<T>.DeterminatePow(
                   new NumberExpression<T>(Arithmetic.Real1, Arithmetic) -
                   Pow<T>.DeterminatePow(value, new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic)),
                   new NumberExpression<T>(Arithmetic.StringToNumber("0.5"), Arithmetic));
    }

    protected override T Evaluate(T value)
    {
        return Arithmetic.Acos(value);
    }

    protected override Function<T> EvaluateExpression(Function<T> value)
    {
        return new Acos<T>(value);
    }

    public override string ToString()
    {
        return $"arccos({Value})";
    }

    public override bool Equals(object? obj)
    {
        var unary = obj as Acos<T>;
        if (unary is null) return false;

        return Value.Equals(unary.Value);
    }

    public override int GetHashCode()
    {
        return 8 * Value.GetHashCode();
    }
}

public class Atan<T> : UnaryExpression<T>
{
    public Atan(Function<T> value) : base(value)
    {
    }

    protected override Function<T> Derivative(Function<T> value)
    {
        return new NumberExpression<T>(Arithmetic.Real1, Arithmetic) /
               (new NumberExpression<T>(Arithmetic.Real1, Arithmetic) + Pow<T>.DeterminatePow(value,
                   new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic)));
    }

    protected override T Evaluate(T value)
    {
        return Arithmetic.Atan(value);
    }

    protected override Function<T> EvaluateExpression(Function<T> value)
    {
        return new Atan<T>(value);
    }

    public override string ToString()
    {
        return $"arctan({Value})";
    }

    public override bool Equals(object? obj)
    {
        var unary = obj as Atan<T>;
        if (unary is null) return false;

        return Value.Equals(unary.Value);
    }

    public override int GetHashCode()
    {
        return 9 * Value.GetHashCode();
    }
}

public class Acot<T> : UnaryExpression<T>
{
    public Acot(Function<T> value) : base(value)
    {
    }

    protected override Function<T> Derivative(Function<T> value)
    {
        return -new NumberExpression<T>(Arithmetic.Real1, Arithmetic) /
               (new NumberExpression<T>(Arithmetic.Real1, Arithmetic) + Pow<T>.DeterminatePow(value,
                   new NumberExpression<T>(Arithmetic.StringToNumber("2"), Arithmetic)));
    }

    protected override T Evaluate(T value)
    {
        return Arithmetic.Acot(value);
    }

    protected override Function<T> EvaluateExpression(Function<T> value)
    {
        return new Acot<T>(value);
    }

    public override string ToString()
    {
        return $"arccot({Value})";
    }

    public override bool Equals(object? obj)
    {
        var unary = obj as Acot<T>;
        if (unary is null) return false;

        return Value.Equals(unary.Value);
    }

    public override int GetHashCode()
    {
        return 10 * Value.GetHashCode();
    }
}