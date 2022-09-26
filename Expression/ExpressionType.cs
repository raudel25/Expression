using BigNum;

namespace Expression;

public abstract class ExpressionType
{
    /// <summary>
    /// Derivada de la expresion
    /// </summary>
    /// <param name="variable">Variable sobre la cual se esta derivando</param>
    /// <returns>Derivada</returns>
    public abstract ExpressionType Derivative(char variable);

    /// <summary>
    /// Evaluar la expresion 
    /// </summary>
    /// <param name="variables">Lista de variables con sus respectivos valores</param>
    /// <returns>Valor de la expresion</returns>
    public abstract RealNumbers Evaluate(List<(char, RealNumbers)> variables);

    /// <summary>
    /// Evaluar la expresion mediante otra expresion
    /// </summary>
    /// <param name="variables">Lista de variables con sus respectivas expresiones</param>
    /// <returns>Nueva expresion</returns>
    public abstract ExpressionType EvaluateExpression(List<(char, ExpressionType)> variables);

    /// <summary>
    /// Prioridad de la operacion
    /// </summary>
    public abstract int Priority { get; }

    /// <summary>
    /// Determinar si la expresion es binaria
    /// </summary>
    /// <returns></returns>
    public virtual bool IsBinary() => this is BinaryExpression;

    /// <summary>
    /// Hallar la n-esima derivada
    /// </summary>
    /// <param name="variable">Variable sobre la cual se esta derivando</param>
    /// <param name="n">indice de la derivada</param>
    /// <returns>Derivada n-esima</returns>
    public ExpressionType Derivative(char variable, int n)
    {
        ExpressionType expression = this.Derivative(variable);

        for (int i = 1; i < n; i++) expression = expression.Derivative(variable);

        return expression;
    }

    #region Operadores

    public static Sum operator +(ExpressionType left, ExpressionType right) => new Sum(left, right);

    public static Subtraction operator -(ExpressionType left, ExpressionType right) =>
        new Subtraction(left, right);

    public static Subtraction operator -(ExpressionType value) =>
        new Subtraction(new NumberExpression(new RealNumbers("0")), value);

    public static Multiply operator *(ExpressionType left, ExpressionType right) => new Multiply(left, right);

    public static Division operator /(ExpressionType left, ExpressionType right) => new Division(left, right);

    public static ExpressionType operator ++(ExpressionType value) => value + new NumberExpression(RealNumbers.Real1);

    public static ExpressionType operator --(ExpressionType value) => value - new NumberExpression(RealNumbers.Real1);

    #endregion
}

public abstract class BinaryExpression : ExpressionType
{
    public readonly ExpressionType Left;

    public readonly ExpressionType Right;

    public BinaryExpression(ExpressionType left, ExpressionType right)
    {
        this.Left = ReduceExpression.Reduce(left);
        this.Right = ReduceExpression.Reduce(right);
    }

    public override ExpressionType Derivative(char variable) => this.Derivative(variable, this.Left, this.Right);

    public override RealNumbers Evaluate(List<(char, RealNumbers)> variables) =>
        this.Evaluate(this.Left.Evaluate(variables), this.Right.Evaluate(variables));

    public override ExpressionType EvaluateExpression(List<(char, ExpressionType)> variables) =>
        this.EvaluateExpression(this.Left.EvaluateExpression(variables), this.Right.EvaluateExpression(variables));

    protected abstract ExpressionType Derivative(char variable, ExpressionType left, ExpressionType right);

    protected abstract RealNumbers Evaluate(RealNumbers left, RealNumbers right);

    protected abstract ExpressionType EvaluateExpression(ExpressionType left, ExpressionType right);

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
            ? Aux.Colocated(this.Left.ToString()!)
            : this.Left.ToString()!,
        this.Right.Priority < this.Priority && this.Right.IsBinary()
            ? Aux.Colocated(this.Right.ToString()!)
            : this.Right.ToString()!);
}

public abstract class UnaryExpression : ExpressionType
{
    public readonly ExpressionType Value;

    public UnaryExpression(ExpressionType value)
    {
        this.Value = ReduceExpression.Reduce(value);
    }

    public override ExpressionType Derivative(char variable) =>
        this.Derivative(this.Value) * this.Value.Derivative(variable);

    public override RealNumbers Evaluate(List<(char, RealNumbers)> variables) =>
        this.Evaluate(this.Value.Evaluate(variables));

    public override ExpressionType EvaluateExpression(List<(char, ExpressionType)> variables) =>
        this.EvaluateExpression(this.Value.EvaluateExpression(variables));


    protected abstract ExpressionType Derivative(ExpressionType value);

    protected abstract RealNumbers Evaluate(RealNumbers x);

    protected abstract ExpressionType EvaluateExpression(ExpressionType value);

    public override int Priority => 5;
}