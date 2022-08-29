using BigNum;
using Expression;

while (true)
{
    Console.Clear();
    Console.WriteLine("Introduzca su expresion o s para salir");

    string? expTxt = Console.ReadLine();

    if (expTxt == "s") break;
    if (expTxt is null) continue;

    ExpressionType? exp = ConvertExpression.Parsing(expTxt);

    if (exp is null) Console.WriteLine("La expresion introducida no es correcta");
    else Options(exp);
}

static void Options(ExpressionType exp)
{
    bool stop = false;

    while (!stop)
    {
        Console.Clear();
        Console.WriteLine(exp);
        Console.WriteLine(
            "Introduza d para derivar su expresion, e para evaluar la expresion, f para sustituir por otra expresion, t para hallar el polinomio de taylor o s para volver");

        string? option = Console.ReadLine();
        switch (option)
        {
            case "s":
                stop = true;
                break;
            case "d":
                Derivative(exp);
                break;
            case "e":
                Evaluate(exp);
                break;
            case "f":
                EvaluateFunc(exp);
                break;
            case "t":
                Taylor(exp);
                break;
        }
    }
}

static void Error()
{
    Console.WriteLine("Operacion invalida");
    Thread.Sleep(500);
}

static void Show(ExpressionType exp)
{
    Console.Clear();
    Console.WriteLine(exp);
}

static void Derivative(ExpressionType exp)
{
    Show(exp);
    Console.WriteLine("Introduzca la variable sobre la cual quiere derivar");

    string? variable = Console.ReadLine();

    if (variable is null) return;
    if (variable.Length != 1)
    {
        Error();
        return;
    }

    ExpressionType derivative = exp.Derivative(variable[0]);
    ExpressionResult(ReduceExpression.Reduce(derivative));
}

static void Evaluate(ExpressionType exp)
{
    Show(exp);
    List<(char, RealNumbers)> evaluate = DeterminateVariables(exp);

    Show(exp);
    Console.WriteLine($"= {exp.Evaluate(evaluate)}");
    Console.WriteLine("Presiona cualquier tecla para volver");
    Console.ReadLine();
}

static void EvaluateFunc(ExpressionType exp)
{
    Show(exp);
    List<(char, ExpressionType)> evaluate = DeterminateVariablesFunc(exp);

    ExpressionResult(ReduceExpression.Reduce(exp.EvaluateExpression(evaluate)));
}

static void Taylor(ExpressionType exp)
{
    Show(exp);
    Console.WriteLine("Introduzca el centro");
    List<(char, RealNumbers)> evaluate = DeterminateVariables(exp);

    Show(exp);
    Console.WriteLine("Introduzca la cantidad de terminos");

    int cant;
    while (!int.TryParse(Console.ReadLine(), out cant)) Error();

    Taylor taylor = new Taylor(exp, evaluate, cant);
    ExpressionResult(ReduceExpression.Reduce(taylor.ExpressionResult));
}

static void ExpressionResult(ExpressionType exp)
{
    Show(exp);
    Console.WriteLine("Presiona a para analizar esta expresion o cualquier tecla para volver");

    string? s = Console.ReadLine();

    if (s == "a") Options(exp);
}

static List<(char, RealNumbers)> DeterminateVariables(ExpressionType exp)
{
    Console.WriteLine("Ingrese los valores de las variables");
    List<char> variables = Aux.VariablesToExpression(exp);
    List<(char, RealNumbers)> evaluate = new List<(char, RealNumbers)>();

    foreach (var item in variables)
    {
        string? s;
        bool positive;

        while (true)
        {
            Show(exp);
            Console.Write($"{item} = ");

            s = Console.ReadLine();
            if (s is null || s == "")
            {
                Error();
                continue;
            }

            positive = s[0] != '-';

            if (RealNumbers.Check(positive ? s : s.Substring(1, s.Length - 1))) break;

            Error();
        }

        evaluate.Add((item, new RealNumbers(s, positive)));
    }

    return evaluate;
}

static List<(char, ExpressionType)> DeterminateVariablesFunc(ExpressionType exp)
{
    Console.WriteLine("Ingrese los valores de las variables");
    List<char> variables = Aux.VariablesToExpression(exp);
    List<(char, ExpressionType)> evaluate = new List<(char, ExpressionType)>();

    foreach (var item in variables)
    {
        string? s;
        ExpressionType? expFunc;

        while (true)
        {
            Show(exp);
            Console.Write($"{item} = ");

            s = Console.ReadLine();
            if (s is null || s == "")
            {
                Error();
                continue;
            }

            expFunc = ConvertExpression.Parsing(s);
            if (expFunc is not null) break;

            Error();
        }

        evaluate.Add((item, expFunc));
    }

    return evaluate;
}