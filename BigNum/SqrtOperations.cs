namespace BigNum;

internal static class SqrtOperations
{
    private static int _precision = 50;

    /// <summary>
    /// Raiz n-esima 
    /// </summary>
    /// <param name="x">Radicando</param>
    /// <param name="yInt">Indice del radical</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Sqrt(RealNumbers x, int yInt)
    {
        RealNumbers y = new RealNumbers(yInt.ToString(), x.Precision, x.Base10, x.IndBase10, yInt >= 0);
        if (yInt == 1) return x;
        if (x == x.Real0) return x.Real0;

        (RealNumbers aux, int ind) = ScalateOne(x.Abs);
        if (ind != 0)
            aux *= new RealNumbers($"{AuxOperations.AddZerosRight("1", yInt - ind % yInt)}.0", x.Precision, x.Base10,
                x.IndBase10);

        bool parity = yInt % 2 == 0;
        bool positive = parity || x.Positive();

        if (parity && !x.Positive()) throw new Exception("Operacion Invalida (el resultado no es real)");

        RealNumbers sqrt = AlgorithmSqrt(aux, BigNumMath.Abs(y), yInt);

        if (ind != 0)
            sqrt /= new RealNumbers($"{AuxOperations.AddZerosRight("1", (ind + yInt - ind % yInt) / yInt)}.0",
                x.Precision, x.Base10, x.IndBase10);

        return y.Positive()
            ? new RealNumbers(sqrt.NumberValue, x.Precision, x.Base10, x.IndBase10, positive)
            : y.Real1 / new RealNumbers(sqrt.NumberValue, x.Precision, x.Base10, x.IndBase10, positive);
    }

    /// <summary>
    /// Algoritmo para aproximar la raiz n-esima
    /// </summary>
    /// <param name="x">Radicando</param>
    /// <param name="y">Indice del radical</param>
    /// <param name="yInt">Indice del radical</param>
    /// <returns>Resultado real</returns>
    private static RealNumbers AlgorithmSqrt(RealNumbers x, RealNumbers y, int yInt)
    {
        (RealNumbers value, bool find) = ApproximateInteger(x, yInt);

        if (find) return value;

        var aux = y - y.Real1;

        //https://es.frwiki.wiki/wiki/Algorithme_de_calcul_de_la_racine_n-i%C3%A8me
        for (int i = 0; i < _precision; i++)
        {
            value = y.Real1 / y * (aux * value + x / BigNumMath.Pow(value, yInt - 1));
        }

        return value;
    }

    /// <summary>
    /// Buscar la potencia n-esima entera mas cercana
    /// </summary>
    /// <param name="x">Radicando</param>
    /// <param name="y">Indice del radical</param>
    /// <returns>Resultado entero</returns>
    private static (RealNumbers, bool) ApproximateInteger(RealNumbers x, int y)
    {
        int cant = x.ToString().Split('.')[0].Length - 1;

        string s = "1";
        s = AuxOperations.AddZerosRight(s, cant / y);
        RealNumbers value = new RealNumbers($"{s}.0", x.Precision, x.Base10, x.IndBase10);

        RealNumbers pow = BigNumMath.Pow(value, y);

        while (pow < x)
        {
            value++;
            pow = BigNumMath.Pow(value, y);
        }

        return pow == x ? (value, true) : (value - x.Real1, false);
    }

    private static (RealNumbers, int) ScalateOne(RealNumbers x)
    {
        int cant = 0;
        RealNumbers real10 = new RealNumbers("10.0", x.Precision, x.Base10, x.IndBase10);

        while (x < x.Real1)
        {
            x *= real10;
            cant++;
        }

        return (x, cant);
    }
}