namespace BigNum;

internal static class SqrtOperations
{
    private static int _precision = 50;

    /// <summary>
    /// Raiz n-esima 
    /// </summary>
    /// <param name="x">Radicando</param>
    /// <param name="y">Indice del radical</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Sqrt(RealNumbers x, IntegerNumbers y)
    {
        if (y == IntegerNumbers.Integer1) return x;
        if (x == RealNumbers.Real0) return RealNumbers.Real0;
        if (y == RealNumbers.Real1) return RealNumbers.Real1;

        bool parity = y % new IntegerNumbers("2") == IntegerNumbers.Integer0;
        bool positive = parity || x.Positive();

        if (parity && !x.Positive()) throw new Exception("Operacion Invalida (el resultado no es real)");

        RealNumbers sqrt = AlgorithmSqrt(x.Abs, BigNumMath.Abs(y));

        return y.Positive()
            ? new RealNumbers(sqrt.NumberValue, positive,x.Precision)
            : RealNumbers.Real1 / new RealNumbers(sqrt.NumberValue, positive,x.Precision);
    }

    /// <summary>
    /// Algoritmo para aproximar la raiz n-esima
    /// </summary>
    /// <param name="x">Radicando</param>
    /// <param name="y">Indice del radical</param>
    /// <returns>Resultado real</returns>
    private static RealNumbers AlgorithmSqrt(RealNumbers x, IntegerNumbers y)
    {
        (RealNumbers value, bool find) = ApproximateInteger(x, y);
        Console.WriteLine(value);
        if (find) return value;

        RealNumbers aux = BigNumMath.IntegerToReal(y - IntegerNumbers.Integer1);
        RealNumbers realY = BigNumMath.IntegerToReal(y);

        //https://es.frwiki.wiki/wiki/Algorithme_de_calcul_de_la_racine_n-i%C3%A8me
        for (int i = 0; i < _precision; i++)
        {
            value = RealNumbers.Real1 / realY * (aux * value + x / BigNumMath.Pow(value, aux));
        }

        return value;
    }

    /// <summary>
    /// Buscar la potencia n-esima entera mas cercana
    /// </summary>
    /// <param name="x">Radicando</param>
    /// <param name="y">Indice del radical</param>
    /// <returns>Resultado entero</returns>
    private static (RealNumbers, bool) ApproximateInteger(RealNumbers x, IntegerNumbers y)
    {
        int sqrt = int.Parse(y.ToString());
        int cant = x.ToString().Split('.')[0].Length - 1;
    
        string s = "1";
        s = AuxOperations.AddZerosRight(s, cant / sqrt);
        RealNumbers value = new RealNumbers($"{s}.0");
    
        RealNumbers pow = BigNumMath.Pow(value, y);
    
        while (pow < x)
        {
            value++;
            pow = BigNumMath.Pow(value, y);
        }
    
        return pow == x ? (value, true) : (value - RealNumbers.Real1, false);
    }
}