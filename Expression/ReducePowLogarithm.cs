using BigNum;

namespace Expression;

internal static class ReducePowLogarithm
{
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

    internal static ExpressionType ReduceLogarithm(BinaryExpression binary)
    {
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1)))
            return new NumberExpression(RealNumbers.Real0);

        return binary;
    }
}