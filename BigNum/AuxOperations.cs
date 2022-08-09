namespace BigNum;

internal static class AuxOperations
{
    internal static string AddZerosLeft(string s, int cant)
    {
        for (int i = 0; i < cant; i++) s = 0 + s;

        return s;
    }

    internal static string EliminateZerosLeft(string s)
    {
        int ind = -1;

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] != '0')
            {
                ind = i;
                break;
            }
        }

        if (ind == -1) return "0";
        return s.Substring(ind, s.Length - ind);
    }

    internal static string EliminateZerosRight(string s)
    {
        int ind = -1;

        for (int i = s.Length - 1; i >= 0; i--)
        {
            if (s[i] != '0')
            {
                ind = i;
                break;
            }
        }

        if (ind == -1) return "0";
        return s.Substring(0, ind + 1);
    }

    internal static string AddZerosRight(string s, int cant)
    {
        for (int i = 0; i < cant; i++) s = s + 0;

        return s;
    }

    internal static int EqualZerosLeft(ref string x, ref string y)
    {
        int max = Math.Max(x.Length, y.Length);

        (x, y) = (AddZerosLeft(x, max - x.Length), AddZerosLeft(y, max - y.Length));

        return max;
    }

    internal static int EqualZerosRight(ref string x, ref string y)
    {
        int max = Math.Max(x.Length, y.Length);

        (x, y) = (AddZerosRight(x, max - x.Length), AddZerosRight(y, max - y.Length));

        return max;
    }
}