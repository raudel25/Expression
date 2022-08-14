using BigNum;
using Expression;

ExpressionType a = new Csc(new VariableExpression('x'));

Console.WriteLine(a.Derivative());

Console.WriteLine(a.Derivative().Evaluate(RealNumbers.Real1));

Console.WriteLine(-BigNumMath.Sin(BigNumMath.Sin(RealNumbers.Real1))*BigNumMath.Cos(RealNumbers.Real1));


