using BigNum;
using Expression;
using Expression.Expressions;
using Expression.Arithmetics;

UserInterface();

static void UserInterface()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Introduzca su expresion o s para salir");

        var expTxt = Console.ReadLine();

        if (expTxt == "s") break;
        if (expTxt is null) continue;

        var arithmetic = new ArithmeticExp<RealNumbers>(new BigNumExp(new BigNumMath(6, 9)));

        var exp = arithmetic.Parsing(expTxt);

        if (exp is null) Error("La expresion introducida no es correcta");
        else Options(exp, arithmetic);
    }
}

static void Options<T>(Function<T> exp, ArithmeticExp<T> arithmeticExp)
{
    var stop = false;

    while (!stop)
    {
        Console.Clear();
        Show(exp);
        Console.WriteLine(
            "Introduza d para derivar su expresion, e para evaluar la expresion, f para sustituir por otra expresion, t para hallar el polinomio de taylor o s para volver");

        var option = Console.ReadLine();
        switch (option)
        {
            case "s":
                stop = true;
                break;
            case "d":
                Derivative(exp, arithmeticExp);
                break;
            case "e":
                Evaluate(exp, arithmeticExp);
                break;
            case "f":
                EvaluateFunc(exp, arithmeticExp);
                break;
            case "t":
                Taylor(exp, arithmeticExp);
                break;
        }
    }
}

static void Error(string message = "Operacion invalida")
{
    Console.Clear();
    Console.WriteLine(message);
    ReturnWith();
}

static void ReturnWith()
{
    Console.WriteLine("Presiona cualquier tecla para volver");
    Console.ReadLine();
}

static void Show<T>(Function<T> exp, string message = "")
{
    Console.Clear();
    Console.WriteLine(exp);
    if (message != "") Console.WriteLine(message);
}

static void Derivative<T>(Function<T> exp, ArithmeticExp<T> arithmeticExp)
{
    Show(exp);
    Console.WriteLine("Introduzca la variable sobre la cual quiere derivar");

    var variable = Console.ReadLine();

    if (variable is null) return;
    if (variable.Length != 1)
    {
        Error();
        return;
    }

    var derivative = exp.Derivative(variable[0]);
    ExpressionResult(derivative.Reduce, arithmeticExp);
}

static void Evaluate<T>(Function<T> exp, ArithmeticExp<T> arithmeticExp)
{
    Show(exp);
    var evaluate = DeterminateVariables(exp, arithmeticExp.Arithmetic);

    Show(exp);
    Console.WriteLine($"= {exp.Evaluate(evaluate)}");
    ReturnWith();
}

static void EvaluateFunc<T>(Function<T> exp, ArithmeticExp<T> arithmeticExp)
{
    Show(exp);
    var evaluate = DeterminateVariablesFunc(exp, arithmeticExp);

    ExpressionResult(exp.EvaluateExpression(evaluate).Reduce, arithmeticExp);
}

static void Taylor<T>(Function<T> exp, ArithmeticExp<T> arithmeticExp)
{
    Show(exp);
    var evaluate = DeterminateVariables(exp, arithmeticExp.Arithmetic, "Introduzca los valores del centro");

    Show(exp);
    Console.WriteLine("Introduzca la cantidad de terminos");

    int cant;
    while (!int.TryParse(Console.ReadLine(), out cant)) Error();

    var taylor = new Taylor<T>(exp, evaluate, cant);
    ExpressionResult(taylor.ExpressionResult.Reduce, arithmeticExp);
}

static void ExpressionResult<T>(Function<T> exp, ArithmeticExp<T> arithmeticExp)
{
    Show(exp);
    Console.WriteLine("Presiona a para analizar esta expresion o cualquier tecla para volver");

    var s = Console.ReadLine();

    if (s == "a") Options(exp, arithmeticExp);
}

static List<(char, T)> DeterminateVariables<T>(Function<T> exp, IArithmetic<T> arithmetic, string message = "")
{
    if (message == "") message = "Ingrese los valores de las variables";
    var variables = exp.VariablesToExpression;
    var evaluate = new List<(char, T)>();

    foreach (var item in variables)
    {
        string? s;
        bool positive;

        while (true)
        {
            Show(exp, message);
            Console.Write($"{item} = ");

            s = Console.ReadLine();
            if (s is null || s == "")
            {
                Error();
                continue;
            }

            positive = s[0] != '-';

            if (RealNumbers.Check(positive ? s : s.Substring(1, s.Length - 1))) break;
            if (s == "pi" || s == "e") break;

            Error();
        }

        if (s == "pi")
        {
            evaluate.Add((item, arithmetic.PI));
            continue;
        }

        if (s == "e")
        {
            evaluate.Add((item, arithmetic.E));
            continue;
        }

        evaluate.Add((item, arithmetic.StringToNumber(s, positive)));
    }

    return evaluate;
}

static List<(char, Function<T>)> DeterminateVariablesFunc<T>(Function<T> exp,
    ArithmeticExp<T> arithmeticExp)
{
    var message = "Ingrese los valores de las variables";
    var variables = exp.VariablesToExpression;
    var evaluate = new List<(char, Function<T>)>();

    foreach (var item in variables)
    {
        Function<T>? expFunc;

        while (true)
        {
            Show(exp, message);
            Console.Write($"{item} = ");

            var s = Console.ReadLine();
            if (s is null || s == "")
            {
                Error();
                continue;
            }

            expFunc = arithmeticExp.Parsing(s);
            if (expFunc is not null) break;

            Error();
        }

        evaluate.Add((item, expFunc));
    }

    return evaluate;
}