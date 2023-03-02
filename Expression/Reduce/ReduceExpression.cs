using Expression.Expressions;

namespace Expression.Reduce;

internal static class ReduceExpression<T>
{
    /// <summary>
    ///     Reducir una expresion
    /// </summary>
    /// <param name="exp">Expresion para reducir</param>
    /// <returns>Expresion reducida</returns>
    internal static Function<T> Reduce(Function<T> exp)
    {
        var unary = exp as UnaryExpression<T>;

        Function<T>? number;

        if (unary is not null)
        {
            number = Aux<T>.Numbers(unary);
            if (number is not null) return number;
        }

        switch (unary)
        {
            case Sin<T>:
                return ReduceTrigonometry<T>.ReduceSin(unary);
            case Cos<T>:
                return ReduceTrigonometry<T>.ReduceCos(unary);
        }

        if (exp is not BinaryExpression<T> binary) return exp;

        number = Aux<T>.Numbers(binary);
        if (number is not null) return number;

        return binary switch
        {
            Sum<T> => ReduceSumSubtraction<T>.ReduceSum(binary),
            Subtraction<T> => ReduceSumSubtraction<T>.ReduceSubtraction(binary),
            Multiply<T> => ReduceMultiplyDivision<T>.ReduceMultiply(binary),
            Division<T> => ReduceMultiplyDivision<T>.ReduceDivision(binary),
            Pow<T> => ReducePowLogarithm<T>.ReducePow(binary),
            Log<T> => ReducePowLogarithm<T>.ReduceLogarithm(binary),
            Sqrt<T> => ReducePowLogarithm<T>.ReduceSqrt(binary),
            _ => exp
        };
    }
}