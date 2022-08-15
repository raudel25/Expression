using BigNum;
using Expression;

ExpressionType a = new Log(new NumberExpression(new RealNumbers("2")),new VariableExpression('x'));

ExpressionType b = new NumberExpression(new RealNumbers("3"));
ExpressionType c = new NumberExpression(new RealNumbers("4.03"));

Console.WriteLine(b/(c*c));

Console.WriteLine(a);
Console.WriteLine(a.Derivative());


