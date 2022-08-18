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

        if (binary is Pow) return ReducePow(binary);

        if (binary is Log)
        {
            if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1)))
                return new NumberExpression(RealNumbers.Real0);
        }

        return exp;
    }

    /// <summary>
    /// Determinar si la expresion es completamente numerica
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    internal static ExpressionType? Numbers(BinaryExpression binary)
    {
        if (binary.Left is NumberExpression && binary.Right is NumberExpression)
            return new NumberExpression(binary.Evaluate(RealNumbers.Real0));

        return null;
    }

    /// <summary>
    /// Reducir una potencia
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    internal static ExpressionType ReducePow(BinaryExpression binary)
    {
        ExpressionType? aux = ReducePowSimple(binary);
        if (aux is not null) return aux;

        Pow? exp = binary.Left as Pow;
        if (exp is not null) return ReducePow(Pow.DeterminatePow(exp.Left, exp.Right * binary.Right));

        return binary;
    }

    /// <summary>
    /// Determinar si una potencia se puede reducir dadas caracteristicas simples
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
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