using System.Text;

namespace BigNum;

internal static class AuxOperations
{
    /// <summary>
    /// Añadir ceros a la izquierda
    /// </summary>
    /// <param name="s">Cadena a modificar</param>
    /// <param name="cant">Cantidad de ceros para añadir</param>
    /// <returns>Cadena modificada</returns>
    internal static string AddZerosLeft(string s, int cant)
    {
        StringBuilder d = new StringBuilder();
        for (int i = 0; i < cant; i++) d.Append("0");

        d.Append(s);

        return d.ToString();
    }

    /// <summary>
    /// Eliminar ceros a la izquierda
    /// </summary>
    /// <param name="s">Cadena a modificar</param>
    /// <returns>Cadena modificada</returns>
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

    /// <summary>
    /// Eliminar ceros a la derecha
    /// </summary>
    /// <param name="s">Cadena a modificar</param>
    /// <returns>Cadena modificada</returns>
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

    /// <summary>
    /// Añadir ceros a la derecha
    /// </summary>
    /// <param name="s">Cadena a modificar</param>
    /// <param name="cant">Cantidad de ceros para añadir</param>
    /// <returns>Cadena modificada</returns>
    internal static string AddZerosRight(string s, int cant)
    {
        StringBuilder d = new StringBuilder(s);
        for (int i = 0; i < cant; i++) d.Append("0");

        return d.ToString();
    }

    /// <summary>
    /// Emparejar el tamaño de dos cadenas añadiendo ceros a la izquierda
    /// </summary>
    /// <param name="x">Cadena a emparejar</param>
    /// <param name="y">Cadena a emparejar</param>
    /// <returns>Cantidad de ceros añadidos</returns>
    internal static int EqualZerosLeft(ref string x, ref string y)
    {
        int max = Math.Max(x.Length, y.Length);

        (x, y) = (AddZerosLeft(x, max - x.Length), AddZerosLeft(y, max - y.Length));

        return max;
    }

    /// <summary>
    /// Emparejar el tamaño de dos cadenas añadiendo ceros a la derecha
    /// </summary>
    /// <paramref name="x">Cadena a emparejar</paramref>
    /// <paramref name="y">Cadena a emparejar</paramref>
    /// <returns>Cantidad de ceros añadidos</returns>
    internal static int EqualZerosRight(ref string x, ref string y)
    {
        int max = Math.Max(x.Length, y.Length);

        (x, y) = (AddZerosRight(x, max - x.Length), AddZerosRight(y, max - y.Length));

        return max;
    }

    internal static List<long> EliminateZerosLeftValue(List<long> numberValue, int precision)
    {
        List<long> l = new List<long>();
        bool act = false;

        for (int i = numberValue.Count - 1; i >= 0; i--)
        {
            if (numberValue[i] != 0 || i == precision) act = true;

            if (act) l.Add(numberValue[i]);
        }

        l.Reverse();
        return l;
    }

    internal static List<long> AddZerosLeftValue(List<long> numberValue, int cant)
    {
        long[] aux = new long[cant];

        return numberValue.Concat(aux).ToList();
    }

    internal static List<long> AddZerosRightValue(List<long> numberValue, int cant)
    {
        long[] aux = new long[cant];

        return aux.Concat(numberValue).ToList();
    }

    internal static (List<long>, List<long>) EqualZerosLeftValue(List<long> x, List<long> y)
    {
        List<long> lx = AddZerosLeftValue(x, Math.Max(x.Count, y.Count) - x.Count);
        List<long> ly = AddZerosLeftValue(y, Math.Max(x.Count, y.Count) - y.Count);

        return (lx, ly);
    }

    /// <summary>
    /// Comparar dos listas
    /// </summary>
    /// <param name="x">Lista</param>
    /// <param name="y">Lista</param>
    /// <returns>1 si x es mayor -1 si y es mayor 0 si son iguales</returns>
    internal static int CompareList(List<long> x, List<long> y)
    {
        (x, y) = EqualZerosLeftValue(x, y);

        for (int i = x.Count - 1; i >= 0; i--)
        {
            if (x[i] > y[i]) return 1;
            if (x[i] < y[i]) return -1;
        }

        return 0;
    }
}