namespace Expression;

internal static class ReducePowLogarithm<T>
{
    /// <summary>
    /// Reducir una potencia
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    internal static ExpressionType<T> ReducePow(BinaryExpression<T> binary)
    {
        ExpressionType<T>? aux = ReducePowPossible(binary);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    /// Determina si es posible reducir una potencia
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null no es posible)</returns>
    private static ExpressionType<T>? ReducePowPossible(BinaryExpression<T> binary)
    {
        ExpressionType<T>? aux = ReducePowSimple(binary);
        if (aux is not null) return aux;

        Pow<T>? pow = binary.Left as Pow<T>;
        if (pow is not null) return ReducePow(Pow<T>.DeterminatePow(pow.Left, pow.Right * binary.Right));

        Log<T>? log = binary.Right as Log<T>;
        if (log is not null)
        {
            if (log.Left.Equals(binary.Left)) return log.Right;
        }

        Multiply<T>? mult = binary.Left as Multiply<T>;
        if (mult is not null)
        {
            if (binary.Right is NumberExpression<T> &&
                (mult.Left is NumberExpression<T> || mult.Right is NumberExpression<T>))
                return ReduceMultiplyDivision<T>.ReduceMultiply(Pow<T>.DeterminatePow(mult.Left, binary.Right) *
                                                                Pow<T>.DeterminatePow(mult.Right, binary.Right));
        }

        Division<T>? div = binary.Left as Division<T>;
        if (div is not null)
        {
            if (binary.Right is NumberExpression<T> &&
                (div.Left is NumberExpression<T> || div.Right is NumberExpression<T>))
                return ReduceMultiplyDivision<T>.ReduceDivision(Pow<T>.DeterminatePow(div.Left, binary.Right) /
                                                                Pow<T>.DeterminatePow(div.Right, binary.Right));
        }

        mult = binary.Right as Multiply<T>;
        if (mult is not null)
        {
            aux = ReducePowPossible(Pow<T>.DeterminatePow(binary.Left, mult.Right));
            if (aux is not null) return ReducePow(Pow<T>.DeterminatePow(aux, mult.Left));

            aux = ReducePowPossible(Pow<T>.DeterminatePow(binary.Left, mult.Left));
            if (aux is not null) return ReducePow(Pow<T>.DeterminatePow(aux, mult.Right));
        }

        div = binary.Right as Division<T>;
        if (div is not null)
        {
            aux = ReducePowPossible(Pow<T>.DeterminatePow(binary.Left, div.Left));
            if (aux is not null)
                return ReducePow(Pow<T>.DeterminatePow(aux,
                    new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic) / div.Right));
        }

        return null;
    }

    /// <summary>
    /// Determinar si una potencia se puede reducir dadas caracteristicas simples
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    internal static ExpressionType<T>? ReducePowSimple(BinaryExpression<T> binary)
    {
        if (binary.Left.Equals(new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic)))
            return new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic);
        if (binary.Right.Equals(new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic)))
            return new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic);
        if (binary.Right.Equals(new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic)))
            return binary.Left;

        return null;
    }

    /// <summary>
    /// Reducir un logaritmo
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    internal static ExpressionType<T> ReduceLogarithm(BinaryExpression<T> binary)
    {
        ExpressionType<T>? aux = ReduceLogarithmSimple(binary);
        if (aux is not null) return aux;

        Pow<T>? pow = binary.Right as Pow<T>;
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
    private static ExpressionType<T>? ReduceLogarithmSimple(BinaryExpression<T> binary)
    {
        if (binary.Right.Equals(new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic)))
            return new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic);

        if (binary.Right.Equals(binary.Left))
            return new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic);

        return null;
    }
}