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

        if (x.Sign == y.Sign) return SumOperation(x, y, true, x.Positive());

        int compare = x.Abs.CompareTo(y.Abs);
        if (compare == 0) return RealNumbers.Real0;
        if (compare == 1) return SumOperation(x.Abs, y.Abs, false, x.Positive());

        return SumOperation(y.Abs, x.Abs, false, y.Positive());
    }

    /// <summary>
    /// Determinar los signos y el tipo de operacion a realizar
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <param name="sum">Operacion a realizar</param>
    /// <param name="positive">Signo del resultado</param>
    /// <returns>Resultado real</returns>
    private static RealNumbers SumOperation(RealNumbers x, RealNumbers y, bool sum, bool positive)
    {
        (string xSumDecimal, string ySumDecimal) = (x.PartDecimal, y.PartDecimal);
        (string xSumNumber, string ySumNumber) = (x.PartNumber, y.PartNumber);

        int mayorDecimal = AuxOperations.EqualZerosRight(ref xSumDecimal, ref ySumDecimal);
        int mayorNumber = AuxOperations.EqualZerosLeft(ref xSumNumber, ref ySumNumber);

        string result = sum
            ? Sum(xSumNumber + xSumDecimal, ySumNumber + ySumDecimal)
            : Subtraction(xSumNumber + xSumDecimal, ySumNumber + ySumDecimal);

        string partDecimal = result.Length == mayorDecimal + mayorNumber
            ? result.Substring(mayorNumber, mayorDecimal)
            : result.Substring(mayorNumber + 1, mayorDecimal);

        string partNumber = result.Length == mayorDecimal + mayorNumber
            ? result.Substring(0, mayorNumber)
            : result.Substring(0, mayorNumber + 1);

        return new RealNumbers(AuxOperations.EliminateZerosLeft(partNumber),
            AuxOperations.EliminateZerosRight(partDecimal),
            positive);
    }

    /// <summary>
    /// Sumar dos cadenas
    /// </summary>
    /// <param name="x">Cadena</param>
    /// <param name="y">Cadena</param>
    /// <returns>Resultado</returns>
    private static string Sum(string x, string y)
    {
        StringBuilder sum = new StringBuilder();
        bool drag = false;
        int len = x.Length;

        for (int i = 0; i < len; i++)
        {
            int n = x[len - 1 - i] - 48 + y[len - 1 - i] - 48;

            n = drag ? n + 1 : n;
            drag = n >= 10;
            sum.Insert(0, n % 10);
        }

        if (drag) sum.Insert(0, 1);

        return sum.ToString();
    }

    /// <summary>
    /// Restar dos cadenas
    /// </summary>
    /// <param name="x">Cadena</param>
    /// <param name="y">Cadena</param>
    /// <returns>Resultado</returns>
    private static string Subtraction(string x, string y)
    {
        StringBuilder sub = new StringBuilder();
        bool drag = false;
        int len = x.Length;

        for (int i = 0; i < len; i++)
        {
            int n = x[len - 1 - i] - 48 - (y[len - 1 - i] - 48);

            n = drag ? n - 1 : n;
            drag = n < 0;
            n = n < 0 ? n + 10 : n;
            sub.Insert(0, n);
        }

        return sub.ToString();
    }
}