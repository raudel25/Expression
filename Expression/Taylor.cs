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

    public Taylor(ExpressionType exp, RealNumbers value, RealNumbers center, int precision = 20)
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
    private ExpressionType TaylorSerial(ExpressionType exp, RealNumbers center, int precision)
    {
        ExpressionType taylorValue = new NumberExpression(exp.Evaluate(center));
        ExpressionType taylorFunction = exp;
        RealNumbers fact = RealNumbers.Real1;
        RealNumbers index = RealNumbers.Real1;

        //Serie de taylor 
        //https://es.wikipedia.org/wiki/Serie_de_Taylor
        for (int i = 1; i < precision; i++)
        {
            fact *= index;
            taylorFunction = taylorFunction.Derivative();
            
            var a = new NumberExpression(taylorFunction.Evaluate(center));
            var b = new Pow(new VariableExpression('x') - new NumberExpression(center),
                new NumberExpression(index));
            var c = new NumberExpression(fact);

            taylorValue += a * b / c;

            index++;
        }

        return taylorValue;
    }
}