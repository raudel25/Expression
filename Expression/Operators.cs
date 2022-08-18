namespace Expression;

public class Operators
{
    public delegate ExpressionType Expression(ExpressionType[] exp);

    /// <summary>
    /// Simbolo del operador
    /// </summary>
    public readonly string Operator;

    /// <summary>
    /// Prioridad por defecto
    /// </summary>
    public readonly int DefaultPriority;

    /// <summary>
    /// Prioridad asignada durante el parsing
    /// </summary>
    public int AssignPriority { get; set; }

    /// <summary>
    /// Posicion del operador
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Expresion que determina el operador
    /// </summary>
    public readonly Expression ExpressionOperator;

    /// <summary>
    /// Determinar si el oerador es binario
    /// </summary>
    public readonly bool Binary;

    public Operators(string s, int priority, Expression expressionOperator, bool binary = false)
    {
        this.Operator = s;
        this.DefaultPriority = priority;
        this.ExpressionOperator = expressionOperator;
        this.Binary = binary;
    }
}