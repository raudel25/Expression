using Expression.Expressions;

namespace Expression.Reduce;

internal static class ReduceSumSubtraction<T>
{
    /// <summary>
    ///     Reducir una suma
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    internal static Function<T> ReduceSum(BinaryExpression<T> binary)
    {
        var aux = ReduceSumSimple(binary);
        if (aux is not null) return aux;

        aux = ReduceSum(binary.Left, binary.Right, binary.Arithmetic.Real1, binary.Arithmetic.Real1);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    ///     Determinar si una suma se puede reducir dadas caracteristicas simples
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    private static Function<T>? ReduceSumSimple(BinaryExpression<T> binary)
    {
        if (binary.Left.Equals(new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic)))
            return binary.Right;
        if (binary.Right.Equals(new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic)))
            return binary.Left;

        return null;
    }

    /// <summary>
    ///     Comprobar si es posible reducir nuevamente la suma
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Coeficiente de la expresion izquierda</param>
    /// <param name="indRight">Coeficiente de la expresion derecha</param>
    /// <returns>Expresion resultante</returns>
    private static Function<T> ReduceSumCheck(Function<T> left, Function<T> right, T indLeft,
        T indRight)
    {
        var aux = ReduceSum(left, right, indLeft, indRight);
        if (aux is not null) return aux;

        BinaryExpression<T> binary = new NumberExpression<T>(indLeft, left.Arithmetic) * left +
                                     new NumberExpression<T>(indRight, left.Arithmetic) * right;

        aux = ReduceSumSimple(binary);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    ///     Determinar si es posible reducir una suma dadas su expresion izquierda y derecha
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Coeficiente de la expresion izquierda</param>
    /// <param name="indRight">Coeficiente de la expresion derecha</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    private static Function<T>? ReduceSum(Function<T> left, Function<T> right, T indLeft, T indRight)
    {
        var auxExp = MultiplyInd(left);
        (left, indLeft) = (auxExp.Item1, left.Arithmetic.Multiply(auxExp.Item2, indLeft));
        auxExp = MultiplyInd(right);
        (right, indRight) = (auxExp.Item1, left.Arithmetic.Multiply(auxExp.Item2, indRight));

        var (leftBinary, rightBinary) =
            (left as BinaryExpression<T>, right as BinaryExpression<T>);

        Function<T>? aux;

        if (leftBinary is Sum<T>)
        {
            aux = ReduceSum(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(leftBinary.Right, aux, indLeft, left.Arithmetic.Real1);
            aux = ReduceSum(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(leftBinary.Left, aux, indLeft, left.Arithmetic.Real1);
        }

        if (rightBinary is Sum<T>)
        {
            aux = ReduceSum(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(aux, rightBinary.Right, left.Arithmetic.Real1, indRight);
            aux = ReduceSum(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(aux, rightBinary.Left, left.Arithmetic.Real1, indRight);
        }

        if (leftBinary is Subtraction<T>)
        {
            aux = ReduceSum(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, leftBinary.Right, left.Arithmetic.Real1, indLeft);
            aux = ReduceSubtraction(right, leftBinary.Right, indRight, indLeft);
            if (aux is not null) return ReduceSumCheck(leftBinary.Left, aux, indLeft, left.Arithmetic.Real1);
        }

        if (rightBinary is Subtraction<T>)
        {
            aux = ReduceSum(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, rightBinary.Right, left.Arithmetic.Real1, indRight);
            aux = ReduceSubtraction(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(aux, rightBinary.Left, left.Arithmetic.Real1, indRight);
        }

        if (left.Equals(right))
        {
            BinaryExpression<T> sum = new NumberExpression<T>(left.Arithmetic.Sum(indLeft, indRight), left.Arithmetic) *
                                      left;
            aux = ReduceMultiplyDivision<T>.ReduceMultiplySimple(sum);
            if (aux is not null) return aux;

            return sum;
        }

        if (left is NumberExpression<T> && right is NumberExpression<T>)
            return new NumberExpression<T>(left.Arithmetic.Sum(
                left.Arithmetic.Multiply(left.Evaluate(new List<(char, T)>()), indLeft),
                left.Arithmetic.Multiply(right.Evaluate(new List<(char, T)>()), indRight)), left.Arithmetic);

        return null;
    }

    /// <summary>
    ///     Determinar si una resta se puede reducir dadas caracteristicas simples
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    internal static Function<T> ReduceSubtraction(BinaryExpression<T> binary)
    {
        var aux = ReduceSubtractionSimple(binary);
        if (aux is not null) return aux;

        aux = ReduceSubtraction(binary.Left, binary.Right, binary.Arithmetic.Real1, binary.Arithmetic.Real1);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    ///     Reducir una resta
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    private static Function<T>? ReduceSubtractionSimple(BinaryExpression<T> binary)
    {
        if (binary.Left.Equals(new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic)))
            return ReduceMultiplyDivision<T>.ReduceMultiply(
                new NumberExpression<T>(binary.Arithmetic.RealN1, binary.Arithmetic) * binary.Right);
        return binary.Right.Equals(new NumberExpression<T>(binary.Arithmetic.Real0, binary.Arithmetic))
            ? binary.Left
            : null;
    }

    /// <summary>
    ///     Comprobar si es posible reducir nuevamente la resta
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Coeficiente de la expresion izquierda</param>
    /// <param name="indRight">Coeficiente de la expresion derecha</param>
    /// <returns>Expresion resultante</returns>
    private static Function<T> ReduceSubtractionCheck(Function<T> left, Function<T> right, T indLeft,
        T indRight)
    {
        var aux = ReduceSubtraction(left, right, indLeft, indRight);
        if (aux is not null) return aux;

        BinaryExpression<T> binary = new NumberExpression<T>(indLeft, left.Arithmetic) * left -
                                     new NumberExpression<T>(indRight, left.Arithmetic) * right;

        aux = ReduceSubtractionSimple(binary);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    ///     Determinar si es posible reducir una resta dadas su expresion izquierda y derecha
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Coeficiente de la expresion izquierda</param>
    /// <param name="indRight">Coeficiente de la expresion derecha</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    private static Function<T>? ReduceSubtraction(Function<T> left, Function<T> right, T indLeft,
        T indRight)
    {
        var auxExp = MultiplyInd(left);
        (left, indLeft) = (auxExp.Item1, left.Arithmetic.Multiply(auxExp.Item2, indLeft));
        auxExp = MultiplyInd(right);
        (right, indRight) = (auxExp.Item1, left.Arithmetic.Multiply(auxExp.Item2, indRight));

        var (leftBinary, rightBinary) =
            (left as BinaryExpression<T>, right as BinaryExpression<T>);

        Function<T>? aux;

        if (leftBinary is Sum<T>)
        {
            aux = ReduceSubtraction(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(leftBinary.Right, aux, indLeft, left.Arithmetic.Real1);
            aux = ReduceSubtraction(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(leftBinary.Left, aux, indLeft, left.Arithmetic.Real1);
        }

        if (rightBinary is Sum<T>)
        {
            aux = ReduceSubtraction(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, rightBinary.Right, left.Arithmetic.Real1, indRight);
            aux = ReduceSubtraction(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, rightBinary.Left, left.Arithmetic.Real1, indRight);
        }

        if (leftBinary is Subtraction<T>)
        {
            aux = ReduceSum(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(leftBinary.Left, aux, indLeft, left.Arithmetic.Real1);
            aux = ReduceSubtraction(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, leftBinary.Right, left.Arithmetic.Real1, indLeft);
        }

        if (rightBinary is Subtraction<T>)
        {
            aux = ReduceSum(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, rightBinary.Left, left.Arithmetic.Real1, indRight);
            aux = ReduceSubtraction(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(aux, rightBinary.Right, left.Arithmetic.Real1, indRight);
        }

        if (left.Equals(right))
        {
            BinaryExpression<T> pow =
                new NumberExpression<T>(left.Arithmetic.Subtraction(indLeft, indRight), left.Arithmetic) * left;
            aux = ReduceMultiplyDivision<T>.ReduceMultiplySimple(pow);
            if (aux is not null) return aux;

            return pow;
        }

        // if (left is NumberExpression && right is NumberExpression)
        //     return new NumberExpression(left.Evaluate(new List<(char, RealNumbers)>()) * indLeft -
        //                                 right.Evaluate(new List<(char, RealNumbers)>()) * indRight);

        return null;
    }

    private static (Function<T>, T) MultiplyInd(Function<T> exp)
    {
        if (exp is not Multiply<T> binary) return (exp, exp.Arithmetic.Real1);

        if (binary.Right is NumberExpression<T> expression) return (binary.Left, expression.Value);
        return binary.Left is NumberExpression<T> numberExpression
            ? (binary.Right, numberExpression.Value)
            : (binary, exp.Arithmetic.Real1);
    }
}