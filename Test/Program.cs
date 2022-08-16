using System.Diagnostics;
using BigNum;
using Expression;

string s = "-sin(x)";

ExpressionType a = new NumberExpression(new RealNumbers("3"));

NumberExpression b = new NumberExpression(new RealNumbers("4"));

ExpressionType c = new PowVariable(new VariableExpression('x'),b);

ExpressionType d=ConvertExpression.ConvertExpressions(s);

Console.WriteLine(d);
Console.WriteLine(d.Evaluate(RealNumbers.Real1));


