namespace Expression;

public abstract class ExpressionType<T>
{
    internal readonly IArithmetic<T> Arithmetic;

    internal ExpressionType(IArithmetic<T> arithmetic)
    {
        Arithmetic = arithmetic;
    }

    /// <summary>
    ///     Prioridad de la operacion
    /// </summary>
    public abstract int Priority { get; }

    /// <summary>
    ///     Derivada de la expresion
    /// </summary>
    /// <param name="variable">Variable sobre la cual se esta derivando</param>
    /// <returns>Derivada</returns>
    public abstract ExpressionType<T> Derivative(char variable);

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
    public abstract ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables);

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
    public ExpressionType<T> Derivative(char variable, int n)
    {
        var expression = Derivative(variable);

        for (var i = 1; i < n; i++) expression = expression.Derivative(variable);

        return expression;
    }

    #region Operadores

    public static Sum<T> operator +(ExpressionType<T> left, ExpressionType<T> right)
    {
        return new(left, right);
    }

    public static Subtraction<T> operator -(ExpressionType<T> left, ExpressionType<T> right)
    {
        return new(left, right);
    }

    public static Subtraction<T> operator -(ExpressionType<T> value)
    {
        return new(new NumberExpression<T>(value.Arithmetic.Real0, value.Arithmetic), value);
    }

    public static Multiply<T> operator *(ExpressionType<T> left, ExpressionType<T> right)
    {
        return new(left, right);
    }

    public static Division<T> operator /(ExpressionType<T> left, ExpressionType<T> right)
    {
        return new(left, right);
    }

    public static ExpressionType<T> operator ++(ExpressionType<T> value)
    {
        return value + new NumberExpression<T>(value.Arithmetic.Real1, value.Arithmetic);
    }

    public static ExpressionType<T> operator --(ExpressionType<T> value)
    {
        return value - new NumberExpression<T>(value.Arithmetic.Real1, value.Arithmetic);
    }

    #endregion
}

public abstract class BinaryExpression<T> : ExpressionType<T>
{
    public readonly ExpressionType<T> Left;

    public readonly ExpressionType<T> Right;

    protected BinaryExpression(ExpressionType<T> left, ExpressionType<T> right) :
        base(left.Arithmetic)
    {
        Left = ReduceExpression<T>.Reduce(left);
        Right = ReduceExpression<T>.Reduce(right);
    }

    public override ExpressionType<T> Derivative(char variable)
    {
        return Derivative(variable, Left, Right);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Evaluate(Left.Evaluate(variables), Right.Evaluate(variables));
    }

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables)
    {
        return EvaluateExpression(Left.EvaluateExpression(variables), Right.EvaluateExpression(variables));
    }

    protected abstract ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right);

    protected abstract T Evaluate(T left, T right);

    protected abstract ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right);

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
    protected (string, string) DeterminatePriority()
    {
        return (
            Left.Priority < Priority && Left.IsBinary()
                ? Aux<T>.Colocated(Left.ToString()!)
                : Left.ToString()!,
            Right.Priority < Priority && Right.IsBinary()
                ? Aux<T>.Colocated(Right.ToString()!)
                : Right.ToString()!);
    }
}

public abstract class UnaryExpression<T> : ExpressionType<T>
{
    public readonly ExpressionType<T> Value;

    protected UnaryExpression(ExpressionType<T> value) : base(value.Arithmetic)
    {
        Value = ReduceExpression<T>.Reduce(value);
    }

    public override int Priority => 5;

    public override ExpressionType<T> Derivative(char variable)
    {
        return Derivative(Value) * Value.Derivative(variable);
    }

    public override T Evaluate(List<(char, T)> variables)
    {
        return Evaluate(Value.Evaluate(variables));
    }

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables)
    {
        return EvaluateExpression(Value.EvaluateExpression(variables));
    }


    protected abstract ExpressionType<T> Derivative(ExpressionType<T> value);

    protected abstract T Evaluate(T x);

    protected abstract ExpressionType<T> EvaluateExpression(ExpressionType<T> value);
}