namespace BigNum;

public static class BigNumMath
{
    /// <summary>
    /// Sumar dos numeros reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Sum(RealNumbers x, RealNumbers y) => SumOperations.Sum(x, y);

    /// <summary>
    /// Sumar dos fracciones reales
    /// </summary>
    /// <param name="x">Fraccion real</param>
    /// <param name="y">Fraccion real</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Sum(Fraction x, Fraction y) =>
        new Fraction(x.Numerator * y.Denominator + y.Numerator * x.Denominator, x.Denominator * y.Denominator);

    /// <summary>
    /// Multiplicar dos numeros reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Product(RealNumbers x, RealNumbers y) => ProductOperations.Product(x, y);

    /// <summary>
    /// Multiplicar dos fracciones reales
    /// </summary>
    /// <param name="x">Fraccion real</param>
    /// <param name="y">Fraccion real</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Product(Fraction x, Fraction y) =>
        new Fraction(x.Numerator * y.Numerator, x.Denominator * y.Denominator);

    /// <summary>
    /// Dividir dos numeros reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <param name="integer">Si es un numero entero</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Division(RealNumbers x, RealNumbers y, bool integer = false) =>
        DivisionOperations.Division(x, y, integer).Item1;

    /// <summary>
    /// Dividir dos fracciones reales
    /// </summary>
    /// <param name="x">Fraccion real</param>
    /// <param name="y">Fraccion real</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Division(Fraction x, Fraction y) => x * new Fraction(y.Denominator, y.Numerator);

    /// <summary>
    /// Resto de la division entre dos enteros
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Rest(IntegerNumbers x, IntegerNumbers y) =>
        DivisionOperations.Division(x, y, true).Item2;

    /// <summary>
    /// Potencia entre un numero real y un numero entero(C#)
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Pow(RealNumbers x, IntegerNumbers y) => PowOperations.Pow(x, y);

    public static RealNumbers Pow(RealNumbers x, RealNumbers y) => PowOperations.Pow(x, y);

    public static RealNumbers Sqrt(RealNumbers x, IntegerNumbers y) => SqrtOperations.Sqrt(x, y);

    /// <summary>
    /// Potencia entre una fraccion real y un numero entero(C#)
    /// </summary>
    /// <param name="x">Fraccion real</param>
    /// <param name="pow">Numero entero(C#)</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Pow(Fraction x, int pow)
    {
        Fraction result = new Fraction(RealNumbers.Real1, RealNumbers.Real1);

        for (int i = 0; i < Math.Abs(pow); i++) result *= x;

        if (pow < 0) result = new Fraction(result.Denominator, result.Numerator);

        return result;
    }

    public static IntegerNumbers Factorial(IntegerNumbers x)
    {
        IntegerNumbers fact = new IntegerNumbers("1");
        int xx = int.Parse(x.PartNumber);
        IntegerNumbers index = IntegerNumbers.Integer1;

        for (int i = 1; i <= xx; i++) fact *= index++;

        return fact;
    }

    /// <summary>
    /// Maximo entre dos numero reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Max(RealNumbers x, RealNumbers y)
    {
        if (x.CompareTo(y) == 1) return x;

        return y;
    }

    public static IntegerNumbers Max(IntegerNumbers x, IntegerNumbers y) => RealToInteger(Max((RealNumbers) x, y));

    /// <summary>
    /// Minimo entre dos numero reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Min(RealNumbers x, RealNumbers y)
    {
        if (x.CompareTo(y) == -1) return x;

        return y;
    }

    public static IntegerNumbers Min(IntegerNumbers x, IntegerNumbers y) => RealToInteger(Min((RealNumbers) x, y));

    /// <summary>
    /// Determinar el modulo de un numero real
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Abs(RealNumbers x) => x.Abs;

    public static IntegerNumbers Abs(IntegerNumbers x) => RealToInteger(x.Abs);

    /// <summary>
    /// Determinar el opuesto de un numero real
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Opposite(RealNumbers x) => new RealNumbers(x.PartNumber, x.PartDecimal, !x.Positive());

    /// <summary>
    /// Determinar el opuesto de una fraccion real
    /// </summary>
    /// <param name="x">Fraccion real</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Opposite(Fraction x) => new Fraction(BigNumMath.Opposite(x.Numerator), x.Denominator);

    /// <summary>
    /// Convertir de double(C#) a numero real
    /// </summary>
    /// <param name="n">Numero double(C#)</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers ConvertToRealNumbers(double n) => new RealNumbers(n + "", n >= 0);

    /// <summary>
    /// Convertir de entero(C#) a numero real
    /// </summary>
    /// <param name="n">Numero entero(C#)</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers ConvertToIntegerNumbers(int n) => new IntegerNumbers(n + "", n >= 0);

    /// <summary>
    /// Convertir de real a entero
    /// </summary>
    /// <param name="n">Numero real</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers RealToInteger(RealNumbers n) => new IntegerNumbers(n.PartNumber, n.Positive());

    public static IntegerNumbers Mcd(IntegerNumbers x, IntegerNumbers y)
    {
        if (x == IntegerNumbers.Integer0 || y == IntegerNumbers.Integer0)
            throw new Exception("Operacion Invalida (division por 0)");

        (IntegerNumbers a, IntegerNumbers b) = (Max(x, y), Min(x, y));
        IntegerNumbers rest = IntegerNumbers.Integer1;

        while (rest != IntegerNumbers.Integer0)
        {
            rest = a % b;
            (a, b) = (b, rest);
        }

        return a;
    }

    public static RealNumbers E = Constants.ConstantE();

    public static RealNumbers Sin(RealNumbers x) =>
        TrigonometryOperations.SinCos(x, (a) => (a & 1) == 0, (a) => a % 4 == 1);

    public static RealNumbers Cos(RealNumbers x) =>
        TrigonometryOperations.SinCos(x, (a) => (a & 1) != 0, (a) => a % 4 == 0);
}