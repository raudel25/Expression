namespace Expression;

public static class Aux<T>
{
    /// <summary>
    /// Colocar parentesis
    /// </summary>
    /// <param name="s">Cadena de texto</param>
    /// <returns>Cadena modificada</returns>
    public static string Colocated(string s) => s[0] == '(' && s[s.Length - 1] == ')' ? s : $"({s})";

    /// <summary>
    /// Colocar el signo negativo
    /// </summary>
    /// <param name="exp">Cadena de texto</param>
    /// <returns>Cadena modificada</returns>
    public static string Opposite(ExpressionType<T> exp)
    {
        string s = exp.ToString()!;
        if (s[0] == '-')
            return s.Substring(1, s.Length - 1);
        if (s == "0") return "0";
        return exp.Priority == 1 ? $"-({exp})" : $"-{exp}";
    }

    /// <summary>
    /// Determinar las varibles de una expresion
    /// </summary>
    /// <param name="exp">Expresion</param>
    /// <returns>Lista de variables de la expresion</returns>
    public static List<char> VariablesToExpression(ExpressionType<T> exp)
    {
        HashSet<char> variables = new HashSet<char>();
        VariablesToExpression(exp, variables);

        return variables.ToList();
    }

    public static void VariablesToExpression(ExpressionType<T> exp, HashSet<char> variables)
    {
        if (exp is VariableExpression<T> variable)
        {
            if (!variables.Contains(variable.Variable)) variables.Add(variable.Variable);
            return;
        }

        if (exp is BinaryExpression<T> binary)
        {
            VariablesToExpression(binary.Left, variables);
            VariablesToExpression(binary.Right, variables);
            return;
        }

        if (exp is UnaryExpression<T> unary) VariablesToExpression(unary.Value, variables);
    }

    /// <summary>
    /// Determinar si la expresion es completamente numerica
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    internal static NumberExpression<T>? Numbers(BinaryExpression<T> binary)
    {
        if (binary.Left is NumberExpression<T> && binary.Right is NumberExpression<T>)
            return new NumberExpression<T>(binary.Evaluate(new List<(char, T)>()), binary.Arithmetic);

        return null;
    }
}