using System.Text;

namespace BigNum;

internal static class SumOperations
{
    /// <summary>
    /// Sumar dos numeros reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers Sum(RealNumbers x, RealNumbers y)
    {
        if (x == RealNumbers.Real0) return y;
        if (y == RealNumbers.Real0) return x;

        if (x.Sign == y.Sign) return new RealNumbers(Sum(x.NumberValue,y.NumberValue,x.Base10),x.Positive());

        int compare = x.Abs.CompareTo(y.Abs);
        if (compare == 0) return RealNumbers.Real0;
        if (compare == 1) return new RealNumbers(Subtraction(x.NumberValue,y.NumberValue,x.Base10),x.Positive());

        return new RealNumbers(Subtraction(x.NumberValue,y.NumberValue,x.Base10),y.Positive());
    }

    // /// <summary>
    // /// Determinar los signos y el tipo de operacion a realizar
    // /// </summary>
    // /// <param name="x">Numero real</param>
    // /// <param name="y">Numero real</param>
    // /// <param name="sum">Operacion a realizar</param>
    // /// <param name="positive">Signo del resultado</param>
    // /// <returns>Resultado real</returns>
    // private static RealNumbers SumOperation(RealNumbers x, RealNumbers y, bool sum, bool positive)
    // {
    //     (string xSumDecimal, string ySumDecimal) = (x.PartDecimal, y.PartDecimal);
    //     (string xSumNumber, string ySumNumber) = (x.PartNumber, y.PartNumber);
    //
    //     int mayorDecimal = AuxOperations.EqualZerosRight(ref xSumDecimal, ref ySumDecimal);
    //     int mayorNumber = AuxOperations.EqualZerosLeft(ref xSumNumber, ref ySumNumber);
    //
    //     string result = sum
    //         ? Sum(xSumNumber + xSumDecimal, ySumNumber + ySumDecimal)
    //         : Subtraction(xSumNumber + xSumDecimal, ySumNumber + ySumDecimal);
    //
    //     string partDecimal = result.Length == mayorDecimal + mayorNumber
    //         ? result.Substring(mayorNumber, mayorDecimal)
    //         : result.Substring(mayorNumber + 1, mayorDecimal);
    //
    //     string partNumber = result.Length == mayorDecimal + mayorNumber
    //         ? result.Substring(0, mayorNumber)
    //         : result.Substring(0, mayorNumber + 1);
    //
    //     return new RealNumbers(AuxOperations.EliminateZerosLeft(partNumber),
    //         AuxOperations.EliminateZerosRight(partDecimal),
    //         positive);
    // }

    /// <summary>
    /// Sumar dos cadenas
    /// </summary>
    /// <param name="x">Cadena</param>
    /// <param name="y">Cadena</param>
    /// <returns>Resultado</returns>
    internal static List<long> Sum(List<long> x, List<long> y,long base10)
    {
        (x, y) = AuxOperations.EqualZerosLeftValue(x, y);
        bool drag = false;
        int len = x.Count;
        List<long> sum = new List<long>(len);

        for (int i = 0; i < len; i++)
        {
            long n = x[i] + y[i];

            n = drag ? n + 1 : n;
            drag = n >= base10;
            sum.Add(n % base10);
        }

        if (drag) sum.Add(1);

        return sum;
    }

    /// <summary>
    /// Restar dos cadenas
    /// </summary>
    /// <param name="x">Cadena</param>
    /// <param name="y">Cadena</param>
    /// <returns>Resultado</returns>
    internal static List<long> Subtraction(List<long> x, List<long> y,long base10)
    {
        (x, y) = AuxOperations.EqualZerosLeftValue(x, y);
        bool drag = false;
        int len = x.Count;
        List<long> sub = new List<long>(len);

        for (int i = 0; i < len; i++)
        {
            long n = x[i] - y[i];

            n = drag ? n - 1 : n;
            drag = n < 0;
            n = n < 0 ? n + base10 : n;
            sub.Add(n);
        }

        return sub;
    }
}