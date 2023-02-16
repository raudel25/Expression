namespace Expression.Expressions;

public class NumberExpression<T> : ExpressionType<T>
{
    public readonly T Value;

    internal NumberExpression(T value, IArithmetic<T> arithmetic) : base(arithmetic)
    {
        Value = value;
    }

    public override int Priority => 6;

    public override ExpressionType<T> Derivative(char variable)
    {
        return new NumberExpression<T>(Arithmetic.Real0, Arithmetic);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Value;
    }

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables)
    {
        return this;
    }

    public override string ToString()
    {
        if (Value is null) return "";
        return Value.ToString() is null ? "" : Value.ToString()!;
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
}

public class VariableExpression<T> : ExpressionType<T>
{
    public readonly char Variable;

    internal VariableExpression(char variable, IArithmetic<T> arithmetic) : base(arithmetic)
    {
        Variable = variable;
    }

    public override int Priority => 6;

    public override ExpressionType<T> Derivative(char variable)
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

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables)
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
}

public class ConstantE<T> : ExpressionType<T>
{
    internal ConstantE(IArithmetic<T> arithmetic) : base(arithmetic)
    {
    }

    public override int Priority => 6;

    public override ExpressionType<T> Derivative(char variable)
    {
        return new NumberExpression<T>(Arithmetic.Real0, Arithmetic);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Arithmetic.E;
    }

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables)
    {
        return this;
    }

    public override string ToString()
    {
        return "e";
    }

    public override bool Equals(object? obj)
    {
        return obj is ConstantE<T>;
    }

    public override int GetHashCode()
    {
        return (int)Math.E;
    }
}

public class ConstantPI<T> : ExpressionType<T>
{
    internal ConstantPI(IArithmetic<T> arithmetic) : base(arithmetic)
    {
    }

    public override int Priority => 6;

    public override ExpressionType<T> Derivative(char variable)
    {
        return new NumberExpression<T>(Arithmetic.Real0, Arithmetic);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Arithmetic.PI;
    }

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables)
    {
        return this;
    }

    public override string ToString()
    {
        return "pi";
    }

    public override bool Equals(object? obj)
    {
        return obj is ConstantE<T>;
    }

    public override int GetHashCode()
    {
        return (int)Math.PI;
    }
}

public class Factorial<T> : ExpressionType<T>
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

    public override ExpressionType<T> Derivative(char variable)
    {
        return new NumberExpression<T>(Arithmetic.Real0, Arithmetic);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Value;
    }

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables)
    {
        return this;
    }

    public override string ToString()
    {
        return $"{_integer}!";
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
}