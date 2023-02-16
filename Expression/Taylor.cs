namespace Expression;

public class Taylor<T>
{
    /// <summary>
    ///     Expresion resultante
    /// </summary>
    public readonly ExpressionType<T> ExpressionResult;

    public Taylor(ExpressionType<T> exp, List<(char, T)> center,
        int precision)
    {
        if (!Check(center, exp))
            throw new Exception(
                "No se ha introducido un valor para cada variable o se ha introducido una variable adicional");

        var centerExpression = new List<(char, ExpressionType<T>)>();
        foreach (var item in center)
            centerExpression.Add((item.Item1, new NumberExpression<T>(item.Item2, exp.Arithmetic)));

        ExpressionResult = TaylorSerial(exp, centerExpression, precision);
    }

    /// <summary>
    ///     Calcular la serie de taylor
    /// </summary>
    /// <param name="exp">Expresion</param>
    /// <param name="center">Numero para centrar</param>
    /// <param name="precision">Precison</param>
    /// <returns></returns>
    private ExpressionType<T> TaylorSerial(ExpressionType<T> exp, List<(char, ExpressionType<T>)> center, int precision)
    {
        var taylorValue = exp.EvaluateExpression(center);
        var fact = exp.Arithmetic.Real1;
        var index = exp.Arithmetic.Real1;
        var taylorFunction = new Queue<(ExpressionType<T>, int[])>();
        taylorFunction.Enqueue((exp, new int[center.Count]));

        //Serie de taylor 
        //https://es.wikipedia.org/wiki/Serie_de_Taylor
        for (var i = 1; i < precision; i++)
        {
            fact = exp.Arithmetic.Multiply(fact, index);
            var expFact = new Factorial<T>(index, exp.Arithmetic)
            {
                Value = fact
            };

            taylorValue += Derivative(taylorFunction, center, i, exp.Arithmetic) / expFact;

            index = exp.Arithmetic.Sum(index, exp.Arithmetic.Real1);
        }

        return taylorValue;
    }

    /// <summary>
    ///     Hallar la derivada n-esima
    /// </summary>
    /// <param name="taylorFunction">Derivadas (n-1)-esimas con sus respectivos indices</param>
    /// <param name="center">Centro para la funcion</param>
    /// <param name="ind"></param>
    /// <param name="arithmetic">Aritmetica</param>
    /// <returns>Indice de la derivada</returns>
    private ExpressionType<T> Derivative(Queue<(ExpressionType<T>, int[])> taylorFunction,
        List<(char, ExpressionType<T>)> center, int ind, IArithmetic<T> arithmetic)
    {
        ExpressionType<T> sum = new NumberExpression<T>(arithmetic.Real0, arithmetic);
        var len = (int)Math.Pow(center.Count, ind - 1);

        for (var i = 0; i < len; i++)
        {
            var (aux, auxInd) = taylorFunction.Peek();

            for (var j = 0; j < center.Count; j++)
            {
                var derivative = aux.Derivative(center[j].Item1);
                var indices = auxInd.ToArray();
                indices[j]++;

                taylorFunction.Enqueue((derivative, indices));

                sum += derivative.EvaluateExpression(center) * Elevate(center, indices, arithmetic);
            }

            taylorFunction.Dequeue();
        }

        return ReduceExpression<T>.Reduce(sum);
    }

    /// <summary>
    ///     Elevar el binomio (x-a) de cada variable a sus respectivos indices
    /// </summary>
    /// <param name="variables">Lista de variables</param>
    /// <param name="indices">Indices para cada variables</param>
    /// <param name="arithmetic">Aritmetica</param>
    /// <returns>Expresion resultante</returns>
    private ExpressionType<T> Elevate(List<(char, ExpressionType<T>)> variables, int[] indices,
        IArithmetic<T> arithmetic)
    {
        ExpressionType<T> product = new NumberExpression<T>(arithmetic.Real1, arithmetic);

        for (var i = 0; i < variables.Count; i++)
            product *= new Pow<T>(new VariableExpression<T>(variables[i].Item1, arithmetic) - variables[i].Item2,
                new NumberExpression<T>(arithmetic.StringToNumber(indices[i].ToString()), arithmetic));

        return product;
    }

    /// <summary>
    ///     Determinar si el centro para la expresion es correcto
    /// </summary>
    /// <param name="variables">Lista de varibles</param>
    /// <param name="exp">Expresion</param>
    /// <returns>Si el centro es correcto</returns>
    private bool Check(List<(char, T)> variables, ExpressionType<T> exp)
    {
        var variablesExp = Aux<T>.VariablesToExpression(exp);

        if (variablesExp.Count != variables.Count) return false;

        foreach (var item in variables)
            if (!variablesExp.Contains(item.Item1))
                return false;

        return true;
    }
}