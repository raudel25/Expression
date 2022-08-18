using BigNum;

namespace Expression;

public static class Aux
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
    public static string Opposite(ExpressionType exp)
    {
        string s = exp.ToString()!;
        if (s[0] == '-')
            return s.Substring(1, s.Length - 1);
        if (s == "0") return "0";
        return exp.Priority == 1 ? $"-({exp})" : $"-{exp}";
    }

    public static int CantVariable(ExpressionType exp)
    {
        HashSet<char> variables = new HashSet<char>();
        CantVariable(exp, variables);

        return variables.Count;
    }

    public static void CantVariable(ExpressionType exp, HashSet<char> variables)
    {
        VariableExpression? variable = exp as VariableExpression;

        if (variable != null)
        {
            if (!variables.Contains(variable.Variable)) variables.Add(variable.Variable);
            return;
        }

        BinaryExpression? binary = exp as BinaryExpression;

        if (binary != null)
        {
            CantVariable(binary.Left, variables);
            CantVariable(binary.Right, variables);
            return;
        }

        UnaryExpression? unary = exp as UnaryExpression;

        if (unary != null) CantVariable(unary.Value, variables);
    }
}