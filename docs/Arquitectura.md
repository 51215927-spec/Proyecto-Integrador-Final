# Arquitectura

Modelo de análisis y diseño de TI, actualizado el 8 de noviembre de 2026 a partir de las decisiones del usuario. Las clases del [UML](diagramas/UML.uxf) todavía no están implementadas; la aplicación conserva `Hello, World!`. Los requisitos están en [Reglas del juego](Reglas-del-juego.md) y el seguimiento en [Plan](Plan.md).

## Distribución y responsabilidades

El servidor coordina la partida y analiza los enfrentamientos. Los dos clientes solicitan elecciones y presentan los resultados. Cada proceso dispone de su propio `Datos.Modelo` en memoria: compartir archivos no comparte objetos ni actualiza automáticamente las tablas.

| Clase | Datos encapsulados | Responsabilidad y delegación |
|---|---|---|
| Personaje | Nombre, ASCII y seis atributos. | Representar una carta válida; ofrecer consultas y configurar su persistencia. No lee archivos directamente. |
| Mazo | Colección de personajes disponibles. | Mezclar, retirar elementos al repartir y reincorporarlos. Recibe objetos ya cargados. |
| Jugador | Nombre, mano disponible y rondas ganadas. | Administrar sus cartas y marcador. Las consultas devuelven una copia de la colección. |
| Partida | Dos jugadores, mazo, ronda actual, número de ronda y jugador mano. | Coordinar inicio, reparto, rondas, recuperación y victoria. Delega la comparación a Combate y el seguimiento de ronda a Ronda. |
| Ronda | Jugador mano, rival y combates registrados. | Determinar el siguiente elector y el ganador de ronda. Calcula las victorias desde los combates, evitando contadores duplicados. |
| Combate | Participantes, atributo, elecciones y ganador. | Registrar las elecciones y resolver la comparación. Los campos aún no elegidos y el ganador pendiente son anulables. |
| InterfazConsola | Sin estado de dominio propio obligatorio. | Construir las pantallas del juego y solicitar opciones usando Utilidades. No determina ganadores. |
| Datos (estática) | Modelo con escritura privada. | Inicializar el modelo de datos del proceso y ofrecer acceso a sus tablas. No aplica las reglas del juego. |

Partida conserva el jugador mano; Ronda recibe ese rol al construirse y lo mantiene durante sus combates. Jugador no conserva otra copia mutable del mismo rol. Las entidades del juego se conservan porque representan conceptos propios del problema y permiten explicar sus reglas.

## Notación de atributos y relaciones

Las referencias a objetos y colecciones de entidades se representan exclusivamente mediante asociaciones, sin duplicarlas en el compartimento de atributos. Cada extremo navegable indica el nombre del rol, su multiplicidad y su visibilidad: `-` privado y `+` público. Por ejemplo, Ronda se relaciona con Jugador mediante los roles privados `- jugadorMano` y `- jugadorRival`, cada uno con multiplicidad 1.

Los roles diferentes se dibujan como asociaciones distintas, incluso si apuntan a la misma clase. El ganador de Combate tiene multiplicidad 0..1 mientras no se resuelva; sus dos cartas también tienen 0..1 durante la elección. Datos tiene la asociación pública estática `+ Modelo`, con lectura pública y escritura privada. Las dependencias de uso y la realización de interfaces no representan atributos y no llevan visibilidad de miembro. Los valores simples y enumeraciones permanecen en el compartimento de atributos.

### Revisión de notación y distribución

Se consultó la [especificación UML 2.5.1 de OMG](https://www.omg.org/spec/UML/2.5.1/PDF): sección 9.4.4 para miembros estáticos y de solo lectura, 10.2.4 para enumeraciones y 11.5.4 para asociaciones. La [documentación de UMLet](https://www.umlet.com/faq.htm) define la representación mediante lt, r1/r2 y m1/m2 y la conexión de extremos a los bordes.

TipoAtributo utiliza la palabra clave «enumeration» y un literal por línea. Por convención solicitada por el usuario, cada literal lleva `+`. Los miembros estáticos se subrayan; la propiedad Modelo se representa en su asociación, subrayada, con acceso público y una aclaración de escritura privada. La realización de IEntidad apunta a la interfaz con triángulo hueco y línea discontinua. Las asociaciones navegables usan línea continua y punta abierta; las dependencias usan línea discontinua y punta abierta. Utilidades se representa como paquete y los nombres externos de DataBases se califican con `::`.

Las firmas conservan nombres de tipos y colecciones de C# para orientar la futura implementación; no constituyen una especificación completa de plantillas UML. El atributo opcional TipoAtributo de Combate expresa multiplicidad `[0..1]`.

El diagrama se exportó con UMLet 15.1 y se revisó visualmente. Se separaron los roles para evitar superponer sus etiquetas. También se comprobaron geométricamente las 21 relaciones: extremos en bordes, tramos ortogonales y ningún tramo dentro de cajas de clases, paquete o notas. Estas comprobaciones no acreditan todavía el protocolo de comunicación.

## Firmas y contratos del modelo

El UML contiene los tipos de todos los parámetros y retornos. Estos contratos de diseño orientan la actividad 4; no sustituyen sus especificaciones detalladas de comportamiento por escrito.

### Personaje

`Personaje(nombre: string, ascii: string, fuerza: int, velocidad: int, resistencia: int, inteligencia: int, energia: int, habilidad: int)` construye un objeto válido. `Validar(): void` es privado: exige seis valores entre 250 y 750, suma exactamente 3000, alguno de al menos 650 y alguno de como máximo 350. Se rechazan datos inválidos al construir; no hay modificación pública de atributos.

`GetNombre(): string`, `GetAscii(): string` y `GetAtributo(tipo: TipoAtributo): int` ofrecen las consultas necesarias. TipoAtributo enumera Fuerza, Velocidad, Resistencia, Inteligencia, Energia y Habilidad.

Implementa `IEntidad<Personaje>` de DataBases y declara `Configurar(entidad: ConstructorEntidad<Personaje>): void`, estático. La configuración declara archivo y columnas mediante expresiones sobre los campos privados. El formato concreto, ruta y posible necesidad de identificadores se completarán en la actividad 5 junto con las referencias necesarias para el intercambio de la actividad 6.

### Mazo y Jugador

`Mazo(personajes: List<Personaje>)` conserva una colección propia. `Mezclar(): void` cambia su orden. `Repartir(cantidad: int): Personaje[]` retira y devuelve elementos disponibles; no fabrica cartas ni devuelve cantidades parciales silenciosamente. `Devolver(personajes: Personaje[]): void` reincorpora elementos. `GetCantidad(): int` consulta los disponibles. Al iniciar, Partida exige al menos seis personajes válidos; el sistema externo garantiza un catálogo sin duplicados.

`Jugador(nombre: string)` crea una mano vacía y un marcador en cero. `RecibirCartas(personajes: Personaje[]): void` incorpora el reparto; `QuitarCarta(personaje: Personaje): void` retira una carta disponible. `EntregarCartas(): Personaje[]` devuelve las restantes y vacía la mano. `GetCartas(): Personaje[]` devuelve una copia. `SumarRondaGanada(): void`, `GetRondasGanadas(): int` y `GetNombre(): string` administran y consultan sus datos.

### Partida, Ronda y Combate

`Partida(j1: Jugador, j2: Jugador, mazo: Mazo)` recibe sus entidades. `Jugar(): Jugador` representa la coordinación completa en el servidor y devuelve al ganador solo en una terminación normal. Ante un error no devuelve un ganador ficticio: se propaga el fallo al punto de ejecución para finalizar. La integración con registros de elecciones y publicaciones queda pendiente de la actividad 6; no se presupone acceso directo del servidor a la consola de un cliente.

Los métodos privados son `Iniciar(): void`, `RepartirCartas(): void`, `JugarRonda(): void`, `RecogerCartas(): void`, `CambiarMano(): void`, `EstaTerminada(): bool` y `DeterminarGanadorPartida(): Jugador`. J1 comienza como mano. Al terminar cada ronda se devuelve al mazo tanto lo jugado en sus combates como lo restante en las manos, una sola vez; después se alterna el jugador mano. Si alguien alcanzó 15 rondas, no se reparte otra ronda.

`Ronda(mano: Jugador, rival: Jugador)` identifica los participantes. `AgregarCombate(combate: Combate): void` admite combates resueltos, en orden, mientras la ronda siga abierta. `EstaDefinida(): bool` comprueba dos victorias. `GetGanador(): Jugador` exige una ronda definida. `GetQuienEligeProximo(): Jugador` exige una ronda abierta: mano en el primer combate, rival en el segundo y mano en el tercero, independientemente de los resultados. La lista empieza vacía y puede contener hasta tres combates. `GetCombates(): Combate[]` devuelve una copia de los combates registrados para recuperar sus cartas al cerrar la ronda.

`Combate(quienElige: Jugador, rival: Jugador)` inicia el enfrentamiento. `SetAtributo(tipo: TipoAtributo): void` y `SetCarta(jugador: Jugador, personaje: Personaje): void` registran elecciones una sola vez. `EstaCompleto(): bool` exige atributo y ambas cartas. `Resolver(): Jugador` exige un combate completo, compara el atributo y asigna la victoria al rival del elector ante empate. Una vez resuelto no se modifican elecciones. `GetGanador(): Jugador` exige resolución previa. `GetAtributo(): TipoAtributo` y `GetCarta(jugador: Jugador): Personaje` permiten consultar las elecciones de un combate completo para presentar el resultado y recuperar las cartas. La interfaz conoce a los dos participantes; el control de revelación se mantiene en el flujo del cliente. El tratamiento exacto de llamadas inválidas se especificará en la actividad 4.

Las cartas retiradas de las manos siguen referenciadas por los combates, permitiendo recuperarlas al cerrar la ronda. La interfaz solo revela ambas cartas y el atributo cuando se completaron las dos elecciones; no se exige secreto ante la lectura manual de los archivos.

## Datos y persistencia

Se usa la API pública de [DataBases](../Mis-Librerias/DataBases/README.md). `Datos` es estática y expone `Modelo: Modelo { get; private set; }` e `Iniciar(): void`. Iniciar registra las entidades mediante ConstructorModelo, construye el modelo y carga las tablas iniciales requeridas para el proceso. La configuración concreta de las tablas de intercambio se completará en la actividad 6.

Las llamadas `Datos.Modelo.Cargar<T>(exigirArchivo)`, `Tabla<T>()` y `Guardar<T>()` permiten recargar, consultar/modificar y publicar datos. No se agregan métodos que solo reenvíen estas operaciones. Los archivos obligatorios, como el catálogo inicial, se cargan exigiendo su existencia. Las tablas de intercambio necesitarán una política explícita de creación inicial en la actividad 6.

La biblioteca reconstruye objetos por sus constructores, interpreta tipos y permite configurar miembros privados. Las listas y objetos no se guardan como columnas: las relaciones usan claves configuradas y se resuelven mediante Uno/ Muchos. No se presupone que todo el grafo de Partida se pueda guardar automáticamente ni que todas las entidades del juego necesiten persistirse.

Cada proceso guarda únicamente sus propias tablas y carga las publicadas por otros; no se utiliza GuardarTodo indiscriminadamente. Guardar reemplaza la tabla completa del archivo. Las actualizaciones requieren Cargar explícito; las referencias a objetos anteriores pueden quedar desactualizadas tras una recarga. El catálogo se carga al iniciar y no se recarga en medio de una partida; la estrategia para incorporar mensajes al estado vivo se definirá en la actividad 6.

DataBases ya proporciona bloqueo por archivo, archivo temporal y reemplazo, además de detección de cambios. No ofrece transacciones entre archivos ni decide qué mensaje ya fue procesado. Su documentación no acredita pruebas en carpetas de red: la verificación en el recurso compartido de SSI sigue pendiente.

## Presentación con Utilidades

[Utilidades](../Mis-Librerias/Utilidades/README.md) aporta Funciones, DecoracionConsola y MyConsole; ColoresMenu y FormatoMenu configuran los menús. En el UML se muestran como dependencias externas, sin reproducir sus implementaciones.

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

- Actividad 4: completar por escrito precondiciones, excepciones, secuencias y algoritmos sobre estas firmas; el borrador anterior deja de ser fuente de firmas vigentes.
- Actividad 5: especificar columnas, ASCII, descripción, rutas y ejemplos del catálogo, coherentes con el formato propio de DataBases y el sistema externo.
- Actividad 6: definir registros de elecciones y publicaciones, identificación de partida/ronda/combate, referencias a personajes, permisos, secuencia, mensajes procesados y cierre coordinado ante fallos. Después completar sus entidades y relaciones en el UML y la integración de Partida y clientes. No se inventan aquí sus firmas.
- Revisar con el docente la representación unificada de carta y personaje frente a la separación exigida para Taller, y con SSI el esquema de varios archivos compartidos. La consigna se conserva intacta y Taller continúa fuera de alcance.
- Obtener revisión de un compañero antes de integrar el PR. El modelo de dominio y las dependencias están actualizados; la actividad 3 no se considera cerrada integralmente mientras sus fronteras de comunicación y la revisión docente sigan pendientes.
