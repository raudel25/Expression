using BigNum;

Numbers a = new Numbers("42.71",false);

Numbers b = new Numbers("53.1");

Console.WriteLine(a);
Console.WriteLine(BigNumMath.Sum(a,b));
