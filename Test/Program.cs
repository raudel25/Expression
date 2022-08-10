using BigNum;

IntegerNumbers a = new IntegerNumbers("5");

IntegerNumbers b = new IntegerNumbers("3",false);

Numbers e = new Numbers("3");

Fraction c = new Fraction(new IntegerNumbers("1"), new IntegerNumbers("2"));

Fraction d = new Fraction(new IntegerNumbers("2"), new IntegerNumbers("3"));

Console.WriteLine(a+a*b);

Console.WriteLine(BigNumMath.Pow(c,-1));