namespace BigNum;

public class RealNumbers : IComparable<RealNumbers>
{
    internal readonly RealNumbers Abs;

    internal readonly List<long> NumberValue;

    internal readonly char Sign;

    internal RealNumbers(string s, int precision, long base10, int indBase10, bool positive = true)
    {
        IndBase10 = indBase10;
        Base10 = base10;
        Precision = precision;
        if (!Check(s)) throw new Exception("El valor introducido no es correcto");

        var part = s.Split('.');

        (var partNumber, var partDecimal) = (AuxOperations.EliminateZerosLeft(part[0]),
            part.Length == 2 ? AuxOperations.EliminateZerosRight(part[1]) : "0");
        NumberValue = CreateNumberValue(partNumber, partDecimal);

        CheckZero(ref positive);
        Sign = positive ? '+' : '-';
        Abs = positive ? this : new RealNumbers(NumberValue, Precision, Base10, IndBase10);
    }

    internal RealNumbers(List<long> numberValue, int precision, long base10, int indBase10, bool positive = true)
    {
        Precision = precision;
        IndBase10 = indBase10;
        Base10 = base10;

        NumberValue = AuxOperations.EliminateZerosLeftValue(numberValue, precision);

        CheckZero(ref positive);
        Sign = positive ? '+' : '-';
        Abs = positive ? this : new RealNumbers(NumberValue, Precision, Base10, IndBase10);
    }

    public int Precision { get; }

    public long Base10 { get; }

    public int IndBase10 { get; }

    public RealNumbers Real1 => new("1", Precision, Base10, IndBase10);

    public RealNumbers RealN1 => new("1", Precision, Base10, IndBase10, false);

    public RealNumbers Real0 => new("0", Precision, Base10, IndBase10);

    public int CompareTo(RealNumbers? n)
    {
        if (n == null) throw new Exception("El valor introducido no es correcto");

        if (Sign == n.Sign)
        {
            if (Sign == '+') return AuxOperations.CompareList(NumberValue, n.NumberValue);
            return AuxOperations.CompareList(n.NumberValue, NumberValue);
        }

        if (Sign == '+') return 1;
        return -1;
    }

    private List<long> CreateNumberValue(string partNumber, string partDecimal)
    {
        var numberValue = new long[Precision + 1].ToList();

        partNumber = AuxOperations.AddZerosLeft(partNumber, IndBase10 - partNumber.Length % IndBase10);
        partDecimal = AuxOperations.AddZerosRight(partDecimal, IndBase10 - partDecimal.Length % IndBase10);

        for (var i = 0; i < partDecimal.Length / IndBase10; i++)
        {
            numberValue[numberValue.Count - i - 2] =
                long.Parse(partDecimal.Substring(i * IndBase10, IndBase10));

            if (i + 1 == Precision) break;
        }


        numberValue[^1] =
            long.Parse(partNumber.Substring(partNumber.Length - IndBase10, IndBase10));

        for (var i = 1; i < partNumber.Length / IndBase10; i++)
            numberValue.Add(long.Parse(partNumber.Substring(partNumber.Length - (i + 1) * IndBase10,
                IndBase10)));

        return numberValue;
    }

    public static bool Check(string s)
    {
        var part = s.Split('.');

        if (part.Length > 2) return false;

        foreach (var number in part)
            if (!CheckNumber(number))
                return false;

        return true;
    }

    private static bool CheckNumber(string number)
    {
        foreach (var item in number)
            if (!char.IsNumber(item))
                return false;

        return true;
    }

    public override bool Equals(object? obj)
    {
        var n = obj as RealNumbers;
        if (n == null) return false;

        return CompareTo(n) == 0;
    }

    public override int GetHashCode()
    {
        return NumberValue.GetHashCode() * Sign.GetHashCode();
    }

    private void CheckZero(ref bool positive)
    {
        if (AuxOperations.CompareList(new List<long> { 0 }, NumberValue) == 0) positive = true;
    }

    public bool Positive()
    {
        return Sign == '+';
    }

    public override string ToString()
    {
        var sign = Sign == '-' ? "-" : "";
        (var partDecimal, var partNumber) = ("", "");

        for (var i = 0; i < Precision; i++)
        {
            var aux = NumberValue[i].ToString();
            partDecimal = $"{AuxOperations.AddZerosLeft(aux, IndBase10 - aux.Length)}{partDecimal}";
        }

        for (var i = Precision; i < NumberValue.Count; i++)
        {
            var aux = NumberValue[i].ToString();
            partNumber = $"{AuxOperations.AddZerosLeft(aux, IndBase10 - aux.Length)}{partNumber}";
        }

        (partNumber, partDecimal) = (AuxOperations.EliminateZerosLeft(partNumber),
            AuxOperations.EliminateZerosRight(partDecimal));

        return partDecimal == "0" ? $"{sign}{partNumber}" : $"{sign}{partNumber}.{partDecimal}";
    }

    #region operadores

    public static RealNumbers operator +(RealNumbers a, RealNumbers b)
    {
        return BigNumMath.Sum(a, b);
    }

    public static RealNumbers operator -(RealNumbers a)
    {
        return BigNumMath.Opposite(a);
    }

    public static RealNumbers operator -(RealNumbers a, RealNumbers b)
    {
        return BigNumMath.Sum(a, BigNumMath.Opposite(b));
    }

    public static RealNumbers operator *(RealNumbers a, RealNumbers b)
    {
        return BigNumMath.Product(a, b);
    }

    public static RealNumbers operator /(RealNumbers a, RealNumbers b)
    {
        return BigNumMath.Division(a, b);
    }

    public static bool operator ==(RealNumbers? a, RealNumbers? b)
    {
        if (a is null || b is null) return a is null && b is null;

        return a.Equals(b);
    }

    public static bool operator !=(RealNumbers? a, RealNumbers? b)
    {
        if (a is null || b is null) return a is null && b is null;

        return !a.Equals(b);
    }

    public static bool operator >(RealNumbers a, RealNumbers b)
    {
        return a.CompareTo(b) == 1;
    }

    public static bool operator <(RealNumbers a, RealNumbers b)
    {
        return a.CompareTo(b) == -1;
    }

    public static bool operator >=(RealNumbers a, RealNumbers b)
    {
        return a.CompareTo(b) != -1;
    }

    public static bool operator <=(RealNumbers a, RealNumbers b)
    {
        return a.CompareTo(b) != 1;
    }

    public static RealNumbers operator ++(RealNumbers a)
    {
        return a + a.Real1;
    }

    public static RealNumbers operator --(RealNumbers a)
    {
        return a - a.Real1;
    }

    #endregion
}