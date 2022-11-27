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
    /// <param name="x">Dividendo</param>
    /// <param name="y">Divisor</param>
    /// <param name="integer">Si es un numero entero</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Division(RealNumbers x, RealNumbers y, bool integer = false) =>
        DivisionOperations.Division(x, y, integer);

    /// <summary>
    /// Dividir dos fracciones reales
    /// </summary>
    /// <param name="x">Dividendo</param>
    /// <param name="y">Divisor</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Division(Fraction x, Fraction y) => x * new Fraction(y.Denominator, y.Numerator);

    /// <summary>
    /// Resto de la division entre dos enteros
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Modulo</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Rest(IntegerNumbers x, IntegerNumbers y) =>
        x-RealToInteger(DivisionOperations.Division(x, y, true)*y);

    /// <summary>
    /// Potencia entre un numero real y un numero entero
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="y">Exponente</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Pow(RealNumbers x, IntegerNumbers y) => PowOperations.Pow(x, y);

    /// <summary>
    /// Potencia entre 2 numeros reales
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="y">Exponente</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Pow(RealNumbers x, RealNumbers y) => PowOperations.Pow(x, y);

    /// <summary>
    /// Potencia entre 2 numeros enteros
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="y">Exponente</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Pow(IntegerNumbers x, IntegerNumbers y) => RealToInteger(PowOperations.Pow(x, y));

    /// <summary>
    /// Raiz n-esima 
    /// </summary>
    /// <param name="x">Radicando</param>
    /// <param name="y">Indice del radical</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Sqrt(RealNumbers x, IntegerNumbers y) => SqrtOperations.Sqrt(x, y);

    /// <summary>
    /// Potencia entre una fraccion real y un numero entero
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="pow">Exponente</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Pow(Fraction x, IntegerNumbers pow) => PowOperations.Pow(x, pow);

    /// <summary>
    /// Maximo entre dos numero reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Max(RealNumbers x, RealNumbers y) => x.CompareTo(y) == 1 ? x : y;

    /// <summary>
    /// Maximo entre dos numero enteros
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Max(IntegerNumbers x, IntegerNumbers y) => RealToInteger(Max((RealNumbers) x, y));

    /// <summary>
    /// Minimo entre dos numero reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Min(RealNumbers x, RealNumbers y) => x.CompareTo(y) == -1 ? x : y;

    /// <summary>
    /// Minimo entre dos numero enteros
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Min(IntegerNumbers x, IntegerNumbers y) => RealToInteger(Min((RealNumbers) x, y));

    /// <summary>
    /// Determinar el modulo de un numero real
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Abs(RealNumbers x) => x.Abs;

    /// <summary>
    /// Determinar el modulo de un numero entero
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Abs(IntegerNumbers x) => RealToInteger(x.Abs);

    /// <summary>
    /// Determinar el opuesto de un numero real
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Opposite(RealNumbers x) => new RealNumbers(x.NumberValue, !x.Positive(),x.Precision);

    /// <summary>
    /// Determinar el opuesto de un numero entero
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Opposite(IntegerNumbers x) => new IntegerNumbers(x.ToString().Split('.')[0], !x.Positive());

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
    public static RealNumbers ConvertToRealNumbers(double n) => new RealNumbers(Math.Abs(n) + "", n >= 0);

    /// <summary>
    /// Convertir de entero(C#) a numero real
    /// </summary>
    /// <param name="n">Numero entero(C#)</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers ConvertToIntegerNumbers(int n) => new IntegerNumbers(Math.Abs(n) + "", n >= 0);

    /// <summary>
    /// Convertir de real a entero
    /// </summary>
    /// <param name="n">Numero real</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers RealToInteger(RealNumbers n) => new IntegerNumbers(n.ToString().Split('.')[0], n.Positive());

    public static bool IsInteger(RealNumbers n) => n.ToString().Split('.')[0] == "0";

    /// <summary>
    /// Maximo comun divisor entre 2 numeros
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Mcd(IntegerNumbers x, IntegerNumbers y) => IntegerOperations.Mcd(x, y);

    /// <summary>
    /// Factorial de un numero
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Factorial(IntegerNumbers x) => IntegerOperations.Factorial(x);

    /// <summary>
    /// Combinaciones sin repecticion
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Combinations(IntegerNumbers x, IntegerNumbers y) =>
        IntegerOperations.Combinations(x, y);

    /// <summary>
    /// Aproximacion del numero E
    /// </summary>
    public static RealNumbers E => Constants.ConstantE();

    /// <summary>
    /// Aproximacion del numero PI
    /// </summary>
    public static RealNumbers PI => Constants.ConstantPI();

    /// <summary>
    /// Logaritmo en base E
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Ln(RealNumbers x) => LogarithmOperations.Ln(x);

    /// <summary>
    /// Logartimo entre 2 numeros reales
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="y">Potencia</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Log(RealNumbers x, RealNumbers y) => LogarithmOperations.Log(x, y);

    /// <summary>
    /// Seno de un numero real
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Sin(RealNumbers x) => TrigonometryOperations.SinCos(x, true);

    /// <summary>
    /// Coseno de un numero real
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Cos(RealNumbers x) => TrigonometryOperations.SinCos(x, false);

    public static RealNumbers Asin(RealNumbers x) => TrigonometryOperations.Asin(x);

    public static RealNumbers Acos(RealNumbers x) => PI / new RealNumbers("2") - Asin(x);

    public static RealNumbers Atan(RealNumbers x) => TrigonometryOperations.Atan(x);

    public static RealNumbers Acot(RealNumbers x) => BigNumMath.PI / new RealNumbers("2") - Atan(x);
}