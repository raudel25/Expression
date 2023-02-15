namespace BigNum;

public static class LogarithmOperations
{
    private static int _precision = 150;

    /// <summary>
    /// Logaritmo en base e
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Ln(RealNumbers x)
    {
        if (x == Constants.ConstantE(x.Real0, x.Real1)) return x.Real1;

        //ln(1/x)=-ln(x)
        bool positive = x.Abs > x.Real1;
        x = positive ? x.Real1 / x : x;
        x = x.Real1 - x;

        RealNumbers pow = x;
        RealNumbers index = x.Real1;
        RealNumbers ln = x.Real0;

        //Serie de Taylor ln(1-x)
        //https://es.wikipedia.org/wiki/Serie_de_Taylor
        for (int i = 1; i <= _precision; i++)
        {
            ln += pow / index;
            pow *= x;
            index++;
        }

        return positive ? ln : -ln;
    }

    /// <summary>
    /// Logaritmo entre 2 numeros
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Log(RealNumbers x, RealNumbers y)
    {
        RealNumbers pow = x.Real1;
        int index = 0;

        while (pow <= y)
        {
            if (pow == y) return new RealNumbers(index.ToString(), x.Precision, x.Base10, x.IndBase10);
            pow *= x;
            index++;
        }

        return Ln(y) / Ln(x);
    }
}