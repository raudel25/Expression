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

        Pow? pow = binary.Left as Pow;
        if (pow is not null) return ReducePow(Pow.DeterminatePow(pow.Left, pow.Right * binary.Right));

        Log? log = binary.Right as Log;
        if (log is not null)
        {
            if (log.Left.Equals(binary.Left)) return log.Right;
        }

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

    /// <summary>
    /// Reducir un logaritmo
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    internal static ExpressionType ReduceLogarithm(BinaryExpression binary)
    {
        ExpressionType? aux = ReduceLogarithmSimple(binary);
        if (aux is not null) return aux;

        Pow? pow = binary.Right as Pow;
        if (pow is not null)
        {
            if (pow.Left.Equals(binary.Left)) return pow.Right;
        }

        return binary;
    }

    /// <summary>
    /// Determinar si un logaritmo
    /// se puede reducir dadas caracteristicas simples
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    private static ExpressionType? ReduceLogarithmSimple(BinaryExpression binary)
    {
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1)))
            return new NumberExpression(RealNumbers.Real0);

        if (binary.Right.Equals(binary.Left)) return new NumberExpression(RealNumbers.Real1);

        return null;
    }
}