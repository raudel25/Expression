using BigNum.Operations;

namespace BigNum;

public class BigNumMath
{
    public BigNumMath(int precision, int indBase10)
    {
        if (precision < 0 || indBase10 < 0 || indBase10 > 9) throw new Exception("Invalid Precision");
        Base10 = (long)Math.Pow(10, indBase10);
        Precision = precision;
        IndBase10 = indBase10;
    }

    public long Base10 { get; }
    public int IndBase10 { get; }
    public int Precision { get; }

    public RealNumbers Real1 => new("1", Precision, Base10, IndBase10);

    public RealNumbers RealN1 => new("1", Precision, Base10, IndBase10, false);

    public RealNumbers Real0 => new("0", Precision, Base10, IndBase10);

    /// <summary>
    ///     Aproximacion del numero E
    /// </summary>
    public RealNumbers E => Constants.ConstantE(Real0, Real1);

    /// <summary>
    ///     Aproximacion del numero PI
    /// </summary>
    public RealNumbers PI => Constants.ConstantPI(Precision, Base10, IndBase10);

    public RealNumbers Real(string s, bool positive = true)
    {
        return new(s, Precision, Base10, IndBase10, positive);
    }

    public IntegerNumbers Int(string s, bool positive = true)
    {
        return new(s, Base10, IndBase10, positive);
    }


    /// <summary>
    ///     Sumar dos numeros reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Sum(RealNumbers x, RealNumbers y)
    {
        return SumOperations.Sum(x, y);
    }

    /// <summary>
    ///     Sumar dos fracciones reales
    /// </summary>
    /// <param name="x">Fraccion real</param>
    /// <param name="y">Fraccion real</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Sum(Fraction x, Fraction y)
    {
        return new(x.Numerator * y.Denominator + y.Numerator * x.Denominator, x.Denominator * y.Denominator);
    }

    /// <summary>
    ///     Multiplicar dos numeros reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Product(RealNumbers x, RealNumbers y)
    {
        return ProductOperations.Product(x, y);
    }

    /// <summary>
    ///     Multiplicar dos fracciones reales
    /// </summary>
    /// <param name="x">Fraccion real</param>
    /// <param name="y">Fraccion real</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Product(Fraction x, Fraction y)
    {
        return new(x.Numerator * y.Numerator, x.Denominator * y.Denominator);
    }

    /// <summary>
    ///     Dividir dos numeros reales
    /// </summary>
    /// <param name="x">Dividendo</param>
    /// <param name="y">Divisor</param>
    /// <param name="integer">Si es un numero entero</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Division(RealNumbers x, RealNumbers y, bool integer = false)
    {
        return DivisionOperations.Division(x, y, integer);
    }

    /// <summary>
    ///     Dividir dos fracciones reales
    /// </summary>
    /// <param name="x">Dividendo</param>
    /// <param name="y">Divisor</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Division(Fraction x, Fraction y)
    {
        return x * new Fraction(y.Denominator, y.Numerator);
    }

    /// <summary>
    ///     Resto de la division entre dos enteros
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Modulo</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Rest(IntegerNumbers x, IntegerNumbers y)
    {
        return x - RealToInteger(DivisionOperations.Division(x, y, true) * y);
    }

    /// <summary>
    ///     Potencia entre un numero real y un numero entero
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="y">Exponente</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Pow(RealNumbers x, int y)
    {
        return PowOperations.Pow(x, y);
    }

    /// <summary>
    ///     Potencia entre 2 numeros reales
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="y">Exponente</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Pow(RealNumbers x, RealNumbers y)
    {
        return PowOperations.Pow(x, y);
    }

    /// <summary>
    ///     Potencia entre 2 numeros enteros
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="y">Exponente</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Pow(IntegerNumbers x, int y)
    {
        return RealToInteger(PowOperations.Pow(x, y));
    }

    /// <summary>
    ///     Raiz n-esima
    /// </summary>
    /// <param name="x">Radicando</param>
    /// <param name="y">Indice del radical</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Sqrt(RealNumbers x, int y)
    {
        return SqrtOperations.Sqrt(x, y);
    }

    /// <summary>
    ///     Potencia entre una fraccion real y un numero entero
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="pow">Exponente</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Pow(Fraction x, int pow)
    {
        return PowOperations.Pow(x, pow);
    }

    /// <summary>
    ///     Maximo entre dos numero reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Max(RealNumbers x, RealNumbers y)
    {
        return x.CompareTo(y) == 1 ? x : y;
    }

    /// <summary>
    ///     Maximo entre dos numero enteros
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Max(IntegerNumbers x, IntegerNumbers y)
    {
        return RealToInteger(Max((RealNumbers)x, y));
    }

    /// <summary>
    ///     Minimo entre dos numero reales
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="y">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Min(RealNumbers x, RealNumbers y)
    {
        return x.CompareTo(y) == -1 ? x : y;
    }

    /// <summary>
    ///     Minimo entre dos numero enteros
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Min(IntegerNumbers x, IntegerNumbers y)
    {
        return RealToInteger(Min((RealNumbers)x, y));
    }

    /// <summary>
    ///     Determinar el modulo de un numero real
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Abs(RealNumbers x)
    {
        return x.Abs;
    }

    /// <summary>
    ///     Determinar el opuesto de un numero real
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Opposite(RealNumbers x)
    {
        return new(x.NumberValue, x.Precision, x.Base10, x.IndBase10, !x.Positive());
    }

    /// <summary>
    ///     Determinar el opuesto de una fraccion real
    /// </summary>
    /// <param name="x">Fraccion real</param>
    /// <returns>Resultado fraccion real</returns>
    public static Fraction Opposite(Fraction x)
    {
        return new(Opposite(x.Numerator), x.Denominator);
    }

    /// <summary>
    ///     Convertir de double(C#) a numero real
    /// </summary>
    /// <param name="n">Numero double(C#)</param>
    /// <returns>Resultado real</returns>
    public RealNumbers ConvertToRealNumbers(double n)
    {
        return new(Math.Abs(n) + "", Precision, Base10, IndBase10, n >= 0);
    }

    /// <summary>
    ///     Convertir de real a entero
    /// </summary>
    /// <param name="n">Numero real</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers RealToInteger(RealNumbers n)
    {
        return new(n.ToString().Split('.')[0], n.Base10, n.IndBase10, n.Positive());
    }

    /// <summary>
    ///     Convertir de entero a real
    /// </summary>
    /// <param name="n">Numero real</param>
    /// <returns>Resultado entero</returns>
    // public static RealNumbers IntegerToReal(RealNumbers n) => new RealNumbers($"{n}.0", n.Positive());
    public static bool IsInteger(RealNumbers n)
    {
        return n.ToString().Split('.').Length == 1;
    }

    /// <summary>
    ///     Maximo comun divisor entre 2 numeros
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Mcd(IntegerNumbers x, IntegerNumbers y)
    {
        return IntegerOperations.Mcd(x, y);
    }

    /// <summary>
    ///     Factorial de un numero
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Factorial(IntegerNumbers x)
    {
        return IntegerOperations.Factorial(x);
    }

    /// <summary>
    ///     Combinaciones sin repecticion
    /// </summary>
    /// <param name="x">Numero entero</param>
    /// <param name="y">Numero entero</param>
    /// <returns>Resultado entero</returns>
    public static IntegerNumbers Combinations(IntegerNumbers x, IntegerNumbers y)
    {
        return IntegerOperations.Combinations(x, y);
    }

    /// <summary>
    ///     Logaritmo en base E
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Ln(RealNumbers x)
    {
        return LogarithmOperations.Ln(x);
    }

    /// <summary>
    ///     Logartimo entre 2 numeros reales
    /// </summary>
    /// <param name="x">Base</param>
    /// <param name="y">Potencia</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Log(RealNumbers x, RealNumbers y)
    {
        return LogarithmOperations.Log(x, y);
    }

    /// <summary>
    ///     Seno de un numero real
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Sin(RealNumbers x)
    {
        return TrigonometryOperations.SinCos(x, true);
    }

    /// <summary>
    ///     Coseno de un numero real
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <returns>Resultado real</returns>
    public static RealNumbers Cos(RealNumbers x)
    {
        return TrigonometryOperations.SinCos(x, false);
    }

    public static RealNumbers Asin(RealNumbers x)
    {
        return TrigonometryOperations.Asin(x);
    }

    public static RealNumbers Acos(RealNumbers x)
    {
        return Constants.ConstantPI(x.Precision, x.Base10, x.IndBase10) /
            new RealNumbers("2", x.Precision, x.Base10, x.IndBase10) - Asin(x);
    }

    public static RealNumbers Atan(RealNumbers x)
    {
        return TrigonometryOperations.Atan(x);
    }

    public static RealNumbers Acot(RealNumbers x)
    {
        return Constants.ConstantPI(x.Precision, x.Base10, x.IndBase10) /
            new RealNumbers("2", x.Precision, x.Base10, x.IndBase10) - Atan(x);
    }
}