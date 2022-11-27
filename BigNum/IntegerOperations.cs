namespace BigNum;

internal static class IntegerOperations
{
    /// <summary>
    /// MCD entre dos numeros
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado entero</returns>
    internal static IntegerNumbers Mcd(IntegerNumbers x, IntegerNumbers y)
    {
        if (x == IntegerNumbers.Integer0 || y == IntegerNumbers.Integer0)
            throw new Exception("Operacion Invalida (division por 0)");

        (IntegerNumbers a, IntegerNumbers b) = (BigNumMath.Max(x, y), BigNumMath.Min(x, y));
        IntegerNumbers rest = IntegerNumbers.Integer1;

        while (rest != IntegerNumbers.Integer0)
        {
            rest = a % b;
            (a, b) = (b, rest);
        }

        return a;
    }

    /// <summary>
    /// Combinaciones sin repeticion
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <returns>Resultado entero</returns>
    internal static IntegerNumbers Factorial(IntegerNumbers x)
    {
        IntegerNumbers fact = IntegerNumbers.Integer1;
        int xx = int.Parse(x.ToString());
        IntegerNumbers index = IntegerNumbers.Integer1;

        for (int i = 1; i <= xx; i++) fact *= index++;

        return fact;
    }

    internal static IntegerNumbers Combinations(IntegerNumbers x, IntegerNumbers y)
    {
        IntegerNumbers min = BigNumMath.Min(x - y, y);

        IntegerNumbers index = x;
        IntegerNumbers fact = IntegerNumbers.Integer1;
        int minInt = int.Parse(min.ToString());

        for (int i = 0; i < minInt; i++) fact *= index--;

        return fact / Factorial(min);
    }
}