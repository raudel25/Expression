namespace BigNum;

internal static class SumOperations
{
    public static Numbers Sum(Numbers x, Numbers y)
    {
        if (x.Sign == y.Sign) return SumOperation(x, y, true, x.Positive());

        if (x.Abs.CompareTo(y.Abs) == 0) return new Numbers("0", "0");
        if (x.Abs.CompareTo(y.Abs) == 1) return SumOperation(x.Abs, y.Abs, false, x.Positive());

        return SumOperation(y.Abs, x.Abs, false, y.Positive());
    }

    private static Numbers SumOperation(Numbers x, Numbers y, bool sum, bool positive)
    {
        int mayorDecimal = Math.Max(x.PartDecimal.Length, y.PartDecimal.Length);
        int mayorNumber = Math.Max(x.PartNumber.Length, y.PartNumber.Length);

        string xSumDecimal = AuxOperations.AddZerosRight(x.PartDecimal, mayorDecimal - x.PartDecimal.Length);
        string ySumDecimal = AuxOperations.AddZerosRight(y.PartDecimal, mayorDecimal - y.PartDecimal.Length);
        string xSumNumber = AuxOperations.AddZerosLeft(x.PartNumber, mayorNumber - x.PartNumber.Length);
        string ySumNumber = AuxOperations.AddZerosLeft(y.PartNumber, mayorNumber - y.PartNumber.Length);

        string result = sum
            ? Sum(xSumNumber + xSumDecimal, ySumNumber + ySumDecimal)
            : Subtraction(xSumNumber + xSumDecimal, ySumNumber + ySumDecimal);

        string partDecimal = result.Length == mayorDecimal + mayorNumber
            ? result.Substring(mayorNumber, mayorDecimal)
            : result.Substring(mayorNumber + 1, mayorDecimal);

        string partNumber = result.Length == mayorDecimal + mayorNumber
            ? result.Substring(0, mayorNumber)
            : result.Substring(0, mayorNumber + 1);

        return new Numbers(AuxOperations.EliminateZerosLeft(partNumber), AuxOperations.EliminateZerosRight(partDecimal),
            positive);
    }

    private static string Sum(string x, string y)
    {
        string sum = "";
        bool drag = false;
        int len = x.Length;

        for (int i = 0; i < len; i++)
        {
            int n = int.Parse(x[len - 1 - i] + "") + int.Parse(y[len - 1 - i] + "");

            n = drag ? n + 1 : n;
            drag = n >= 10;
            sum = (n % 10) + sum;
        }

        if (drag) sum = 1 + sum;

        return sum;
    }

    private static string Subtraction(string x, string y)
    {
        string sub = "";
        bool drag = false;
        int len = x.Length;

        for (int i = 0; i < len; i++)
        {
            int n = int.Parse(x[len - 1 - i] + "") - int.Parse(y[len - 1 - i] + "");

            n = drag ? n - 1 : n;
            drag = n < 0;
            n = n < 0 ? n + 10 : n;
            sub = n + sub;
        }

        return sub;
    }
}