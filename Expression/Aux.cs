namespace Expression;

public static class Aux
{
    public static string Colocated(string s) => s[0] == '(' && s[s.Length - 1] == ')' ? s : "(" + s + ")";

    public static string Opposite(ExpressionType exp)
    {
        if (exp.ToString()![0] == '-')
            return exp.ToString()!.Substring(1, exp.ToString()!.Length - 1);
        return exp.Priority == 1 ? "-(" + exp + ")" : "-" + exp;
    }
}