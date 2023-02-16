using Expression.Expressions;

namespace Expression;

internal class Operators<T>
{
    internal delegate ExpressionType<T> Expression(ExpressionType<T>[] exp);

    /// <summary>
    ///     Determinar si el oerador es binario
    /// </summary>
    internal readonly bool Binary;

    /// <summary>
    ///     Prioridad por defecto
    /// </summary>
    internal readonly int DefaultPriority;

    /// <summary>
    ///     Expresion que determina el operador
    /// </summary>
    internal readonly Expression ExpressionOperator;

    /// <summary>
    ///     Simbolo del operador
    /// </summary>
    internal readonly string Operator;

    internal Operators(string s, int priority, Expression expressionOperator, bool binary = false)
    {
        Operator = s;
        DefaultPriority = priority;
        ExpressionOperator = expressionOperator;
        Binary = binary;
    }

    /// <summary>
    ///     Prioridad asignada durante el parsing
    /// </summary>
    internal int AssignPriority { get; set; }

    /// <summary>
    ///     Posicion del operador
    /// </summary>
    internal int Position { get; set; }
}