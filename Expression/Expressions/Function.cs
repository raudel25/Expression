using Expression.Reduce;
using Expression.Arithmetics;

namespace Expression.Expressions;

public abstract class Function<T>
{
    internal readonly IArithmetic<T> Arithmetic;

    /// <summary>
    ///     Aritmetica
    /// </summary>
    /// <param name="arithmetic">Aritmetica</param>
    internal Function(IArithmetic<T> arithmetic)
    {
        Arithmetic = arithmetic;
    }

    /// <summary>
    ///     Determinar las varibles de una expresion
    /// </summary>
    /// <returns>Lista de variables de la expresion</returns>
    public List<char> VariablesToExpression => Aux<T>.VariablesToExpression(this);


    /// <summary>
    ///     Reducir una expresion
    /// </summary>
    /// <returns>Expresion reducida</returns>
    public Function<T> Reduce => ReduceExpression<T>.Reduce(this);

    /// <summary>
    /// Expresion en latex
    /// </summary>
    /// <returns>Expresion en latex</returns>
    public abstract string ToLatex();

    /// <summary>
    ///     Prioridad de la operacion
    /// </summary>
    public abstract int Priority { get; }

    /// <summary>
    ///     Derivada de la expresion
    /// </summary>
    /// <param name="variable">Variable sobre la cual se esta derivando</param>
    /// <returns>Derivada</returns>
    public abstract Function<T> Derivative(char variable);

    /// <summary>
    ///     Evaluar la expresion
    /// </summary>
    /// <param name="variables">Lista de variables con sus respectivos valores</param>
    /// <returns>Valor de la expresion</returns>
    public abstract T Evaluate(List<(char, T)> variables);

    /// <summary>
    ///     Evaluar la expresion mediante otra expresion
    /// </summary>
    /// <param name="variables">Lista de variables con sus respectivas expresiones</param>
    /// <returns>Nueva expresion</returns>
    public abstract Function<T> EvaluateExpression(List<(char, Function<T>)> variables);

    /// <summary>
    ///     Determinar si la expresion es binaria
    /// </summary>
    /// <returns></returns>
    public virtual bool IsBinary()
    {
        return this is BinaryExpression<T>;
    }

    /// <summary>
    ///     Hallar la n-esima derivada
    /// </summary>
    /// <param name="variable">Variable sobre la cual se esta derivando</param>
    /// <param name="n">indice de la derivada</param>
    /// <returns>Derivada n-esima</returns>
    public Function<T> Derivative(char variable, int n)
    {
        var expression = Derivative(variable);

        for (var i = 1; i < n; i++) expression = expression.Derivative(variable);

        return expression;
    }

    #region Operadores

    public static Sum<T> operator +(Function<T> left, Function<T> right)
    {
        return new(left, right);
    }

    public static Subtraction<T> operator -(Function<T> left, Function<T> right)
    {
        return new(left, right);
    }

    public static Subtraction<T> operator -(Function<T> value)
    {
        return new(new NumberExpression<T>(value.Arithmetic.Real0, value.Arithmetic), value);
    }

    public static Multiply<T> operator *(Function<T> left, Function<T> right)
    {
        return new(left, right);
    }

    public static Division<T> operator /(Function<T> left, Function<T> right)
    {
        return new(left, right);
    }

    public static Function<T> operator ++(Function<T> value)
    {
        return value + new NumberExpression<T>(value.Arithmetic.Real1, value.Arithmetic);
    }

    public static Function<T> operator --(Function<T> value)
    {
        return value - new NumberExpression<T>(value.Arithmetic.Real1, value.Arithmetic);
    }

    #endregion
}

public abstract class BinaryExpression<T> : Function<T>
{
    public readonly Function<T> Left;

    public readonly Function<T> Right;

    protected BinaryExpression(Function<T> left, Function<T> right) :
        base(left.Arithmetic)
    {
        Left = ReduceExpression<T>.Reduce(left);
        Right = ReduceExpression<T>.Reduce(right);
    }

    public override Function<T> Derivative(char variable)
    {
        return Derivative(variable, Left, Right);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Evaluate(Left.Evaluate(variables), Right.Evaluate(variables));
    }

    public override Function<T> EvaluateExpression(List<(char, Function<T>)> variables)
    {
        return EvaluateExpression(Left.EvaluateExpression(variables), Right.EvaluateExpression(variables));
    }

    protected abstract Function<T> Derivative(char variable, Function<T> left, Function<T> right);

    protected abstract T Evaluate(T left, T right);

    protected abstract Function<T> EvaluateExpression(Function<T> left, Function<T> right);

    protected abstract bool IsBinaryImplement();

    public override bool IsBinary()
    {
        if (Left.IsBinary() || Right.IsBinary()) return true;

        return IsBinaryImplement();
    }

    /// <summary>
    ///     Reterminar la prioridad o colocar parentesis
    /// </summary>
    /// <returns>Cadenas modificadas</returns>
    protected (string, string) DeterminatePriority(string left, string right, bool latex = false)
    {
        return (
            Left.Priority < Priority && Left.IsBinary()
                ? Aux<T>.Colocated(left, latex)
                : left,
            Right.Priority < Priority && Right.IsBinary()
                ? Aux<T>.Colocated(right, latex)
                : right);
    }
}

public abstract class UnaryExpression<T> : Function<T>
{
    public readonly Function<T> Value;

    protected UnaryExpression(Function<T> value) : base(value.Arithmetic)
    {
        Value = ReduceExpression<T>.Reduce(value);
    }

    public override int Priority => 5;

    public override Function<T> Derivative(char variable)
    {
        return Derivative(Value) * Value.Derivative(variable);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Evaluate(Value.Evaluate(variables));
    }

    public override Function<T> EvaluateExpression(List<(char, Function<T>)> variables)
    {
        return EvaluateExpression(Value.EvaluateExpression(variables));
    }


    protected abstract Function<T> Derivative(Function<T> value);

    protected abstract T Evaluate(T x);

    protected abstract Function<T> EvaluateExpression(Function<T> value);
}