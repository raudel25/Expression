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
        ExpressionType? aux = ReducePowPossible(binary);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    /// Determina si es posible reducir una potencia
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null no es posible)</returns>
    private static ExpressionType? ReducePowPossible(BinaryExpression binary)
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

        Multiply? mult = binary.Left as Multiply;
        if (mult is not null)
        {
            if (binary.Right is NumberExpression && (mult.Left is NumberExpression || mult.Right is NumberExpression))
                return ReduceMultiplyDivision.ReduceMultiply(Pow.DeterminatePow(mult.Left, binary.Right) *
                                                             Pow.DeterminatePow(mult.Right, binary.Right));
        }

        Division? div = binary.Left as Division;
        if (div is not null)
        {
            if (binary.Right is NumberExpression && (div.Left is NumberExpression || div.Right is NumberExpression))
                return ReduceMultiplyDivision.ReduceDivision(Pow.DeterminatePow(div.Left, binary.Right) /
                                                             Pow.DeterminatePow(div.Right, binary.Right));
        }

        mult = binary.Right as Multiply;
        if (mult is not null)
        {
            aux = ReducePowPossible(Pow.DeterminatePow(binary.Left, mult.Right));
            if (aux is not null) return ReducePow(Pow.DeterminatePow(aux,mult.Left));
            
            aux = ReducePowPossible(Pow.DeterminatePow(binary.Left, mult.Left));
            if (aux is not null) return ReducePow(Pow.DeterminatePow(aux,mult.Right));
        }
        
        div = binary.Right as Division;
        if (div is not null)
        {
            aux = ReducePowPossible(Pow.DeterminatePow(binary.Left, div.Left));
            if (aux is not null)
                return ReducePow(Pow.DeterminatePow(aux, new NumberExpression(RealNumbers.Real1) / div.Right));
        }

        return null;
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