namespace BigNum;

internal static class Constants
{
    private static int _precisionE = 20;

    private static int _precisionPI = 2;

    /// <summary>
    /// Aproximacion de E
    /// </summary>
    /// <returns>Numero E</returns>
    internal static RealNumbers ConstantE()
    {
        RealNumbers e = RealNumbers.Real0;
        RealNumbers fact = RealNumbers.Real1;
        RealNumbers index = RealNumbers.Real0;

        //Formula de taylor e^x
        //https://es.wikipedia.org/wiki/Serie_de_Taylor
        for (int i = 0; i < _precisionE; i++)
        {
            if (i != 0) fact *= index;
            e += RealNumbers.Real1 / fact;
            index++;
        }

        return e;
    }

    /// <summary>
    /// Aproximacion de PI
    /// </summary>
    /// <returns>Resultado Real</returns>
    internal static RealNumbers ConstantPI()
    {
        IntegerNumbers index = IntegerNumbers.Integer0;
        RealNumbers pi = RealNumbers.Real0;

        //Agoritmo de Ramanujan-Chudnovsky
        //https://en.wikipedia.org/wiki/Chudnovsky_algorithm
        for (int i = 0; i <= _precisionPI; i++)
        {
            RealNumbers numerator = BigNumMath.Factorial(new IntegerNumbers("6") * index) *
                                    (new RealNumbers("545140134", "0") * index + new RealNumbers("13591409", "0"));
            RealNumbers denominator = BigNumMath.Factorial(new IntegerNumbers("3") * index) *
                                      BigNumMath.Pow(BigNumMath.Factorial(index), new IntegerNumbers("3")) *
                                      BigNumMath.Pow(new RealNumbers("640320", "0"), new IntegerNumbers("3") * index);
            
            pi = (i & 1) == 0 ? pi + numerator / denominator : pi - numerator / denominator;
        }

        return RealNumbers.Real1 / pi * BigNumMath.Pow(new RealNumbers("640320", "0"), new RealNumbers("1", "5")) /
               new RealNumbers("12", "0");
    }
}