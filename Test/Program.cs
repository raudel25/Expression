using System.Diagnostics;
using BigNum;
using Expression;


string s = "a^2+a^3+2*a^2";

ExpressionType exp = ConvertExpression.Parsing(s);

Console.WriteLine(exp);


    
    
    


