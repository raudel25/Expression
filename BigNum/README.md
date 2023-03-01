# BigNum

## Implementación en BigNum

El funcionamiento básico de esta biblioteca es realizar las operaciones matemáticas, mediante números almacenados
en un formato de artimética de punto fijo en base 10, esto para buscar mayor precisión a la hora de efectuar las operaciones, manteniendo una precisión exacta de los lugares decimales, mientras tanto en la representación de la parte entera si no contamos con ningún límite, por lo que dentro de nuestros límites computacionales podemos representar cualquier número. Además también cuenta con el objeto `IntegerNumbers` que hereda de `RealNumbers` y modela el comportamiento de un número entero.

Para la optimización de las operaciones en `BigNum` y aprovechándonos de que la aritmética de los enteros en la computadora es exacta, variamos la base de la aritmética a potencias de 10, siempre y cuando no no excedan la aritmética entera que suele ser en la mayoría de los casos de 64 bits.

### Uso de la biblioteca `BigNum`

Para usar las implementaciones de la biblioteca debe instaciar la clase `BigNumMath` que recive los parámetros `precision`, cantidad de lugares
de bits para la parte decimal, `indBase10`, exponente de la potencia en base 10 que representa la base. Luego mediante los métodos `Int` y `Real`
se pueden obtener los números enteros y reales que representa dicha aritmética.

```csharp
var big = new BigNumMath(6, 9);
var real = big.Real("2");
var integer = big.Int("2");
```

Puede instalar la biblioteca para usarla en sus proyectos mediante siguiente paquete <a href="https://www.nuget.org/packages/BigNum/">nuget</a>.

### Operaciones

En la clase `BigNumMath` se encuentran implementadas las operaciones que se pueden realizar con los objetos del
tipo `RealNumbers`.

- Suma: Está implementada la suma clásica bit a bit entre dos números.

```csharp
public static RealNumbers Sum(RealNumbers x, RealNumbers y) => SumOperations.Sum(x, y);
```

- Multiplicación: Está implementada la multiplicación mediante
  el <a href="https://es.wikipedia.org/wiki/Algoritmo_de_Karatsuba#:~:text=El%20paso%20b%C3%A1sico%20del%20algoritmo,sumas%20y%20desplazamientos%20de%20d%C3%ADgitos.">
  algortimo de Karatsuba</a>, apoyado en la suma antes definida.

```csharp
public static RealNumbers Product(RealNumbers x, RealNumbers y) => ProductOperations.Product(x, y);
```

- División : Está implementada la división clásica basada en el algoritmo de euclides para la división y la suma y
  multiplicación antes definida.

```csharp
public static RealNumbers Division(RealNumbers x, RealNumbers y, bool integer = false) =>
    DivisionOperations.Division(x, y, integer).Item1;
```

- Resto de División: Operación realizada entre objetos de la clase `IntegerNumber`, basada en la división antes
  definida.

```csharp
public static IntegerNumbers Rest(IntegerNumbers x, IntegerNumbers y) =>
    DivisionOperations.Division(x, y, true).Item2;
```

- Potencia(exponente entero): Operación realizada entre un objeto de la clase `RealNumbers`(base), basada en la repetición de la multiplicación de la base antes definida.

```csharp
public static RealNumbers Pow(RealNumbers x, int y) => PowOperations.Pow(x, y);
```

- Raíz n-ésima: Para esta operación se trata de aproximar mediante una potencia entera y luego se aproxima mediante el
  siguiente <a href="https://es.frwiki.wiki/wiki/Algorithme_de_calcul_de_la_racine_n-i%C3%A8me">algoritmo</a>.

```csharp
public static RealNumbers Sqrt(RealNumbers x, int y) => SqrtOperations.Sqrt(x, y);
```

- Potencia: Para esta operación se busca la fracción que genera el exponente y luego se calcula la raíz y la potencia
  correspondiente.

```csharp
public static RealNumbers Pow(RealNumbers x, RealNumbers y) => PowOperations.Pow(x, y);
```

- Número $\pi$: Se aproxima mediante la serie de <a href="https://es.wikipedia.org/wiki/Serie_de_Taylor">Taylor</a> de
  la función $arcsin(x)$.

```csharp
public static RealNumbers PI => Constants.ConstantPI();
```

- Número $e$: Se aproxima mediante la serie de <a href="https://es.wikipedia.org/wiki/Serie_de_Taylor">Taylor</a> de la
  función $e^x$.

```csharp
public static RealNumbers E => Constants.ConstantE();
```

- Logaritmo en base e: Se aproxima mediante la serie de <a href="https://es.wikipedia.org/wiki/Serie_de_Taylor">
  Taylor</a> de la
  función $ln(1-x)$, con $|x| \leq 1$, si $|x| > 1$ se utiliza la siguiente identidad $ln({1\over x})=-ln(x)$.

```csharp
public static RealNumbers Ln(RealNumbers x) => LogarithmOperations.Ln(x);
```

- Logaritmo: Se aproxima mediante la identidad $log(a)(b)={ln(a)\over ln(b)}$, con el cálculo de los logaritmos en base
  $e$ correspondientes.

```csharp
public static RealNumbers Log(RealNumbers x, RealNumbers y) => LogarithmOperations.Log(x, y);
```

- Seno: Se aproxima mediante la serie de <a href="https://es.wikipedia.org/wiki/Serie_de_Taylor">Taylor</a> de la
  función $sin(x)$.

```csharp
public static RealNumbers Sin(RealNumbers x) => TrigonometryOperations.SinCos(x, true);
```

- Coseno: Se aproxima mediante la serie de <a href="https://es.wikipedia.org/wiki/Serie_de_Taylor">Taylor</a> de la
  función $cos(x)$.

```csharp
public static RealNumbers Cos(RealNumbers x) => TrigonometryOperations.SinCos(x, false);
```

- Arcoseno: Se aproxima mediante la serie de <a href="https://es.wikipedia.org/wiki/Serie_de_Taylor">Taylor</a> de la
  función $arcsin(x)$.

```csharp
public static RealNumbers Asin(RealNumbers x) => TrigonometryOperations.Asin(x);
```

- Arcocoseno: Se aproxima mediante la identidad $arccos(x)={\pi \over 2}-arcsin(x)$, con el cálculo del arcoseno
  correspondiente.

```csharp
public static RealNumbers Acos(RealNumbers x) => PI / new RealNumbers("2", "0") - Asin(x);
```

- Arcotangente: Se aproxima mediante la serie de <a href="https://es.wikipedia.org/wiki/Serie_de_Taylor">Taylor</a> de
  la función $arctan(x)$, con $|x| \leq 1$, si $|x| > 1$ se utiliza la siguiente identidad $arctan(x)={\pi \over 2}-arctan({1 \over x})$.

```csharp
public static RealNumbers Atan(RealNumbers x) => TrigonometryOperations.Atan(x);
```

- Arcocotangente: Se aproxima mediante la identidad $arccot(x)={\pi \over 2}-arctan(x)$, con el cálculo de la
  arcotangente correspondiente.

```csharp
public static RealNumbers Acot(RealNumbers x) => BigNumMath.PI / new RealNumbers("2", "0") - Atan(x);
```
