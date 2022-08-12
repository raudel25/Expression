namespace Expression;

public static class Aux
{
    public static string Colocated(string s) => "(" + s + ")";

    public static void IsBinary(ExpressionValue exp, ref string s)
    {
        if (exp is BinaryExpression) s = Colocated(s);
    }

    public static string Opposite(ExpressionValue exp)
    {
        if (exp.ToString()![0] == '-')
            return exp.ToString()!.Substring(1, exp.ToString()!.Length - 1);
        return exp.Priority == 1 ? "-(" + exp + ")" : "-" + exp;
    }
}