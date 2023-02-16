namespace Expression.Reduce;

internal static class ReduceMultiplyDivision<T>
{
    /// <summary>
    ///     Reducir una multiplicacion
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    internal static ExpressionType<T> ReduceMultiply(BinaryExpression<T> binary)
    {
        var aux = ReduceMultiplySimple(binary);
        if (aux is not null) return aux;

        aux = ReduceMultiply(binary.Left, binary.Right, binary.Arithmetic.Real1, binary.Arithmetic.Real1);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    ///     Determinar si una multiplicacion se puede reducir dadas caracteristicas simples
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    internal static ExpressionType<T>? ReduceMultiplySimple(BinaryExpression<T> binary)
    {
        if (binary.Left.Equals(new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic)) ||
            binary.Right.Equals(new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic)))
            return new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic);
        if (binary.Left.Equals(new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic)))
            return binary.Right;
        if (binary.Right.Equals(new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic)))
            return binary.Left;

        return null;
    }

    /// <summary>
    ///     Comprobar si es posible reducir nuevamente la multiplicacion
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Indice de la expresion izquierda</param>
    /// <param name="indRight">Indice de la expresion derecha</param>
    /// <returns>Expresion resultante</returns>
    private static ExpressionType<T> ReduceMultiplyCheck(ExpressionType<T> left, ExpressionType<T> right, T indLeft,
        T indRight)
    {
        var aux = ReduceMultiply(left, right, indLeft, indRight);
        if (aux is not null) return aux;

        BinaryExpression<T> binary = Pow<T>.DeterminatePow(left, new NumberExpression<T>(indLeft, left.Arithmetic)) *
                                     Pow<T>.DeterminatePow(right, new NumberExpression<T>(indRight, left.Arithmetic));

        aux = ReduceMultiplySimple(binary);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    ///     Determinar si es posible reducir una multiplicacion dadas su expresion izquierda y derecha
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Indice de la expresion izquierda</param>
    /// <param name="indRight">Indice de la expresion derecha</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    private static ExpressionType<T>? ReduceMultiply(ExpressionType<T> left, ExpressionType<T> right, T indLeft,
        T indRight)
    {
        var auxExp = PowInd(left);
        (left, indLeft) = (auxExp.Item1, left.Arithmetic.Multiply(auxExp.Item2, indLeft));
        auxExp = PowInd(right);
        (right, indRight) = (auxExp.Item1, left.Arithmetic.Multiply(auxExp.Item2, indRight));

        var (leftBinary, rightBinary) =
            (left as BinaryExpression<T>, right as BinaryExpression<T>);

        ExpressionType<T>? aux;

        if (leftBinary is Multiply<T>)
        {
            aux = ReduceMultiply(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(leftBinary.Right, aux, indLeft, left.Arithmetic.Real1);
            aux = ReduceMultiply(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(leftBinary.Left, aux, indLeft, left.Arithmetic.Real1);
        }

        if (rightBinary is Multiply<T>)
        {
            aux = ReduceMultiply(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(aux, rightBinary.Right, left.Arithmetic.Real1, indRight);
            aux = ReduceMultiply(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(aux, rightBinary.Left, left.Arithmetic.Real1, indRight);
        }

        if (leftBinary is Division<T>)
        {
            aux = ReduceMultiply(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, leftBinary.Right, left.Arithmetic.Real1, indLeft);
            aux = ReduceDivision(right, leftBinary.Right, indRight, indLeft);
            if (aux is not null) return ReduceMultiplyCheck(leftBinary.Left, aux, indLeft, left.Arithmetic.Real1);
        }

        if (rightBinary is Division<T>)
        {
            aux = ReduceMultiply(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, rightBinary.Right, left.Arithmetic.Real1, indRight);
            aux = ReduceDivision(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(aux, rightBinary.Left, left.Arithmetic.Real1, indRight);
        }

        if (left.Equals(right))
        {
            BinaryExpression<T> pow = Pow<T>.DeterminatePow(left,
                new NumberExpression<T>(left.Arithmetic.Sum(indLeft, indRight), left.Arithmetic));
            aux = ReducePowLogarithm<T>.ReducePowSimple(pow);
            if (aux is not null) return aux;

            return pow;
        }

        if (left is NumberExpression<T> && right is NumberExpression<T>)
            return new NumberExpression<T>(left.Arithmetic.Multiply(
                left.Arithmetic.Pow(left.Evaluate(new List<(char, T)>()), indLeft),
                left.Arithmetic.Pow(right.Evaluate(new List<(char, T)>()), indRight)), left.Arithmetic);

        return null;
    }

    /// <summary>
    ///     Reducir una division
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    internal static ExpressionType<T> ReduceDivision(BinaryExpression<T> binary)
    {
        var aux = ReduceDivisionSimple(binary);
        if (aux is not null) return aux;

        aux = ReduceDivision(binary.Left, binary.Right, binary.Arithmetic.Real1, binary.Arithmetic.Real1);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    ///     Determinar si una division se puede reducir dadas caracteristicas simples
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    private static ExpressionType<T>? ReduceDivisionSimple(BinaryExpression<T> binary)

    {
        if (binary.Left.Equals(new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic)))
            return new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic);
        if (binary.Right.Equals(new NumberExpression<T>(binary.Arithmetic.Real1, binary.Arithmetic)))
            return binary.Left;

        return null;
    }

    /// <summary>
    ///     Comprobar si es posible reducir nuevamente la division
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Indice de la expresion izquierda</param>
    /// <param name="indRight">Indice de la expresion derecha</param>
    /// <returns>Expresion resultante</returns>
    private static ExpressionType<T> ReduceDivisionCheck(ExpressionType<T> left, ExpressionType<T> right, T indLeft,
        T indRight)
    {
        var aux = ReduceDivision(left, right, indLeft, indRight);
        if (aux is not null) return aux;

        BinaryExpression<T> binary = Pow<T>.DeterminatePow(left, new NumberExpression<T>(indLeft, left.Arithmetic)) /
                                     Pow<T>.DeterminatePow(right, new NumberExpression<T>(indRight, left.Arithmetic));

        aux = ReduceDivisionSimple(binary);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    ///     Determinar si es posible reducir una division dadas su expresion izquierda y derecha
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Indice de la expresion izquierda</param>
    /// <param name="indRight">Indice de la expresion derecha</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    private static ExpressionType<T>? ReduceDivision(ExpressionType<T> left, ExpressionType<T> right, T indLeft,
        T indRight)
    {
        var auxExp = PowInd(left);
        (left, indLeft) = (auxExp.Item1, left.Arithmetic.Multiply(auxExp.Item2, indLeft));
        auxExp = PowInd(right);
        (right, indRight) = (auxExp.Item1, left.Arithmetic.Multiply(auxExp.Item2, indRight));

        var (leftBinary, rightBinary) =
            (left as BinaryExpression<T>, right as BinaryExpression<T>);

        ExpressionType<T>? aux;

        if (leftBinary is Multiply<T>)
        {
            aux = ReduceDivision(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(leftBinary.Right, aux, indLeft, left.Arithmetic.Real1);
            aux = ReduceDivision(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(leftBinary.Left, aux, indLeft, left.Arithmetic.Real1);
        }

        if (rightBinary is Multiply<T>)
        {
            aux = ReduceDivision(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, rightBinary.Right, left.Arithmetic.Real1, indRight);
            aux = ReduceDivision(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, rightBinary.Left, left.Arithmetic.Real1, indRight);
        }

        if (leftBinary is Division<T>)
        {
            aux = ReduceMultiply(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(leftBinary.Left, aux, indLeft, left.Arithmetic.Real1);
            aux = ReduceDivision(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, leftBinary.Right, left.Arithmetic.Real1, indLeft);
        }

        if (rightBinary is Division<T>)
        {
            aux = ReduceMultiply(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, rightBinary.Left, left.Arithmetic.Real1, indRight);
            aux = ReduceDivision(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(aux, rightBinary.Right, left.Arithmetic.Real1, indRight);
        }

        if (left.Equals(right))
        {
            BinaryExpression<T> pow = Pow<T>.DeterminatePow(left,
                new NumberExpression<T>(left.Arithmetic.Subtraction(indLeft, indRight), left.Arithmetic));
            aux = ReducePowLogarithm<T>.ReducePowSimple(pow);
            if (aux is not null) return aux;

            return pow;
        }

        if (left is NumberExpression<T> && right is NumberExpression<T>)
            return new NumberExpression<T>(left.Arithmetic.Division(
                left.Arithmetic.Pow(left.Evaluate(new List<(char, T)>()), indLeft),
                left.Arithmetic.Pow(right.Evaluate(new List<(char, T)>()), indRight)), left.Arithmetic);

        return null;
    }

    private static (ExpressionType<T>, T) PowInd(ExpressionType<T> exp)
    {
        var binary = exp as Pow<T>;
        if (binary is null) return (exp, exp.Arithmetic.Real1);

        if (binary.Right is NumberExpression<T> expression) return (binary.Left, expression.Value);

        return (binary, exp.Arithmetic.Real1);
    }
}