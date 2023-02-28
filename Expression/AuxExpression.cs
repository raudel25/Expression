using Expression.Expressions;

namespace Expression;

internal static class Aux<T>
{
    /// <summary>
    ///     Colocar parentesis
    /// </summary>
    /// <param name="s">Cadena de texto</param>
    /// <param name="latex">Ganerar codigo de latex</param>
    /// <returns>Cadena modificada</returns>
    internal static string Colocated(string s, bool latex = false)
    {
        var (l, r) = latex ? ("\\left", "\\right") : ("", "");

        return s[0] == '(' && s[^1] == ')' ? s : $"{l}({s}{r})";
    }

    /// <summary>
    ///     Colocar el signo negativo
    /// </summary>
    /// <param name="exp">Cadena de texto</param>
    /// <param name="priority">Prioridad</param>
    /// <param name="latex">Ganerar codigo de latex</param>
    /// <returns>Cadena modificada</returns>
    internal static string Opposite(string exp, int priority, bool latex = false)
    {
        var (l, r) = latex ? ("\\left", "\\right") : ("", "");
        var s = exp;

        if (s[0] == '-')
            return s.Substring(1, s.Length - 1);
        if (s == "0") return "0";
        return priority == 1 ? $"-{l}({exp}{r})" : $"-{exp}";
    }

    /// <summary>
    ///     Determinar las varibles de una expresion
    /// </summary>
    /// <param name="exp">Expresion</param>
    /// <returns>Lista de variables de la expresion</returns>
    internal static List<char> VariablesToExpression(Function<T> exp)
    {
        var variables = new HashSet<char>();
        VariablesToExpression(exp, variables);

        return variables.ToList();
    }

    private static void VariablesToExpression(Function<T> exp, HashSet<char> variables)
    {
        switch (exp)
        {
            case VariableExpression<T> variable:
            {
                if (!variables.Contains(variable.Variable)) variables.Add(variable.Variable);
                return;
            }
            case BinaryExpression<T> binary:
                VariablesToExpression(binary.Left, variables);
                VariablesToExpression(binary.Right, variables);
                return;
            case UnaryExpression<T> unary:
                VariablesToExpression(unary.Value, variables);
                break;
        }
    }

    /// <summary>
    ///     Determinar si la expresion es completamente numerica
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    internal static NumberExpression<T>? Numbers(BinaryExpression<T> binary)
    {
        return binary is { Left: NumberExpression<T>, Right: NumberExpression<T> }
            ? new NumberExpression<T>(binary.Evaluate(new List<(char, T)>()), binary.Arithmetic)
            : null;
    }
}