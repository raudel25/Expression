namespace BigNum;

internal static class TrigonometryOperations
{
    internal delegate bool Condition(int x);

    private static int _precisionSinCos = 40;

    private static int _precisionAtan = 1000;

    private static int _precisionAsin = 200;

    /// <summary>
    /// Calcular seno o coseno
    /// </summary>
    /// <param name="x">Numero real</param>
    /// <param name="sin">Calculo del seno</param>
    /// <returns>Resultado real</returns>
    internal static RealNumbers SinCos(RealNumbers x, bool sin)
    {
        Condition filter1 = sin ? (a) => (a & 1) == 0 : (a) => (a & 1) != 0;
        Condition filter2 = sin ? (a) => a % 4 == 1 : (a) => a % 4 == 0;

        RealNumbers result = x.Real0;
        RealNumbers pow = x.Real1;
        RealNumbers fact = x.Real1;
        RealNumbers index = x.Real0;

        //Serie de taylor sen x cos x
        //https://es.wikipedia.org/wiki/Serie_de_Taylor
        for (int i = 0; i < _precisionSinCos; i++)
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

        RealNumbers pow = x;
        RealNumbers index = x.Real1;
        RealNumbers arctan = x.Real0;
        RealNumbers xx = BigNumMath.Pow(x, new IntegerNumbers("2", x.Base10, x.IndBase10));

        for (int i = 1; i < 2 * _precisionAtan; i += 2)
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

        RealNumbers index = x.Real1;
        RealNumbers even = x.Real1;
        RealNumbers odd = x.Real1;
        RealNumbers pow = x.Real1;
        RealNumbers arcsin = x.Real0;

        for (int i = 1; i <= _precisionAsin; i++)
        {
            pow *= x;
            if ((i & 1) == 0) even *= index;

            if ((i & 1) == 1)
            {
                arcsin += (odd * pow) / (even * index);
                odd *= index;
            }

            index++;
        }

        return arcsin;
    }
}