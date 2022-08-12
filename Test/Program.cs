using System.Diagnostics;
using BigNum;
using Expression;

ExpressionType a = new Pow(- new VariableExpression('x') + new NumberExpression(new RealNumbers("0")),
    new NumberExpression(new RealNumbers("2")));

ExpressionType b = new NumberExpression(new RealNumbers("0")) - new NumberExpression(new RealNumbers("2", false));
ExpressionType c = new Sin(new VariableExpression('p'));

Stopwatch crono = new Stopwatch();
crono.Start();
Taylor d = new Taylor(c, new RealNumbers("0"), new RealNumbers("1"), 12);
crono.Stop();
Console.WriteLine(crono.ElapsedMilliseconds);

Console.WriteLine(c);

//Console.WriteLine(d.ExpressionResult);

//Console.WriteLine(BigNumMath.Factorial(new IntegerNumbers("20")));