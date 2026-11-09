# Arquitectura

Modelo de análisis y diseño de TI, actualizado el 8 de noviembre de 2026 a partir de las decisiones del usuario. Las clases del [UML](diagramas/UML.uxf) todavía no están implementadas; la aplicación conserva `Hello, World!`. Los requisitos están en [Reglas del juego](Reglas-del-juego.md) y el seguimiento en [Plan](Plan.md).

## Distribución y responsabilidades

El servidor coordina la partida y analiza los enfrentamientos. Los dos clientes solicitan elecciones y presentan los resultados. Cada proceso dispone de su propio `Datos.Modelo` en memoria: compartir archivos no comparte objetos ni actualiza automáticamente las tablas.

| Clase | Datos encapsulados | Responsabilidad y delegación |
|---|---|---|
| Personaje | Nombre, ASCII y seis atributos. | Representar una carta válida; ofrecer consultas y configurar su persistencia. No lee archivos directamente. |
| Mazo | Colección de personajes disponibles. | Mezclar, retirar elementos al repartir y reincorporarlos. Recibe objetos ya cargados. |
| Jugador | Nombre, mano disponible y rondas ganadas. | Administrar sus cartas y marcador. Las consultas devuelven una copia de la colección. |
| Partida | Dos jugadores, mazo, ronda actual y número de ronda. | Coordinar inicio, reparto, rondas, recuperación y victoria. Delega la comparación a Combate y el seguimiento de ronda a Ronda. |
| Ronda | Jugador mano, rival y combates registrados. | Determinar el siguiente elector y el ganador de ronda. Calcula las victorias desde los combates, evitando contadores duplicados. |
| Combate | Participantes, atributo, elecciones y ganador. | Registrar las elecciones y resolver la comparación. Los campos aún no elegidos y el ganador pendiente son anulables. |
| InterfazConsola | Sin estado de dominio propio obligatorio. | Construir las pantallas del juego y solicitar opciones usando Utilidades. No determina ganadores. |
| Datos (estática) | Constantes de rutas, tipo de proceso y Modelo con escritura privada. | Configurar la identidad del proceso, inicializar su modelo y ofrecer acceso a sus tablas. No aplica las reglas del juego. |

El jugador mano pertenece exclusivamente a Ronda y permanece fijo durante sus combates. Partida numera las rondas desde 1 y calcula los argumentos al construirlas: J1 es mano en las impares y J2 en las pares. No conserva otra referencia de jugador mano ni necesita un método para alternarla. Jugador tampoco conserva una copia de ese rol. Las entidades del juego se conservan porque representan conceptos propios del problema y permiten explicar sus reglas.

## Notación de atributos y relaciones

Las referencias a objetos y colecciones de entidades se representan exclusivamente mediante asociaciones, sin duplicarlas en el compartimento de atributos. Cada extremo navegable indica el nombre del rol, su multiplicidad y su visibilidad: `-` privado y `+` público. Por ejemplo, Ronda se relaciona con Jugador mediante los roles privados `- jugadorMano` y `- jugadorRival`, cada uno con multiplicidad 1.

Los roles diferentes se dibujan como asociaciones distintas, incluso si apuntan a la misma clase. El ganador de Combate tiene multiplicidad 0..1 mientras no se resuelva; sus dos cartas también tienen 0..1 durante la elección. Datos tiene la asociación pública estática `+ Modelo`, con lectura pública y escritura privada. Las dependencias de uso y la realización de interfaces no representan atributos y no llevan visibilidad de miembro. Los valores simples y enumeraciones permanecen en el compartimento de atributos.

### Revisión de notación y distribución

Se consultó la [especificación UML 2.5.1 de OMG](https://www.omg.org/spec/UML/2.5.1/PDF): sección 9.4.4 para miembros estáticos y de solo lectura, 10.2.4 para enumeraciones y 11.5.4 para asociaciones. La [documentación de UMLet](https://www.umlet.com/faq.htm) define la representación mediante lt, r1/r2 y m1/m2 y la conexión de extremos a los bordes.

TipoAtributo utiliza la palabra clave «enumeration» y un literal por línea. Por convención solicitada por el usuario, cada literal lleva `+`. Los miembros estáticos se subrayan; la propiedad Modelo se representa en su asociación, subrayada, con acceso público y una aclaración de escritura privada. La realización de IEntidad apunta a la interfaz con triángulo hueco y línea discontinua. Las asociaciones navegables usan línea continua y punta abierta; las dependencias de uso se explican por escrito y no se dibujan con flechas `.>`, por indicación del usuario. Utilidades se representa como paquete y los nombres externos de DataBases se califican con `::`.

Las firmas conservan nombres de tipos y colecciones de C# para orientar la futura implementación; no constituyen una especificación completa de plantillas UML. El atributo opcional TipoAtributo de Combate expresa multiplicidad `[0..1]`.

El diagrama se exportó con UMLet 15.1 y se revisó visualmente. Se separaron los roles para evitar superponer sus etiquetas. También se comprobaron geométricamente las 16 relaciones: extremos en bordes, tramos ortogonales y ningún tramo dentro de cajas de clases, paquete o notas. Estas comprobaciones no acreditan todavía el protocolo de comunicación.

## Especificación de clases y métodos — actividad 4

### Personaje

Representa una carta válida con nombre, ASCII y seis atributos privados. No ofrece modificación pública de esos valores. Implementa `IEntidad<Personaje>` sin encargarse de leer archivos.

#### Constructor `Personaje(nombre: string, ascii: string, fuerza: int, velocidad: int, resistencia: int, inteligencia: int, energia: int, habilidad: int)`

- **Características:** público, de instancia, sin retorno declarado. Recibe dos textos y los seis atributos enteros homónimos.
- **Contrato:** nombre y ASCII no nulos; nombre no vacío ni compuesto solo por espacios. Conserva los datos y ejecuta Validar antes de completar la construcción. Nulos producen `ArgumentNullException`; nombre vacío, `ArgumentException`; valores numéricos inválidos se rechazan según Validar. El formato y límite del ASCII se precisarán en la actividad 5.
- **Por qué existe:** garantiza que el resto del dominio reciba personajes válidos y no tenga que reparar sus atributos.

#### `Validar(): void`

- **Características:** privado, de instancia, sin parámetros ni retorno. Consulta los campos asignados durante la construcción y no los modifica.
- **Contrato:** comprueba primero cada atributo en el intervalo inclusivo 250–750; después, total exactamente 3000, alguno mayor o igual a 650 y alguno menor o igual a 350. Seis valores de 500 quedan excluidos por los umbrales. Atributo fuera de rango produce `ArgumentOutOfRangeException`; total o umbrales incorrectos, `ArgumentException`.
- **Por qué existe:** concentra las restricciones en una operación interna. No necesita acceso público porque los atributos no tienen modificadores públicos.

#### `GetNombre(): string`

- **Características:** público, de instancia y sin parámetros; retorna el nombre conservado.
- **Contrato:** requiere construcción válida, no modifica estado y no tiene errores de dominio en ese estado.
- **Por qué existe:** permite identificar la carta sin exponer el campo para escritura.

#### `GetAscii(): string`

- **Características:** público, de instancia y sin parámetros; retorna el texto ASCII conservado.
- **Contrato:** requiere construcción válida; no imprime ni modifica datos y no tiene errores de dominio en ese estado.
- **Por qué existe:** separa los datos visuales de su presentación en consola.

#### `GetAtributo(tipo: TipoAtributo): int`

- **Características:** público, de instancia; recibe uno de los seis atributos y retorna el entero correspondiente.
- **Contrato:** exige un literal definido en TipoAtributo; un valor no definido produce `ArgumentOutOfRangeException`, sin retornar un valor por defecto. No modifica el personaje.
- **Por qué existe:** permite comparar cualquier atributo mediante una consulta común sin hacer públicos seis campos modificables.

#### `Configurar(entidad: ConstructorEntidad<Personaje>): void`

- **Características:** público y estático; recibe el constructor de configuración de DataBases, sin retorno. No necesita una instancia de Personaje.
- **Contrato:** entidad no nula; de lo contrario, `ArgumentNullException`. Declara archivo mediante Datos.RUTA_PERSONAJES y columnas mediante expresiones sobre los campos privados, compatibles con los nombres y tipos del constructor. No lee CSV. Las columnas definitivas, formato y referencias del catálogo quedan para la actividad 5. Configuraciones incompatibles generan `CsvConfiguracionException` al configurar o construir el modelo, según la validación de DataBases.
- **Por qué existe:** satisface IEntidad<Personaje> y describe la persistencia sin hacer públicos los campos ni duplicar un lector CSV.

### Mazo

Administra una colección privada de personajes disponibles, su orden y sus transferencias; no controla marcadores ni compara atributos.

#### Constructor `Mazo(personajes: List<Personaje>)`

- **Características:** público, de instancia, sin retorno declarado; recibe la lista inicial.
- **Contrato:** conserva una copia de la colección. Lista nula produce `ArgumentNullException`; elementos nulos o una referencia repetida, `ArgumentException`. Puede estar vacía: el mínimo de seis se controla al iniciar Partida. No identifica duplicados de catálogo por nombre.
- **Por qué existe:** recibe personajes ya cargados y evita que modificar la lista original altere el mazo.

#### `Mezclar(): void`

- **Características:** público, de instancia, sin parámetros ni retorno.
- **Contrato:** reordena aleatoriamente las referencias disponibles sin cambiar su cantidad ni composición. Puede utilizar Fisher–Yates, intercambiando cada posición con una posición aleatoria del tramo restante. Cero o una carta no requieren cambios. No garantiza un orden diferente del anterior ni tiene errores de dominio sobre un mazo válido.
- **Por qué existe:** prepara repartos variables sin asumir responsabilidades de ronda o persistencia.

#### `Repartir(cantidad: int): Personaje[]`

- **Características:** público, de instancia; recibe la cantidad y retorna exactamente esa cantidad de referencias en un arreglo.
- **Contrato:** cantidad negativa produce `ArgumentOutOfRangeException`; superior a la disponible, `InvalidOperationException`, sin retirar cartas. Cero devuelve un arreglo vacío. Retira desde el comienzo de la colección, conserva ese orden en el resultado y reduce la cantidad disponible; no realiza entregas parciales.
- **Por qué existe:** concentra la extracción para que las cartas repartidas dejen de estar disponibles.

#### `Devolver(personajes: Personaje[]): void`

- **Características:** público, de instancia; recibe las referencias que se reincorporarán, sin retorno.
- **Contrato:** arreglo nulo produce `ArgumentNullException`; elementos nulos, referencias repetidas o ya disponibles en el mazo, `ArgumentException`. Valida todo antes de agregarlo al final de la colección. Un arreglo vacío no cambia nada; la mezcla posterior determina el orden para repartir.
- **Por qué existe:** recupera cartas sin crear copias ni duplicar referencias disponibles.

#### `GetCantidad(): int`

- **Características:** público, de instancia, sin parámetros; retorna la cantidad disponible, incluido cero.
- **Contrato:** no incluye cartas repartidas ni modifica la colección. Sin errores de dominio en un mazo válido.
- **Por qué existe:** permite verificar la posibilidad de reparto sin exponer la colección privada.

### Jugador

Conserva nombre, mano privada de hasta tres cartas y rondas ganadas. El rol de jugador mano pertenece a Ronda, no a Jugador.

#### Constructor `Jugador(nombre: string)`

- **Características:** público, de instancia, sin retorno declarado; recibe el nombre.
- **Contrato:** nulo produce `ArgumentNullException`; vacío o solo espacios, `ArgumentException`. Conserva el nombre, crea mano vacía e inicia el marcador en cero. Dos referencias no se consideran iguales por compartir nombre.
- **Por qué existe:** prepara un participante sin cartas ni victorias de una partida anterior.

#### `RecibirCartas(personajes: Personaje[]): void`

- **Características:** público, de instancia; recibe un arreglo, sin retorno.
- **Contrato:** arreglo nulo produce `ArgumentNullException`; elementos nulos, referencias repetidas o ya presentes, `ArgumentException`; superar tres cartas en la mano resultante, `InvalidOperationException`. Valida el conjunto antes de incorporar las referencias en su orden, sin conservar el arreglo externo como mano. Arreglo vacío no cambia nada.
- **Por qué existe:** protege la mano y su capacidad. El reparto normal de tres lo coordina Partida, no Jugador.

#### `QuitarCarta(personaje: Personaje): void`

- **Características:** público, de instancia; recibe la carta que sale de la mano, sin retorno.
- **Contrato:** nulo produce `ArgumentNullException`; referencia no disponible, `ArgumentException`. Retira exactamente esa referencia. En la secuencia acordada lo llama Combate.SetCarta tras validar la elección; el coordinador no debe retirarla otra vez.
- **Por qué existe:** encapsula el consumo de una carta y evita reutilizar una elección de la ronda.

#### `EntregarCartas(): Personaje[]`

- **Características:** público, de instancia, sin parámetros; retorna las referencias restantes en su orden actual.
- **Contrato:** vacía la mano y no modifica el marcador. Mano ya vacía devuelve arreglo vacío, sin error de dominio.
- **Por qué existe:** permite recuperar las cartas no jugadas al cerrar la ronda.

#### `GetCartas(): Personaje[]`

- **Características:** público, de instancia, sin parámetros; retorna una copia de la colección disponible.
- **Contrato:** puede retornar arreglo vacío. No retira cartas; cambiar el arreglo devuelto no altera la mano. Sin errores de dominio en un jugador válido.
- **Por qué existe:** permite construir la selección de cartas sin exponer la colección privada para escritura.

#### `SumarRondaGanada(): void`

- **Características:** público, de instancia, sin parámetros ni retorno.
- **Contrato:** incrementa el marcador en uno. Partida debe llamarlo una sola vez para el ganador de una ronda definida. No valida qué ronda se ganó, porque no la recibe; no es idempotente y cada llamada suma. La garantía de una llamada pertenece a JugarRonda.
- **Por qué existe:** protege la escritura del contador sin decidir el ganador ni duplicar el umbral de finalización de Partida.

#### `GetRondasGanadas(): int`

- **Características:** público, de instancia y sin parámetros; retorna el marcador acumulado, inicialmente cero.
- **Contrato:** no modifica datos ni tiene errores de dominio en un jugador válido.
- **Por qué existe:** permite mostrar el marcador y comprobar la victoria sin permitir asignaciones externas.

#### `GetNombre(): string`

- **Características:** público, de instancia y sin parámetros; retorna el nombre conservado.
- **Contrato:** no modifica datos ni tiene errores de dominio en un jugador válido.
- **Por qué existe:** identifica al participante manteniendo privado su campo.

### Combate

Conserva participantes, elector, atributo, cartas y ganador. Las elecciones y el ganador empiezan ausentes. No suma rondas al marcador.

#### Constructor `Combate(quienElige: Jugador, rival: Jugador)`

- **Características:** público, de instancia, sin retorno declarado; recibe al elector y a su rival.
- **Contrato:** nulos producen `ArgumentNullException`; misma referencia en ambos roles, `ArgumentException`. Registra los roles y deja las elecciones y el ganador pendientes. Ronda determina el elector correcto según la posición del combate.
- **Por qué existe:** conserva quién elige y quién obtiene la victoria ante empate.

#### `SetAtributo(tipo: TipoAtributo): void`

- **Características:** público, de instancia; recibe el atributo, sin retorno.
- **Contrato:** valor no definido produce `ArgumentOutOfRangeException`; atributo ya asignado o combate resuelto, `InvalidOperationException`, incluso si se repite el mismo valor. Registra una sola vez y no revela información en consola. La autorización del elector pertenece al flujo: esta firma no recibe jugador.
- **Por qué existe:** impide cambiar la regla de comparación después de conocer las elecciones.

#### `SetCarta(jugador: Jugador, personaje: Personaje): void`

- **Características:** público, de instancia; recibe participante y carta, sin retorno.
- **Contrato:** nulos producen `ArgumentNullException`; jugador ajeno o carta no disponible en su mano, `ArgumentException`; elección previa de ese jugador o combate resuelto, `InvalidOperationException`. Rechaza también la misma referencia de personaje para ambos participantes. Valida antes de modificar.
- **Resultado:** llama a jugador.QuitarCarta y conserva la referencia en su rol. No se vuelve a retirar desde el coordinador. Las referencias permanecen en Combate para recuperarlas al cerrar la ronda.
- **Por qué existe:** vincula el registro y consumo de la elección. La autorización de mensajes y su secuencia se completan en la actividad 6.

#### `EstaCompleto(): bool`

- **Características:** público, de instancia y sin parámetros; retorna si existen atributo y ambas cartas.
- **Contrato:** no exige resolución, no modifica datos ni revela elecciones; devuelve false mientras falte alguna elección.
- **Por qué existe:** permite distinguir un combate pendiente de uno listo para resolver.

#### `Resolver(): Jugador`

- **Características:** público, de instancia y sin parámetros; retorna el ganador.
- **Contrato:** exige combate completo; de lo contrario, `InvalidOperationException`. Consulta el atributo de ambas cartas; gana el elector solo si su valor es estrictamente mayor, y en otro caso el rival, incluido el empate. Conserva el resultado. Si ya está resuelto retorna el mismo ganador sin repetir efectos ni modificar marcadores.
- **Por qué existe:** centraliza comparación y desempate; Ronda cuenta combates y Partida registra la ronda ganada.

Ejemplo ilustrativo del núcleo de comparación, después de comprobar y extraer las elecciones completas. No es código implementado en la aplicación:

```csharp
int valorElector = cartaQueElige.GetAtributo(atributoElegido);
int valorRival = cartaRival.GetAtributo(atributoElegido);
ganador = valorElector > valorRival ? jugadorQueElige : jugadorRival;
```

#### `GetGanador(): Jugador`

- **Características:** público, de instancia y sin parámetros; retorna el ganador conservado.
- **Contrato:** exige resolución previa; si no existe resultado produce `InvalidOperationException`, aunque el combate esté completo. No resuelve implícitamente.
- **Por qué existe:** consulta un resultado real sin inventar un ganador ni repetir la resolución.

#### `GetAtributo(): TipoAtributo`

- **Características:** público, de instancia y sin parámetros; retorna el atributo elegido.
- **Contrato:** exige combate completo; antes produce `InvalidOperationException`, aunque el atributo ya esté asignado. No modifica datos.
- **Por qué existe:** ofrece la consulta para presentar el enfrentamiento una vez completadas ambas elecciones.

#### `GetCarta(jugador: Jugador): Personaje`

- **Características:** público, de instancia; recibe un participante y retorna su carta registrada.
- **Contrato:** jugador nulo produce `ArgumentNullException`; ajeno, `ArgumentException`; combate incompleto, `InvalidOperationException`. Consultar no retira la carta del combate ni la devuelve al mazo.
- **Por qué existe:** permite presentar y recuperar las cartas sin exponer sus campos privados.

#### `GetParticipantes(): Jugador[]`

- **Características:** público, de instancia y sin parámetros; retorna una copia con dos referencias en orden elector y rival.
- **Contrato:** disponible desde la construcción, sin modificar estado ni revelar cartas o atributo.
- **Por qué existe:** permite obtener nombres y consultar después las cartas sin depender de participantes implícitos de la interfaz.

### Ronda

Conserva jugador mano, rival y hasta tres combates registrados. Calcula victorias desde los resultados, sin contadores duplicados. El jugador mano permanece fijo durante la ronda.

#### Constructor `Ronda(mano: Jugador, rival: Jugador)`

- **Características:** público, de instancia, sin retorno declarado; recibe ambos participantes.
- **Contrato:** nulos producen `ArgumentNullException`; misma referencia, `ArgumentException`. Registra los roles y crea una colección vacía de combates. Partida determina quién es mano según el número de ronda.
- **Por qué existe:** separa la serie de enfrentamientos del marcador acumulado de partida.

#### `AgregarCombate(combate: Combate): void`

- **Características:** público, de instancia; recibe un combate, sin retorno.
- **Precondiciones:** ronda abierta y combate resuelto de sus participantes. El elector debe ser mano, rival o mano para el primer, segundo o tercer combate. No admite cartas ya usadas en otro combate registrado.
- **Errores y efecto:** nulo produce `ArgumentNullException`; combate repetido, participantes ajenos, elector incorrecto o carta reutilizada, `ArgumentException`; combate sin resolver o ronda ya definida, `InvalidOperationException`. Valida todo antes de incorporar una sola vez. Un cuarto combate nunca es válido: al tercero la ronda ya queda definida. No suma rondas al marcador.
- **Por qué existe:** protege el historial para que las victorias no se cuenten por duplicado ni con enfrentamientos ajenos.

#### `EstaDefinida(): bool`

- **Características:** público, de instancia y sin parámetros; retorna si alguno alcanzó dos victorias.
- **Contrato:** cuenta ganadores de los combates registrados, sin modificar marcadores. Con cero o uno devuelve false; con tres válidos necesariamente true.
- **Por qué existe:** permite terminar con dos combates y evita jugar el tercero cuando ya hay ganador.

#### `GetGanador(): Jugador`

- **Características:** público, de instancia y sin parámetros; retorna al participante con dos victorias.
- **Contrato:** exige ronda definida; antes produce `InvalidOperationException`. No suma el marcador.
- **Por qué existe:** identifica al ganador desde los resultados sin duplicar su registro en Jugador.

#### `GetQuienEligeProximo(): Jugador`

- **Características:** público, de instancia y sin parámetros; retorna el próximo elector.
- **Contrato:** exige ronda abierta; si está definida produce `InvalidOperationException`. Devuelve mano con cero o dos combates y rival con uno, independientemente de quién ganó. No cambia el jugador mano.
- **Por qué existe:** concentra la alternancia por posición del combate.

#### `GetCombates(): Combate[]`

- **Características:** público, de instancia y sin parámetros; retorna una copia de los combates registrados en orden de juego.
- **Contrato:** puede retornar arreglo vacío. No elimina combates ni devuelve automáticamente sus cartas al mazo.
- **Por qué existe:** permite a Partida recuperar personajes sin permitir cambios externos en la colección privada.

### Partida

Coordina en el servidor a J1, J2, Mazo y la ronda actual. Conserva número de ronda y la constante privada estática de solo lectura RONDAS_PARA_GANAR = 15. Sus operaciones privadas protegen la secuencia interna frente a llamadas externas arbitrarias.

#### Constructor `Partida(j1: Jugador, j2: Jugador, mazo: Mazo)`

- **Características:** público, de instancia, sin retorno declarado; recibe participantes y mazo.
- **Contrato:** nulos producen `ArgumentNullException`; misma referencia en ambos jugadores, `ArgumentException`. Exige manos vacías y marcadores cero; de lo contrario, `ArgumentException`. Conserva las referencias y deja rondaActual ausente. No lee archivos ni solicita elecciones en la consola del servidor.
- **Por qué existe:** explicita las dependencias y evita heredar estado de otra partida.

#### `Jugar(): Jugador`

- **Características:** público, de instancia y sin parámetros; retorna exclusivamente al ganador normal.
- **Precondición:** instancia aún no iniciada. Si rondaActual ya existe produce `InvalidOperationException`. Tras un fallo de inicio tampoco se reintenta: se finaliza la ejecución.
- **Secuencia:** llama a Iniciar, juega la ronda y recoge todas sus cartas. Si EstaTerminada devuelve true, retorna DeterminarGanadorPartida. En otro caso incrementa numeroRonda, construye una Ronda con J1 como mano en impares y J2 en pares, llama a RepartirCartas y continúa.
- **Errores:** propaga los fallos para finalizar; no devuelve un ganador ficticio. Recoge también la última ronda y no reparte otra después de alcanzar 15.
- **Por qué existe:** ofrece una única operación pública para coordinar. El transporte y publicación de elecciones dependen de la actividad 6; no se sustituyen por consola local del servidor.

#### `Iniciar(): void`

- **Características:** privado, de instancia, sin parámetros ni retorno.
- **Precondiciones:** no existe ronda actual; manos vacías y marcadores cero. Estado incompatible produce `InvalidOperationException`.
- **Secuencia:** exige al menos seis cartas antes de repartir, fija numeroRonda en 1, crea Ronda con J1 como mano y llama a RepartirCartas. Con menos de seis produce `InvalidOperationException` con «No se puede jugar porque no hay suficientes cartas»; el punto de ejecución lo presenta y termina.
- **Por qué existe:** concentra las comprobaciones iniciales y evita un reparto inicial incompleto.

#### `RepartirCartas(): void`

- **Características:** privado, de instancia, sin parámetros ni retorno.
- **Precondiciones:** ronda actual nueva sin combates, manos vacías y seis cartas disponibles como mínimo. Incumplimientos producen `InvalidOperationException` antes de repartir.
- **Secuencia y efecto:** llama a Mazo.Mezclar, retira tres cartas para J1 y tres para J2 con Repartir y las entrega con RecibirCartas. Quedan tres referencias distintas en cada mano y seis menos en el mazo.
- **Por qué existe:** usa la misma preparación inicial y entre rondas, delegando extracción y almacenamiento en sus clases responsables.

#### `JugarRonda(): void`

- **Características:** privado, de instancia, sin parámetros ni retorno.
- **Precondiciones:** ronda actual nueva y manos de tres cartas. No se vuelve a ejecutar sobre una ronda comenzada o definida; estado incompatible produce `InvalidOperationException`.
- **Secuencia:** mientras no esté definida, obtiene el próximo elector, crea Combate con él y su rival, incorpora elecciones con SetAtributo y SetCarta, espera elecciones completas, resuelve y agrega el combate. Al terminar, obtiene GetGanador y llama exactamente una vez a SumarRondaGanada.
- **Resultado y límites:** dos o tres combates y una ronda ganada añadida al marcador. SetCarta ya retira cada carta: no se llama otra vez a QuitarCarta. La espera, transporte y publicación se definirán en la actividad 6; no se inventan aquí sus firmas.
- **Por qué existe:** coordina la ronda delegando alternancia, comparación y conteo sin jugar un tercer combate innecesario.

#### `RecogerCartas(): void`

- **Características:** privado, de instancia, sin parámetros ni retorno.
- **Precondiciones:** ronda actual definida y todavía no recogida. Ausente o abierta produce `InvalidOperationException`; el coordinador garantiza una llamada por cierre. Mazo.Devolver rechaza una devolución duplicada.
- **Secuencia:** reúne las dos cartas de cada combate registrado usando GetParticipantes y GetCarta y las restantes de ambos jugadores mediante EntregarCartas. Devuelve ese conjunto al mazo una sola vez.
- **Resultado:** manos vacías y mazo completo: cuatro cartas jugadas más dos restantes en una ronda de dos combates, o seis jugadas en una de tres. Las no repartidas nunca salieron del mazo. El historial conserva referencias, sin convertirlas en cartas disponibles adicionales. Un fallo interrumpe el juego; no se promete una transacción ni recuperación.
- **Por qué existe:** evita perder cartas no jugadas o devolver por duplicado las que ya salieron de la mano.

#### `EstaTerminada(): bool`

- **Características:** privado, de instancia y sin parámetros; retorna si alguno alcanzó o superó RONDAS_PARA_GANAR.
- **Contrato:** consulta marcadores sin modificarlos ni avanzar la ronda. Sin errores de dominio sobre el estado válido.
- **Por qué existe:** centraliza el umbral que decide si se permite preparar otra ronda.

#### `DeterminarGanadorPartida(): Jugador`

- **Características:** privado, de instancia y sin parámetros; retorna al único jugador que alcanzó el umbral.
- **Contrato:** si ninguno o ambos lo alcanzaron produce `InvalidOperationException`; ambos sería una inconsistencia del flujo, no un empate permitido.
- **Por qué existe:** impide presentar una finalización prematura o incoherente como una victoria normal.

### Datos

Clase estática, sin instancias. Conserva tipoProceso privado opcional y las rutas públicas constantes del UML. Modelo permite lectura pública y escritura privada y no está disponible hasta completar Iniciar. Las constantes son datos de configuración, no métodos; su valor real se fijará con SSI.

#### `Configurar(tipo: TipoProceso): void`

- **Características:** público y estático; recibe Servidor, ClienteJ1 o ClienteJ2, sin retorno.
- **Contrato:** exige primera configuración. Valor no definido produce `ArgumentOutOfRangeException`; segunda configuración, incluso con el mismo rol, `InvalidOperationException`, sin cambiar el anterior. Conserva la identidad para toda la ejecución, sin leer consola ni acceder a disco.
- **Por qué existe:** permite seleccionar responsabilidades y rutas de las entidades antes de construir el modelo, sin asumir un servidor por defecto ni reemplazar permisos del sistema operativo.

#### `GetTipoProceso(): TipoProceso`

- **Características:** público y estático, sin parámetros; retorna el rol conservado.
- **Contrato:** exige Configurar completado; antes produce `InvalidOperationException`. No modifica la identidad.
- **Por qué existe:** permite consultar el rol desde la configuración de entidades sin permitir su modificación.

#### `Iniciar(): void`

- **Características:** público y estático, sin parámetros ni retorno.
- **Precondiciones:** identidad configurada y modelo aún no inicializado; de lo contrario, `InvalidOperationException`.
- **Secuencia:** prepara una variable local con ConstructorModelo, registra entidades, construye el modelo y carga las tablas iniciales requeridas. Exige existencia del catálogo de personajes. Solo al completar todo asigna Modelo, sin publicar una carga parcial.
- **Errores y límites:** propaga errores de configuración, formato, construcción de personajes y acceso a archivos. Ante un fallo finaliza la ejecución sin reintentar. La configuración y creación inicial de tablas de intercambio quedan para la actividad 6.
- **Por qué existe:** establece el orden Configurar → Iniciar → uso de Modelo en un único punto de preparación.

### InterfazConsola

Presentación de los clientes, sin decidir ganadores ni modificar marcadores. Todos sus métodos son públicos y de instancia, como indica el UML. Usa Utilidades y MyConsole. El flujo del cliente controla qué información puede presentarse.

#### `MostrarMano(jugador: Jugador): void`

- **Características:** recibe al jugador cuya mano se mostrará; sin retorno.
- **Contrato:** nulo produce `ArgumentNullException`. Consulta GetCartas y presenta nombre, ASCII y seis atributos mediante MyConsole y DecoracionConsola. Mano vacía se informa sin abrir un menú vacío; no modifica cartas.
- **Por qué existe:** reúne la presentación de cartas disponibles, sin consultar por iniciativa propia la mano rival.

#### `PedirCarta(jugador: Jugador): Personaje`

- **Características:** recibe al jugador local; retorna una referencia de su mano disponible.
- **Contrato:** nulo produce `ArgumentNullException`; mano vacía, `InvalidOperationException` antes de abrir un menú. Obtiene una copia estable de GetCartas y transforma el índice desde cero devuelto por PedirOpcionMenu en la carta correspondiente. Una opción incorrecta se vuelve a pedir mediante Utilidades.
- **Resultado y errores:** no retira la carta, no llama a SetCarta ni publica archivos. El flujo registra después la elección. Fin de entrada propaga `EndOfStreamException`; otros fallos técnicos no se convierten en elecciones. Se supone una elección activa por cliente, sin cambios concurrentes de mano durante el menú.
- **Por qué existe:** separa la interacción de la aplicación de reglas del dominio.

#### `PedirAtributo(): TipoAtributo`

- **Características:** sin parámetros; retorna uno de los seis valores mediante PedirEnum<TipoAtributo>.
- **Contrato:** el flujo lo invoca solo cuando corresponde elegir; la firma no permite comprobar identidad o turno. Opciones incorrectas se vuelven a pedir; fin de entrada propaga `EndOfStreamException`. No registra el atributo en Combate ni comunica mensajes.
- **Por qué existe:** concentra la entrada tipada y evita conversiones dispersas desde texto.

#### `MostrarMarcador(j1: Jugador, j2: Jugador, ronda: int): void`

- **Características:** recibe participantes y número de ronda actual desde 1, sin retorno.
- **Contrato:** nulos producen `ArgumentNullException`; mismo jugador, `ArgumentException`; ronda menor que 1, `ArgumentOutOfRangeException`. Presenta nombres, rondas ganadas y número de ronda mediante MostrarTabla, sin cambiar contadores.
- **Por qué existe:** concentra la visualización del progreso sin calcular victorias.

#### `MostrarEsperando(mensaje: string): void`

- **Características:** recibe el texto de espera, sin retorno.
- **Contrato:** nulo produce `ArgumentNullException`; vacío o solo espacios, `ArgumentException`. Presenta mediante MyConsole; no pide teclado, consulta archivos ni ejecuta un bucle de espera.
- **Por qué existe:** informa al jugador sin confundir presentación con sincronización.

#### `MostrarCombate(combate: Combate): void`

- **Características:** recibe el enfrentamiento a presentar, sin retorno.
- **Contrato:** nulo produce `ArgumentNullException`; incompleto o sin resolver, `InvalidOperationException` antes de mostrar elecciones. Obtiene participantes, nombres, cartas, atributo, valores comparados y ganador existente. No llama a Resolver ni cambia puntuaciones.
- **Por qué existe:** presenta un resultado coherente sin revelar elecciones parciales. El flujo del cliente espera la publicación completa; no se exige secreto ante lectura manual de archivos.

#### `MostrarGanadorRonda(ganador: Jugador): void`

- **Características:** recibe al ganador, sin retorno.
- **Contrato:** nulo produce `ArgumentNullException`. El flujo debe haber determinado el ganador: la firma no recibe Ronda para comprobarlo. Presenta su nombre sin sumar victorias.
- **Por qué existe:** separa el anuncio de la lógica que determina y registra la ronda ganada.

#### `MostrarGanadorPartida(ganador: Jugador): void`

- **Características:** recibe al ganador final, sin retorno.
- **Contrato:** nulo produce `ArgumentNullException`. El flujo suministra el ganador normal devuelto por Partida o comunicado por el servidor. Presenta el anuncio sin recalcular el umbral ni cerrar por sí mismo los otros procesos.
- **Por qué existe:** concentra la presentación final sin duplicar reglas ni acoplarla al protocolo.

#### `MostrarError(mensaje: string): void`

- **Características:** recibe el texto del error, sin retorno.
- **Contrato:** nulo produce `ArgumentNullException`; vacío o solo espacios, `ArgumentException`. Delega en la función de error de Utilidades. No recupera la partida; el cierre corresponde al punto de ejecución. Si también falla la consola, el error técnico se propaga: no garantiza poder mostrarlo.
- **Por qué existe:** unifica el formato de errores sin confundir informar con finalizar los procesos.

### DataBases::Modelo — clase externa representada en el UML

Pertenece a la biblioteca y no se reimplementa. Cada proceso tiene su instancia mediante Datos.Modelo después de Iniciar. Se especifican las tres operaciones que muestra el UML; el resto de su API se consulta en el README de DataBases.

#### `Cargar<T>(exigirArchivo: bool = false): Tabla<T>`

- **Características:** público, de instancia y genérico; T identifica la entidad registrada; el booleano indica si exige el archivo. Retorna la tabla cargada.
- **Contrato:** exige modelo construido y entidad registrada; uso incompatible produce `InvalidOperationException`. Archivo ausente con false admite tabla vacía; con true produce `FileNotFoundException`. Una recarga conserva la instancia de Tabla pero reemplaza sus elementos; si falla la lectura conserva el contenido anterior. Propaga errores de formato (`CsvFormatException`), construcción de entidades y acceso o bloqueo (`IOException`, entre otros).
- **Por qué existe:** actualiza explícitamente la vista local de los archivos; no hay actualización automática. El catálogo exige existencia y no se recarga durante la partida; la política de intercambio queda en la actividad 6.

#### `Tabla<T>(): Tabla<T>`

- **Características:** público, de instancia y genérico, sin parámetros de valor; retorna la tabla viva de T en memoria, no una copia.
- **Contrato:** exige entidad registrada y tabla cargada; en otro caso produce `InvalidOperationException`. No lee disco. Las modificaciones en la tabla no se publican hasta Guardar.
- **Por qué existe:** permite consultar o modificar datos cargados sin agregar métodos de Datos que solo reenvíen llamadas.

#### `Guardar<T>(): void`

- **Características:** público, de instancia y genérico, sin parámetros de valor ni retorno; T identifica la tabla a publicar.
- **Contrato:** exige tabla cargada y que el proceso pueda escribir ese destino. Reemplaza todo el contenido del archivo con la tabla actual; no constituye una transacción entre archivos. Entidad desconocida o tabla sin cargar produce `InvalidOperationException`; cambios externos desde la carga, `CsvConcurrenciaException`; problemas de acceso o bloqueo propagan sus errores técnicos, incluidos `IOException`. El juego finaliza ante el fallo, aunque la biblioteca admita recargar y reintentar.
- **Por qué existe:** publica elecciones o resultados mediante la protección de archivos de DataBases, sin duplicar CSV. La restricción de un escritor por archivo pertenece al diseño del juego, no se deduce automáticamente de TipoProceso.

### DataBases::IEntidad<Personaje>

Interfaz externa sin instancias ni estado de carta. Su contrato requiere la siguiente operación, cuya implementación concreta se especifica en Personaje.

#### `Configurar(entidad: ConstructorEntidad<Personaje>): void`

- **Características:** público, estático y abstracto en la interfaz; recibe el constructor de configuración, sin retorno.
- **Contrato:** exige que Personaje proporcione archivo y mapeo de miembros compatibles. La interfaz no ejecuta una segunda configuración propia: comportamiento y validaciones corresponden a Personaje.Configurar.
- **Por qué existe:** permite que ConstructorModelo configure la entidad de forma tipada sin necesitar una instancia de Personaje.

### Enumeraciones y paquete externo

TipoAtributo contiene Fuerza, Velocidad, Resistencia, Inteligencia, Energia y Habilidad; TipoProceso contiene Servidor, ClienteJ1 y ClienteJ2. No declaran métodos en el UML. Delimitan opciones válidas sin usar textos ambiguos; convertir un entero no definido no lo convierte en una opción admitida.

Utilidades es un paquete externo que agrupa Funciones, DecoracionConsola, MyConsole, ColoresMenu y FormatoMenu. No se rediseñan esas clases ni se les inventan métodos: la delegación necesaria está descrita en InterfazConsola y el apartado de presentación.

## Datos y persistencia

Se usa la API pública de [DataBases](../Mis-Librerias/DataBases/README.md). Todos los miembros de Datos son estáticos. Modelo mantiene lectura pública y escritura privada y se representa exclusivamente mediante una asociación en el UML, con multiplicidad 0..1 porque no existe antes de una inicialización correcta. El uso de sus tablas requiere Iniciar completado; si falla el inicio no se continúa ni se publica un modelo parcial.

Datos declara las constantes públicas de tipo string `RUTA_PERSONAJES`, `RUTA_ELECCIONES_J1`, `RUTA_ELECCIONES_J2` y `RUTA_PUBLICACIONES_SERVIDOR`. Las clases persistidas pueden utilizarlas desde su Configurar para declarar Archivo. Son rutas absolutas al recurso compartido, iguales en todos los procesos que accedan al mismo archivo. Sus valores concretos se fijarán con la ubicación real de SSI; no se inventa aquí un servidor o una carpeta de red. Las cuatro constantes identifican destinos lógicos iniciales; el protocolo de la actividad 6 determinará si se necesitan más archivos o rutas.

`Configurar(tipo: TipoProceso): void` identifica al proceso como Servidor, ClienteJ1 o ClienteJ2 y conserva ese valor en el miembro privado tipoProceso. `GetTipoProceso(): TipoProceso` permite consultar esa identidad al seleccionar archivos en la configuración de las entidades. Antes de configurarse, tipoProceso está ausente (`[0..1]`); no se asume un servidor por defecto. Se rechazan valores no definidos y la consulta o inicialización sin configuración. La identidad queda fija para toda la ejecución: una segunda configuración se considera un error de uso, sin cambiar el modelo ni los archivos.

El punto de entrada determina el rol y llama primero a Configurar y después a `Iniciar(): void`. Configurar no solicita entradas de consola ni accede a disco. Iniciar registra las entidades mediante ConstructorModelo, construye el modelo y carga las tablas iniciales requeridas; publica Modelo después de que todas esas operaciones tengan éxito. Solo se inicializa una vez. Esto permite conocer la identidad antes de que Agregar<T> invoque el Configurar de cada entidad. El rol identifica permisos previstos, pero no reemplaza los permisos del recurso compartido.

El servidor solo publicará sus resultados y cada cliente solo sus elecciones; todos usan RUTA_PERSONAJES como origen de lectura. La configuración concreta de las tablas, selección de rutas por entidad y sus relaciones se completará en la actividad 6. No se asignan dos entidades al mismo archivo en un Modelo, restricción de DataBases.

Las llamadas `Datos.Modelo.Cargar<T>(exigirArchivo)`, `Tabla<T>()` y `Guardar<T>()` permiten recargar, consultar/modificar y publicar datos. No se agregan métodos que solo reenvíen estas operaciones. Los archivos obligatorios, como el catálogo inicial, se cargan exigiendo su existencia. Las tablas de intercambio necesitarán una política explícita de creación inicial en la actividad 6.

La biblioteca reconstruye objetos por sus constructores, interpreta tipos y permite configurar miembros privados. Las listas y objetos no se guardan como columnas: las relaciones usan claves configuradas y se resuelven mediante Uno/ Muchos. No se presupone que todo el grafo de Partida se pueda guardar automáticamente ni que todas las entidades del juego necesiten persistirse.

Cada proceso guarda únicamente sus propias tablas y carga las publicadas por otros; no se utiliza GuardarTodo indiscriminadamente. Guardar reemplaza la tabla completa del archivo. Las actualizaciones requieren Cargar explícito; las referencias a objetos anteriores pueden quedar desactualizadas tras una recarga. El catálogo se carga al iniciar y no se recarga en medio de una partida; la estrategia para incorporar mensajes al estado vivo se definirá en la actividad 6.

DataBases ya proporciona bloqueo por archivo, archivo temporal y reemplazo, además de detección de cambios. No ofrece transacciones entre archivos ni decide qué mensaje ya fue procesado. Su documentación no acredita pruebas en carpetas de red: la verificación en el recurso compartido de SSI sigue pendiente.

## Presentación con Utilidades

[Utilidades](../Mis-Librerias/Utilidades/README.md) aporta Funciones, DecoracionConsola y MyConsole; ColoresMenu y FormatoMenu configuran los menús. Utilidades se muestra como paquete externo en el UML; su uso se describe en esta sección, sin flechas de dependencia ni reproducción de sus implementaciones.

| Firma de InterfazConsola | Delegación |
|---|---|
| `MostrarMano(jugador: Jugador): void` | MyConsole para ASCII y DecoracionConsola para atributos. |
| `PedirCarta(jugador: Jugador): Personaje` | PedirOpcionMenu devuelve un índice desde cero sobre una copia de la mano disponible. |
| `PedirAtributo(): TipoAtributo` | PedirEnum<TipoAtributo> limita la elección a los seis valores. |
| `MostrarMarcador(j1: Jugador, j2: Jugador, ronda: int): void` | MostrarTabla presenta marcadores y ronda. |
| `MostrarEsperando(mensaje: string): void` | Mensaje de espera sin solicitar entradas. |
| `MostrarCombate(combate: Combate): void` | Presenta elecciones completas, atributo y ganador resuelto. |
| `MostrarGanadorRonda(ganador: Jugador): void` | Presenta el resultado de ronda. |
| `MostrarGanadorPartida(ganador: Jugador): void` | Presenta el resultado final. |
| `MostrarError(mensaje: string): void` | Mensaje de error de la biblioteca. |

El menú con FormatoMenu y flechas limpia la pantalla e historial. Para conservar la mano visible se puede usar SeleccionNumerica o incorporar el contexto al propio menú. La escritura de pantalla se hace mediante MyConsole para respetar su historial. La validación de opciones de entrada pertenece al cliente; una opción de teclado inválida se vuelve a pedir. Los fallos de lectura, persistencia o ejecución siguen la política de cierre completo sin recuperación.

## Decisiones pendientes y límites de cierre

- Actividad 4: especificaciones por clase y método redactadas a partir del UML, con características, contratos y justificación; pendientes de revisión del usuario. Los detalles de formatos e integración siguen en las actividades 5 y 6.
- Actividad 5: especificar columnas, ASCII, descripción, rutas y ejemplos del catálogo, coherentes con el formato propio de DataBases y el sistema externo.
- Actividad 6: definir registros de elecciones y publicaciones, identificación de partida/ronda/combate, referencias a personajes, permisos, secuencia, mensajes procesados y cierre coordinado ante fallos. Después completar sus entidades y relaciones en el UML y la integración de Partida y clientes. No se inventan aquí sus firmas.
La representación unificada y el intercambio mediante varios archivos son las decisiones vigentes del usuario. La consigna original se conserva sin modificar como referencia; Taller continúa fuera de alcance y la verificación técnica en red se mantiene en la actividad 6.
La actividad 3 está completada por confirmación del usuario: modelo de dominio y justificación revisados. Las entidades, relaciones, configuración en Datos y firmas de integración que resulten del protocolo son tareas de la actividad 6 y no condicionan este cierre. La integración del PR mantiene la revisión de un compañero exigida por CONTRIBUTING; no se confunde con una tarea de modelado.

Las cajas se ajustaron a las medidas del texto exportado por UMLet, con margen mínimo de lectura. Tras ampliar Datos y agregar TipoProceso se recalcularon las asociaciones y se verificó la ausencia de tramos dentro de cajas; la exportación SVG se comprobó para asegurar que los textos no excedan sus límites.

## Resultado de la revisión final del dominio

Se corrigió la pertenencia del jugador mano a Ronda, se explicitó la consulta de participantes de Combate para la presentación y se ajustó la multiplicidad de Modelo a su ciclo de inicialización. Se precisaron los contratos de incorporación de combates, resolución y recuperación completa de personajes. No se agregaron clases ni implementación. La descripción del personaje y los identificadores del catálogo siguen siendo definiciones de la actividad 5; las entidades de intercambio y el estado compartido corresponden a la actividad 6 y deberán reflejarse después en este modelo.
