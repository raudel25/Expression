using System.Diagnostics;
using BigNum;
using Expression;


string s = "a^a";

ExpressionType exp = ConvertExpression.Parsing(s);

Console.WriteLine(exp.Derivative('a'));


    
    
    


