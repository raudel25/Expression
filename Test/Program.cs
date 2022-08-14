using BigNum;
using Expression;

ExpressionType a = new Cos(new VariableExpression('x'));

Taylor b = new Taylor(a, new RealNumbers("2"), RealNumbers.Real0,25);

Console.WriteLine(b.ValueResult); 
Console.WriteLine(BigNumMath.Cos(new RealNumbers("2")));
Console.WriteLine(Math.Cos(2));
