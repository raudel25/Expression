using BigNum;

namespace Expression;

public static class ConvertExpression
{
    private delegate Operators OperatorsArrayDelegate();

    private static int _maxPriority = 10;

    private static int _maxLenOperator = 6;

    private static OperatorsArrayDelegate[] OperatorsArray = new OperatorsArrayDelegate[]
    {
        () => new Operators("+", 1, (exp) => new Sum(exp[0], exp[1]), true),
        () => new Operators("-", 1, (exp) => new Subtraction(exp[0], exp[1]), true),
        () => new Operators("*", 2, (exp) => new Multiply(exp[0], exp[1]), true),
        () => new Operators("/", 3, (exp) => new Division(exp[0], exp[1]), true),
        () => new Operators("^", 4, (exp) => Pow.DeterminatePow(exp[0], exp[1]), true),
        () => new Operators("log", 5, (exp) => Log.DeterminateLog(exp[0], exp[1]), true),
        () => new Operators("ln", 5, (exp) => new Ln(exp[0])),
        () => new Operators("sin", 6, (exp) => new Sin(exp[0])),
        () => new Operators("cos", 6, (exp) => new Cos(exp[0])),
        () => new Operators("tan", 6, (exp) => new Tan(exp[0])),
        () => new Operators("cot", 6, (exp) => new Cot(exp[0])),
        () => new Operators("sec", 6, (exp) => new Sec(exp[0])),
        () => new Operators("csc", 6, (exp) => new Csc(exp[0])),
        () => new Operators("arcsin", 6, (exp) => new Asin(exp[0])),
        () => new Operators("arccos", 6, (exp) => new Acos(exp[0])),
        () => new Operators("arctan", 6, (exp) => new Atan(exp[0])),
        () => new Operators("arccot", 6, (exp) => new Acot(exp[0]))
    };

    public static ExpressionType? ConvertExpressions(string s)
    {
        s = EliminateSpaces(s);
        List<Operators> operators = new List<Operators>();

        int cantParent = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '(') cantParent++;
            if (s[i] == ')') cantParent--;
            if (cantParent < 0) return null;

            for (int j = _maxLenOperator; j >= 1; j--)
            {
                if (j > s.Length - i) continue;

                Operators? op = DeterminateOperator(s.Substring(i, j));

                if (op != null)
                {
                    op.AssignPriority = op.DefaultPriority + _maxPriority * cantParent;
                    op.Position = i;
                    operators.Add(op);
                    i += j - 1;
                    break;
                }
            }
        }

        if (cantParent != 0) return null;

        operators.Reverse();
        operators.Sort((o1, o2) => o1.AssignPriority.CompareTo(o2.AssignPriority));

        return DeterminateExpression(s, 0, s.Length - 1, new bool[operators.Count], operators);
    }

    private static ExpressionType? DeterminateExpression(string s, int start, int end, bool[] visited,
        List<Operators> operators)
    {
        if (start > end) return null;

        int index = -1;
        for (int i = 0; i < visited.Length; i++)
        {
            if (!visited[i] && operators[i].Position >= start && operators[i].Position <= end)
            {
                index = i;
                visited[i] = true;
                break;
            }
        }

        if (index == -1) return VariableOrNumber(s.Substring(start, end - start + 1));

        if (operators[index].Binary) return ConvertBinary(s, start, end, visited, operators, index);

        return ConvertUnary(s, start, end, visited, operators, index);
    }

    private static Operators? DeterminateOperator(string s)
    {
        foreach (var item in OperatorsArray)
        {
            var aux = item();
            if (s == aux.Operator) return aux;
        }

        return null;
    }

    private static string EliminateSpaces(string s)
    {
        string result = "";

        foreach (var item in s)
        {
            if (item == ' ') continue;
            result += item;
        }

        return result;
    }

    private static ExpressionType? VariableOrNumber(string s)
    {
        (int start, int end) = (EliminateParentLeft(s, 0, s.Length - 1), EliminateParentRight(s, 0, s.Length - 1));
        if (start == -1 || end == -1) return null;

        string aux = s.Substring(start, end - start + 1);

        if (aux == "e") return new ConstantE();
        if (aux == "pi") return new ConstantPI();

        if (aux.Length == 1)
        {
            if (char.IsLetter(aux[0])) return new VariableExpression(aux[0]);
        }

        double number = 0;
        if (double.TryParse(aux, out number)) return new NumberExpression(new RealNumbers(aux));

        return null;
    }

    private static ExpressionType? ConvertBinary(string s, int start, int end, bool[] visited,
        List<Operators> operators, int index)
    {
        ExpressionType? left;
        ExpressionType? right;

        if (operators[index].Operator == "-")
        {
            if (operators[index].Position == start)
            {
                right = DeterminateExpression(s, operators[index].Position + 1, end, visited, operators);
                left = new NumberExpression(RealNumbers.Real0);

                return right is null ? null : operators[index].ExpressionOperator(new[] {left, right});
            }

            if (s[operators[index].Position - 1] == '(')
            {
                right = DeterminateExpression(s, operators[index].Position + 1, end, visited, operators);
                left = new NumberExpression(RealNumbers.Real0);

                return right is null ? null : operators[index].ExpressionOperator(new[] {left, right});
            }
        }

        if (operators[index].Operator == "log")
        {
            start = EliminateParentLeft(s, start, end);
            int ind = DeterminateEndParent(s, start + operators[index].Operator.Length);

            left = DeterminateExpression(s, start + operators[index].Operator.Length + 1, ind - 1, visited, operators);
            right = DeterminateExpression(s, ind + 2, end - 1, visited, operators);

            return left is null || right is null ? null : operators[index].ExpressionOperator(new[] {left, right});
        }

        left = DeterminateExpression(s, start, operators[index].Position - 1, visited, operators);
        right = DeterminateExpression(s, operators[index].Position + 1, end, visited, operators);

        return left is null || right is null ? null : operators[index].ExpressionOperator(new[] {left, right});
    }

    private static ExpressionType? ConvertUnary(string s, int start, int end, bool[] visited,
        List<Operators> operators, int index)
    {
        start = EliminateParentLeft(s, start, end);
        if (start == -1) return null;

        ExpressionType? value = DeterminateExpression(s, start + 1 + operators[index].Operator.Length, end - 1,
            visited, operators);

        return value is null ? null : operators[index].ExpressionOperator(new[] {value});
    }

    private static int DeterminateEndParent(string s, int ind)
    {
        int cantParent = 0;
        for (int i = ind; i < s.Length; i++)
        {
            if (s[i] == '(') cantParent++;
            if (s[i] == ')') cantParent--;
            if (cantParent == 0) return i;
        }

        return -1;
    }

    private static int EliminateParentLeft(string s, int start, int end)
    {
        int i = start;
        while (s[i] == '(')
        {
            i++;
            if (i == end + 1) return -1;
        }

        return i;
    }

    private static int EliminateParentRight(string s, int start, int end)
    {
        int j = end;

        while (s[j] == ')')
        {
            j--;
            if (j == start - 1) return -1;
        }

        return j;
    }
}