using BigNum;

namespace Expression;

internal static class ReduceTrigonometry
{
    /// <summary>
    /// Reducir la expresion Seno
    /// </summary>
    /// <param name="exp">Expresion para reducir</param>
    /// <returns>Expresion reducida</returns>
    internal static ExpressionType ReduceSin(UnaryExpression exp) => ReduceSinCos(exp, true);

    /// <summary>
    /// Reducir la expresion Coseno
    /// </summary>
    /// <param name="exp">Expresion para reducir</param>
    /// <returns>Expresion reducida</returns>
    internal static ExpressionType ReduceCos(UnaryExpression exp) => ReduceSinCos(exp, false);

    /// <summary>
    /// Reducir la expresion Seno o Coseno
    /// </summary>
    /// <param name="exp">Expresion para reducir</param>
    /// <param name="sin">Seno</param>
    /// <returns>Expresion reducida</returns>
    private static ExpressionType ReduceSinCos(UnaryExpression exp, bool sin)
    {
        NumberExpression? number = exp.Value as NumberExpression;

        if (number is not null)
        {
            if (number.Value == RealNumbers.Real0)
                return sin ? new NumberExpression(RealNumbers.Real0) : new NumberExpression(RealNumbers.Real1);
        }

        if (exp.Value is ConstantPI)
            return sin ? new NumberExpression(RealNumbers.Real0) : new NumberExpression(RealNumbers.RealN1);

        Multiply? multiply = exp.Value as Multiply;

        ExpressionType? index = null;
        ExpressionType? reduce = null;
        if (multiply is not null)
        {
            if (multiply.Left.Equals(new ConstantPI())) index = multiply.Right;
            if (multiply.Right.Equals(new ConstantPI())) index = multiply.Left;

            number = index as NumberExpression;
            if (number is not null) reduce = Determinate(number.Value, sin);

            BinaryExpression? binary = index as BinaryExpression;
            if (binary is not null)
            {
                number = Aux.Numbers(binary);
                if (number is not null) reduce = Determinate(number.Value, sin);
            }
        }

        return reduce is null ? exp : reduce;
    }

    /// <summary>
    /// Dado el coeficiente de pi determinar si se puede reducir
    /// </summary>
    /// <param name="number">Coeficiente de pi</param>
    /// <param name="sin">Seno</param>
    /// <returns>Expresion reducida(si es null no se puede reducir)</returns>
    private static ExpressionType? Determinate(RealNumbers number, bool sin)
    {
        if (BigNumMath.IsInteger(number))
        {
            if (sin) return new NumberExpression(RealNumbers.Real0);

            return BigNumMath.RealToInteger(number) % new IntegerNumbers("2") == RealNumbers.Real0
                ? new NumberExpression(RealNumbers.Real1)
                : new NumberExpression(RealNumbers.RealN1);
        }

        RealNumbers integer = number >= RealNumbers.Real0
            ? number - new RealNumbers("0.5")
            : number + new RealNumbers("0.5");

        if (BigNumMath.IsInteger(integer))
        {
            ExpressionType? result = Determinate(integer, !sin);

            if (result is null) return null;
            return number >= RealNumbers.Real0 ? result : new NumberExpression(RealNumbers.RealN1) * result;
        }

        return null;
    }
}