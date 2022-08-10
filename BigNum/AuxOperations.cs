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
        for (int i = 0; i < cant; i++) s = 0 + s;

        return s;
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
        for (int i = 0; i < cant; i++) s = s + 0;

        return s;
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
}