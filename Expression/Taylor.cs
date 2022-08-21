using BigNum;

namespace Expression;

public class Taylor
{
    /// <summary>
    /// Expresion resultante
    /// </summary>
    public readonly ExpressionType ExpressionResult;

    /// <summary>
    /// Valor de la expresion resultante
    /// </summary>
    public readonly RealNumbers ValueResult;

    public Taylor(ExpressionType exp, List<(char, RealNumbers)> value, List<(char, RealNumbers)> center,
        int precision)
    {
        if (!Check(center, exp))
            throw new Exception(
                "No se ha introducido un valor para cada variable o se ha introducido una variable adicional");

        List<(char, ExpressionType)> centerExpression = new List<(char, ExpressionType)>();
        foreach (var item in center) centerExpression.Add((item.Item1, new NumberExpression(item.Item2)));

        this.ExpressionResult = TaylorSerial(exp, centerExpression, precision);
        this.ValueResult = this.ExpressionResult.Evaluate(value);
    }

    /// <summary>
    /// Calcular la serie de taylor
    /// </summary>
    /// <param name="exp">Expresion</param>
    /// <param name="center">Numero para centrar</param>
    /// <param name="precision">Precison</param>
    /// <returns></returns>
    private ExpressionType TaylorSerial(ExpressionType exp, List<(char, ExpressionType)> center, int precision)
    {
        ExpressionType taylorValue = exp.EvaluateExpression(center);
        RealNumbers fact = RealNumbers.Real1;
        RealNumbers index = RealNumbers.Real1;
        Queue<(ExpressionType, int[])> taylorFunction = new Queue<(ExpressionType, int[])>();
        taylorFunction.Enqueue((exp, new int[center.Count]));

        //Serie de taylor 
        //https://es.wikipedia.org/wiki/Serie_de_Taylor
        for (int i = 1; i < precision; i++)
        {
            fact *= index;

            taylorValue += Derivative(taylorFunction, center, i) / new NumberExpression(fact);

            index++;
        }

        return taylorValue;
    }

    /// <summary>
    /// Hallar la derivada n-esima
    /// </summary>
    /// <param name="taylorFunction">Derivadas (n-1)-esimas con sus respectivos indices</param>
    /// <param name="center">Centro para la funcion</param>
    /// <param name="ind"></param>
    /// <returns>Indice de la derivada</returns>
    private ExpressionType Derivative(Queue<(ExpressionType, int[])> taylorFunction,
        List<(char, ExpressionType)> center, int ind)
    {
        ExpressionType sum = new NumberExpression(RealNumbers.Real0);
        int len = (int) Math.Pow(center.Count, ind - 1);

        for (int i = 0; i < len; i++)
        {
            (ExpressionType aux, int[] auxInd) = taylorFunction.Peek();

            for (int j = 0; j < center.Count; j++)
            {
                ExpressionType derivative = aux.Derivative(center[j].Item1);
                int[] indices = auxInd.ToArray();
                indices[j]++;

                taylorFunction.Enqueue((derivative, indices));

                sum += derivative.EvaluateExpression(center) * Elevate(center, indices);
            }

            taylorFunction.Dequeue();
        }

        return ReduceExpression.Reduce(sum);
    }

    /// <summary>
    /// Elevar el binomio (x-a) de cada variable a sus respectivos indices
    /// </summary>
    /// <param name="variables">Lista de variables</param>
    /// <param name="indices">Indices para cada variables</param>
    /// <returns>Expresion resultante</returns>
    private ExpressionType Elevate(List<(char, ExpressionType)> variables, int[] indices)
    {
        ExpressionType product = new NumberExpression(RealNumbers.Real1);

        for (int i = 0; i < variables.Count; i++)
        {
            product *= new Pow(new VariableExpression(variables[i].Item1) - variables[i].Item2,
                new NumberExpression(BigNumMath.ConvertToIntegerNumbers(indices[i])));
        }

        return product;
    }

    /// <summary>
    /// Determinar si el centro para la expresion es correcto
    /// </summary>
    /// <param name="variables">Lista de varibles</param>
    /// <param name="exp">Expresion</param>
    /// <returns>Si el centro es correcto</returns>
    private bool Check(List<(char, RealNumbers)> variables, ExpressionType exp)
    {
        List<char> variablesExp = Aux.VariablesToExpression(exp);

        if (variablesExp.Count != variables.Count) return false;

        foreach (var item in variables)
        {
            if (!variablesExp.Contains(item.Item1)) return false;
        }

        return true;
    }
}