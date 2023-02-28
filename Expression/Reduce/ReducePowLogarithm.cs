using System.Runtime.CompilerServices;
using Expression.Expressions;

namespace Expression.Reduce;

internal static class ReducePowLogarithm<T>
{
    /// <summary>
    ///     Reducir una potencia
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    internal static Function<T> ReducePow(BinaryExpression<T> binary)
    {
        var aux = ReducePowPossible(binary);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    ///     Determina si es posible reducir una potencia
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null no es posible)</returns>
    private static Function<T>? ReducePowPossible(BinaryExpression<T> binary)
    {
        var aux = ReducePowSimple(binary);
        if (aux is not null) return aux;

        if (binary.Left is Pow<T> pow) return ReducePow(Pow<T>.DeterminatePow(pow.Left, pow.Right * binary.Right));

        if (binary.Right is Log<T> log)
            if (log.Left.Equals(binary.Left))
                return log.Right;

        if (binary.Left is Multiply<T> multL)
            if (binary.Right is NumberExpression<T> &&
                (multL.Left is NumberExpression<T> || multL.Right is NumberExpression<T>))
                return ReduceMultiplyDivision<T>.ReduceMultiply(Pow<T>.DeterminatePow(multL.Left, binary.Right) *
                                                                Pow<T>.DeterminatePow(multL.Right, binary.Right));

        if (binary.Left is Division<T> divL)
            if (binary.Right is NumberExpression<T> &&
                (divL.Left is NumberExpression<T> || divL.Right is NumberExpression<T>))
                return ReduceMultiplyDivision<T>.ReduceDivision(Pow<T>.DeterminatePow(divL.Left, binary.Right) /
                                                                Pow<T>.DeterminatePow(divL.Right, binary.Right));

        if (binary.Right is Multiply<T> multR)
        {
            aux = ReducePowPossible(Pow<T>.DeterminatePow(binary.Left, multR.Right));
            if (aux is not null) return ReducePow(Pow<T>.DeterminatePow(aux, multR.Left));

            aux = ReducePowPossible(Pow<T>.DeterminatePow(binary.Left, multR.Left));
            if (aux is not null) return ReducePow(Pow<T>.DeterminatePow(aux, multR.Right));
        }

        if (binary.Right is Division<T> divR)
        {
            aux = ReducePowPossible(Pow<T>.DeterminatePow(binary.Left, divR.Left));
            if (aux is not null)
                return ReducePow(Pow<T>.DeterminatePow(aux,
                    new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic) / divR.Right));
        }

        return null;
    }

    /// <summary>
    ///     Determinar si una potencia se puede reducir dadas caracteristicas simples
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    internal static Function<T>? ReducePowSimple(BinaryExpression<T> binary)
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
    ///     Reducir un logaritmo
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    internal static Function<T> ReduceLogarithm(BinaryExpression<T> binary)
    {
        var aux = ReduceLogarithmSimple(binary);
        if (aux is not null) return aux;

        if (binary.Right.Equals(new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic)))
            return new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic);
        if (binary.Right is not Pow<T> pow) return binary;
        
        return pow.Left.Equals(binary.Left) ? pow.Right : binary;
    }

    /// <summary>
    ///     Determinar si un logaritmo
    ///     se puede reducir dadas caracteristicas simples
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    private static Function<T>? ReduceLogarithmSimple(BinaryExpression<T> binary)
    {
        if (binary.Right.Equals(new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic)))
            return new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic);

        if (binary.Right.Equals(binary.Left))
            return new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic);

        return null;
    }
}