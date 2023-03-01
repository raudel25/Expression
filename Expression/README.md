# Expression

## Implementación en Expression

Las expresiones se modelan en forma de árbol mediante la clase abstracta `Function`, que cuenta con los
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

Puede instalar la biblioteca para usarla en sus proyectos mediante siguiente paquete <a href="https://www.nuget.org/packages/Expression/">nuget</a>.

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

- Multiplicación (**x \* y**): efectúa la multiplicación entre dos expresiones.
- División (**x / y**): efectúa la división entre dos expresiones.

Prioridad 3:

- Potencia (**x ^ y**): efectúa la exponenciasión entre dos expresiones.

Prioridad 4:

- Logaritmo (**log\[x\](y)**): efectúa el logaritmo **y** en base **x**.
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
