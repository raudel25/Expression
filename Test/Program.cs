using System.Diagnostics;
using BigNum;
using Expression;

string s = "ln(x+y)";
ExpressionType e = ConvertExpression.Parsing(s);
Console.WriteLine(e.EvaluateExpression(new List<(char, ExpressionType)>(){('x',new Sin(new VariableExpression('x')))}));

// List<(char, RealNumbers)> a = new List<(char, RealNumbers)>() {('x', RealNumbers.Real1), ('y', RealNumbers.Real1)};
//
// Taylor t = new Taylor(e, a, a, 3);







