namespace BigNum;

internal static class DivisionOperations
{
    /// <summary>
    /// Dividir dos numeros reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <param name="integer">Determinar si los numeros son enteros</param>
    /// <returns>Cociente y resto</returns>
    internal static RealNumbers Division(RealNumbers x, RealNumbers y, bool integer = false)
    {
        if (x.Precision != y.Precision) throw new Exception("Numeros de representacion diferente");
        
        bool positive = x.Sign == y.Sign;

        if (y == RealNumbers.Real0) throw new Exception("Operacion Invalida (division por 0)");
        if (y.Abs == RealNumbers.Real1)
            return new RealNumbers(x.NumberValue, positive, x.Precision);
        
        var result = AlgorithmD(x.NumberValue, y.NumberValue, integer, x.Base10, x.Precision);

        return new RealNumbers(result, positive, x.Precision);
    }

    /// <summary>
    /// Algortimo para la division
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <param name="integer">Determinar si los numeros son enteros</param>
    /// <param name="base10">Base</param>
    /// <param name="precision">Precision decimal</param>
    /// <returns>Cociente y resto</returns>
    private static List<long> AlgorithmD(List<long> x, List<long> y, bool integer, long base10, int precision)
    {
        (x, y) = Normalize(x, y, base10);

        List<long> result = new List<long>();
        List<long> rest = x.Skip(x.Count - y.Count + 1).ToList();
        long aux;

        for (int t = x.Count - y.Count; t >= 0; t--)
        {
            (aux, rest) = DivisionImmediate(new[] { x[t] }.Concat(rest).ToList(), y, base10, precision);
            result.Add(aux);
        }

        for (int i = 0; i < precision; i++)
        {
            if (!integer)
                (aux, rest) = DivisionImmediate(new long[] { 0 }.Concat(rest).ToList(), y, base10, precision);
            else aux = 0;

            result.Add(aux);
        }

        result.Reverse();

        return result;
    }

    /// <summary>
    /// Dividir un numero por un digito
    /// </summary>
    /// <param name="div">Numero a dividir</param>
    /// <param name="divisor">Divisor</param>
    /// <param name="base10">Base</param>
    /// <param name="precision">Precision decimal</param>
    /// <returns>cociente y resto de la divison</returns>
    private static (long, List<long>) DivisionImmediate(List<long> div, List<long> divisor, long base10, int precision)
    {
        long result;
        if (div.Count < divisor.Count) return (0, div);

        if (div.Count == divisor.Count) result = div[div.Count - 1] / divisor[div.Count - 1];
        else result = (div[div.Count - 1] * base10 + div[div.Count - 2]) / divisor[divisor.Count - 1];

        List<long> aux;
        while (true)
        {
            aux = ProductOperations.SimpleMultiplication(divisor, result, base10);

            if (AuxOperations.CompareList(div, aux) != -1) break;

            result -= 1;
        }

        return (result, AuxOperations.EliminateZerosLeftValue(SumOperations.Subtraction(div, aux, base10), precision));
    }

    /// <summary>
    /// Normalizar el divisor
    /// </summary>
    /// <param name="x">Dividendo</param>
    /// <param name="y">Divisor</param>
    /// <param name="base10">Base</param>
    /// <returns>Dividendo y divisor normalizados</returns>
    private static (List<long>, List<long>) Normalize(List<long> x, List<long> y, long base10)
    {
        if (y[y.Count - 1] < base10 / 2)
        {
            long mult = 1;
            long aux = y[y.Count - 1] / (base10 / 10);

            if (aux == 0)
            {
                mult = base10 / (int)Math.Pow(10, (int)Math.Log10(y[y.Count - 1])) / 10;
                aux = y[y.Count - 1] * mult / (base10 / 10);
            }

            if (aux == 1) mult *= 5;
            else if (aux == 2) mult *= 3;
            else if (aux == 3) mult *= 2;
            else if (aux == 4) mult *= 2;

            return (ProductOperations.SimpleMultiplication(x, mult, base10),
                ProductOperations.SimpleMultiplication(y, mult, base10));
        }

        return (x, y);
    }
}