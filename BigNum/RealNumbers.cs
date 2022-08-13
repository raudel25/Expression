namespace BigNum;

public class RealNumbers : IComparable<RealNumbers>
{
    private static int _precision = 20;
    
    public static readonly RealNumbers Real1 = new RealNumbers("1");
    
    public static readonly RealNumbers RealN1 = new RealNumbers("1",false);

    public static readonly RealNumbers Real0 = new RealNumbers("0");
    
    internal readonly string PartDecimal;

    internal readonly string PartNumber;

    internal readonly char Sign;

    internal readonly RealNumbers Abs;

    public RealNumbers(string s, bool positive = true)
    {
        if (!Check(s)) throw new Exception("El valor introducido no es correcto");

        string[] part = s.Split('.');

        (string partNumber, string partDecimal) = (AuxOperations.EliminateZerosLeft(part[0]),
            part.Length == 2 ? AuxOperations.EliminateZerosRight(part[1]) : "0");
        
        this.PartNumber = partNumber;
        this.PartDecimal = DeterminatePrecision(partDecimal);
        CheckZero(ref positive);
        this.Sign = positive ? '+' : '-';
        this.Abs = positive ? this : new RealNumbers(this.PartNumber, this.PartDecimal);
    }

    internal RealNumbers(string partNumber, string partDecimal, bool positive = true)
    {
        (partNumber, partDecimal) = (AuxOperations.EliminateZerosLeft(partNumber),
            AuxOperations.EliminateZerosRight(partDecimal));
        
        this.PartNumber = partNumber;
        this.PartDecimal = DeterminatePrecision(partDecimal);
        CheckZero(ref positive);
        this.Sign = positive ? '+' : '-';
        this.Abs = positive ? this : new RealNumbers(this.PartNumber, this.PartDecimal);
    }

    private bool Check(string s)
    {
        string[] part = s.Split('.');

        if (part.Length > 2) return false;

        foreach (var number in part)
        {
            foreach (var item in number)
                if (!char.IsNumber(item))
                    return false;
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        RealNumbers? n = obj as RealNumbers;
        if (n == null) return false;

        return n.PartNumber == this.PartNumber && n.PartDecimal == this.PartDecimal && n.Sign == this.Sign;
    }

    public override int GetHashCode()
    {
        return this.PartDecimal.GetHashCode() * this.PartNumber.GetHashCode() * this.Sign.GetHashCode();
    }

    public int CompareTo(RealNumbers? n)
    {
        if (n == null) throw new Exception("El valor introducido no es correcto");

        if (this.Sign == n.Sign)
        {
            if (this.Sign == '+') return CompareNumber(this, n);
            return CompareNumber(n, this);
        }

        if (this.Sign == '+') return 1;
        return -1;
    }

    private static int CompareNumber(RealNumbers m, RealNumbers n)
    {
        if (m.PartNumber.Length > n.PartNumber.Length) return 1;
        if (m.PartNumber.Length < n.PartNumber.Length) return -1;

        for (int i = 0; i < m.PartNumber.Length; i++)
        {
            int x = int.Parse(m.PartNumber[i] + "");
            int y = int.Parse(n.PartNumber[i] + "");

            if (x > y) return 1;
            if (x < y) return -1;
        }

        for (int i = 0; i < Math.Min(m.PartDecimal.Length, n.PartDecimal.Length); i++)
        {
            int x = int.Parse(m.PartDecimal[i] + "");
            int y = int.Parse(n.PartDecimal[i] + "");

            if (x > y) return 1;
            if (x < y) return -1;
        }

        if (m.PartDecimal.Length > n.PartDecimal.Length) return -1;
        if (m.PartDecimal.Length < n.PartDecimal.Length) return 1;

        return 0;
    }

    private void CheckZero(ref bool positive)
    {
        if (this.PartNumber == "0" && this.PartDecimal == "0") positive = true;
    }

    public bool Positive() => this.Sign == '+';

    public override string ToString()
    {
        string sign = this.Sign == '-' ? "-" : "";
        string partDecimal = this.PartDecimal == "0" ? "" : "." + this.PartDecimal;

        return sign + this.PartNumber + partDecimal;
    }

    private static string DeterminatePrecision(string s) => s.Length >= _precision ? s.Substring(0, _precision) : s;

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
    
    public static RealNumbers operator ++(RealNumbers a) => a + new RealNumbers("1");
    
    public static RealNumbers operator --(RealNumbers a) => a - new RealNumbers("1");

    #endregion
}