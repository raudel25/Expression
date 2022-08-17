using System.Diagnostics;
using BigNum;
using Expression;


string s = "av(log(x)(3))";

ExpressionType exp = ConvertExpression.ConvertExpressions(s);

Console.WriteLine(exp);
Stopwatch crono = new Stopwatch();
crono.Start();
Console.WriteLine(exp.Derivative());
crono.Stop();

Console.WriteLine(crono.ElapsedMilliseconds);
    
    
    


