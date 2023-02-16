namespace BigNum;

internal static class TrigonometryOperations
{
    private static readonly int _precisionSinCos = 40;

    private static readonly int _precisionAtan = 1000;

    private static readonly int _precisionAsin = 200;

    /// <summary>
    ///     Calcular seno o coseno
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="sin">Calculo del seno</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers SinCos(RealNumbers x, bool sin)
    {
        Condition filter1 = sin ? a => (a & 1) == 0 : a => (a & 1) != 0;
        Condition filter2 = sin ? a => a % 4 == 1 : a => a % 4 == 0;

        var result = x.Real0;
        var pow = x.Real1;
        var fact = x.Real1;
        var index = x.Real0;

        //Serie de taylor sen x cos x
        //https://es.wikipedia.org/wiki/Serie_de_Taylor
        for (var i = 0; i < _precisionSinCos; i++)
        {
            if (i != 0) fact *= index;
            if (i != 0) pow *= x;
            index++;
            if (filter1(i)) continue;
            result = filter2(i) ? result + pow / fact : result - pow / fact;
        }

        return result;
    }

    internal static RealNumbers Atan(RealNumbers x)
    {
        //arctan(x)+arctan(1/x)=pi/2
        //arctan(1/x)=arccot(x)
        if (x.Abs > x.Real1) return BigNumMath.Acot(x.Real1 / x);

        var pow = x;
        var index = x.Real1;
        var arctan = x.Real0;
        var xx = BigNumMath.Pow(x, new IntegerNumbers("2", x.Base10, x.IndBase10));

        for (var i = 1; i < 2 * _precisionAtan; i += 2)
        {
            arctan = i % 4 == 1 ? arctan + pow / index : arctan - pow / index;
            pow *= xx;
            index += new RealNumbers("2.0", x.Precision, x.Base10, x.IndBase10);
        }

        return arctan;
    }

    internal static RealNumbers Asin(RealNumbers x)
    {
        if (x.Abs > x.Real1) throw new Exception("Operacion Invalida (arcsin recive valores entre 1 y -1)");

        var index = x.Real1;
        var even = x.Real1;
        var odd = x.Real1;
        var pow = x.Real1;
        var arcsin = x.Real0;

        for (var i = 1; i <= _precisionAsin; i++)
        {
            pow *= x;
            if ((i & 1) == 0) even *= index;

            if ((i & 1) == 1)
            {
                arcsin += odd * pow / (even * index);
                odd *= index;
            }

            index++;
        }

        return arcsin;
    }

    internal delegate bool Condition(int x);
}