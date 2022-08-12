using BigNum;

namespace Expression;

public class Taylor
{
    public readonly ExpressionType ExpressionResult;

    public readonly RealNumbers ValueResult;

    public Taylor(ExpressionType exp, RealNumbers center, RealNumbers value, int precision)
    {
        this.ExpressionResult = TaylorSerial(exp, center, precision);
        //this.ValueResult = this.ExpressionResult.Evaluate(value);
    }

    private ExpressionType TaylorSerial(ExpressionType exp, RealNumbers center, int precision)
    {
        ExpressionType taylorValue = new NumberExpression(exp.Evaluate(center));
        ExpressionType taylorFunction = exp;
        IntegerNumbers fact = new IntegerNumbers("1");
        IntegerNumbers precisionBig = BigNumMath.ConvertToIntegerNumbers(precision);

        for (IntegerNumbers i = new IntegerNumbers("1"); i < precisionBig; i++)
        {
            taylorFunction = taylorFunction.Derivative();

           //var a = new NumberExpression(taylorFunction.Evaluate(center));
           // var b = new Pow(new VariableExpression('x') - new NumberExpression(center),
             //   new NumberExpression(i));
            //var c = new NumberExpression(fact);

            //taylorValue += a * b / c;

            fact *= i;
        }

        return taylorValue;
    }
}