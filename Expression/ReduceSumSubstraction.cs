using BigNum;

namespace Expression;

internal static class ReduceSumSubtraction
{
    /// <summary>
    /// Reducir una suma
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    internal static ExpressionType ReduceSum(BinaryExpression binary)
    {
        ExpressionType? aux = ReduceSumSimple(binary);
        if (aux is not null) return aux;

        aux = ReduceSum(binary.Left, binary.Right, RealNumbers.Real1, RealNumbers.Real1);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    /// Determinar si una suma se puede reducir dadas caracteristicas simples
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    internal static ExpressionType? ReduceSumSimple(BinaryExpression binary)
    {
        if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Right;
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Left;

        return null;
    }

    /// <summary>
    /// Comprobar si es posible reducir nuevamente la suma
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Coeficiente de la expresion izquierda</param>
    /// <param name="indRight">Coeficiente de la expresion derecha</param>
    /// <returns>Expresion resultante</returns>
    private static ExpressionType ReduceSumCheck(ExpressionType left, ExpressionType right, RealNumbers indLeft,
        RealNumbers indRight)
    {
        ExpressionType? aux = ReduceSum(left, right, indLeft, indRight);
        if (aux is not null) return aux;

        BinaryExpression binary = new NumberExpression(indLeft) * left + new NumberExpression(indRight) * right;

        aux = ReduceSumSimple(binary);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    /// Determinar si es posible reducir una suma dadas su expresion izquierda y derecha
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Coeficiente de la expresion izquierda</param>
    /// <param name="indRight">Coeficiente de la expresion derecha</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    private static ExpressionType? ReduceSum(ExpressionType left, ExpressionType right, RealNumbers indLeft,
        RealNumbers indRight)
    {
        var auxExp = MultiplyInd(left);
        (left, indLeft) = (auxExp.Item1, auxExp.Item2 * indLeft);
        auxExp = MultiplyInd(right);
        (right, indRight) = (auxExp.Item1, auxExp.Item2 * indRight);

        (BinaryExpression? leftBinary, BinaryExpression? rightBinary) =
            (left as BinaryExpression, right as BinaryExpression);

        ExpressionType? aux;

        if (leftBinary is Sum)
        {
            aux = ReduceSum(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(leftBinary.Right, aux, indLeft, RealNumbers.Real1);
            aux = ReduceSum(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(leftBinary.Left, aux, indLeft, RealNumbers.Real1);
        }

        if (rightBinary is Sum)
        {
            aux = ReduceSum(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(aux, rightBinary.Right, RealNumbers.Real1, indRight);
            aux = ReduceSum(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(aux, rightBinary.Left, RealNumbers.Real1, indRight);
        }

        if (leftBinary is Subtraction)
        {
            aux = ReduceSum(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, leftBinary.Right, RealNumbers.Real1, indLeft);
            aux = ReduceSubtraction(right, leftBinary.Right, indRight, indLeft);
            if (aux is not null) return ReduceSumCheck(leftBinary.Left, aux, indLeft, RealNumbers.Real1);
        }

        if (rightBinary is Subtraction)
        {
            aux = ReduceSum(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, rightBinary.Right, RealNumbers.Real1, indRight);
            aux = ReduceSubtraction(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(aux, rightBinary.Left, RealNumbers.Real1, indRight);
        }

        if (left.Equals(right))
        {
            BinaryExpression sum = new NumberExpression(indLeft + indRight) * left;
            aux = ReduceMultiplyDivision.ReduceMultiplySimple(sum);
            if (aux is not null) return aux;

            return sum;
        }

        if (left is NumberExpression && right is NumberExpression)
            return new NumberExpression(left.Evaluate(new List<(char, RealNumbers)>()) * indLeft +
                                        right.Evaluate(new List<(char, RealNumbers)>()) * indRight);

        return null;
    }

    /// <summary>
    /// Determinar si una resta se puede reducir dadas caracteristicas simples
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    internal static ExpressionType ReduceSubtraction(BinaryExpression binary)
    {
        ExpressionType? aux = ReduceSubtractionSimple(binary);
        if (aux is not null) return aux;

        aux = ReduceSubtraction(binary.Left, binary.Right, RealNumbers.Real1, RealNumbers.Real1);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    /// Reducir una resta
    /// </summary>
    /// <param name="binary">Expresion binaria</param>
    /// <returns>Expresion resultante</returns>
    internal static ExpressionType? ReduceSubtractionSimple(BinaryExpression binary)
    {
        if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0)))
            return new NumberExpression(RealNumbers.RealN1) * binary.Right;
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Left;

        return null;
    }

    /// <summary>
    /// Comprobar si es posible reducir nuevamente la resta
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Coeficiente de la expresion izquierda</param>
    /// <param name="indRight">Coeficiente de la expresion derecha</param>
    /// <returns>Expresion resultante</returns>
    private static ExpressionType ReduceSubtractionCheck(ExpressionType left, ExpressionType right, RealNumbers indLeft,
        RealNumbers indRight)
    {
        ExpressionType? aux = ReduceSubtraction(left, right, indLeft, indRight);
        if (aux is not null) return aux;

        BinaryExpression binary = new NumberExpression(indLeft) * left - new NumberExpression(indRight) * right;

        aux = ReduceSubtractionSimple(binary);
        if (aux is not null) return aux;

        return binary;
    }

    /// <summary>
    /// Determinar si es posible reducir una resta dadas su expresion izquierda y derecha
    /// </summary>
    /// <param name="left">Expresion izquierda</param>
    /// <param name="right">Expresion derecha</param>
    /// <param name="indLeft">Coeficiente de la expresion izquierda</param>
    /// <param name="indRight">Coeficiente de la expresion derecha</param>
    /// <returns>Expresion resultante(si es null es que no se pudo reducir)</returns>
    private static ExpressionType? ReduceSubtraction(ExpressionType left, ExpressionType right, RealNumbers indLeft,
        RealNumbers indRight)
    {
        var auxExp = MultiplyInd(left);
        (left, indLeft) = (auxExp.Item1, auxExp.Item2 * indLeft);
        auxExp = MultiplyInd(right);
        (right, indRight) = (auxExp.Item1, auxExp.Item2 * indRight);

        (BinaryExpression? leftBinary, BinaryExpression? rightBinary) =
            (left as BinaryExpression, right as BinaryExpression);

        ExpressionType? aux;

        if (leftBinary is Sum)
        {
            aux = ReduceSubtraction(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(leftBinary.Right, aux, indLeft, RealNumbers.Real1);
            aux = ReduceSubtraction(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(leftBinary.Left, aux, indLeft, RealNumbers.Real1);
        }

        if (rightBinary is Sum)
        {
            aux = ReduceSubtraction(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, rightBinary.Right, RealNumbers.Real1, indRight);
            aux = ReduceSubtraction(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, rightBinary.Left, RealNumbers.Real1, indRight);
        }

        if (leftBinary is Subtraction)
        {
            aux = ReduceSum(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(leftBinary.Left, aux, indLeft, RealNumbers.Real1);
            aux = ReduceSubtraction(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, leftBinary.Right, RealNumbers.Real1, indLeft);
        }

        if (rightBinary is Subtraction)
        {
            aux = ReduceSum(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceSubtractionCheck(aux, rightBinary.Left, RealNumbers.Real1, indRight);
            aux = ReduceSubtraction(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceSumCheck(aux, rightBinary.Right, RealNumbers.Real1, indRight);
        }

        if (left.Equals(right))
        {
            BinaryExpression pow = new NumberExpression(indLeft - indRight) * left;
            aux = ReduceMultiplyDivision.ReduceMultiplySimple(pow);
            if (aux is not null) return aux;

            return pow;
        }

        // if (left is NumberExpression && right is NumberExpression)
        //     return new NumberExpression(left.Evaluate(new List<(char, RealNumbers)>()) * indLeft -
        //                                 right.Evaluate(new List<(char, RealNumbers)>()) * indRight);

        return null;
    }

    private static (ExpressionType, RealNumbers) MultiplyInd(ExpressionType exp)
    {
        Multiply? binary = exp as Multiply;
        if (binary is null) return (exp, RealNumbers.Real1);

        if (binary.Right is NumberExpression) return (binary.Left, ((NumberExpression) binary.Right).Value);
        if (binary.Left is NumberExpression) return (binary.Right, ((NumberExpression) binary.Left).Value);

        return (binary, RealNumbers.Real1);
    }
}