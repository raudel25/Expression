# Expression

Este proyecto tiene como objetivo crear un generador de expresiones matemáticas e implementar la
biblioteca `BigNum`, la cual cual se encarga de realizar los cálculos que generan las expresiones con un nivel de
precisión un poco mas elevado que las bibliotecas ofrecidas por el lenguaje. Cuenta con la posibilidad de generar
expresiones matemáticas dadas mediante una cadena de texto, hallar la derivada y los valores numéricos que
genera dicha expresión.

## Dependencias

El proyecto está implementado en C# 11 y .NET Core 7.0, para ejecutar el proyecto, solo debe escribir en consola:

```bash
make
```

si estas en linux y

```bash
dotnet run --project UserInterface
```

si estas en windows.

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

## Implementación en Expression

Las expresiones se modelan en forma de árbol mediante la clase abstracta `ExpressionType`, que cuenta con los
m'etodos `Evaluate` y `Derivative`,
los cuales se encargan de obtener el valor numérico y la derivada de la expresión. Luego las expresiones binarias y
unarias,
se modelan mediante las clases abstractas `BinaryExpression` y `UnaryExpression`, las cuales sirven de plantilla, para
las demás expreiones.

### Uso de la biblioteca Expression

Para usar la biblioteca primero debe definir su propia aritmética, la cual se puede definir mediante una clase que implemente la interfaz
`IAritmetic`. Por defecto la biblioteca cuenta cuenta con una artmética implementada con la biblioteca `BigNum` mediante la clase
`BigNumExp` y la aritmética nativa del lenguaje mediante la clase `NativeExp`. Una vez definida la aritmética se debe instancear la clase
`ArithmeticExp` y mediante los métodos `NumberExpression` y `VariableExpression` puede obtener las expresiones que le sirven para definir las 
restantes.

```csharp
var big = new BigNumMath(6, 9);
var arithmetic = new ArithmeticExp<RealNumbers>(new BigNumExp(big));
var number10 = arithmetic.NumberExpression(big.Real("10"));
```

### ConvertExpression

El parsing de las expresiones está basado en el
algoritmo <a href="https://es.wikipedia.org/wiki/Algoritmo_shunting_yard">shunting_yard</a>
y se realiza en el método `Parsing` de la clase `ConvertExpression` que recibe un `string` y devuelve un árbol
de expresiones (si este es `null` la expresión no es valida). Las expresiones se manejan con el siguiente lenguaje,
que describe los operadores usados y su nivel de prioridad (un nivel de prioridad mayor indica que la operación
debe realizarse primero).

Prioridad 1:

- Suma (**x + y**): efectúa la suma entre dos expresiones.
- Resta (**x - y**): efectúa la resta entre dos expresiones, si actúa sobre una sola expresión, devuelve su opuesto(**-x**).

Prioridad 2:

- Multiplicación (**x * y**): efectúa la multiplicación entre dos expresiones.
- División (**x / y**): efectúa la división entre dos expresiones.

Prioridad 3:

- Potencia (**x ^ y**): efectúa la exponenciasión entre dos expresiones.

Prioridad 4:

- Logaritmo (**log(x)(y)**): efectúa el logaritmo **y** en base **x**.
- Logaritmo en base $e$ (**ln(x)**): efectúa el logaritmo **x** en base $e$.

Prioridad 5:

- Seno (**sin(x)**): efectúa el seno de una expresión.
- Coseno (**cos(x)**): efectúa el coseno de una expresión.
- Tangente (**tan(x)**): efectúa la tangente de una expresión.
- Cotangente (**cot(x)**): efectúa la cotangente de una expresión.
- Secante (**sec(x)**): efectúa la secante de una expresión.
- Cosecante (**csc(x)**): efectúa la cosecante de una expresión.
- Arcoseno (**arcsin(x)**): efectúa el arcoseno de una expresión.
- Arcocoseno (**arccos(x)**): efectúa el arcocoseno de una expresión.
- Arcotangente (**arctan(x)**): efectúa la arcotangente de una expresión.
- Arcocotangente (**arccot(x)**): efectúa la arcocotangente de una expresión.

Prioridad 6:

- Números (**0.98**): número real.
- Variable (**x**): variable real.
- Constante $\pi$ (**pi**): número $\pi$.
- Constante $e$ (**e**): número $e$.

### ReduceExpression

Para acortar el árbol de expresiones la biblioteca cuenta con la clase `ReduceExpression`, que se encarga de
reducir las expresiones (ejem: $a\cdot 1=a$ ó $a+0=a$). Para ello cada operación cuenta con una implementación de
como reducirse bajo ciertas condiciones.
