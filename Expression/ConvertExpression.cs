using System.Text;
using Expression.Reduce;
using Expression.Expressions;
using Expression.Arithmetics;

namespace Expression;

internal static class ConvertExpression<T>
{
    private const int MaxPriority = 8;

    private const int MaxLenOperator = 6;

    /// <summary>
    ///     Lista de operadores
    /// </summary>
    private static readonly OperatorsArrayDelegate[] OperatorsArray =
    {
        () => new Operators<T>("+", 1, exp => new Sum<T>(exp[0], exp[1]), true),
        () => new Operators<T>("-", 1, exp => new Subtraction<T>(exp[0], exp[1]), true),
        () => new Operators<T>("*", 2, exp => new Multiply<T>(exp[0], exp[1]), true),
        () => new Operators<T>("/", 2, exp => new Division<T>(exp[0], exp[1]), true),
        () => new Operators<T>("^", 3, exp => Pow<T>.DeterminatePow(exp[0], exp[1]), true),
        () => new Operators<T>("sqrt", 3, exp => new Sqrt<T>(exp[0], (NumberExpression<T>)exp[1]), true),
        () => new Operators<T>("log", 4, exp => Log<T>.DeterminateLog(exp[0], exp[1]), true),
        () => new Operators<T>("ln", 4, exp => new Ln<T>(exp[0])),
        () => new Operators<T>("sin", 5, exp => new Sin<T>(exp[0])),
        () => new Operators<T>("cos", 5, exp => new Cos<T>(exp[0])),
        () => new Operators<T>("tan", 5, exp => new Tan<T>(exp[0])),
        () => new Operators<T>("cot", 5, exp => new Cot<T>(exp[0])),
        () => new Operators<T>("sec", 5, exp => new Sec<T>(exp[0])),
        () => new Operators<T>("csc", 5, exp => new Csc<T>(exp[0])),
        () => new Operators<T>("arcsin", 5, exp => new Asin<T>(exp[0])),
        () => new Operators<T>("arccos", 5, exp => new Acos<T>(exp[0])),
        () => new Operators<T>("arctan", 5, exp => new Atan<T>(exp[0])),
        () => new Operators<T>("arccot", 5, exp => new Acot<T>(exp[0]))
    };

    /// <summary>
    ///     Convertir una cadena de texto en una expresion
    /// </summary>
    /// <param name="s">Cadena de texto</param>
    /// <param name="arithmetic">Aritmetica</param>
    /// <returns>Expresion resultante(si devuelve null la expresion no es correcta)</returns>
    internal static Function<T>? Parsing(string s, IArithmetic<T> arithmetic)
    {
        if (s == "") return null;
        s = FormatStringExp(s);
        var operators = new List<Operators<T>>();

        var cantParent = 0;
        for (var i = 0; i < s.Length; i++)
        {
            if (s[i] == '(' || s[i] == '[') cantParent++;
            if (s[i] == ')' || s[i] == ']') cantParent--;
            if (cantParent < 0) return null;

            //Determinar el operador
            for (var j = MaxLenOperator; j >= 1; j--)
            {
                if (j > s.Length - i) continue;

                var op = DeterminateOperator(s.Substring(i, j));

                if (op == null) continue;
                //Asignar el operador
                op.AssignPriority = op.DefaultPriority + MaxPriority * cantParent;
                op.Position = i;
                operators.Add(op);
                i += j - 1;
                break;
            }
        }

        if (cantParent != 0) return null;

        operators.Reverse();
        operators.Sort((o1, o2) => o1.AssignPriority.CompareTo(o2.AssignPriority));

        var exp =
            DeterminateExpression(s, 0, s.Length - 1, new bool[operators.Count], operators, arithmetic);
        return exp is null ? null : ReduceExpression<T>.Reduce(exp);
    }

    /// <summary>
    ///     Eliminar espacios
    /// </summary>
    /// <param name="s">String</param>
    /// <returns>String Formateado</returns>
    private static string EliminateSpaces(string s)
    {
        var exp = new StringBuilder();

        foreach (var i in s.Where(i => i != ' '))
        {
            exp.Append(i);
        }

        return exp.ToString();
    }

    /// <summary>
    ///     Formatear la expresion
    /// </summary>
    /// <param name="s">Expresion</param>
    /// <returns>Expresion formateada</returns>
    private static string FormatStringExp(string s)
    {
        s = EliminateSpaces(s);
        var exp = new StringBuilder();

        for (var i = 0; i < s.Length - 1; i++)
        {
            var findOp = false;
            for (var j = MaxLenOperator; j >= 1; j--)
            {
                if (j > s.Length - i) continue;

                var op = DeterminateOperator(s.Substring(i, j));

                if (op == null) continue;
                findOp = true;
                exp.Append(s.AsSpan(i, j));
                i += j - 1;
                break;
            }

            if (findOp) continue;

            exp.Append(s[i]);
            if (s[i] == 'p' && s[i + 1] == 'i') continue;

            if (AddMult(s, i))
                exp.Append('*');
        }

        exp.Append(s[^1]);
        return exp.ToString();
    }

    private static bool AddMult(string s, int ind)
    {
        var noLog = s[ind] != '[' && s[ind] != ']' && s[ind + 1] != ']';
        var noPoint = s[ind] != '.';
        var noParents = s[ind + 1] != ')' && s[ind] != '(';
        var noBinary = s[ind + 1] != '+' && s[ind + 1] != '-' && s[ind + 1] != '*' && s[ind + 1] != '/' &&
                       s[ind + 1] != '^';
        var noNumber = !(char.IsDigit(s[ind]) && (char.IsDigit(s[ind + 1]) || s[ind + 1] == '.'));
        return noPoint && noNumber && noParents && noBinary && noLog;
    }

    /// <summary>
    ///     Determinar la expresion dado el operador
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="start">Puntero inicial</param>
    /// <param name="end">Puntero final</param>
    /// <param name="visited">Operadores ya usados</param>
    /// <param name="operators">Lista de operadores</param>
    /// <param name="arithmetic">Aritmetica</param>
    /// <returns>Expresion resultante(si devuelve null la expresion no es correcta)</returns>
    private static Function<T>? DeterminateExpression(string s, int start, int end, bool[] visited,
        List<Operators<T>> operators, IArithmetic<T> arithmetic)
    {
        if (start > end) return null;

        //Determinar el operador a usar
        var index = -1;
        for (var i = 0; i < visited.Length; i++)
            if (!visited[i] && operators[i].Position >= start && operators[i].Position <= end)
            {
                index = i;
                visited[i] = true;
                break;
            }

        //Si no quedan operadores procedemos verificamos si la expresion es una variable o un numero
        if (index == -1) return VariableOrNumberOrFact(s.Substring(start, end - start + 1), arithmetic);

        if (operators[index].Binary) return ConvertBinary(s, start, end, visited, operators, index, arithmetic);

        return ConvertUnary(s, start, end, visited, operators, index, arithmetic);
    }

    /// <summary>
    ///     Determinar el operador dado el simbolo
    /// </summary>
    /// <param name="s">Simbolo del operador</param>
    /// <returns>Operador</returns>
    private static Operators<T>? DeterminateOperator(string s)
    {
        foreach (var item in OperatorsArray)
        {
            var aux = item();
            if (s == aux.Operator) return aux;
        }

        return null;
    }


    /// <summary>
    ///     Determinar si la expresion es una variable o un numero
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="arithmetic">Aritmetica</param>
    /// <returns>Expresion resultante</returns>
    private static Function<T>? VariableOrNumberOrFact(string s, IArithmetic<T> arithmetic)
    {
        var (start, end) = (EliminateParentLeft(s, 0, s.Length - 1), EliminateParentRight(s, 0, s.Length - 1));
        if (start == -1 || end == -1) return null;

        var aux = s.Substring(start, end - start + 1);

        if (aux == "e") return new ConstantE<T>(arithmetic);
        if (aux == "pi") return new ConstantPI<T>(arithmetic);

        if (aux.Length == 1)
            if (char.IsLetter(aux[0]))
                return new VariableExpression<T>(aux[0], arithmetic);

        if (double.TryParse(aux, out _)) return new NumberExpression<T>(arithmetic.StringToNumber(aux), arithmetic);

        if (aux[^1] != '!') return null;
        if (int.TryParse(aux.AsSpan(0, aux.Length - 1), out var integer) && integer >= 0)
            return new Factorial<T>(arithmetic.StringToNumber(aux.Substring(0, aux.Length - 1)), arithmetic);

        return null;
    }

    /// <summary>
    ///     Determinar la expresion dado un operador binario
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="start">Puntero inicial</param>
    /// <param name="end">Puntero final</param>
    /// <param name="visited">Operadores ya usados</param>
    /// <param name="operators">Lista de operadores</param>
    /// <param name="index">Operador actual</param>
    /// <param name="arithmetic">Aritmetica</param>
    /// <returns>Expresion resultante(si devuelve null la expresion no es correcta)</returns>
    private static Function<T>? ConvertBinary(string s, int start, int end, bool[] visited,
        List<Operators<T>> operators, int index, IArithmetic<T> arithmetic)
    {
        Function<T>? left;
        Function<T>? right;

        if (operators[index].Operator == "-")
        {
            if (operators[index].Position == start)
            {
                right = DeterminateExpression(s, operators[index].Position + 1, end, visited, operators, arithmetic);
                left = new NumberExpression<T>(arithmetic.Real0, arithmetic);

                return right is null ? null : operators[index].ExpressionOperator(new[] { left, right });
            }

            if (s[operators[index].Position - 1] == '(')
            {
                right = DeterminateExpression(s, operators[index].Position + 1, end, visited, operators, arithmetic);
                left = new NumberExpression<T>(arithmetic.Real0, arithmetic);

                return right is null ? null : operators[index].ExpressionOperator(new[] { left, right });
            }
        }

        if (operators[index].Operator == "log")
        {
            start = EliminateParentLeft(s, start, end);
            var ind = DeterminateEndParent(s, start + operators[index].Operator.Length);

            left = DeterminateExpression(s, start + operators[index].Operator.Length + 1, ind - 1, visited, operators,
                arithmetic);
            right = DeterminateExpression(s, ind + 2, end - 1, visited, operators, arithmetic);

            return left is null || right is null ? null : operators[index].ExpressionOperator(new[] { left, right });
        }

        if (operators[index].Operator == "sqrt")
        {
            start = EliminateParentLeft(s, start, end);

            var ind = DeterminateEndParent(s, start + operators[index].Operator.Length);

            var indSqrt = ind == start + operators[index].Operator.Length
                ? "2"
                : s.Substring(start + operators[index].Operator.Length + 1,
                    ind - (start + operators[index].Operator.Length + 1));
            if (ind == start + operators[index].Operator.Length) ind--;

            var validInd = int.TryParse(indSqrt, out var indSqrtInt);
            if (indSqrtInt <= 0) validInd = false;
            right = validInd ? new NumberExpression<T>(arithmetic.StringToNumber(indSqrt), arithmetic) : null;

            left = DeterminateExpression(s, ind + 2, end - 1, visited, operators, arithmetic);

            return left is null || right is null ? null : operators[index].ExpressionOperator(new[] { left, right });
        }

        left = DeterminateExpression(s, start, operators[index].Position - 1, visited, operators, arithmetic);
        right = DeterminateExpression(s, operators[index].Position + 1, end, visited, operators, arithmetic);

        return left is null || right is null ? null : operators[index].ExpressionOperator(new[] { left, right });
    }

    /// <summary>
    ///     Determinar la expresion dado un operador unario
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="start">Puntero inicial</param>
    /// <param name="end">Puntero final</param>
    /// <param name="visited">Operadores ya usados</param>
    /// <param name="operators">Lista de operadores</param>
    /// <param name="index">Operador actual</param>
    /// <param name="arithmetic">Aritmetica</param>
    /// <returns>Expresion resultante(si devuelve null la expresion no es correcta)</returns>
    private static Function<T>? ConvertUnary(string s, int start, int end, bool[] visited,
        List<Operators<T>> operators, int index, IArithmetic<T> arithmetic)
    {
        start = EliminateParentLeft(s, start, end);
        if (start == -1) return null;

        var value = DeterminateExpression(s, start + 1 + operators[index].Operator.Length, end - 1,
            visited, operators, arithmetic);

        return value is null ? null : operators[index].ExpressionOperator(new[] { value });
    }

    /// <summary>
    ///     Dado un parentesis abierto, determinar el parentesis cerraddo correspondiente
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="ind">Indice del parentesis abierto</param>
    /// <returns>Indice del parentesis resultante</returns>
    private static int DeterminateEndParent(string s, int ind)
    {
        var cantParent = 0;
        for (var i = ind; i < s.Length; i++)
        {
            if (s[i] == '[') cantParent++;
            if (s[i] == ']') cantParent--;
            if (cantParent == 0) return i;
        }

        return -1;
    }

    /// <summary>
    ///     Eliminar parentesis de la izqierda
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="start">Puntero inicial</param>
    /// <param name="end">Puntero final</param>
    /// <returns>Indice del puntero inicial resultante</returns>
    private static int EliminateParentLeft(string s, int start, int end)
    {
        var i = start;
        while (s[i] == '(')
        {
            i++;
            if (i == end + 1) return -1;
        }

        return i;
    }

    /// <summary>
    ///     Eliminar parentesis de la derecha
    /// </summary>
    /// <param name="s">Cadena</param>
    /// <param name="start">Puntero inicial</param>
    /// <param name="end">Puntero final</param>
    /// <returns>Indice del puntero inicial resultante</returns>
    private static int EliminateParentRight(string s, int start, int end)
    {
        var j = end;

        while (s[j] == ')')
        {
            j--;
            if (j == start - 1) return -1;
        }

        return j;
    }

    private delegate Operators<T> OperatorsArrayDelegate();
}