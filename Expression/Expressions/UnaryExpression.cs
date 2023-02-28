namespace Expression.Expressions;

public class NumberExpression<T> : Function<T>
{
    public readonly T Value;

    internal NumberExpression(T value, IArithmetic<T> arithmetic) : base(arithmetic)
    {
        Value = value;
    }

    public override int Priority => 6;

    public override Function<T> Derivative(char variable)
    {
        return new NumberExpression<T>(Arithmetic.Real0, Arithmetic);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Value;
    }

    public override Function<T> EvaluateExpression(List<(char, Function<T>)> variables)
    {
        return this;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not NumberExpression<T> exp) return false;
        return exp.Value is not null && exp.Value.Equals(Value);
    }

    public override int GetHashCode()
    {
        if (Value is null) return 0;
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        if (Value is null) return "";
        return Value.ToString() is null ? "" : Value.ToString()!;
    }

    public override string ToLatex()
    {
        return ToString();
    }
}

public class VariableExpression<T> : Function<T>
{
    public readonly char Variable;

    internal VariableExpression(char variable, IArithmetic<T> arithmetic) : base(arithmetic)
    {
        Variable = variable;
    }

    public override int Priority => 6;

    public override Function<T> Derivative(char variable)
    {
        return variable == Variable
            ? new NumberExpression<T>(Arithmetic.Real1, Arithmetic)
            : new NumberExpression<T>(Arithmetic.Real0, Arithmetic);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        foreach (var item in variables)
            if (item.Item1 == Variable)
                return item.Item2;

        throw new Exception("No se ha introducido un valor para cada variable");
    }

    public override Function<T> EvaluateExpression(List<(char, Function<T>)> variables)
    {
        foreach (var item in variables)
            if (item.Item1 == Variable)
                return item.Item2;

        return this;
    }

    public override bool Equals(object? obj)
    {
        var exp = obj as VariableExpression<T>;
        if (exp is null) return false;

        return exp.Variable == Variable;
    }

    public override int GetHashCode()
    {
        return Variable.GetHashCode();
    }

    public override string ToString()
    {
        return Variable.ToString();
    }

    public override string ToLatex()
    {
        return ToString();
    }
}

public class ConstantE<T> : Function<T>
{
    internal ConstantE(IArithmetic<T> arithmetic) : base(arithmetic)
    {
    }

    public override int Priority => 6;

    public override Function<T> Derivative(char variable)
    {
        return new NumberExpression<T>(Arithmetic.Real0, Arithmetic);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Arithmetic.E;
    }

    public override Function<T> EvaluateExpression(List<(char, Function<T>)> variables)
    {
        return this;
    }

    public override bool Equals(object? obj)
    {
        return obj is ConstantE<T>;
    }

    public override int GetHashCode()
    {
        return (int)Math.E;
    }

    public override string ToString()
    {
        return "e";
    }

    public override string ToLatex()
    {
        return ToString();
    }
}

public class ConstantPI<T> : Function<T>
{
    internal ConstantPI(IArithmetic<T> arithmetic) : base(arithmetic)
    {
    }

    public override int Priority => 6;

    public override Function<T> Derivative(char variable)
    {
        return new NumberExpression<T>(Arithmetic.Real0, Arithmetic);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Arithmetic.PI;
    }

    public override Function<T> EvaluateExpression(List<(char, Function<T>)> variables)
    {
        return this;
    }

    public override bool Equals(object? obj)
    {
        return obj is ConstantPI<T>;
    }

    public override int GetHashCode()
    {
        return (int)Math.PI;
    }

    public override string ToString()
    {
        return "pi";
    }

    public override string ToLatex()
    {
        return "\\pi";
    }
}

public class Factorial<T> : Function<T>
{
    private readonly T _integer;

    private T? _value;

    internal Factorial(T value, IArithmetic<T> arithmetic) : base(arithmetic)
    {
        _integer = value;
        _value = default;
    }

    public T Value
    {
        get
        {
            if (_value is null || _value.Equals(default(T))) _value = Arithmetic.Factorial(_integer);

            return _value;
        }
        set
        {
            if (_value is null || _value.Equals(default(T))) _value = value;
        }
    }

    public override int Priority => 5;

    public override Function<T> Derivative(char variable)
    {
        return new NumberExpression<T>(Arithmetic.Real0, Arithmetic);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Value;
    }

    public override Function<T> EvaluateExpression(List<(char, Function<T>)> variables)
    {
        return this;
    }

    public override bool Equals(object? obj)
    {
        var exp = obj as NumberExpression<T>;
        if (exp is null) return false;
        if (exp.Value is null) return false;

        return exp.Value.Equals(Value);
    }

    public override int GetHashCode()
    {
        if (Value is null) return 0;
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return $"{_integer}!";
    }

    public override string ToLatex()
    {
        return ToString();
    }
}