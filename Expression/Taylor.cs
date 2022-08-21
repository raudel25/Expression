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

    public Taylor(ExpressionType exp, List<(char,RealNumbers)> value, List<(char,RealNumbers)> center, int precision = 20)
    {
        this.ExpressionResult = TaylorSerial(exp, center, precision);
        this.ValueResult = this.ExpressionResult.Evaluate(value);
    }

    /// <summary>
    /// Calcular la serie de taylor
    /// </summary>
    /// <param name="exp">Expresion</param>
    /// <param name="center">Numero para centrar</param>
    /// <param name="precision">Precison</param>
    /// <returns></returns>
    private ExpressionType TaylorSerial(ExpressionType exp, List<(char,RealNumbers)> center, int precision)
    {
        ExpressionType taylorValue = new NumberExpression(exp.Evaluate(center));
        RealNumbers fact = RealNumbers.Real1;
        RealNumbers index = RealNumbers.Real1;
        Queue<(ExpressionType,int[])> taylorFunction = new Queue<(ExpressionType,int[])>();
        taylorFunction.Enqueue((exp,new int[center.Count]));

        //Serie de taylor 
        //https://es.wikipedia.org/wiki/Serie_de_Taylor
        for (int i = 1; i < precision; i++)
        {
            fact *= index;
            
            Console.WriteLine(Derivative(taylorFunction,center,i));
            // var a = new NumberExpression(taylorFunction.Evaluate(center));
            // var b = new Pow(new VariableExpression('x') - new NumberExpression(center),
            //     new NumberExpression(index));
            // var c = new NumberExpression(fact);
            //
            // taylorValue += a * b / c;
            //
            // index++;
        }

        return taylorValue;
    }

    private ExpressionType Derivative(Queue<(ExpressionType,int[])> taylorFunction,List<(char,RealNumbers)> center,int ind)
    {
        ExpressionType sum = new NumberExpression(RealNumbers.Real0);
        int len = (int)Math.Pow(center.Count, ind - 1);

        for (int i = 0; i < len; i++)
        {
            (ExpressionType aux, int[] auxInd) = taylorFunction.Peek();

            for (int j = 0; j < center.Count; j++)
            {
                ExpressionType derivative = aux.Derivative(center[j].Item1);
                int[] indices = auxInd.ToArray();
                indices[j]++;

                taylorFunction.Enqueue((derivative,indices));

                sum += derivative*Elevate(center,indices);
            }

            taylorFunction.Dequeue();
        }

        return ReduceExpression.Reduce(sum);
    }

    private ExpressionType Elevate(List<(char, RealNumbers)> variables, int[] indices)
    {
        ExpressionType product = new NumberExpression(RealNumbers.Real1);

        for (int i=0;i<variables.Count;i++)
        {
            product *= new Pow(new VariableExpression(variables[i].Item1) - new NumberExpression(variables[i].Item2),
                new NumberExpression(BigNumMath.ConvertToIntegerNumbers(indices[i])));
        }

        return product;
    }
}