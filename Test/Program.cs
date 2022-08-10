using BigNum;

Numbers a = new Numbers("1");

Numbers b = new Numbers("3",false);

Fraction c = new Fraction(new IntegerNumbers("1"), new IntegerNumbers("2"));

Fraction d = new Fraction(new IntegerNumbers("2"), new IntegerNumbers("3"));

Console.WriteLine(a+a*b);

Console.WriteLine(BigNumMath.Division(a,b));
