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
        UnaryExpression? unary = exp as UnaryExpression;

        if (unary is Sin) return ReduceTrigonometry.ReduceSin(unary);
        
        if (unary is Cos) return ReduceTrigonometry.ReduceCos(unary);
        
        BinaryExpression? binary = exp as BinaryExpression;

        if (binary is null) return exp;

        ExpressionType? number = Aux.Numbers(binary);
        if (number is not null) return number;

        if (binary is Sum) return ReduceSumSubtraction.ReduceSum(binary);

        if (binary is Subtraction) return ReduceSumSubtraction.ReduceSubtraction(binary);

        if (binary is Multiply) return ReduceMultiplyDivision.ReduceMultiply(binary);

        if (binary is Division) return ReduceMultiplyDivision.ReduceDivision(binary);

        if (binary is Pow) return ReducePowLogarithm.ReducePow(binary);

        if (binary is Log) return ReducePowLogarithm.ReduceLogarithm(binary);

        return exp;
    }
}