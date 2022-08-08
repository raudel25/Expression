using BigNum;

Numbers a = new Numbers("1234",false);

Numbers b = new Numbers("2468",false);

Console.WriteLine(a);
Console.WriteLine(BigNumMath.Sum(a,b));