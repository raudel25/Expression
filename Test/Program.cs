using System.Diagnostics;
using BigNum;
using Expression;

string s = "(a+b)*c+e^x";

ExpressionType a = new NumberExpression(new RealNumbers("3"));

NumberExpression b = new NumberExpression(new RealNumbers("4"));

ExpressionType c = new PowVariable(new VariableExpression('x'),b);

Console.WriteLine(c/b/(a * a));


