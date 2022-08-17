using System.Diagnostics;
using BigNum;
using Expression;


string s = "a^a";

ExpressionType exp = ConvertExpression.ConvertExpressions(s);

Console.WriteLine(exp);
Stopwatch crono = new Stopwatch();
crono.Start();
Console.WriteLine(exp.Derivative('a'));
crono.Stop();

Console.WriteLine(Aux.CantVariable(exp));


    
    
    


