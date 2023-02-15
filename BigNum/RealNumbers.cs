namespace BigNum;

public class RealNumbers : IComparable<RealNumbers>
{
    public int Precision { get; private set; }

    public long Base10 { get; private set; }

    public int IndBase10 { get; private set; }

    public RealNumbers Real1 => new("1", this.Precision, this.Base10, this.IndBase10);

    public RealNumbers RealN1 => new("1", this.Precision, this.Base10, this.IndBase10, false);

    public RealNumbers Real0 => new("0", this.Precision, this.Base10, this.IndBase10);

    internal readonly List<long> NumberValue;

    internal readonly char Sign;

    internal readonly RealNumbers Abs;

    internal RealNumbers(string s, int precision, long base10, int indBase10, bool positive = true)
    {
        this.IndBase10 = indBase10;
        this.Base10 = base10;
        this.Precision = precision;
        if (!Check(s)) throw new Exception("El valor introducido no es correcto");

        string[] part = s.Split('.');

        (string partNumber, string partDecimal) = (AuxOperations.EliminateZerosLeft(part[0]),
            part.Length == 2 ? AuxOperations.EliminateZerosRight(part[1]) : "0");
        this.NumberValue = CreateNumberValue(partNumber, partDecimal);

        CheckZero(ref positive);
        this.Sign = positive ? '+' : '-';
        this.Abs = positive ? this : new RealNumbers(this.NumberValue, this.Precision, this.Base10, this.IndBase10);
    }

    private List<long> CreateNumberValue(string partNumber, string partDecimal)
    {
        List<long> numberValue = new long[this.Precision + 1].ToList();

        partNumber = AuxOperations.AddZerosLeft(partNumber, this.IndBase10 - partNumber.Length % this.IndBase10);
        partDecimal = AuxOperations.AddZerosRight(partDecimal, this.IndBase10 - partDecimal.Length % this.IndBase10);

        for (int i = 0; i < partDecimal.Length / this.IndBase10; i++)
        {
            numberValue[numberValue.Count - i - 2] =
                long.Parse(partDecimal.Substring(i * this.IndBase10, this.IndBase10));

            if (i + 1 == this.Precision) break;
        }


        numberValue[^1] =
            long.Parse(partNumber.Substring(partNumber.Length - this.IndBase10, this.IndBase10));

        for (int i = 1; i < partNumber.Length / this.IndBase10; i++)
        {
            numberValue.Add(long.Parse(partNumber.Substring(partNumber.Length - (i + 1) * this.IndBase10,
                this.IndBase10)));
        }

        return numberValue;
    }

    internal RealNumbers(List<long> numberValue, int precision, long base10, int indBase10, bool positive = true)
    {
        this.Precision = precision;
        this.IndBase10 = indBase10;
        this.Base10 = base10;

        this.NumberValue = AuxOperations.EliminateZerosLeftValue(numberValue, precision);

        CheckZero(ref positive);
        this.Sign = positive ? '+' : '-';
        this.Abs = positive ? this : new RealNumbers(this.NumberValue, this.Precision, this.Base10, this.IndBase10);
    }

    public static bool Check(string s)
    {
        string[] part = s.Split('.');

        if (part.Length > 2) return false;

        foreach (var number in part)
        {
            if (!CheckNumber(number)) return false;
        }

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
        RealNumbers? n = obj as RealNumbers;
        if (n == null) return false;

        return this.CompareTo(n) == 0;
    }

    public override int GetHashCode()
    {
        return this.NumberValue.GetHashCode() * this.Sign.GetHashCode();
    }

    public int CompareTo(RealNumbers? n)
    {
        if (n == null) throw new Exception("El valor introducido no es correcto");

        if (this.Sign == n.Sign)
        {
            if (this.Sign == '+') return AuxOperations.CompareList(this.NumberValue, n.NumberValue);
            return AuxOperations.CompareList(n.NumberValue, this.NumberValue);
        }

        if (this.Sign == '+') return 1;
        return -1;
    }

    private void CheckZero(ref bool positive)
    {
        if (AuxOperations.CompareList(new List<long>() { 0 }, this.NumberValue) == 0) positive = true;
    }

    public bool Positive() => this.Sign == '+';

    public override string ToString()
    {
        string sign = this.Sign == '-' ? "-" : "";
        (string partDecimal, string partNumber) = ("", "");

        for (int i = 0; i < this.Precision; i++)
        {
            string aux = this.NumberValue[i].ToString();
            partDecimal = $"{AuxOperations.AddZerosLeft(aux, this.IndBase10 - aux.Length)}{partDecimal}";
        }

        for (int i = this.Precision; i < this.NumberValue.Count; i++)
        {
            string aux = this.NumberValue[i].ToString();
            partNumber = $"{AuxOperations.AddZerosLeft(aux, this.IndBase10 - aux.Length)}{partNumber}";
        }

        (partNumber, partDecimal) = (AuxOperations.EliminateZerosLeft(partNumber),
            AuxOperations.EliminateZerosRight(partDecimal));

        return partDecimal == "0" ? $"{sign}{partNumber}" : $"{sign}{partNumber}.{partDecimal}";
    }

    #region operadores

    public static RealNumbers operator +(RealNumbers a, RealNumbers b) => BigNumMath.Sum(a, b);

    public static RealNumbers operator -(RealNumbers a) => BigNumMath.Opposite(a);

    public static RealNumbers operator -(RealNumbers a, RealNumbers b) => BigNumMath.Sum(a, BigNumMath.Opposite(b));

    public static RealNumbers operator *(RealNumbers a, RealNumbers b) => BigNumMath.Product(a, b);

    public static RealNumbers operator /(RealNumbers a, RealNumbers b) => BigNumMath.Division(a, b);

    public static bool operator ==(RealNumbers? a, RealNumbers? b)
    {
        if (a is null || b is null)
        {
            return a is null && b is null;
        }

        return a.Equals(b);
    }

    public static bool operator !=(RealNumbers? a, RealNumbers? b)
    {
        if (a is null || b is null)
        {
            return a is null && b is null;
        }

        return !a.Equals(b);
    }

    public static bool operator >(RealNumbers a, RealNumbers b) => a.CompareTo(b) == 1;

    public static bool operator <(RealNumbers a, RealNumbers b) => a.CompareTo(b) == -1;

    public static bool operator >=(RealNumbers a, RealNumbers b) => a.CompareTo(b) != -1;

    public static bool operator <=(RealNumbers a, RealNumbers b) => a.CompareTo(b) != 1;

    public static RealNumbers operator ++(RealNumbers a) => a + a.Real1;

    public static RealNumbers operator --(RealNumbers a) => a - a.Real1;

    #endregion
}