using BigNum;

namespace Expression;

public abstract class ExpressionType<T>
{
    internal IArithmetic<T> Arithmetic;

    internal ExpressionType(IArithmetic<T> arithmetic)
    {
        this.Arithmetic = arithmetic;
    }

    /// <summary>
    /// Derivada de la expresion
    /// </summary>
    /// <param name="variable">Variable sobre la cual se esta derivando</param>
    /// <returns>Derivada</returns>
    public abstract ExpressionType<T> Derivative(char variable);

    /// <summary>
    /// Evaluar la expresion 
    /// </summary>
    /// <param name="variables">Lista de variables con sus respectivos valores</param>
    /// <returns>Valor de la expresion</returns>
    public abstract T Evaluate(List<(char, T)> variables);

    /// <summary>
    /// Evaluar la expresion mediante otra expresion
    /// </summary>
    /// <param name="variables">Lista de variables con sus respectivas expresiones</param>
    /// <returns>Nueva expresion</returns>
    public abstract ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables);

    /// <summary>
    /// Prioridad de la operacion
    /// </summary>
    public abstract int Priority { get; }

    /// <summary>
    /// Determinar si la expresion es binaria
    /// </summary>
    /// <returns></returns>
    public virtual bool IsBinary() => this is BinaryExpression<T>;

    /// <summary>
    /// Hallar la n-esima derivada
    /// </summary>
    /// <param name="variable">Variable sobre la cual se esta derivando</param>
    /// <param name="n">indice de la derivada</param>
    /// <returns>Derivada n-esima</returns>
    public ExpressionType<T> Derivative(char variable, int n)
    {
        ExpressionType<T> expression = this.Derivative(variable);

        for (int i = 1; i < n; i++) expression = expression.Derivative(variable);

        return expression;
    }

    #region Operadores

    public static Sum<T> operator +(ExpressionType<T> left, ExpressionType<T> right) =>
        new(left, right, left.Arithmetic);

    public static Subtraction<T> operator -(ExpressionType<T> left, ExpressionType<T> right) =>
        new(left, right, left.Arithmetic);

    public static Subtraction<T> operator -(ExpressionType<T> value) =>
        new(new NumberExpression<T>(value.Arithmetic.Real0, value.Arithmetic), value, value.Arithmetic);

    public static Multiply<T> operator *(ExpressionType<T> left, ExpressionType<T> right) =>
        new(left, right, left.Arithmetic);

    public static Division<T> operator /(ExpressionType<T> left, ExpressionType<T> right) =>
        new(left, right, left.Arithmetic);

    public static ExpressionType<T> operator ++(ExpressionType<T> value) =>
        value + new NumberExpression<T>(value.Arithmetic.Real1, value.Arithmetic);

    public static ExpressionType<T> operator --(ExpressionType<T> value) =>
        value - new NumberExpression<T>(value.Arithmetic.Real1, value.Arithmetic);

    #endregion
}

public abstract class BinaryExpression<T> : ExpressionType<T>
{
    public readonly ExpressionType<T> Left;

    public readonly ExpressionType<T> Right;

    public BinaryExpression(ExpressionType<T> left, ExpressionType<T> right, IArithmetic<T> arithmetic) :
        base(arithmetic)
    {
        this.Left = ReduceExpression.Reduce(left);
        this.Right = ReduceExpression.Reduce(right);
    }

    public override ExpressionType<T> Derivative(char variable) => this.Derivative(variable, this.Left, this.Right);

    public override T Evaluate(List<(char, T)> variables) =>
        this.Evaluate(this.Left.Evaluate(variables), this.Right.Evaluate(variables));

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables) =>
        this.EvaluateExpression(this.Left.EvaluateExpression(variables), this.Right.EvaluateExpression(variables));

    protected abstract ExpressionType<T> Derivative(char variable, ExpressionType<T> left, ExpressionType<T> right);

    protected abstract T Evaluate(T left, T right);

    protected abstract ExpressionType<T> EvaluateExpression(ExpressionType<T> left, ExpressionType<T> right);

    protected abstract bool IsBinaryImplement();

    public override bool IsBinary()
    {
        if (this.Left.IsBinary() || this.Right.IsBinary()) return true;

        return IsBinaryImplement();
    }

    /// <summary>
    /// Reterminar la prioridad o colocar parentesis
    /// </summary>
    /// <returns>Cadenas modificadas</returns>
    protected (string, string) DeterminatePriority() => (
        this.Left.Priority < this.Priority && this.Left.IsBinary()
            ? Aux<T>.Colocated(this.Left.ToString()!)
            : this.Left.ToString()!,
        this.Right.Priority < this.Priority && this.Right.IsBinary()
            ? Aux<T>.Colocated(this.Right.ToString()!)
            : this.Right.ToString()!);
}

public abstract class UnaryExpression<T> : ExpressionType<T>
{
    public readonly ExpressionType<T> Value;

    public UnaryExpression(ExpressionType<T> value, IArithmetic<T> arithmetic) : base(arithmetic)
    {
        this.Value = ReduceExpression.Reduce(value);
    }

    public override ExpressionType<T> Derivative(char variable) =>
        this.Derivative(this.Value) * this.Value.Derivative(variable);

    public override T Evaluate(List<(char, T)> variables) =>
        this.Evaluate(this.Value.Evaluate(variables));

    public override ExpressionType<T> EvaluateExpression(List<(char, ExpressionType<T>)> variables) =>
        this.EvaluateExpression(this.Value.EvaluateExpression(variables));


    protected abstract ExpressionType<T> Derivative(ExpressionType<T> value);

    protected abstract T Evaluate(T x);

    protected abstract ExpressionType<T> EvaluateExpression(ExpressionType<T> value);

    public override int Priority => 5;
}