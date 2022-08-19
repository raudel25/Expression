using System.Text;

namespace BigNum;

internal static class DivisionOperations
{
    /// <summary>
    /// Cantidad maxima de cifras despues de la coma
    /// </summary>
    private static int _precision = 20;

    /// <summary>
    /// Dividir dos numeros reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <param name="integer">Determinar si los numeros son enteros</param>
    /// <returns>Cociente y resto</returns>
    internal static (RealNumbers, IntegerNumbers) Division(RealNumbers x, RealNumbers y, bool integer = false)
    {
        bool positive = x.Sign == y.Sign;

        if (y == RealNumbers.Real0) throw new Exception("Operacion Invalida (division por 0)");
        if (y.Abs == RealNumbers.Real1)
            return (new RealNumbers(x.PartNumber, x.PartDecimal, positive), IntegerNumbers.Integer0);

        (string xPartDecimal, string yPartDecimal) = (x.PartDecimal, y.PartDecimal);
        AuxOperations.EqualZerosRight(ref xPartDecimal, ref yPartDecimal);
        if (integer) (xPartDecimal, yPartDecimal) = ("", "");

        IntegerNumbers m = new IntegerNumbers(x.PartNumber + xPartDecimal, "0");
        IntegerNumbers n = new IntegerNumbers(y.PartNumber + yPartDecimal, "0");

        int cantDecimal = 0;

        (string result, IntegerNumbers rest) = DivisionAlgorithm(m, n, integer, ref cantDecimal);

        if (cantDecimal != 0)
            return (new RealNumbers(result.Substring(0, result.Length - cantDecimal),
                result.Substring(result.Length - cantDecimal, cantDecimal), positive), rest);
        return (new RealNumbers(result, "0", positive), rest);
    }

    /// <summary>
    /// Algortimo para la division
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <param name="integer">Determinar si los numeros son enteros</param>
    /// <param name="cantDecimal">Cantidad de cifras para correr la coma</param>
    /// <returns>Cociente y resto</returns>
    private static (string, IntegerNumbers) DivisionAlgorithm(IntegerNumbers x, IntegerNumbers y, bool integer,
        ref int cantDecimal)
    {
        StringBuilder result = new StringBuilder();
        IntegerNumbers rest = IntegerNumbers.Integer0;

        foreach (var t in x.PartNumber)
        {
            IntegerNumbers div = new IntegerNumbers(rest.PartNumber + t, "0");

            rest = DivisionImmediate(div, y, result);
        }

        while (rest != IntegerNumbers.Integer0 && !integer)
        {
            IntegerNumbers div = new IntegerNumbers(rest.PartNumber + "0", "0");

            rest = DivisionImmediate(div, y, result);

            cantDecimal++;

            if (cantDecimal == _precision) break;
        }

        return (result.ToString(), rest);
    }

    /// <summary>
    /// Dividir un numero por un digito
    /// </summary>
    /// <param name="div">Numero a dividir</param>
    /// <param name="divisor">Divisor</param>
    /// <param name="result">Cociente</param>
    /// <returns>Resto de la divison</returns>
    private static IntegerNumbers DivisionImmediate(IntegerNumbers div, IntegerNumbers divisor, StringBuilder result)
    {
        IntegerNumbers aux = IntegerNumbers.Integer0;
        for (int j = 9; j >= 0; j--)
        {
            aux = divisor * BigNumMath.ConvertToIntegerNumbers(j);

            if (aux <= div)
            {
                result.Append(j);
                break;
            }
        }

        return div - aux;
    }
}