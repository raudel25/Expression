using System.Diagnostics;
using BigNum;
using Expression;

string s = "1+e+4*e+a";
ExpressionType e = ConvertExpression.Parsing(s);
Console.WriteLine(e);
Console.WriteLine(e.Evaluate(new List<(char, RealNumbers)>(){('a',RealNumbers.Real1),('b',RealNumbers.Real1)}));







