namespace Expression;

public static class ConvertExpression
{
    private static int _maxPriority =7;
    
    private static Operators[] OperatorsArray = new[]
    {
        new Operators("+", 1, (exp) => new Sum(exp[0], exp[1]),true),
        new Operators("-", 1, (exp) => new Subtraction(exp[0], exp[1]),true),
        new Operators("*", 2, (exp) => new Multiply(exp[0], exp[1]),true),
        new Operators("/", 3, (exp) => new Division(exp[0], exp[1]),true),
        new Operators("e^", 4, (exp) => new PowE(exp[0])),
        new Operators("^", 4, (exp) => new Pow(exp[0], exp[1]),true),
        new Operators("log", 5, (exp) => new Log(exp[0], exp[1]),true),
        new Operators("ln", 5, (exp) => new Ln(exp[0])),
        new Operators("sin", 6, (exp) => new Sin(exp[0])),
        new Operators("cos", 6, (exp) => new Cos(exp[0])),
        new Operators("tan", 6, (exp) => new Tan(exp[0])),
        new Operators("cot", 6, (exp) => new Cot(exp[0])),
        new Operators("sec", 6, (exp) => new Sec(exp[0])),
        new Operators("csc", 6, (exp) => new Csc(exp[0])),
    };

    public static void ConvertExpressions(string s)
    {
        s = EliminateSpaces(s);
        List<Operators> operators = new List<Operators>();

        int cantParent = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '(') cantParent++;
            if (s[i] == ')') cantParent--;
            
            Console.WriteLine(i);

            for (int j = 3; j >= 1; j--)
            {
                if(j>s.Length-i) continue;

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

        operators.Sort((o1,o2)=>o1.AssignPriority.CompareTo(o1.AssignPriority));
        foreach (var VARIABLE in operators)
        {
            Console.WriteLine(VARIABLE.Operator+" "+VARIABLE.AssignPriority);
        }
    }

    private static ExpressionType DeterminateExpression(string s, int start, int end,bool[] visited,
        List<Operators> operators)
    {
        int index = -1;
        for (int i = 0; i < visited.Length; i++)
        {
            if (!visited[i])
            {
                index = i;
                visited[i] = true;
                break;
            }
        }
        
        if (operators[index].Binary)
        {
            ExpressionType left = DeterminateExpression(s, start, operators[index].Position - 1, visited, operators);
            ExpressionType right = DeterminateExpression(s, operators[index].Position + 1,start , visited, operators);

            return operators[index].ExpressionOperator(new[] {left, right});
        }

        return null;

    }
    
    private static Operators? DeterminateOperator(string s)
    {
        foreach (var item in OperatorsArray)
        {
            if (s == item.Operator) return item;
        }

        return null;
    }

    private static string EliminateSpaces(string s)
    {
        string result = "";

        foreach (var item in s)
        {
            if(item==' ') continue;
            result += item;
        }

        return  result;
    }
}