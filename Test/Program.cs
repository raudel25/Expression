using BigNum;
using Expression;

ExpressionType a = new PowE(new Cos(new VariableExpression('x')));

Console.WriteLine(a.Derivative());


