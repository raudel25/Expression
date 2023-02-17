namespace BigNum.Operations;

internal static class IntegerOperations
{
    /// <summary>
    ///     MCD entre dos numeros
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado entero</returns>
    internal static IntegerNumbers Mcd(IntegerNumbers x, IntegerNumbers y)
    {
        if (x == x.Integer0 || y == y.Integer0)
            throw new Exception("Invalid Operation (division by 0)");

        var (a, b) = (BigNumMath.Max(x, y), BigNumMath.Min(x, y));
        var rest = x.Integer1;

        while (rest != x.Integer0)
        {
            rest = a % b;
            (a, b) = (b, rest);
        }

        return a;
    }

    /// <summary>
    ///     Combinaciones sin repeticion
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <returns>Resultado entero</returns>
    internal static IntegerNumbers Factorial(IntegerNumbers x)
    {
        var fact = x.Integer1;
        var xx = int.Parse(x.ToString());
        var index = x.Integer1;

        for (var i = 1; i <= xx; i++) fact *= index++;

        return fact;
    }

    internal static IntegerNumbers Combinations(IntegerNumbers x, IntegerNumbers y)
    {
        var min = BigNumMath.Min(x - y, y);

        var index = x;
        var fact = x.Integer1;
        var minInt = int.Parse(min.ToString());

        for (var i = 0; i < minInt; i++) fact *= index--;

        return fact / Factorial(min);
    }
}