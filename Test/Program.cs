using BigNum;
using Expression;

ExpressionValue a = new Pow(- new ExpressionVariable('x') + new ExpressionNumber(new Numbers("0")),
    new ExpressionNumber(new Numbers("2")));

ExpressionValue b = new ExpressionNumber(new Numbers("0")) - new ExpressionNumber(new Numbers("2", false));
ExpressionValue c = new Sin(new ExpressionVariable('p'));

Taylor d = new Taylor(c, new Numbers("0"), new Numbers("1"), 10);

Console.WriteLine(c);

Console.WriteLine(d.ExpressionResult);

Console.WriteLine(a);