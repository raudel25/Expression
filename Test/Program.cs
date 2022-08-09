using BigNum;

Numbers a = new Numbers("0");

Numbers b = new Numbers("6",false);

Fraction c = new Fraction(new IntegerNumbers("1"), new IntegerNumbers("2"));

Fraction d = new Fraction(new IntegerNumbers("2"), new IntegerNumbers("3"));

Console.WriteLine(c-d);
