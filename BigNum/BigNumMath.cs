namespace BigNum;

public static class BigNumMath
{
    public static Numbers Sum(Numbers x, Numbers y) => SumOperations.Sum(x, y);

    public static Fraction Sum(Fraction x, Fraction y) =>
        new Fraction(x.Numerator * y.Denominator + y.Numerator * x.Denominator, x.Denominator * y.Denominator);

    public static Numbers Product(Numbers x, Numbers y) => ProductOperations.Product(x, y);

    public static Fraction Product(Fraction x, Fraction y) =>
        new Fraction(x.Numerator * y.Numerator, x.Denominator * y.Denominator);

    public static Numbers Division(Numbers x, Numbers y) => DivisionOperations.Division(x, y);

    public static Numbers Max(Numbers x, Numbers y)
    {
        if (x.CompareTo(y) == 1) return x;

        return y;
    }

    public static Numbers Min(Numbers x, Numbers y)
    {
        if (x.CompareTo(y) == -1) return x;

        return y;
    }

    public static Numbers Abs(Numbers x) => x.Abs;

    public static Numbers Opposite(Numbers x) => new Numbers(x.PartNumber, x.PartDecimal, !x.Positive());

    public static Fraction Opposite(Fraction x) => new Fraction(BigNumMath.Opposite(x.Numerator), x.Denominator);

    public static Numbers ConvertToNumbers(double n) => new Numbers(n + "", n >= 0);

    public static IntegerNumbers ConvertToIntegerNumbers(int n) => new IntegerNumbers(n + "", n >= 0);

    public static IntegerNumbers NumbersToInteger(Numbers n) => new IntegerNumbers(n.PartNumber, n.Positive());
}