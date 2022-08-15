using System.Diagnostics;
using BigNum;
using Expression;

ExpressionType a = new Sin(new VariableExpression('x'));

Taylor b = new Taylor(a, RealNumbers.Real1, RealNumbers.Real1, 5);

Console.WriteLine(b.ExpressionResult);
Console.WriteLine(b.ValueResult);


