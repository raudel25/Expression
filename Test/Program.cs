using System.Diagnostics;
using BigNum;
using Expression;

string s = "x^3+y^3";
ExpressionType e = ConvertExpression.Parsing(s);
//Console.WriteLine(e.EvaluateExpression(new List<(char, ExpressionType)>(){('x',new Sin(new VariableExpression('x')))}));

List<(char, RealNumbers)> a = new List<(char, RealNumbers)>() {('x', RealNumbers.Real1),('y',RealNumbers.Real1)};

List<(char, RealNumbers)> ab = new List<(char, RealNumbers)>() {('x', RealNumbers.Real1),('y',RealNumbers.Real1)};

Taylor t = new Taylor(e, ab, a, 3);

Console.WriteLine(t.ExpressionResult);
Console.WriteLine(t.ValueResult);







