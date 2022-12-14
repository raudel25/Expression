using BigNum;

namespace Expression;

public static class ConvertExpression
{
    private delegate Operators OperatorsArrayDelegate();

    private static int _maxPriority = 8;

    private static int _maxLenOperator = 6;

    /// <summary>
    /// Lista de operadores
    /// </summary>
    private static OperatorsArrayDelegate[] _operatorsArray = new OperatorsArrayDelegate[]
    {
        () => new Operators("+", 1, (exp) => new Sum(exp[0], exp[1]), true),
        () => new Operators("-", 1, (exp) => new Subtraction(exp[0], exp[1]), true),
        () => new Operators("*", 2, (exp) => new Multiply(exp[0], exp[1]), true),
        () => new Operators("/", 2, (exp) => new Division(exp[0], exp[1]), true),
        () => new Operators("^", 3, (exp) => Pow.DeterminatePow(exp[0], exp[1]), true),
        () => new Operators("log", 4, (exp) => Log.DeterminateLog(exp[0], exp[1]), true),
        () => new Operators("ln", 4, (exp) => new Ln(exp[0])),
        () => new Operators("sin", 5, (exp) => new Sin(exp[0])),
        () => new Operators("cos", 5, (exp) => new Cos(exp[0])),
        () => new Operators("tan", 5, (exp) => new Tan(exp[0])),
        () => new Operators("cot", 5, (exp) => new Cot(exp[0])),
        () => new Operators("sec", 5, (exp) => new Sec(exp[0])),
        () => new Operators("csc", 5, (exp) => new Csc(exp[0])),
        () => new Operators("arcsin", 5, (exp) => new Asin(exp[0])),
        () => new Operators("arccos", 5, (exp) => new Acos(exp[0])),
        () => new Operators("arctan", 5, (exp) => new Atan(exp[0])),
        () => new Operators("arccot", 5, (exp) => new Acot(exp[0]))
    };

    /// <summary>
    /// Convertir una cadena de texto en una expresion
    /// </summary>
    /// <param name="s">Cadena de texto</param>
    /// <returns>Expresion resultante(si devuelve null la expresion no es correcta)</returns>
    public static ExpressionType? Parsing(string s)
    {
        s = EliminateSpaces(s);
        List<Operators> operators = new List<Operators>();

        int cantParent = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '(') cantParent++;
            if (s[i] == ')') cantParent--;
            if (cantParent < 0) return null;

            //Determinar el operador
            for (int j = _maxLenOperator; j >= 1; j--)
            {
                if (j > s.Length - i) continue;

                Operators? op = DeterminateOperator(s.Substring(i, j));

                if (op != null)
                {
                    //Asignar el operador
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

        ExpressionType? exp = DeterminateExpression(s, 0, s.Length - 1, new bool[operators.Count], operators);
        return exp is null ? null : ReduceExpression.Reduce(exp);
    }

    /// <summary>
    /// Determinar la expresion dado el operador
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="start">Puntero inicial</param>
    /// <param name="end">Puntero final</param>
    /// <param name="visited">Operadores ya usados</param>
    /// <param name="operators">Lista de operadores</param>
    /// <returns>Expresion resultante(si devuelve null la expresion no es correcta)</returns>
    private static ExpressionType? DeterminateExpression(string s, int start, int end, bool[] visited,
        List<Operators> operators)
    {
        if (start > end) return null;

        //Determinar el operador a usar
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

        //Si no quedan operadores procedemos verificamos si la expresion es una variable o un numero
        if (index == -1) return VariableOrNumberOrFact(s.Substring(start, end - start + 1));

        if (operators[index].Binary) return ConvertBinary(s, start, end, visited, operators, index);

        return ConvertUnary(s, start, end, visited, operators, index);
    }

    /// <summary>
    /// Determinar el operador dado el simbolo
    /// </summary>
    /// <param name="s">Simbolo del operador</param>
    /// <returns>Operador</returns>
    private static Operators? DeterminateOperator(string s)
    {
        foreach (var item in _operatorsArray)
        {
            var aux = item();
            if (s == aux.Operator) return aux;
        }

        return null;
    }

    /// <summary>
    /// Eliminar espacios
    /// </summary>
    /// <param name="s">Cadena de texto</param>
    /// <returns>Cadena modificada</returns>
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

    /// <summary>
    /// Determinar si la expresion es una variable o un numero
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <returns>Expresion resultante</returns>
    private static ExpressionType? VariableOrNumberOrFact(string s)
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
        
        if (double.TryParse(aux, out _)) return new NumberExpression(new RealNumbers(aux));

        if (aux[^1] == '!')
        {
            if (int.TryParse(aux.Substring(0, aux.Length - 1), out int integer) && integer >= 0)
                return new Factorial(new IntegerNumbers(aux.Substring(0, aux.Length - 1)));
        }

        return null;
    }

    /// <summary>
    /// Determinar la expresion dado un operador binario
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="start">Puntero inicial</param>
    /// <param name="end">Puntero final</param>
    /// <param name="visited">Operadores ya usados</param>
    /// <param name="operators">Lista de operadores</param>
    /// <param name="index">Operador actual</param>
    /// <returns>Expresion resultante(si devuelve null la expresion no es correcta)</returns>
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

    /// <summary>
    /// Determinar la expresion dado un operador unario
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="start">Puntero inicial</param>
    /// <param name="end">Puntero final</param>
    /// <param name="visited">Operadores ya usados</param>
    /// <param name="operators">Lista de operadores</param>
    /// <param name="index">Operador actual</param>
    /// <returns>Expresion resultante(si devuelve null la expresion no es correcta)</returns>
    private static ExpressionType? ConvertUnary(string s, int start, int end, bool[] visited,
        List<Operators> operators, int index)
    {
        start = EliminateParentLeft(s, start, end);
        if (start == -1) return null;

        ExpressionType? value = DeterminateExpression(s, start + 1 + operators[index].Operator.Length, end - 1,
            visited, operators);

        return value is null ? null : operators[index].ExpressionOperator(new[] {value});
    }

    /// <summary>
    /// Dado un parentesis abierto, determinar el parentesis cerraddo correspondiente
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="ind">Indice del parentesis abierto</param>
    /// <returns>Indice del parentesis resultante</returns>
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

    /// <summary>
    /// Eliminar parentesis de la izqierda
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="start">Puntero inicial</param>
    /// <param name="end">Puntero final</param>
    /// <returns>Indice del puntero inicial resultante</returns>
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

    /// <summary>
    /// Eliminar parentesis de la derecha
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="start">Puntero inicial</param>
    /// <param name="end">Puntero final</param>
    /// <returns>Indice del puntero inicial resultante</returns>
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