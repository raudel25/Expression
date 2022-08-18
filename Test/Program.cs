using System.Diagnostics;
using BigNum;
using Expression;


string s = "e+e+pi^2+pi^2";

ExpressionType exp = ConvertExpression.Parsing(s);

Console.WriteLine(exp);


    
    
    


