using BigNum;
using Expression;

ExpressionValue a=new ExpressionNumber(new Numbers("2",false))*new ExpressionNumber(new Numbers("2")) / new ExpressionNumber(new Numbers("2"));

ExpressionValue b = new ExpressionNumber(new Numbers("0")) - new ExpressionNumber(new Numbers("2", false));
ExpressionValue c = new Cos(new Sin(new Cos(new ExpressionNumber(new Numbers("1"))-new ExpressionNumber(new Numbers("3",false)))));

Console.WriteLine(c);