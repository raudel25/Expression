namespace BigNum;

public class RealNumbers : IComparable<RealNumbers>
{
    internal int Precision { get; private set; }

    internal long Base10 => 1000000000;

    internal int IndBase10 => 9;

    public static readonly RealNumbers Real1 = new RealNumbers("1");

    public static readonly RealNumbers RealN1 = new RealNumbers("1", false);

    public static readonly RealNumbers Real0 = new RealNumbers("0");

    internal readonly List<long> NumberValue;

    internal readonly char Sign;

    internal readonly RealNumbers Abs;

    public RealNumbers(string s, bool positive = true)
    {
        int precision = 6;

        this.Precision = precision;
        if (!Check(s)) throw new Exception("El valor introducido no es correcto");

        string[] part = s.Split('.');

        (string partNumber, string partDecimal) = (AuxOperations.EliminateZerosLeft(part[0]),
            part.Length == 2 ? AuxOperations.EliminateZerosRight(part[1]) : "0");
        this.NumberValue = CreateNumberValue(partNumber, partDecimal);

        CheckZero(ref positive);
        this.Sign = positive ? '+' : '-';
        this.Abs = positive ? this : new RealNumbers(this.NumberValue, true, this.Precision);
    }

    protected RealNumbers(string s, bool positive,int precision)
    {
        this.Precision = precision;
        if (!Check(s)) throw new Exception("El valor introducido no es correcto");

        string[] part = s.Split('.');

        (string partNumber, string partDecimal) = (AuxOperations.EliminateZerosLeft(part[0]),
            part.Length == 2 ? AuxOperations.EliminateZerosRight(part[1]) : "0");
        this.NumberValue = CreateNumberValue(partNumber, partDecimal);

        CheckZero(ref positive);
        this.Sign = positive ? '+' : '-';
        this.Abs = positive ? this : new RealNumbers(this.NumberValue, true, this.Precision);
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

    internal RealNumbers(List<long> numberValue, bool positive, int precision)
    {
        this.Precision = precision;
        this.NumberValue = AuxOperations.EliminateZerosLeftValue(numberValue, precision);

        CheckZero(ref positive);
        this.Sign = positive ? '+' : '-';
        this.Abs = positive ? this : new RealNumbers(this.NumberValue, true, this.Precision);
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

        return $"{sign}{partNumber}.{partDecimal}";
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

    public static RealNumbers operator ++(RealNumbers a) => a + Real1;

    public static RealNumbers operator --(RealNumbers a) => a - Real1;

    #endregion
}