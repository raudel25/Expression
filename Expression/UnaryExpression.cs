using BigNum;

namespace Expression;

public class NumberExpression<T> : ExpressionType<T>
{
    public readonly T Value;

    public NumberExpression(T value, IArithmetic<T> arithmetic) : base(arithmetic)
    {
        this.Value = value;
    }

    public override ExpressionType<T> Derivative(char variable) =>
        new NumberExpression<T>(Arithmetic.Real0, Arithmetic);

    public override T Evaluate(List<(char, T)> variables) => this.Value;

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables) => this;

    public override string ToString()
    {
        if (this.Value is null) return "";
        if (this.Value.ToString() is null) return "";
        return this.Value.ToString()!;
    }

    public override int Priority => 6;

    public override bool Equals(object? obj)
    {
        NumberExpression<T>? exp = obj as NumberExpression<T>;
        if (exp is null) return false;
        if (exp.Value is null) return false;

        return exp.Value.Equals(this.Value);
    }

    public override int GetHashCode()
    {
        if (this.Value is null) return 0;
        return this.Value.GetHashCode();
    }
}

public class VariableExpression<T> : ExpressionType<T>
{
    public readonly char Variable;

    public VariableExpression(char variable, IArithmetic<T> arithmetic) : base(arithmetic)
    {
        this.Variable = variable;
    }

    public override ExpressionType<T> Derivative(char variable) => variable == this.Variable
        ? new NumberExpression<T>(Arithmetic.Real1, Arithmetic)
        : new NumberExpression<T>(Arithmetic.Real0, Arithmetic);

    public override T Evaluate(List<(char, T)> variables)
    {
        foreach (var item in variables)
        {
            if (item.Item1 == this.Variable) return item.Item2;
        }

        throw new Exception("No se ha introducido un valor para cada variable");
    }

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables)
    {
        foreach (var item in variables)
        {
            if (item.Item1 == this.Variable) return item.Item2;
        }

        return this;
    }

    public override int Priority => 6;

    public override bool Equals(object? obj)
    {
        VariableExpression<T>? exp = obj as VariableExpression<T>;
        if (exp is null) return false;

        return exp.Variable == this.Variable;
    }

    public override int GetHashCode() => this.Variable.GetHashCode();

    public override string ToString() => this.Variable.ToString();
}

public class ConstantE<T> : ExpressionType<T>
{
    public ConstantE(IArithmetic<T> arithmetic) : base(arithmetic)
    {
    }

    public override ExpressionType<T> Derivative(char variable) =>
        new NumberExpression<T>(Arithmetic.Real0, Arithmetic);

    public override T Evaluate(List<(char, T)> variables) => Arithmetic.E;

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables) => this;

    public override int Priority => 6;

    public override string ToString() => "e";

    public override bool Equals(object? obj) => obj is ConstantE<T>;

    public override int GetHashCode() => (int)Math.E;
}

public class ConstantPI<T> : ExpressionType<T>
{
    public ConstantPI(IArithmetic<T> arithmetic) : base(arithmetic)
    {
    }

    public override ExpressionType<T> Derivative(char variable) =>
        new NumberExpression<T>(Arithmetic.Real0, Arithmetic);

    public override T Evaluate(List<(char, T)> variables) => Arithmetic.PI;

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables) => this;

    public override int Priority => 6;

    public override string ToString() => "pi";

    public override bool Equals(object? obj) => obj is ConstantE<T>;

    public override int GetHashCode() => (int)Math.PI;
}

public class Factorial<T> : ExpressionType<T>
{
    private readonly T _integer;

    private T? _value;

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

    public Factorial(T value, IArithmetic<T> arithmetic) : base(arithmetic)
    {
        this._integer = value;
        this._value = default(T);
    }

    public override ExpressionType<T> Derivative(char variable) =>
        new NumberExpression<T>(Arithmetic.Real0, Arithmetic);

    public override T Evaluate(List<(char, T)> variables) => this.Value;

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables) => this;

    public override string ToString() => $"{this._integer}!";

    public override int Priority
    {
        get => 5;
    }

    public override bool Equals(object? obj)
    {
        NumberExpression<T>? exp = obj as NumberExpression<T>;
        if (exp is null) return false;
        if (exp.Value is null) return false;

        return exp.Value.Equals(this.Value);
    }

    public override int GetHashCode()
    {
        if (this.Value is null) return 0;
        return this.Value.GetHashCode();
    }
}