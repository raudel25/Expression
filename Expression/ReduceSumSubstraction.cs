using BigNum;

namespace Expression;

internal static class ReduceSumSubtraction
{
    internal static ExpressionType ReduceSum(BinaryExpression binary)
    {
        ExpressionType? aux = ReduceSumSimple(binary);
        if (aux is not null) return aux;

        aux = ReduceSum(binary.Left, binary.Right, RealNumbers.Real1, RealNumbers.Real1);
        if (aux is not null) return aux;

        return binary;
    }

    internal static ExpressionType? ReduceSumSimple(BinaryExpression binary)
    {
        if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Right;
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Left;

        return null;
    }

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
            return new NumberExpression(left.Evaluate(RealNumbers.Real0) * indLeft +
                                        right.Evaluate(RealNumbers.Real0) * indRight);

        return null;
    }

    internal static ExpressionType ReduceSubtraction(BinaryExpression binary)
    {
        ExpressionType? aux = ReduceSubtractionSimple(binary);
        if (aux is not null) return aux;

        aux = ReduceSubtraction(binary.Left, binary.Right, RealNumbers.Real1, RealNumbers.Real1);
        if (aux is not null) return aux;

        return binary;
    }

    internal static ExpressionType? ReduceSubtractionSimple(BinaryExpression binary)
    {
        if (binary.Left.Equals(new NumberExpression(RealNumbers.Real0)))
            return new NumberExpression(RealNumbers.RealN1)*binary.Right;
        if (binary.Right.Equals(new NumberExpression(RealNumbers.Real0))) return binary.Left;

        return null;
    }

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
        
        if (left is NumberExpression && right is NumberExpression)
            return new NumberExpression(left.Evaluate(RealNumbers.Real0) * indLeft -
                                        right.Evaluate(RealNumbers.Real0) * indRight);

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