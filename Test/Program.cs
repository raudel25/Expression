using BigNum;
using Expression;

ExpressionType a = new PowVariable(new VariableExpression('x'),new NumberExpression(new RealNumbers("3")));

ExpressionType b = new NumberExpression(new RealNumbers("4"));
ExpressionType c = new NumberExpression(new RealNumbers("3"));
var e = b * b;
var d = c * b;
Console.WriteLine(new Pow(e,d));

Console.WriteLine(a.Derivative());


