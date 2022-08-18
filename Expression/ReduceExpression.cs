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

        if (!(binary is null))
        {
            if (binary is Sum)
            {
                if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Right;
                if (binary.Right.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Left;
            }

            if (binary is Subtraction)
            {
                if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0)) && binary.Right is NumberExpression)
                    return new NumberExpression(-((NumberExpression) binary.Right).Value);
                if (binary.Right.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Left;
            }

            if (binary is Multiply) return ReduceMultiply(binary);

            if (binary is Division) return ReduceDivision(binary);

            if (binary is Pow)
            {
                ExpressionType? aux = ReducePowSimple(binary);
                return aux is null ? exp : aux;
            }

            if (binary is Log)
            {
                if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1)))
                    return new NumberExpression(RealNumbers.Real0);
            }
        }

        return exp;
    }

    private static ExpressionType ReduceMultiply(BinaryExpression binary)
    {
        ExpressionType? aux = ReduceMultiplySimple(binary);
        if (aux is not null) return aux;

        aux = ReduceMultiply(binary.Left, binary.Right, RealNumbers.Real1, RealNumbers.Real1);
        if (aux is not null) return aux;

        return binary;
    }

    private static ExpressionType? ReduceMultiplySimple(BinaryExpression binary)
    {
        if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0)) ||
            binary.Right.Equals(new NumberExpression(RealNumbers.Real0)))
            return new NumberExpression(RealNumbers.Real0);
        if (binary.Left.Equals(new NumberExpression(RealNumbers.Real1))) return binary.Right;
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1))) return binary.Left;

        return null;
    }

    private static ExpressionType ReduceMultiplyCheck(ExpressionType left, ExpressionType right, RealNumbers indLeft,
        RealNumbers indRight)
    {
        ExpressionType? aux = ReduceMultiply(left, right, indLeft, indRight);
        if (aux is not null) return aux;

        BinaryExpression binary = Pow.DeterminatePow(left, new NumberExpression(indLeft)) *
                                  Pow.DeterminatePow(right, new NumberExpression(indRight));

        aux = ReduceMultiplySimple(binary);
        if (aux is not null) return aux;

        return binary;
    }

    private static ExpressionType? ReduceMultiply(ExpressionType left, ExpressionType right, RealNumbers indLeft,
        RealNumbers indRight)
    {
        var auxExp = PowInd(left);
        (left, indLeft) = (auxExp.Item1, auxExp.Item2 * indLeft);
        auxExp = PowInd(right);
        (right, indRight) = (auxExp.Item1, auxExp.Item2 * indRight);

        (BinaryExpression? leftBinary, BinaryExpression? rightBinary) =
            (left as BinaryExpression, right as BinaryExpression);

        ExpressionType? aux;

        if (leftBinary is Multiply)
        {
            aux = ReduceMultiply(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(leftBinary.Right, aux, indLeft, RealNumbers.Real1);
            aux = ReduceMultiply(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(leftBinary.Left, aux, indLeft, RealNumbers.Real1);
        }

        if (rightBinary is Multiply)
        {
            aux = ReduceMultiply(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(aux, rightBinary.Right, RealNumbers.Real1, indRight);
            aux = ReduceMultiply(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(aux, rightBinary.Left, RealNumbers.Real1, indRight);
        }

        if (leftBinary is Division)
        {
            aux = ReduceMultiply(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, leftBinary.Right, RealNumbers.Real1, indLeft);
            aux = ReduceDivision(right, leftBinary.Right, indRight, indLeft);
            if (aux is not null) return ReduceMultiplyCheck(leftBinary.Left, aux, indLeft, RealNumbers.Real1);
        }

        if (rightBinary is Division)
        {
            aux = ReduceMultiply(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, rightBinary.Right, RealNumbers.Real1, indRight);
            aux = ReduceDivision(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(aux, rightBinary.Left, RealNumbers.Real1, indRight);
        }

        if (left.Equals(right))
        {
            BinaryExpression pow = Pow.DeterminatePow(left, new NumberExpression(indLeft + indRight));
            aux = ReducePowSimple(pow);
            if (aux is not null) return aux;

            return pow;
        }

        return null;
    }

    private static ExpressionType ReduceDivision(BinaryExpression binary)
    {
        ExpressionType? aux = ReduceDivisionSimple(binary);
        if (aux is not null) return aux;

        aux = ReduceDivision(binary.Left, binary.Right, RealNumbers.Real1, RealNumbers.Real1);
        if (aux is not null) return aux;

        return binary;
    }

    private static ExpressionType? ReduceDivisionSimple(BinaryExpression binary)
    {
        if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0)))
            return new NumberExpression(RealNumbers.Real0);
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1))) return binary.Left;

        return null;
    }

    private static ExpressionType ReduceDivisionCheck(ExpressionType left, ExpressionType right, RealNumbers indLeft,
        RealNumbers indRight)
    {
        ExpressionType? aux = ReduceDivision(left, right, indLeft, indRight);
        if (aux is not null) return aux;

        BinaryExpression binary = Pow.DeterminatePow(left, new NumberExpression(indLeft)) /
                                  Pow.DeterminatePow(right, new NumberExpression(indRight));

        aux = ReduceDivisionSimple(binary);
        if (aux is not null) return aux;

        return binary;
    }

    private static ExpressionType? ReduceDivision(ExpressionType left, ExpressionType right, RealNumbers indLeft,
        RealNumbers indRight)
    {
        var auxExp = PowInd(left);
        (left, indLeft) = (auxExp.Item1, auxExp.Item2 * indLeft);
        auxExp = PowInd(right);
        (right, indRight) = (auxExp.Item1, auxExp.Item2 * indRight);

        (BinaryExpression? leftBinary, BinaryExpression? rightBinary) =
            (left as BinaryExpression, right as BinaryExpression);

        ExpressionType? aux;

        if (leftBinary is Multiply)
        {
            aux = ReduceDivision(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(leftBinary.Right, aux, indLeft, RealNumbers.Real1);
            aux = ReduceDivision(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(leftBinary.Left, aux, indLeft, RealNumbers.Real1);
        }

        if (rightBinary is Multiply)
        {
            aux = ReduceDivision(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, rightBinary.Right, RealNumbers.Real1, indRight);
            aux = ReduceDivision(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, rightBinary.Left, RealNumbers.Real1, indRight);
        }

        if (leftBinary is Division)
        {
            aux = ReduceMultiply(leftBinary.Right, right, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(leftBinary.Left, aux, indLeft, RealNumbers.Real1);
            aux = ReduceDivision(leftBinary.Left, right, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, leftBinary.Right, RealNumbers.Real1, indLeft);
        }

        if (rightBinary is Division)
        {
            aux = ReduceMultiply(left, rightBinary.Right, indLeft, indRight);
            if (aux is not null) return ReduceDivisionCheck(aux, rightBinary.Left, RealNumbers.Real1, indRight);
            aux = ReduceDivision(left, rightBinary.Left, indLeft, indRight);
            if (aux is not null) return ReduceMultiplyCheck(aux, rightBinary.Right, RealNumbers.Real1, indRight);
        }

        if (left.Equals(right))
        {
            BinaryExpression pow = Pow.DeterminatePow(left, new NumberExpression(indLeft - indRight));
            aux = ReducePowSimple(pow);
            if (aux is not null) return aux;

            return pow;
        }

        return null;
    }

    private static ExpressionType? ReducePowSimple(BinaryExpression binary)
    {
        if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0)))
            return new NumberExpression(RealNumbers.Real0);
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real0)))
            return new NumberExpression(RealNumbers.Real1);
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real1)))
            return binary.Left;

        return null;
    }

    private static (ExpressionType, RealNumbers) PowInd(ExpressionType exp)
    {
        BinaryExpression? binary = exp as BinaryExpression;
        if (binary is null) return (exp, RealNumbers.Real1);

        if (binary.Right is NumberExpression) return (binary.Left, ((NumberExpression) binary.Right).Value);

        return (binary, RealNumbers.Real1);
    }
}