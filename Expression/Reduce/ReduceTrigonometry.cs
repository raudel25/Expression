using Expression.Expressions;

namespace Expression.Reduce;

internal static class ReduceTrigonometry<T>
{
    /// <summary>
    ///     Reducir la expresion Seno
    /// </summary>
    /// <param name="exp">Expresion para reducir</param>
    /// <returns>Expresion reducida</returns>
    internal static ExpressionType<T> ReduceSin(UnaryExpression<T> exp)
    {
        return ReduceSinCos(exp, true);
    }

    /// <summary>
    ///     Reducir la expresion Coseno
    /// </summary>
    /// <param name="exp">Expresion para reducir</param>
    /// <returns>Expresion reducida</returns>
    internal static ExpressionType<T> ReduceCos(UnaryExpression<T> exp)
    {
        return ReduceSinCos(exp, false);
    }

    /// <summary>
    ///     Reducir la expresion Seno o Coseno
    /// </summary>
    /// <param name="exp">Expresion para reducir</param>
    /// <param name="sin">Seno</param>
    /// <returns>Expresion reducida</returns>
    private static ExpressionType<T> ReduceSinCos(UnaryExpression<T> exp, bool sin)
    {
        var number = exp.Value as NumberExpression<T>;

        if (number is not null && number.Value is not null)
            if (number.Value.Equals(exp.Arithmetic.Real0))
                return sin
                    ? new NumberExpression<T>(exp.Arithmetic.Real0, exp.Arithmetic)
                    : new NumberExpression<T>
                        (exp.Arithmetic.Real1, exp.Arithmetic);

        if (exp.Value is ConstantPI<T>)
            return sin
                ? new NumberExpression<T>(exp.Arithmetic.Real0, exp.Arithmetic)
                : new NumberExpression<T>(exp.Arithmetic.RealN1, exp.Arithmetic);

        var multiply = exp.Value as Multiply<T>;

        ExpressionType<T>? index = null;
        ExpressionType<T>? reduce = null;
        if (multiply is null) return reduce ?? exp;
        if (multiply.Left.Equals(new ConstantPI<T>(exp.Arithmetic))) index = multiply.Right;
        if (multiply.Right.Equals(new ConstantPI<T>(exp.Arithmetic))) index = multiply.Left;

        number = index as NumberExpression<T>;
        if (number is not null) reduce = Determinate(number.Value, sin, exp.Arithmetic);

        if (index is BinaryExpression<T> binary)
        {
            number = Aux<T>.Numbers(binary);
            if (number is not null) reduce = Determinate(number.Value, sin, exp.Arithmetic);
        }

        return reduce ?? exp;
    }

    /// <summary>
    ///     Dado el coeficiente de pi determinar si se puede reducir
    /// </summary>
    /// <param name="number">Coeficiente de pi</param>
    /// <param name="sin">Seno</param>
    /// <param name="arithmetic">Aritmetica</param>
    /// <returns>Expresion reducida(si es null no se puede reducir)</returns>
    private static ExpressionType<T>? Determinate(T number, bool sin, IArithmetic<T> arithmetic)
    {
        if (arithmetic.IsInteger(number))
        {
            if (sin) return new NumberExpression<T>(arithmetic.Real0, arithmetic);

            var aux = arithmetic.Rest(number, arithmetic.StringToNumber("2"));

            return aux is not null && aux.Equals(arithmetic.Real0)
                ? new NumberExpression<T>(arithmetic.Real1, arithmetic)
                : new NumberExpression<T>(arithmetic.RealN1, arithmetic);
        }

        var integer = arithmetic.Positive(number)
            ? arithmetic.Subtraction(number, arithmetic.StringToNumber("0.5"))
            : arithmetic.Sum(number, arithmetic.StringToNumber("0.5"));

        if (arithmetic.IsInteger(integer))
        {
            var result = Determinate(integer, !sin, arithmetic);

            if (result is null) return null;
            return arithmetic.Positive(number)
                ? result
                : new NumberExpression<T>(arithmetic.RealN1, arithmetic) * result;
        }

        return null;
    }
}