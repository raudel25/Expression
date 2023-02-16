namespace Expression;

public class Operators<T>
{
    public delegate ExpressionType<T> Expression(ExpressionType<T>[] exp);

    /// <summary>
    ///     Determinar si el oerador es binario
    /// </summary>
    public readonly bool Binary;

    /// <summary>
    ///     Prioridad por defecto
    /// </summary>
    public readonly int DefaultPriority;

    /// <summary>
    ///     Expresion que determina el operador
    /// </summary>
    public readonly Expression ExpressionOperator;

    /// <summary>
    ///     Simbolo del operador
    /// </summary>
    public readonly string Operator;

    public Operators(string s, int priority, Expression expressionOperator, bool binary = false)
    {
        Operator = s;
        DefaultPriority = priority;
        ExpressionOperator = expressionOperator;
        Binary = binary;
    }

    /// <summary>
    ///     Prioridad asignada durante el parsing
    /// </summary>
    public int AssignPriority { get; set; }

    /// <summary>
    ///     Posicion del operador
    /// </summary>
    public int Position { get; set; }
}