# Arquitectura

Modelo de análisis y diseño de TI, actualizado el 9 de noviembre de 2026 a partir de las decisiones del usuario. El modelo del [UML](diagramas/UML.uxf) no está implementado por completo; la aplicación conserva `Hello, World!`. La revisión del 13 de noviembre encontró código parcial de Personaje y Mazo, sin pruebas de comportamiento; Personaje aún no incluye descripcion ni configura su persistencia. Los requisitos están en [Reglas del juego](Reglas-del-juego.md) y el seguimiento en [Plan](Plan.md).

Las bibliotecas DataBases y Utilidades existen en el submódulo `Mis-Librerias/`, están referenciadas por la aplicación e incluidas en la solución principal. No hay flujo ejecutable de carga o comunicación conectado a ellas. Las llamadas descritas a continuación siguen siendo contratos del diseño.

El [Plan](Plan.md#actividad-6--archivo-de-partida-compartida) registra los avances posteriores de las ramas de protocolos por archivos y web, aún no integrados. Las secciones de comunicación de este documento describen la base de main: su política de cierre y el uso de DataBases para intercambio deberán conciliarse con el diseño JSON y la recuperación documentados en esas ramas, antes de integrar. No se adopta REST como sustituto del requisito académico por registrar su borrador.

## Distribución y responsabilidades

El servidor coordina la partida y analiza los enfrentamientos. Los dos clientes solicitan elecciones y presentan los resultados. Cada proceso dispone de su propio `Datos.Modelo` en memoria: compartir archivos no comparte objetos ni actualiza automáticamente las tablas.

| Clase | Datos encapsulados | Responsabilidad y delegación |
|---|---|---|
| Personaje | Nombre, descripción, ASCII y seis atributos. | Representar una carta válida; ofrecer consultas y configurar su persistencia. No lee archivos directamente. |
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

Las firmas indican parámetros y retornos. Salvo donde se indica estático, los métodos son de instancia. Las condiciones que garantiza el flujo se describen como precondiciones, sin exigir que cada método vuelva a comprobarlas. Los datos externos se validan al recibirlos: las opciones en la entrada, las reglas numéricas al construir Personaje y la disponibilidad de la carta en Combate.SetCarta. Los controles de los mensajes compartidos se definirán en la actividad 6. Los errores técnicos de entrada o archivos llegan al punto de ejecución para informar y finalizar.

### Personaje

Representa una carta válida con nombre, descripción, ASCII y seis atributos privados. No ofrece modificación pública de esos valores. Implementa `IEntidad<Personaje>` sin encargarse de leer archivos.

#### Constructor `Personaje(nombre: string, descripcion: string, ascii: string, fuerza: int, velocidad: int, resistencia: int, inteligencia: int, energia: int, habilidad: int)`

Es público y recibe el nombre, la descripción, el ASCII y los seis atributos. La descripción contiene el texto sobre la historia o las características del personaje y se conserva en el campo privado descripcion; es opcional, admite texto vacío y no tiene límite de longitud definido por el juego. La ausencia de descripción se representa mediante texto vacío (""), según la definición del usuario; se conserva el tipo string. No se utiliza null para representar esa ausencia. Comprueba que el nombre no sea nulo, vacío ni solo espacios y que el ASCII no sea nulo; si falta alguno, rechaza la construcción con ArgumentException. Guarda los datos y llama a Validar para comprobar las reglas numéricas. El ASCII se prepara con contenido, con un máximo de 25 líneas de 120 caracteres cada una y sin tabulaciones, conforme al formato de la actividad 5. Por indicación del usuario, no se agrega una validación de texto vacío; se conserva el rechazo de null ya especificado. Así, las demás clases pueden usar el personaje sin volver a validar sus datos.

#### `Validar(): void`

Es privado y comprueba que cada atributo esté entre 250 y 750, que los seis sumen 3000, que alguno sea mayor o igual a 650 y que alguno sea menor o igual a 350. Si no se cumple alguna condición, rechaza la construcción con ArgumentException e indica el motivo. No modifica los valores.

#### `GetNombre(): string`

Es público y devuelve el nombre del personaje para identificarlo, sin modificarlo.

#### `GetDescripcion(): string`

Es público y devuelve la descripción almacenada del personaje, sin modificarla ni exponer una operación de escritura. No define cuándo debe presentarse en consola; esa decisión sigue pendiente.

#### `GetAscii(): string`

Es público y devuelve el ASCII del personaje para que InterfazConsola lo muestre.

#### `GetAtributo(tipo: TipoAtributo): int`

Es público y recibe un TipoAtributo elegido entre las seis opciones del juego. Devuelve el valor de ese atributo para permitir la comparación de cartas. La opción se valida al recibir la entrada; este método solo consulta el valor.

#### `Configurar(entidad: ConstructorEntidad<Personaje>): void`

Es público y estático. Recibe el ConstructorEntidad que proporciona DataBases y configura el archivo Datos.RUTA_PERSONAJES y las columnas asociadas a los campos privados, incluida descripcion de tipo string. El constructor compatible recibe también descripcion para reconstruir el objeto sin utilizar setters. DataBases comprueba la configuración. El formato del catálogo está especificado en el apartado de la actividad 5 de este documento.

### Mazo

Administra una colección privada de personajes disponibles, su orden y sus transferencias; no controla marcadores ni compara atributos.

#### Constructor `Mazo(personajes: List<Personaje>)`

Es público y recibe los personajes cargados del catálogo. Copia la lista para que los cambios externos en ella no alteren el mazo. La carga del catálogo debe resolver datos faltantes o duplicados; el constructor no repite esas comprobaciones. La cantidad mínima para jugar se controla en Partida.Iniciar.

#### `Mezclar(): void`

Es público y cambia al azar el orden de las cartas disponibles, conservando las mismas cartas y su cantidad. Puede implementarse con Fisher–Yates. Se usa antes de repartir; con menos de dos cartas no necesita hacer cambios.

#### `Repartir(cantidad: int): Personaje[]`

Es público y recibe la cantidad de cartas que debe retirar. Devuelve esas cartas en un arreglo y las quita del comienzo del mazo. Partida lo llama con tres después de comprobar que hay suficientes cartas para ambos jugadores; el método no repite esa comprobación.

#### `Devolver(personajes: Personaje[]): void`

Es público y agrega al final del mazo las cartas recibidas. Partida reúne las cartas de la ronda y realiza una sola devolución al cerrarla. El método no necesita buscar duplicados porque el flujo de reparto y recogida conserva cada carta una sola vez.

#### `GetCantidad(): int`

Es público y devuelve la cantidad de cartas disponibles en el mazo, sin contar las que están en juego.

### Jugador

Conserva nombre, mano privada de hasta tres cartas y rondas ganadas. El rol de jugador mano pertenece a Ronda, no a Jugador.

#### Constructor `Jugador(nombre: string)`

Es público y recibe el nombre del jugador, comprobado al solicitarlo. Guarda el nombre, crea una mano vacía e inicia en cero el contador de rondas ganadas.

#### `RecibirCartas(personajes: Personaje[]): void`

Es público y agrega a la mano las cartas recibidas, conservando su orden. Partida entrega las tres cartas que obtuvo del mazo al comenzar la ronda, cuando la mano está vacía. Jugador mantiene su propia colección y no vuelve a comprobar el reparto.

#### `QuitarCarta(personaje: Personaje): void`

Es público y retira de la mano la carta indicada. Lo llama Combate.SetCarta después de comprobar que la carta está disponible, por lo que no repite esa validación. El coordinador no debe retirarla una segunda vez.

#### `EntregarCartas(): Personaje[]`

Es público y devuelve las cartas que quedan en la mano en un arreglo, dejando la mano vacía. Partida lo usa para recoger las cartas no jugadas; si no queda ninguna, devuelve un arreglo vacío.

#### `GetCartas(): Personaje[]`

Es público y devuelve una copia de la mano disponible para mostrar las cartas o comprobar una elección. Consultar la mano no retira cartas y modificar el arreglo devuelto no cambia la colección del jugador.

#### `SumarRondaGanada(): void`

Es público e incrementa en uno las rondas ganadas. Partida.JugarRonda lo llama una sola vez, después de obtener el ganador de la ronda.

#### `GetRondasGanadas(): int`

Es público y devuelve la cantidad de rondas ganadas, que comienza en cero. Permite mostrar el marcador y comprobar el final de la partida.

#### `GetNombre(): string`

Es público y devuelve el nombre del jugador para identificarlo en pantalla.

### Combate

Conserva participantes, elector, atributo, cartas y ganador. Las elecciones y el ganador empiezan ausentes. No suma rondas al marcador.

#### Constructor `Combate(quienElige: Jugador, rival: Jugador)`

Es público y recibe al jugador que elige el atributo y a su rival, determinados por Ronda. Guarda ambos participantes y deja pendientes el atributo, las cartas y el ganador.

#### `SetAtributo(tipo: TipoAtributo): void`

Es público y guarda el atributo elegido para el combate. Recibe una opción ya validada en la entrada y se llama una sola vez, antes de resolver el enfrentamiento. La coordinación del turno corresponde al flujo de la partida.

#### `SetCarta(jugador: Jugador, personaje: Personaje): void`

Es público y recibe al jugador y su carta elegida. Comprueba que el jugador participe en el combate, que todavía no haya registrado su carta y que esta siga en su mano. Si la elección no cumple esas condiciones, la rechaza sin cambiar el estado. Si es válida, llama a QuitarCarta y guarda la referencia en el combate. Esta es la comprobación de la elección; QuitarCarta no la repite. La recepción y validación de mensajes se definirán en la actividad 6.

#### `EstaCompleto(): bool`

Es público y devuelve true cuando ya están registrados el atributo y las dos cartas; mientras falte alguno devuelve false. Permite al coordinador saber cuándo puede resolver el combate.

#### `Resolver(): Jugador`

Es público y se llama cuando EstaCompleto devuelve true. Compara el atributo elegido de ambas cartas: gana quien eligió solo si su valor es mayor; en caso contrario gana el rival, incluido el empate. Guarda y devuelve el ganador. No modifica el marcador de rondas.

Ejemplo ilustrativo del núcleo de comparación, después de comprobar y extraer las elecciones completas. No es código implementado en la aplicación:

```csharp
int valorElector = cartaQueElige.GetAtributo(atributoElegido);
int valorRival = cartaRival.GetAtributo(atributoElegido);
ganador = valorElector > valorRival ? jugadorQueElige : jugadorRival;
```

#### `GetGanador(): Jugador`

Es público y devuelve el ganador guardado por Resolver. Se consulta después de resolver el combate, para contar las victorias o mostrar el resultado.

#### `GetAtributo(): TipoAtributo`

Es público y devuelve el atributo registrado. InterfazConsola lo consulta al presentar el resultado completo del combate.

#### `GetCarta(jugador: Jugador): Personaje`

Es público y recibe uno de los participantes para devolver su carta registrada. Se usa después de completar el combate, tanto para mostrar el resultado como para recuperar las cartas al cerrar la ronda. No retira ni devuelve cartas por sí mismo.

#### `GetParticipantes(): Jugador[]`

Es público y devuelve un arreglo con los dos participantes, primero quien elige y después su rival. Permite consultar sus nombres y cartas sin modificar los roles del combate.

### Ronda

Conserva jugador mano, rival y hasta tres combates registrados. Calcula victorias desde los resultados, sin contadores duplicados. El jugador mano permanece fijo durante la ronda.

#### Constructor `Ronda(mano: Jugador, rival: Jugador)`

Es público y recibe al jugador mano y a su rival, determinados por Partida según el número de ronda. Guarda esos roles y crea la colección vacía de combates.

#### `AgregarCombate(combate: Combate): void`

Es público y agrega al historial el combate que Partida acaba de resolver. Partida lo llama una sola vez por combate mientras la ronda siga abierta. La selección de participantes corresponde al coordinador y la disponibilidad de las cartas ya se comprobó en SetCarta; aquí solo se registra el resultado.

#### `EstaDefinida(): bool`

Es público y devuelve true cuando uno de los jugadores ganó dos de los combates registrados. Calcula las victorias a partir de sus ganadores. Permite cerrar la ronda después de dos combates si ganó el mismo jugador, o después del tercero si ganó uno cada uno.

#### `GetGanador(): Jugador`

Es público y devuelve al jugador que ganó dos combates. Partida lo consulta cuando EstaDefinida devuelve true y se encarga de sumar la ronda al marcador.

#### `GetQuienEligeProximo(): Jugador`

Es público y devuelve quién elige el atributo del siguiente combate: el jugador mano en el primero y tercero, y su rival en el segundo. Partida lo consulta mientras la ronda no está definida. El resultado de los combates no altera este orden.

#### `GetCombates(): Combate[]`

Es público y devuelve una copia de los combates registrados en orden de juego. Partida los consulta para reunir las cartas jugadas al cerrar la ronda.

### Partida

Coordina en el servidor a J1, J2, Mazo y la ronda actual. Conserva número de ronda y la constante privada estática de solo lectura RONDAS_PARA_GANAR = 15. Sus operaciones privadas organizan los pasos que ejecuta Jugar.

#### Constructor `Partida(j1: Jugador, j2: Jugador, mazo: Mazo)`

Es público y recibe los dos jugadores recién creados y el mazo cargado. Guarda esas referencias y deja pendiente la primera ronda. La preparación de los jugadores corresponde al inicio de la aplicación; el constructor no vuelve a revisar sus manos ni sus marcadores.

#### `Jugar(): Jugador`

Es público y devuelve al ganador de la partida. Se llama una vez: inicia la partida, juega una ronda y recoge sus cartas. Si algún jugador llegó a 15 rondas ganadas, devuelve ese ganador. Si no, aumenta el número de ronda, crea la siguiente con J1 como mano en las impares y J2 en las pares, reparte y continúa. Los errores se propagan al punto de ejecución para informar y finalizar. La comunicación con los clientes se completará en la actividad 6.

#### `Iniciar(): void`

Es privado y prepara la primera ronda. Comprueba que el mazo tenga al menos seis cartas; si no, informa «No se puede jugar porque no hay suficientes cartas» mediante InvalidOperationException. Si alcanza, fija el número de ronda en 1, crea la ronda con J1 como mano y llama a RepartirCartas. Esta comprobación se realiza al comenzar: cada cierre devuelve las seis cartas antes del siguiente reparto.

#### `RepartirCartas(): void`

Es privado y prepara las manos de una ronda nueva. Mezcla el mazo, retira tres cartas para cada jugador con Repartir y las entrega con RecibirCartas. Iniciar garantiza la cantidad inicial y RecogerCartas restituye las cartas entre rondas, de modo que no hace falta repetir esos controles.

#### `JugarRonda(): void`

Es privado y juega la ronda actual. Mientras no está definida, consulta quién elige, crea un Combate con ese jugador y su rival, registra el atributo y las cartas recibidas, espera a que EstaCompleto devuelva true, resuelve y agrega el combate a la ronda. Al terminar, obtiene el ganador y llama una vez a SumarRondaGanada. SetCarta ya retira las cartas de las manos. La espera y el intercambio con los clientes se definirán en la actividad 6.

#### `RecogerCartas(): void`

Es privado y se llama una vez al cerrar cada ronda, incluida la última. Reúne las dos cartas de cada combate y las que quedan en ambas manos mediante EntregarCartas; luego devuelve el conjunto al mazo. Siempre recupera seis cartas: cuatro jugadas y dos restantes si hubo dos combates, o seis jugadas si hubo tres. Las manos quedan vacías para el siguiente reparto.

#### `EstaTerminada(): bool`

Es privado y devuelve true si alguno de los jugadores alcanzó RONDAS_PARA_GANAR, cuyo valor es 15. Se consulta después de registrar el ganador de cada ronda.

#### `DeterminarGanadorPartida(): Jugador`

Es privado y devuelve al jugador que llegó a 15 rondas ganadas. Jugar lo llama después de que EstaTerminada devuelve true. Como cada ronda suma una victoria a un solo jugador y la partida termina al alcanzar el umbral, no es necesario volver a comprobar si ambos o ninguno ganaron.

### Datos

Clase estática, sin instancias. Conserva tipoProceso privado opcional y las rutas públicas constantes del UML. Modelo permite lectura pública y escritura privada y no está disponible hasta completar Iniciar. Las constantes son datos de configuración, no métodos; su valor real se fijará con SSI.

#### `Configurar(tipo: TipoProceso): void`

Es público y estático. Recibe el rol elegido y validado al iniciar la aplicación: Servidor, ClienteJ1 o ClienteJ2. Lo guarda para seleccionar las responsabilidades y rutas del proceso. El punto de entrada lo llama una sola vez, antes de Iniciar.

#### `GetTipoProceso(): TipoProceso`

Es público y estático. Devuelve el rol guardado por Configurar para que las entidades puedan elegir sus rutas. Se consulta después de configurar el proceso.

#### `Iniciar(): void`

Es público y estático. Se llama una vez, después de Configurar. Registra las entidades con ConstructorModelo, construye el modelo y carga las tablas iniciales, exigiendo que exista el catálogo de personajes. Asigna Modelo cuando la preparación termina correctamente. Los errores de configuración, datos o archivos se propagan al punto de ejecución, que los informa y finaliza. Las tablas de intercambio se completarán en la actividad 6.

### InterfazConsola

Presentación de los clientes, sin decidir ganadores ni modificar marcadores. Todos sus métodos son públicos y de instancia, como indica el UML. Usa Utilidades y MyConsole. El flujo del cliente controla qué información puede presentarse.

#### `MostrarMano(jugador: Jugador): void`

Recibe al jugador y muestra su mano: nombre, ASCII y seis atributos de cada carta, usando MyConsole y DecoracionConsola. Si está vacía, lo informa. Solo presenta los datos que recibe del flujo del cliente.

#### `PedirCarta(jugador: Jugador): Personaje`

Recibe al jugador local y devuelve la carta que selecciona de su mano. El flujo lo llama cuando tiene cartas disponibles. Obtiene una copia de GetCartas, muestra el menú y usa el índice devuelto por PedirOpcionMenu para obtener la carta. Utilidades vuelve a pedir las opciones incorrectas. El método no retira la carta: SetCarta registra y consume la elección.

#### `PedirAtributo(): TipoAtributo`

Devuelve uno de los seis atributos mediante PedirEnum<TipoAtributo>, que vuelve a solicitar la entrada cuando es incorrecta. El flujo del cliente lo llama cuando corresponde elegir y luego comunica la opción.

#### `MostrarMarcador(j1: Jugador, j2: Jugador, ronda: int): void`

Recibe a los dos jugadores y el número de ronda. Muestra sus nombres, las rondas ganadas y la ronda actual mediante MostrarTabla. Usa los datos preparados por Partida sin volver a validarlos.

#### `MostrarEsperando(mensaje: string): void`

Recibe un mensaje de espera y lo muestra mediante MyConsole. La consulta de archivos y la espera de respuestas corresponden al flujo del cliente.

#### `MostrarCombate(combate: Combate): void`

Recibe un combate resuelto y presenta los participantes, sus cartas, el atributo comparado, los valores y el ganador. El flujo del cliente lo llama cuando dispone del resultado completo; el método no vuelve a resolverlo ni modifica los marcadores.

#### `MostrarGanadorRonda(ganador: Jugador): void`

Recibe al ganador de la ronda, ya determinado por el flujo, y muestra su nombre. La actualización del marcador corresponde a Partida.

#### `MostrarGanadorPartida(ganador: Jugador): void`

Recibe al ganador final, devuelto por Partida o comunicado por el servidor, y muestra el anuncio de victoria.

#### `MostrarError(mensaje: string): void`

Recibe el mensaje de error y lo presenta mediante la función de error de Utilidades. El punto de ejecución decide el cierre de la aplicación.

### DataBases::Modelo — clase externa representada en el UML

Pertenece a la biblioteca y no se reimplementa. Cada proceso tiene su instancia mediante Datos.Modelo después de Iniciar. Se especifican las tres operaciones que muestra el UML; el resto de su API se consulta en el README de DataBases.

#### `Cargar<T>(exigirArchivo: bool = false): Tabla<T>`

Es público y genérico. Carga desde el archivo la tabla de la entidad T registrada en el modelo y la devuelve. Con exigirArchivo en true, un archivo ausente produce FileNotFoundException; con false permite una tabla vacía. Al recargar, conserva la instancia de Tabla y reemplaza sus elementos; si falla la lectura conserva el contenido anterior. Los errores de formato, construcción o acceso a archivos se propagan. El catálogo se carga al iniciar; la recarga de mensajes se definirá en la actividad 6.

#### `Tabla<T>(): Tabla<T>`

Es público y genérico. Devuelve la tabla de T previamente cargada, con sus datos en memoria. No es una copia: los cambios afectan esa tabla y se publican al llamar a Guardar. No lee el archivo de nuevo.

#### `Guardar<T>(): void`

Es público y genérico. Escribe en el archivo todo el contenido de la tabla cargada de T. Si detecta cambios externos desde la carga, DataBases lanza CsvConcurrenciaException; los errores de acceso también se propagan. El punto de ejecución informa el fallo y finaliza. Cada proceso guarda solo sus propias tablas; no hay una transacción entre archivos.

### DataBases::IEntidad<Personaje>

Interfaz externa sin instancias ni estado de carta. Su contrato requiere la siguiente operación, cuya implementación concreta se especifica en Personaje.

#### `Configurar(entidad: ConstructorEntidad<Personaje>): void`

Es público, estático y abstracto. Recibe el ConstructorEntidad con el que Personaje debe declarar su archivo y sus columnas. ConstructorModelo utiliza este contrato para configurar la entidad sin crear un personaje. La implementación se describe en Personaje.Configurar.

### Enumeraciones y paquete externo

TipoAtributo contiene Fuerza, Velocidad, Resistencia, Inteligencia, Energia y Habilidad; TipoProceso contiene Servidor, ClienteJ1 y ClienteJ2. No declaran métodos en el UML. Las entradas se convierten a estas opciones después de validarlas; los métodos que las reciben no repiten ese control.

Utilidades es un paquete externo que agrupa Funciones, DecoracionConsola, MyConsole, ColoresMenu y FormatoMenu. No se rediseñan esas clases ni se les inventan métodos: la delegación necesaria está descrita en InterfazConsola y el apartado de presentación.

## Datos y persistencia

Se usa la API pública de [DataBases](../Mis-Librerias/DataBases/README.md). Todos los miembros de Datos son estáticos. Modelo mantiene lectura pública y escritura privada y se representa exclusivamente mediante una asociación en el UML, con multiplicidad 0..1 porque no existe antes de una inicialización correcta. El uso de sus tablas requiere Iniciar completado; si falla el inicio no se continúa ni se publica un modelo parcial.

Datos declara las constantes públicas de tipo string `RUTA_PERSONAJES`, `RUTA_ELECCIONES_J1`, `RUTA_ELECCIONES_J2` y `RUTA_PUBLICACIONES_SERVIDOR`. Las clases persistidas pueden utilizarlas desde su Configurar para declarar Archivo. Son rutas absolutas al recurso compartido, iguales en todos los procesos que accedan al mismo archivo. Sus valores concretos se fijarán con la ubicación real de SSI; no se inventa aquí un servidor o una carpeta de red. Las cuatro constantes identifican destinos lógicos iniciales; el protocolo de la actividad 6 determinará si se necesitan más archivos o rutas.

`Configurar(tipo: TipoProceso): void` identifica al proceso como Servidor, ClienteJ1 o ClienteJ2 y conserva ese valor en el miembro privado tipoProceso. `GetTipoProceso(): TipoProceso` permite consultar esa identidad al seleccionar archivos en la configuración de las entidades. Antes de configurarse, tipoProceso está ausente (`[0..1]`); no se asume un servidor por defecto. El punto de entrada valida el rol y configura el proceso una sola vez, antes de inicializar el modelo. La identidad se conserva durante toda la ejecución.

El punto de entrada determina el rol y llama primero a Configurar y después a `Iniciar(): void`. Configurar no solicita entradas de consola ni accede a disco. Iniciar registra las entidades mediante ConstructorModelo, construye el modelo y carga las tablas iniciales requeridas; publica Modelo después de que todas esas operaciones tengan éxito. Solo se inicializa una vez. Esto permite conocer la identidad antes de que Agregar<T> invoque el Configurar de cada entidad. El rol identifica permisos previstos, pero no reemplaza los permisos del recurso compartido.

El servidor solo publicará sus resultados y cada cliente solo sus elecciones; todos usan RUTA_PERSONAJES como origen de lectura. La configuración concreta de las tablas, selección de rutas por entidad y sus relaciones se completará en la actividad 6. No se asignan dos entidades al mismo archivo en un Modelo, restricción de DataBases.

Las llamadas `Datos.Modelo.Cargar<T>(exigirArchivo)`, `Tabla<T>()` y `Guardar<T>()` permiten recargar, consultar/modificar y publicar datos. No se agregan métodos que solo reenvíen estas operaciones. Los archivos obligatorios, como el catálogo inicial, se cargan exigiendo su existencia. Las tablas de intercambio necesitarán una política explícita de creación inicial en la actividad 6.

La biblioteca reconstruye objetos por sus constructores, interpreta tipos y permite configurar miembros privados. Las listas y objetos no se guardan como columnas: las relaciones usan claves configuradas y se resuelven mediante Uno/ Muchos. No se presupone que todo el grafo de Partida se pueda guardar automáticamente ni que todas las entidades del juego necesiten persistirse.

Cada proceso guarda únicamente sus propias tablas y carga las publicadas por otros; no se utiliza GuardarTodo indiscriminadamente. Guardar reemplaza la tabla completa del archivo. Las actualizaciones requieren Cargar explícito; las referencias a objetos anteriores pueden quedar desactualizadas tras una recarga. El catálogo se carga al iniciar y no se recarga en medio de una partida; la estrategia para incorporar mensajes al estado vivo se definirá en la actividad 6.

DataBases ya proporciona bloqueo por archivo, archivo temporal y reemplazo, además de detección de cambios. No ofrece transacciones entre archivos ni decide qué mensaje ya fue procesado. Su documentación no acredita pruebas en carpetas de red: la verificación en el recurso compartido de SSI sigue pendiente.

## Formato de archivos de personajes — actividad 5

**Completada por confirmación del usuario.** Se documentan las definiciones acordadas y las condiciones de DataBases. El cierre corresponde al análisis y diseño; no implica implementación.

### Entidades persistentes y columnas

El usuario indicó que las clases persistentes serán Personaje y Accion y que las columnas corresponderán a sus atributos. Cada miembro persistente debe declararse mediante Columna en la configuración de su entidad; DataBases no descubre ni guarda automáticamente todos los miembros. El contrato de Accion y su integración con las elecciones de los clientes y las publicaciones del servidor quedan pendientes de definición en la actividad 6.

Personaje tiene actualmente los siguientes campos en el modelo, sin agregar otros por inferencia:

| Campo / columna | Tipo | Condición ya definida |
|---|---|---|
| nombre | string | No nulo, vacío ni compuesto solo por espacios. |
| descripcion | string | Opcional y sin límite de longitud. La ausencia se representa con texto vacío (""), no con null. |
| ascii | string | No nulo; máximo 25 líneas y 120 caracteres por línea; sin tabulaciones. Se entrega con contenido, pero no se valida si está vacío. |
| fuerza | int | Entre 250 y 750, inclusive. |
| velocidad | int | Entre 250 y 750, inclusive. |
| resistencia | int | Entre 250 y 750, inclusive. |
| inteligencia | int | Entre 250 y 750, inclusive. |
| energia | int | Entre 250 y 750, inclusive. |
| habilidad | int | Entre 250 y 750, inclusive. |

Los seis atributos numéricos deben sumar exactamente 3000, incluir al menos uno mayor o igual a 650 y al menos uno menor o igual a 350. La validación pertenece al constructor de Personaje mediante Validar, según el contrato de la actividad 4.

### Reglas del formato proporcionadas por DataBases

El archivo usa el formato propio descrito en el [README de DataBases](../Mis-Librerias/DataBases/README.md#3-el-formato-del-archivo-csv): encabezado con los nombres de columnas, separador coma y un objeto por línea física. La biblioteca asocia las columnas por nombre al leer, por lo que admite reordenarlas. Al guardar utiliza UTF-8 sin BOM y escribe los textos entre comillas.

Dentro de los textos, las barras, comillas, saltos de línea, retornos de carro y tabulaciones se representan mediante los escapes de DataBases. Un ASCII con varios renglones puede almacenarse como texto en una sola línea física usando saltos escapados. El límite confirmado es de 25 líneas de contenido y 120 caracteres por línea; los saltos escapados permiten representar esas líneas dentro del registro. No se permiten caracteres de tabulación en el ASCII, aunque DataBases pueda representarlos mediante su escape. El usuario garantiza que el ASCII tendrá contenido y pidió no validar si está vacío; esta decisión no elimina el rechazo de null ya establecido en el constructor. La descripción se incorpora como texto en la columna descripcion y utiliza los mismos escapes de DataBases. Cuando no hay descripción, la celda se escribe como ""; la columna permanece en el encabezado y en cada registro.

La palabra null sin comillas representa ausencia de valor; las comillas vacías representan texto vacío y la palabra null entre comillas representa texto literal. DataBases distingue esos valores; las restricciones de Personaje determinan cuáles son admisibles.

### Lectura y errores ya especificados

El catálogo se carga al iniciar mediante Cargar<Personaje>(exigirArchivo: true). Si falta, se produce FileNotFoundException. DataBases informa encabezados inválidos, cantidad incorrecta de celdas, conversiones imposibles y datos rechazados por el constructor mediante CsvFormatException. Los errores de entrada y salida o de permisos se propagan sin envolver. Conforme a la política vigente, el punto de ejecución informa el error y finaliza el juego completo, sin recuperación.

DataBases no impone unicidad de personajes. Por confirmación del usuario, el catálogo estará preparado antes de la ejecución y él garantizará que no contenga duplicados. La carga no queda a cargo del usuario final del juego; no se incorpora un mecanismo de detección de duplicados ni un identificador adicional por este motivo.

La conversión de la información de los personajes al catálogo CSV se definirá posteriormente. El usuario considera como posibilidades un script o solicitar la generación del CSV a partir de los datos, sin haber elegido todavía una alternativa. No se exige por ahora que cada alumno entregue un CSV individual ni se define un formato de conversión.

### Aspectos posteriores que no condicionan el cierre

- Descripción: presentación en consola. Su contenido es opcional, no tiene límite de longitud y su ausencia se representa con texto vacío ("").

El usuario confirmó el cierre de la actividad 5 con la especificación actual. No se agregaron ejemplos propios de personajes válidos e inválidos; no se presentan como realizados ni se mantienen como condición para reabrir la actividad. La conversión al CSV permanece postergada por indicación del usuario. Las rutas reales del recurso compartido y el protocolo de Accion siguen pendientes de la actividad 6.

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

- Actividad 4: completada por confirmación del usuario el 9 de noviembre de 2026. Las 58 especificaciones se revisaron para mejorar la lectura y evitar validaciones repetidas. Los detalles de formatos e integración siguen en las actividades 5 y 6.
- Actividad 5: completada por confirmación del usuario. Columnas, descripción, ASCII y lectura especificados; conversión al CSV postergada. Las rutas reales corresponden a la actividad 6.
- Actividad 6: definir registros de elecciones y publicaciones, identificación de partida/ronda/combate, referencias a personajes, permisos, secuencia, mensajes procesados y cierre coordinado ante fallos. Después completar sus entidades y relaciones en el UML y la integración de Partida y clientes. No se inventan aquí sus firmas.
La representación unificada y el intercambio mediante varios archivos son las decisiones vigentes del usuario. La consigna original se conserva sin modificar como referencia; Taller continúa fuera de alcance y la verificación técnica en red se mantiene en la actividad 6.
La actividad 3 está completada por confirmación del usuario: modelo de dominio y justificación revisados. Las entidades, relaciones, configuración en Datos y firmas de integración que resulten del protocolo son tareas de la actividad 6 y no condicionan este cierre. El modelo y las especificaciones quedaron integrados mediante los PR #9 y #10. Los nuevos PR mantienen la revisión de un compañero exigida por CONTRIBUTING; no se confunde con una tarea de modelado.

Las cajas se ajustaron a las medidas del texto exportado por UMLet, con margen mínimo de lectura. Tras ampliar Datos y agregar TipoProceso se recalcularon las asociaciones y se verificó la ausencia de tramos dentro de cajas; la exportación SVG se comprobó para asegurar que los textos no excedan sus límites.

## Resultado de la revisión final del dominio

Se corrigió la pertenencia del jugador mano a Ronda, se explicitó la consulta de participantes de Combate para la presentación y se ajustó la multiplicidad de Modelo a su ciclo de inicialización. Se precisaron los contratos de incorporación de combates, resolución y recuperación completa de personajes. No se agregaron clases ni implementación. La incorporación posterior de la descripción a Personaje, su constructor, GetDescripcion y la columna descripcion se documenta en la actividad 5 por indicación del usuario. La descripción es opcional y no tiene límite de longitud; su ausencia se representa con texto vacío ("") y su presentación sigue pendiente. No se ha agregado un identificador al catálogo; las entidades de intercambio y el estado compartido corresponden a la actividad 6 y deberán reflejarse después en este modelo.
