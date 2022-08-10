using System.Diagnostics;

namespace BigNum;

public static class BigNumMath
{
    public static Numbers Sum(Numbers x, Numbers y) => SumOperations.Sum(x, y);

    public static Fraction Sum(Fraction x, Fraction y) =>
        new Fraction(x.Numerator * y.Denominator + y.Numerator * x.Denominator, x.Denominator * y.Denominator);

    public static Numbers Product(Numbers x, Numbers y) => ProductOperations.Product(x, y);

    public static Fraction Product(Fraction x, Fraction y) =>
        new Fraction(x.Numerator * y.Numerator, x.Denominator * y.Denominator);

    public static Numbers Division(Numbers x, Numbers y, bool integer = false) =>
        DivisionOperations.Division(x, y, integer).Item1;

    public static Fraction Division(Fraction x, Fraction y) => x * new Fraction(y.Denominator, y.Numerator);

    public static IntegerNumbers Rest(IntegerNumbers x, IntegerNumbers y) =>
        DivisionOperations.Division(x, y, true).Item2;

    public static Numbers Pow(Numbers x, int pow)
    {
        Numbers result = new Numbers("1");

        for (int i = 0; i < Math.Abs(pow); i++) result *= x;

        if (pow < 0) result = new Numbers("1") / result;

        return result;
    }
    
    public static Fraction Pow(Fraction x, int pow)
    {
        Fraction result = new Fraction(new Numbers("1"),new Numbers("1"));

        for (int i = 0; i < Math.Abs(pow); i++) result *= x;

        if (pow < 0) result = new Fraction(result.Denominator,result.Numerator);

        return result;
    }

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