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

    /// <summary>
    /// Reducir una expresion
    /// </summary>
    /// <param name="exp">Expresion para reducir</param>
    /// <returns>Expresion reducida</returns>
    public static ExpressionType ReduceExpression(ExpressionType exp)
    {
        BinaryExpression? binary = exp as BinaryExpression;

        if (!(binary is null))
        {
            if (binary is Sum)
            {
                if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Right;
                if (binary.Right.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Left;
            }

            if (binary is Subtraction)
            {
                if (binary.Right.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Left;
            }

            if (binary is Multiply)
            {
                if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0)) ||
                    binary.Right.Equals(new NumberExpression(RealNumbers.Real0)))
                    return new NumberExpression(RealNumbers.Real0);
                if (binary.Left.Equals(new NumberExpression(RealNumbers.Real1))) return binary.Right;
                if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1))) return binary.Left;
            }

            if (binary is Division)
            {
                if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0)))
                    return new NumberExpression(RealNumbers.Real0);
                if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1))) return binary.Left;
            }

            if (binary is Pow)
            {
                if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0)))
                    return new NumberExpression(RealNumbers.Real0);
                if (binary.Right.Equals(new NumberExpression(RealNumbers.Real0)))
                    return new NumberExpression(RealNumbers.Real1);
                if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1))) return binary.Left;
            }

            if (binary is Log)
            {
                if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1)))
                    return new NumberExpression(RealNumbers.Real0);
            }
        }

        return exp;
    }
}