namespace Expression.Reduce;

public static class ReduceExpression<T>
{
    /// <summary>
    ///     Reducir una expresion
    /// </summary>
    /// <param name="exp">Expresion para reducir</param>
    /// <returns>Expresion reducida</returns>
    public static ExpressionType<T> Reduce(ExpressionType<T> exp)
    {
        var unary = exp as UnaryExpression<T>;

        if (unary is Sin<T>) return ReduceTrigonometry<T>.ReduceSin(unary);

        if (unary is Cos<T>) return ReduceTrigonometry<T>.ReduceCos(unary);

        var binary = exp as BinaryExpression<T>;

        if (binary is null) return exp;

        ExpressionType<T>? number = Aux<T>.Numbers(binary);
        if (number is not null) return number;

        if (binary is Sum<T>) return ReduceSumSubtraction<T>.ReduceSum(binary);

        if (binary is Subtraction<T>) return ReduceSumSubtraction<T>.ReduceSubtraction(binary);

        if (binary is Multiply<T>) return ReduceMultiplyDivision<T>.ReduceMultiply(binary);

        if (binary is Division<T>) return ReduceMultiplyDivision<T>.ReduceDivision(binary);

        if (binary is Pow<T>) return ReducePowLogarithm<T>.ReducePow(binary);

        if (binary is Log<T>) return ReducePowLogarithm<T>.ReduceLogarithm(binary);

        return exp;
    }
}