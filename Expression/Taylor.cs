using BigNum;

namespace Expression;

public class Taylor
{
    public readonly ExpressionValue ExpressionResult;

    public readonly Numbers ValueResult;

    public Taylor(ExpressionValue exp, Numbers center, Numbers value, int precision)
    {
        this.ExpressionResult = TaylorSerial(exp, center, precision);
        //this.ValueResult = this.ExpressionResult.Evaluate(value);
    }

    private ExpressionValue TaylorSerial(ExpressionValue exp, Numbers center, int precision)
    {
        ExpressionValue taylorValue = new ExpressionNumber(exp.Evaluate(center));
        ExpressionValue taylorFunction = exp;
        int fact = 1;

        for (int i = 1; i < precision; i++)
        {
            taylorFunction = taylorFunction.Derivative();

            var a = new ExpressionNumber(taylorFunction.Evaluate(center));
            var b = new Pow(new ExpressionVariable('x') - new ExpressionNumber(center),
                new ExpressionNumber(BigNumMath.ConvertToNumbers(i)));
            var c = new ExpressionNumber(BigNumMath.ConvertToNumbers(fact));

            taylorValue += a * b / c;

            fact *= i;
        }

        return taylorValue;
    }
}