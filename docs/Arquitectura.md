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

`Partida(j1: Jugador, j2: Jugador, mazo: Mazo)` recibe dos participantes distintos y su mazo. El número de ronda comienza en 1 al iniciar el juego. `Jugar(): Jugador` representa la coordinación completa en el servidor y devuelve al ganador solo en una terminación normal. Ante un error no devuelve un ganador ficticio: se propaga el fallo al punto de ejecución para finalizar. La integración con registros de elecciones y publicaciones queda pendiente de la actividad 6; no se presupone acceso directo del servidor a la consola de un cliente.

Los métodos privados son `Iniciar(): void`, `RepartirCartas(): void`, `JugarRonda(): void`, `RecogerCartas(): void`, `EstaTerminada(): bool` y `DeterminarGanadorPartida(): Jugador`. J1 comienza como mano. Al terminar cada ronda se devuelve al mazo tanto lo jugado en sus combates como lo restante en las manos, una sola vez. Si hay una ronda siguiente, se incrementa el número y se construye con el jugador mano que corresponde por paridad. Si alguien alcanzó 15 rondas, no se reparte otra ronda.

`Ronda(mano: Jugador, rival: Jugador)` identifica los participantes. `AgregarCombate(combate: Combate): void` admite combates resueltos de sus mismos participantes, una sola vez y con el elector correspondiente a su posición, mientras la ronda siga abierta. No se registra el mismo combate dos veces ni se reutilizan personajes ya jugados en ella. `EstaDefinida(): bool` comprueba dos victorias. `GetGanador(): Jugador` exige una ronda definida. `GetQuienEligeProximo(): Jugador` exige una ronda abierta: mano en el primer combate, rival en el segundo y mano en el tercero, independientemente de los resultados. La lista empieza vacía y puede contener hasta tres combates. `GetCombates(): Combate[]` devuelve una copia de los combates registrados para recuperar sus cartas al cerrar la ronda.

`Combate(quienElige: Jugador, rival: Jugador)` inicia el enfrentamiento. `SetAtributo(tipo: TipoAtributo): void` y `SetCarta(jugador: Jugador, personaje: Personaje): void` registran elecciones una sola vez. `EstaCompleto(): bool` exige atributo y ambas cartas. `Resolver(): Jugador` exige un combate completo, compara el atributo y asigna la victoria al rival del elector ante empate. Solo se aceptan elecciones de los dos participantes y valores definidos de TipoAtributo. Una vez resuelto no se modifican elecciones; Resolver no vuelve a producir efectos si ya se resolvió, sino que devuelve el resultado existente. `GetGanador(): Jugador` exige resolución previa. `GetAtributo(): TipoAtributo` y `GetCarta(jugador: Jugador): Personaje` permiten consultar las elecciones de un combate completo para presentar el resultado y recuperar las cartas. `GetParticipantes(): Jugador[]` devuelve una copia con los dos participantes en orden elector y rival; permite a MostrarCombate consultar sus nombres y cartas sin acceder a relaciones privadas ni depender de estado implícito de la interfaz. El control de revelación se mantiene en el flujo del cliente. El tratamiento exacto de llamadas inválidas se especificará en la actividad 4.

Las cartas retiradas de las manos siguen referenciadas por los combates, permitiendo recuperarlas al cerrar la ronda. La interfaz solo revela ambas cartas y el atributo cuando se completaron las dos elecciones; no se exige secreto ante la lectura manual de los archivos.

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

- Actividad 4: completar por escrito precondiciones, excepciones, secuencias y algoritmos sobre estas firmas; el borrador anterior deja de ser fuente de firmas vigentes.
- Actividad 5: especificar columnas, ASCII, descripción, rutas y ejemplos del catálogo, coherentes con el formato propio de DataBases y el sistema externo.
- Actividad 6: definir registros de elecciones y publicaciones, identificación de partida/ronda/combate, referencias a personajes, permisos, secuencia, mensajes procesados y cierre coordinado ante fallos. Después completar sus entidades y relaciones en el UML y la integración de Partida y clientes. No se inventan aquí sus firmas.
La representación unificada y el intercambio mediante varios archivos son las decisiones vigentes del usuario. La consigna original se conserva sin modificar como referencia; Taller continúa fuera de alcance y la verificación técnica en red se mantiene en la actividad 6.
La actividad 3 está completada por confirmación del usuario: modelo de dominio y justificación revisados. Las entidades, relaciones, configuración en Datos y firmas de integración que resulten del protocolo son tareas de la actividad 6 y no condicionan este cierre. La integración del PR mantiene la revisión de un compañero exigida por CONTRIBUTING; no se confunde con una tarea de modelado.

Las cajas se ajustaron a las medidas del texto exportado por UMLet, con margen mínimo de lectura. Tras ampliar Datos y agregar TipoProceso se recalcularon las asociaciones y se verificó la ausencia de tramos dentro de cajas; la exportación SVG se comprobó para asegurar que los textos no excedan sus límites.

## Resultado de la revisión final del dominio

Se corrigió la pertenencia del jugador mano a Ronda, se explicitó la consulta de participantes de Combate para la presentación y se ajustó la multiplicidad de Modelo a su ciclo de inicialización. Se precisaron los contratos de incorporación de combates, resolución y recuperación completa de personajes. No se agregaron clases ni implementación. La descripción del personaje y los identificadores del catálogo siguen siendo definiciones de la actividad 5; las entidades de intercambio y el estado compartido corresponden a la actividad 6 y deberán reflejarse después en este modelo.
