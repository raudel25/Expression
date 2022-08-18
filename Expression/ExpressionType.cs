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
    /// <param name="x">Numero para evaluar</param>
    /// <returns>Valor de la expresion</returns>
    public abstract RealNumbers Evaluate(RealNumbers x);

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

    public override RealNumbers Evaluate(RealNumbers x) =>
        this.EvaluateBinary(this.Left.Evaluate(x), this.Right.Evaluate(x));

    protected abstract ExpressionType Derivative(char variable, ExpressionType left, ExpressionType right);

    protected abstract RealNumbers EvaluateBinary(RealNumbers left, RealNumbers right);

    protected abstract bool IsBinaryImplement();

    public override bool IsBinary()
    {
        if (this.Left.IsBinary() || this.Right.IsBinary()) return true;

        return IsBinaryImplement();
    }

    public override bool Equals(object? obj)
    {
        BinaryExpression? binary = obj as BinaryExpression;
        if (binary is null) return false;

        return this.Left.Equals(binary.Left) && this.Right.Equals(binary.Right);
    }

    /// <summary>
    /// Determinar la igualdad de expresiones conmutativas
    /// </summary>
    /// <param name="obj">Objeto para comparar</param>
    /// <returns>Igualdad de las expresiones</returns>
    protected bool EqualsCommutative(object? obj)
    {
        BinaryExpression? binary = obj as BinaryExpression;
        if (binary is null) return false;

        return (this.Left.Equals(binary.Left) && this.Right.Equals(binary.Right)) ||
               (this.Left.Equals(binary.Right) && this.Right.Equals(binary.Left));
    }

    public override int GetHashCode() => this.Left.GetHashCode() * this.Right.GetHashCode();

    /// <summary>
    /// Reterminar la prioridad u colocar parentesis
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
        this.Value = value;
    }

    public override ExpressionType Derivative(char variable) =>
        this.Derivative(this.Value) * this.Value.Derivative(variable);

    public override RealNumbers Evaluate(RealNumbers x) => this.EvaluateUnary(this.Value.Evaluate(x));

    protected abstract ExpressionType Derivative(ExpressionType value);

    protected abstract RealNumbers EvaluateUnary(RealNumbers x);
    
    public override int Priority
    {
        get => 4;
    }
    
    public override bool Equals(object? obj)
    {
        UnaryExpression? unary = obj as UnaryExpression;
        if (unary is null) return false;

        return this.Value.Equals(unary.Value);
    }

    public override int GetHashCode() => this.Value.GetHashCode();
}