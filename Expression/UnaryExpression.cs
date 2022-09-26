using BigNum;

namespace Expression;

public class NumberExpression : ExpressionType
{
    public readonly RealNumbers Value;

    public NumberExpression(RealNumbers value)
    {
        this.Value = value;
    }

    public override ExpressionType Derivative(char variable) => new NumberExpression(RealNumbers.Real0);

    public override RealNumbers Evaluate(List<(char, RealNumbers)> variables) => this.Value;

    public override ExpressionType EvaluateExpression(List<(char, ExpressionType)> variables) => this;

    public override string ToString() => this.Value.ToString();

    public override int Priority
    {
        get => 6;
    }

    public override bool Equals(object? obj)
    {
        NumberExpression? exp = obj as NumberExpression;
        if (exp is null) return false;

        return exp.Value == this.Value;
    }

    public override int GetHashCode() => this.Value.GetHashCode();
}

public class VariableExpression : ExpressionType
{
    public readonly char Variable;

    public VariableExpression(char variable)
    {
        this.Variable = variable;
    }

    public override ExpressionType Derivative(char variable) => variable == this.Variable
        ? new NumberExpression(RealNumbers.Real1)
        : new NumberExpression(RealNumbers.Real0);

    public override RealNumbers Evaluate(List<(char, RealNumbers)> variables)
    {
        foreach (var item in variables)
        {
            if (item.Item1 == this.Variable) return item.Item2;
        }

        throw new Exception("No se ha introducido un valor para cada variable");
    }

    public override ExpressionType EvaluateExpression(List<(char, ExpressionType)> variables)
    {
        foreach (var item in variables)
        {
            if (item.Item1 == this.Variable) return item.Item2;
        }

        return this;
    }

    public override int Priority
    {
        get => 6;
    }

    public override bool Equals(object? obj)
    {
        VariableExpression? exp = obj as VariableExpression;
        if (exp is null) return false;

        return exp.Variable == this.Variable;
    }

    public override int GetHashCode() => this.Variable.GetHashCode();

    public override string ToString() => this.Variable.ToString();
}

public class ConstantE : ExpressionType
{
    public override ExpressionType Derivative(char variable) => new NumberExpression(RealNumbers.Real0);

    public override RealNumbers Evaluate(List<(char, RealNumbers)> variables) => BigNumMath.E;

    public override ExpressionType EvaluateExpression(List<(char, ExpressionType)> variables) => this;

    public override int Priority
    {
        get => 6;
    }

    public override string ToString() => "e";

    public override bool Equals(object? obj) => obj is ConstantE;

    public override int GetHashCode() => (int) Math.E;
}

public class ConstantPI : ExpressionType
{
    public override ExpressionType Derivative(char variable) => new NumberExpression(RealNumbers.Real0);

    public override RealNumbers Evaluate(List<(char, RealNumbers)> variables) => BigNumMath.PI;

    public override ExpressionType EvaluateExpression(List<(char, ExpressionType)> variables) => this;

    public override int Priority
    {
        get => 6;
    }

    public override string ToString() => "pi";

    public override bool Equals(object? obj) => obj is ConstantPI;

    public override int GetHashCode() => (int) Math.PI;
}

public class Factorial : ExpressionType
{
    private readonly IntegerNumbers _integer;

    private IntegerNumbers _value;

    public IntegerNumbers Value
    {
        get
        {
            if (_value == IntegerNumbers.IntegerN1) _value = BigNumMath.Factorial(_integer);

            return _value;
        }
        set
        {
            if (_value == IntegerNumbers.IntegerN1) _value = value;
        }
    }

    public Factorial(IntegerNumbers value)
    {
        this._integer = value;
        this._value = IntegerNumbers.IntegerN1;
    }

    public override ExpressionType Derivative(char variable) => new NumberExpression(RealNumbers.Real0);

    public override RealNumbers Evaluate(List<(char, RealNumbers)> variables) => this.Value;

    public override ExpressionType EvaluateExpression(List<(char, ExpressionType)> variables) => this;

    public override string ToString() => $"{this._integer}!";

    public override int Priority
    {
        get => 5;
    }

    public override bool Equals(object? obj)
    {
        NumberExpression? exp = obj as NumberExpression;
        if (exp is null) return false;

        return exp.Value == this.Value;
    }

    public override int GetHashCode() => this.Value.GetHashCode();
}