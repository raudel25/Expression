using BigNum;

namespace Expression;

public static class ReduceExpression
{
    /// <summary>
    /// Reducir una expresion
    /// </summary>
    /// <param name="exp">Expresion para reducir</param>
    /// <returns>Expresion reducida</returns>
    public static ExpressionType Reduce(ExpressionType exp)
    {
        BinaryExpression? binary = exp as BinaryExpression;

        if (binary is null) return exp;

        ExpressionType? number = Numbers(binary);
        if (number is not null) return number;

        if (binary is Sum) return ReduceSumSubtraction.ReduceSum(binary);

        if (binary is Subtraction) return ReduceSumSubtraction.ReduceSubtraction(binary);

        if (binary is Multiply) return ReduceMultiplyDivision.ReduceMultiply(binary);

        if (binary is Division) return ReduceMultiplyDivision.ReduceDivision(binary);

        if (binary is Pow)
        {
            ExpressionType? aux = ReducePowSimple(binary);
            return aux is null ? exp : aux;
        }

        if (binary is Log)
        {
            if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1)))
                return new NumberExpression(RealNumbers.Real0);
        }

        return exp;
    }

    internal static ExpressionType? Numbers(BinaryExpression binary)
    {
        if (binary.Left is NumberExpression && binary.Right is NumberExpression)
            return new NumberExpression(binary.Evaluate(RealNumbers.Real0));

        return null;
    }

    internal static ExpressionType? ReducePowSimple(BinaryExpression binary)
    {
        if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0)))
            return new NumberExpression(RealNumbers.Real0);
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real0)))
            return new NumberExpression(RealNumbers.Real1);
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1)))
            return binary.Left;

        return null;
    }
}