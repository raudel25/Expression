namespace BigNum;

public class Fraction : IComparable<Fraction>
{
    internal readonly RealNumbers Numerator;

    internal readonly RealNumbers Denominator;

    internal readonly char Sign;

    public readonly RealNumbers Valor;

    public Fraction(RealNumbers numerator, RealNumbers denominator)
    {
        this.Numerator = numerator;
        this.Denominator = denominator;
        this.Sign = numerator.Positive() && denominator.Positive() ? '+' : '-';
        this.Valor = numerator / denominator;
    }

    public override bool Equals(object? obj)
    {
        Fraction? frac = obj as Fraction;

        if (frac is null) return false;

        return this.Numerator * frac.Denominator == this.Denominator * frac.Numerator;
    }

    public override int GetHashCode()
    {
        return this.Numerator.GetHashCode() * this.Denominator.GetHashCode();
    }

    public int CompareTo(Fraction? frac)
    {
        if (frac is null) throw new Exception("El valor introducido no es correcto");

        if (this.Sign == frac.Sign)
        {
            if (this.Positive())
                return (this.Numerator * frac.Denominator).Abs.CompareTo((this.Denominator * frac.Numerator).Abs);

            return (this.Denominator * frac.Numerator).Abs.CompareTo((this.Numerator * frac.Denominator).Abs);
        }

        if (this.Positive()) return 1;
        return -1;
    }

    public bool Positive()
    {
        return this.Sign == '+';
    }

    public override string ToString()
    {
        string sign = this.Sign == '-' ? "-" : "";

        return sign + this.Numerator.Abs + "/" + this.Denominator.Abs;
    }

    #region Operadores

    public static Fraction operator +(Fraction a, Fraction b) => BigNumMath.Sum(a, b);

    public static Fraction operator -(Fraction a) => BigNumMath.Opposite(a);

    public static Fraction operator -(Fraction a, Fraction b) => BigNumMath.Sum(a, BigNumMath.Opposite(b));

    public static Fraction operator *(Fraction a, Fraction b) => BigNumMath.Product(a, b);

    public static Fraction operator /(Fraction a, Fraction b) => BigNumMath.Division(a, b);

    public static bool operator ==(Fraction? a, Fraction? b)
    {
        if (a is null || b is null)
        {
            return a is null && b is null;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Fraction? a, Fraction? b)
    {
        if (a is null || b is null)
        {
            return a is null && b is null;
        }

        return !a.Equals(b);
    }

    public static bool operator >(Fraction a, Fraction b) => a.CompareTo(b) == 1;

    public static bool operator <(Fraction a, Fraction b) => a.CompareTo(b) == -1;

    public static bool operator >=(Fraction a, Fraction b) => a.CompareTo(b) != -1;

    public static bool operator <=(Fraction a, Fraction b) => a.CompareTo(b) != 1;

    #endregion
}