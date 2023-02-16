namespace BigNum;

public class Fraction : IComparable<Fraction>
{
    internal readonly RealNumbers Denominator;
    internal readonly RealNumbers Numerator;

    internal readonly char Sign;

    public readonly RealNumbers Valor;

    public Fraction(RealNumbers numerator, RealNumbers denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
        Sign = numerator.Positive() && denominator.Positive() ? '+' : '-';
        Valor = numerator / denominator;
    }

    public Fraction Fraction1 => new(Numerator.Real1, Denominator.Real1);

    public int CompareTo(Fraction? frac)
    {
        if (frac is null) throw new Exception("El valor introducido no es correcto");

        if (Sign == frac.Sign)
        {
            if (Positive())
                return (Numerator * frac.Denominator).Abs.CompareTo((Denominator * frac.Numerator).Abs);

            return (Denominator * frac.Numerator).Abs.CompareTo((Numerator * frac.Denominator).Abs);
        }

        if (Positive()) return 1;
        return -1;
    }

    public override bool Equals(object? obj)
    {
        var frac = obj as Fraction;

        if (frac is null) return false;

        return Numerator * frac.Denominator == Denominator * frac.Numerator;
    }

    public override int GetHashCode()
    {
        return Numerator.GetHashCode() * Denominator.GetHashCode();
    }

    public bool Positive()
    {
        return Sign == '+';
    }

    public override string ToString()
    {
        var sign = Sign == '-' ? "-" : "";

        return $"{sign}{Numerator.Abs}/{Denominator.Abs}";
    }

    #region Operadores

    public static Fraction operator +(Fraction a, Fraction b)
    {
        return BigNumMath.Sum(a, b);
    }

    public static Fraction operator -(Fraction a)
    {
        return BigNumMath.Opposite(a);
    }

    public static Fraction operator -(Fraction a, Fraction b)
    {
        return BigNumMath.Sum(a, BigNumMath.Opposite(b));
    }

    public static Fraction operator *(Fraction a, Fraction b)
    {
        return BigNumMath.Product(a, b);
    }

    public static Fraction operator /(Fraction a, Fraction b)
    {
        return BigNumMath.Division(a, b);
    }

    public static bool operator ==(Fraction? a, Fraction? b)
    {
        if (a is null || b is null) return a is null && b is null;

        return a.Equals(b);
    }

    public static bool operator !=(Fraction? a, Fraction? b)
    {
        if (a is null || b is null) return a is null && b is null;

        return !a.Equals(b);
    }

    public static bool operator >(Fraction a, Fraction b)
    {
        return a.CompareTo(b) == 1;
    }

    public static bool operator <(Fraction a, Fraction b)
    {
        return a.CompareTo(b) == -1;
    }

    public static bool operator >=(Fraction a, Fraction b)
    {
        return a.CompareTo(b) != -1;
    }

    public static bool operator <=(Fraction a, Fraction b)
    {
        return a.CompareTo(b) != 1;
    }

    #endregion
}