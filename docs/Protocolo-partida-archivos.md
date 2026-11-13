# Protocolo de partida compartida — actividad 6

Especificación de TI, sin implementación. Decisiones funcionales confirmadas por el usuario el 12 de noviembre de 2026; los mecanismos, formato y contratos siguientes son una propuesta técnica para revisión. Complementa [Arquitectura](Arquitectura.md), [Reglas](Reglas-del-juego.md) y [Plan](Plan.md). La consigna solicita un archivo compartido; por decisión expresa del usuario se utilizan varios archivos con un escritor por historial.

## Decisiones vigentes

Windows, carpeta de red preparada antes de ejecutar y tres roles elegibles al abrir el programa: Servidor, ClienteJ1 y ClienteJ2. No hay asignación automática por orden de apertura. El servidor dirige el juego; los clientes muestran información y devuelven las elecciones solicitadas. Solo el servidor inicia explícitamente otra partida y únicamente con los tres procesos presentes. Los historiales se borran solo entonces.

Cada proceso conserva su historial de acciones JSON por anexado. El servidor guarda por separado una fotografía completa de la partida en estado.json. Si falta un proceso se suspende el avance, se esperan cinco segundos y se ofrece abandonar o seguir esperando. Se repite sin límite; si vuelve el ausente se retoma el estado confirmado. Abandonar solo cierra el proceso local. Cierre voluntario y error tienen el mismo efecto. Cuando deja de existir cualquier proceso activo, la partida deja de ser recuperable aunque queden historiales. El ganador normal sigue siendo quien alcanza 15 rondas.

Estas decisiones sustituyen la política anterior de cierre completo inmediato sin recuperación. No se autoriza implementación de Taller.

## Archivos y permisos

| Archivo en la misma carpeta de red | Escritor | Lectores / uso |
|---|---|---|
| Catálogo de personajes CSV, nombre real pendiente | Sistema externo, antes de jugar | Los tres procesos mediante DataBases |
| server.log | Servidor | Los tres; registro autoritativo de partida |
| jugador1.log | ClienteJ1 | Los tres; solicitudes respondidas y confirmaciones |
| jugador2.log | ClienteJ2 | Los tres; solicitudes respondidas y confirmaciones |
| estado.json | Servidor | Recuperación del servidor; los clientes reciben una presentación preparada, no interpretan este estado |
| server.lock, jugador1.lock, jugador2.lock | Titular del rol | Reserva exclusiva durante la ejecución; los demás prueban disponibilidad |
| control.lock | Un proceso por vez, brevemente | Serializa altas, bajas, consultas de pertenencia e inicio de partida |
| sesion.lock | Todos los procesos de la sesión | Reserva compartida durante su ejecución; permite detectar ausencia total |

La ruta raíz será una configuración absoluta común, todavía sin valor real. Datos conserva RUTA_PERSONAJES, RUTA_ELECCIONES_J1, RUTA_ELECCIONES_J2 y RUTA_PUBLICACIONES_SERVIDOR, apuntando respectivamente al CSV y a los tres logs; agrega RUTA_ESTADO, RUTA_CONTROL y RUTA_SESION. Los locks de rol se derivan del nombre del rol. No confundir estos archivos con los locks temporales internos del CSV de DataBases. Los procesos necesitan leer todos los logs, escribir el propio y abrir los locks; los clientes solo vacían su propio log por orden de nueva partida. El servidor nunca trunca el log ajeno directamente. Los permisos de la carpeta deben permitir estas operaciones; el protocolo comprueba el rol, no proporciona seguridad contra edición manual.

Un lock se reconoce por la reserva activa del sistema operativo, nunca por la existencia del archivo. Un acceso denegado o fallo de red no equivale a un rol libre. El titular mantiene reservado el .lock de rol durante su ejecución y es el único que anexa acciones a su .log. Los lectores pueden leer mientras se escribe y solo aceptan líneas completas. DataBases interviene únicamente en el catálogo CSV, con sus propios bloqueos temporales. Los modos concretos y su comportamiento SMB deben verificarse en Windows antes de dar por validado este diseño.

## Sesión activa y adquisición de roles

Propuesta para resolver el caso en que todos los procesos mueren sin poder escribir un cierre: bajo control.lock, cada nuevo proceso intenta reservar sesion.lock de forma exclusiva. Si puede, no hay supervivientes y los historiales anteriores quedan invalidados para recuperación; crea un identificador de sesión nuevo y convierte su reserva en compartida sin soltar control.lock. Si no puede por una reserva compartida activa, se incorpora a la sesión vigente y toma también una reserva compartida. Los procesos mantienen esa reserva aunque estén esperando al resto.

El identificador de sesión se conserva en el contenido de sesion.lock, escrito solo al crear sesión y leído por quienes se incorporan. El archivo no se elimina ni se recrea mientras haya titulares. La transición exclusiva/compartida queda protegida por control.lock y debe ensayarse en SMB. Luego se reserva el lock del rol seleccionado; si está ocupado se rechaza ese rol y se permite elegir otro o salir, liberando la reserva compartida si no se participa. Ningún alta publica acciones antes de completar ambos pasos.

Las bajas liberan sus reservas bajo control.lock cuando es posible; los errores pueden liberar manejadores por terminación del proceso. La ausencia total hace adquirible la reserva exclusiva: el siguiente inicio crea otra sesión sin recuperar la partida antigua y sin borrar sus logs. Los tres programas informan su presencia a la sesión nueva por un mensaje de conexión que no pertenece al historial de una partida. Hasta que el servidor ordene nueva partida los historiales antiguos se conservan e ignoran por su sesión.

Las conexiones se publican en archivos de lock de rol antes de quedar habilitado el proceso: sesión, rol, identificador de instancia nuevo en cada apertura y nombre local si ya se conoce. Solo se leen estos datos con una reserva de rol observada; bajo control.lock un rol libre se prueba y se libera inmediatamente, sin atribuirle actividad por esa prueba. El mismo mutex impide dos altas o un inicio de partida simultáneos. Una instancia reincorporada no sustituye una instancia que aún mantiene su reserva, aunque parezca congelada.

Limitación expresa: un proceso congelado puede conservar sus locks. Se considera activo pero no preparado; no se lo suplanta ni se fuerza su cierre. Se muestran espera y opción de salida. La prueba de red debe confirmar que una caída de conexión no permite dos escritores del mismo rol; ante pérdida de reserva o acceso se suspenden todas las publicaciones del proceso afectado y se cierra localmente si no puede garantizar exclusividad.

## Nueva partida y limpieza coordinada

1. Los tres roles reservados confirman preparación para la sesión actual. Solo el servidor ofrece iniciar nueva partida; no se reinicia una partida en curso sin una orden explícita.
2. Bajo control.lock se verifica presencia de los tres y se publica en el contenido de sesion.lock una preparación con un nuevo idPartida y una generación de limpieza. Se suspende la lectura y publicación de acciones de la partida anterior.
3. Cada titular vacía únicamente su log, vacía su historial JSON y confirma el identificador de partida y confirma esa generación en su lock de rol. No se vacía ningún archivo de rol ausente.
4. El servidor espera las tres confirmaciones; solo entonces guarda el Estado inicial con catálogo validado y comienza la preparación de nombres y reparto. Los lectores no interpretan los archivos mientras la generación esté preparando.
5. Si alguien falta durante la limpieza, se espera con el ciclo de cinco segundos; al volver completa la misma generación. Si vuelve el servidor, lee la preparación y las confirmaciones y continúa sin volver a truncar un log ya confirmado. Si no sobrevive nadie se invalida la sesión.

El estado pequeño de coordinación de sesion.lock se escribe solo bajo control.lock, usando versión, longitud y comprobación de integridad. Una escritura de coordinación dañada no habilita jugar ni borrar: se informa el error y se impide continuar esa sesión. No se promete recuperación ante corrupción arbitraria del soporte. No se usan timestamps como prueba de presencia.

## Diseño vigente — JSON para Accion y Estado

Actualización aprobada el 13 de noviembre de 2026. Personaje conserva su CSV con DataBases y su id permanente. Accion y Estado usan JSON y no implementan IEntidad ni se registran en modelos de DataBases. Esto sustituye las propuestas anteriores de doce columnas CSV, diecisiete mensajes y reconstrucción del servidor desde los logs. Las decisiones funcionales de roles, espera y finalización permanecen vigentes.

Estado es una fotografía completa de los datos del juego. Accion representa un mensaje entre procesos. El servidor concentra lógica y persistencia; los clientes muestran datos preparados y envían respuestas. Los contratos siguientes son la concreción documental propuesta de ese diseño aprobado; no son código implementado.

## DTO Estado

No es la serialización directa de objetos vivos de C#. Contiene valores y DTO anidados que permiten reconstruir los objetos de dominio y la coordinación, sin ciclos de referencias. Las entidades referidas varias veces se identifican por sus IDs o por su rol. Todos los datos de una captura deben corresponder a una misma revisión.

| Campo de Estado | Tipo de diseño | Contenido |
|---|---|---|
| versionFormato | int | Versión 1 del contrato JSON. |
| idSesion | Guid | Sesión cuya continuidad se comprueba mediante locks. |
| idPartida | Guid | Identificador de la partida vigente. |
| revision | long | Aumenta en cada cambio confirmado. |
| fase | FasePartida | Preparacion, EsperandoElector, EsperandoRival, EsperandoConfirmaciones o Finalizada. |
| catalogo | PersonajeDto[] | Todos los personajes utilizados, con ID, nombre, descripción, ASCII y seis atributos. |
| mazo | MazoDto | IDs de cartas disponibles, conservando su orden exacto. |
| jugadores | JugadorDto[] | Exactamente J1 y J2: rol, nombre, IDs de su mano y rondas ganadas. |
| numeroRonda | int | Cero durante preparación; después desde uno. |
| ronda | RondaDto? | Mano, rival y combates de la ronda, incluido el actual si existe. |
| ganadorPartida | TipoProceso? | Ausente hasta finalizar normalmente. |
| solicitudesPendientes | SolicitudDto[] | Identidad, destinatario, opciones ofrecidas y respuesta que falta. |
| accionesProcesadas | ReferenciaAccionDto[] | Identidades de respuestas cuyos efectos ya están incluidos. |
| publicacionesPendientes | Accion[] | Mensajes que deben publicarse o volver a comprobarse después de guardar el estado. |
| presentaciones | PresentacionDto[] | Pantalla e instrucción actuales preparadas para cada cliente, con su solicitud vigente. |
| siguienteIdServidor | long | Próximo ID disponible para una acción del servidor. |

PersonajeDto incluye id: int, nombre, descripcion, ascii y los seis atributos; preserva las restricciones 3000, rango 250–750 y umbrales 650/350. El catálogo incluido evita depender de una recarga del CSV al recuperar. Los IDs ya son atributos permanentes; no se generan según la posición ni cambian al volver a abrir.

MazoDto conserva idsDisponibles en orden. JugadorDto conserva rol, nombre, idsMano y rondasGanadas. RondaDto conserva numero, jugadorMano y combates; el actual es el último aún pendiente. Cada CombateDto conserva numero, elector, rival, atributo opcional, idCartaElector opcional, idCartaRival opcional y ganador opcional. Un combate resuelto y uno pendiente no se confunden. Las victorias de combate de la ronda se derivan de esos resultados; no se agrega otro contador como segunda autoridad.

ReferenciaAccionDto contiene idPartida, origen y id. SolicitudDto contiene esa referencia, destinatario, clase de entrada solicitada y opciones válidas. PresentacionDto contiene destinatario, textos y datos de cartas/marcador ya seleccionados por el servidor, y la solicitud opcional que debe responderse. No incluye elecciones rivales aún ocultas. El detalle visual de los textos se delega a InterfazConsola, sin cálculo de turnos ni ganadores en el cliente.

Estado no incluye consola, streams, locks, temporizadores, rutas de máquina ni Modelo de DataBases. Se preparan nuevamente al iniciar cada proceso. La recuperación solo se permite si la sesión sigue activa por otro proceso; disponer de estado.json no habilita retomar tras el cierre de todos.

### Contratos de Estado y persistencia

Estado es un DTO sin reglas de juego ni operaciones de E/S. Su construcción/deserialización conserva valores; antes de adoptar una captura, el coordinador valida versión, sesión/partida, dos roles distintos, referencias existentes, conservación de cartas sin duplicados, resultados coherentes y solicitudes/publicaciones identificables. Un fallo impide continuar con esa captura, sin inventar cartas ni marcadores.

`Partida.CapturarEstado(): Estado` reúne el dominio; el coordinador incorpora los datos de intercambio en la misma captura. `Partida.Restaurar(estado: Estado): Partida`, estático, reconstruye personajes, mazo ordenado, jugadores, ronda y combates a partir de IDs. No vuelve a mezclar ni reparte otra vez y no ejecuta el inicio de una partida nueva. Ambos son contratos propuestos que amplían el dominio previo.

`AlmacenEstado.Cargar(): Estado` exige un archivo completo válido de la sesión/partida esperadas. `AlmacenEstado.Guardar(estado: Estado): void` escribe JSON completo a un temporal de la misma carpeta, vacía los buffers y reemplaza estado.json. Solo el servidor lo invoca. No publica un estado parcial y no modifica el archivo anterior antes de completar el temporal. Los modos concretos de reemplazo deben comprobarse en Windows/SMB. No se garantiza recuperación ante corrupción física del almacenamiento.

## DTO Accion y formato de los historiales

Cada archivo contiene una acción JSON por línea física, en UTF-8. Los saltos internos se escapan. Una acción publicada no se modifica; su identidad es (idPartida, origen, id), con origen obtenido del nombre del log, no de un emisor que pueda contradecirlo.

| Campo de Accion | Tipo | Uso |
|---|---|---|
| versionFormato | int | Versión del mensaje. |
| id | long | Consecutivo del log; no se reutiliza durante una partida. |
| idPartida | Guid | Partida vigente. |
| tipo | TipoAccion | Presentar, Responder o SolicitarEstado. |
| destinatario | TipoProceso | Un rol concreto; el servidor publica una acción por destinatario. |
| solicitud | ReferenciaAccionDto? | Orden que se responde, o SolicitarEstado al que contesta Presentar. |
| presentacion | PresentacionDto? | Solo Presentar; contiene todo lo que ese cliente debe mostrar y pedir. |
| respuesta | RespuestaDto? | Solo Responder; contiene los datos solicitados al jugador. |

Simplificación propuesta de TipoAccion: Presentar, Responder y SolicitarEstado reemplazan los diecisiete tipos anteriores. Presentar reemplaza toda la pantalla e instrucción actuales; puede mostrar mano, marcador, espera, resultado o ganador y pedir nombre, carta, carta/atributo o nada. RespuestaDto contiene nombre, idPersonaje y atributo opcionales según la solicitud. Los campos no aplicables son null. Los DTO transportan datos y la clase de entrada, no código ni órdenes ejecutables.

Una solicitud de entrada se identifica por la acción Presentar que la originó. Si solo se vuelve a mostrar una pantalla pendiente, PresentacionDto conserva la referencia de esa solicitud original; una sincronización no genera otra elección. El cliente envía Responder con esa referencia. El servidor comprueba rol, partida, solicitud pendiente, carta disponible y atributo permitido; Accion no resuelve esas reglas.

SolicitarEstado no necesita conocer ronda ni combate: el cliente conoce la partida desde la coordinación de sesión. El servidor contesta con una única Presentar completa correlacionada con la solicitud y correspondiente al cliente. Se retiran InicioEstado/CartaEstado/MarcadorEstado/TurnoEstado/FinEstado y origenSolicitud como campo suelto: la referencia anidada contiene origen e id. La revisión de la captura figura en PresentacionDto para identificar la pantalla actual.

`Accion` y los DTO ofrecen datos de solo lectura una vez preparados y no acceden a archivos. La adaptación de los Get de la propuesta CSV queda sustituida por propiedades DTO; no se implementa Configurar de DataBases. RegistroAcciones es responsable de JSON y de los archivos, no de decidir turnos.

### Lectura y publicación

`RegistroAcciones.LeerNuevas(): Accion[]` acepta solo líneas completas y conserva posición hasta la siguiente línea terminada. Valida estructura, versión e identidad; no adopta un mensaje JSON inválido. Una cola sin terminar se espera. Al recuperar el escritor elimina únicamente la cola incompleta de su propio archivo antes de continuar; no elimina líneas completas ni escribe en logs ajenos.

`RegistroAcciones.Anexar(accion: Accion): void` serializa toda la línea en memoria, escribe como titular único y fuerza el vaciado antes de considerar terminada la publicación. `Reiniciar(idPartida: Guid): void` vacía solo el historial propio cuando la barrera explícita de nueva partida lo ordena. Los IDs ya publicados se reconocen al recuperar para no emitir dos veces la misma identidad.

## Orden de guardado y recuperación

Propuesta para coordinar estado y logs sin una transacción entre archivos:

1. El servidor lee una respuesta y consulta accionesProcesadas. Si ya fue incorporada, no vuelve a consumir carta ni sumar un resultado.
2. Prepara una copia del Estado confirmado, aplica allí el cambio con las reglas del dominio, aumenta revision e incluye la identidad procesada y las acciones que debe publicar. Las publicaciones reciben sus IDs antes del guardado; el próximo ID también queda conservado.
3. Guarda la captura completa en estado.json. Solo entonces adopta ese estado en memoria y anexa las publicaciones a server.log.
4. Al completar las publicaciones puede guardar de nuevo Estado retirándolas de la lista pendiente. Si se interrumpe antes de ese último guardado, comprueba sus IDs en server.log y solo publica las que falten.
5. Si se interrumpe antes de guardar Estado, la respuesta aún no consta como procesada y se puede aplicar al volver. Si se interrumpe después, los efectos ya están confirmados y únicamente se completan publicaciones pendientes.

Al recuperar, el servidor carga estado.json, comprueba sesión y rol, restaura el dominio y completa publicaciones pendientes. No reproduce todos los resultados de los logs para reconstruir el juego; solo consulta identidades para completar publicaciones y buscar respuestas aún no procesadas. Si encuentra respuestas con referencia ya consumida las ignora; respuestas contradictorias se rechazan sin alterar el juego.

El reparto se calcula completo en memoria, incluida mezcla y manos, y se guarda como una sola captura antes de mostrarlo. Un cierre previo conserva el estado anterior; uno posterior recupera exactamente el reparto guardado. No se necesitan marcadores de reparto parcial en el historial. El mismo criterio se aplica a resultados y cambios de ronda.

Cada cliente, al volver, solicita su presentación. No reconstruye objetos Partida/Ronda/Combate, no calcula marcadores y no lee estado.json como su fuente de reglas. Si ya había enviado una respuesta a la solicitud original, puede reenviar la misma identidad conservada o esperar al servidor; no pide otra elección para esa solicitud. Una pantalla completa puede volver a mostrarse; un efecto del juego no puede aplicarse dos veces.

Sondeo propuesto cada 250 ms. Al faltar un proceso se suspende el avance, se esperan cinco segundos y se ofrece abandonar o continuar indefinidamente. El diálogo no impide detectar que el ausente volvió. Si ya no hay ningún proceso activo, la siguiente sesión invalida el estado anterior sin borrar archivos hasta una nueva partida explícita. Si finaliza normalmente a 15 rondas, el Estado final y sus presentaciones se guardan antes del anuncio.

## Integración de componentes

CoordinadorServidor tiene una Partida, SesionCompartida, AlmacenEstado, un Estado confirmado y tres RegistroAcciones. CoordinadorCliente tiene SesionCompartida, InterfazConsola y registros propio/servidor. No tiene una Partida ni un motor de reconstrucción. Datos conserva el catálogo Personaje en DataBases y las rutas del intercambio JSON; no crea modelos para Accion o Estado.

CoordinadorServidor.Ejecutar coordina preparación, guardado, publicaciones y recuperación. CoordinadorCliente.Ejecutar muestra PresentacionDto, captura la respuesta solicitada y la publica. InterfazConsola mantiene PedirRol, PedirNombre, PedirNuevaPartida y PedirAbandono, además de los métodos de presentación anteriores. Solo el servidor ofrece nueva partida cuando están los tres. Los contratos de SesionCompartida siguen siendo Unirse(rol): bool, ConsultarPresentes(): TipoProceso[], PrepararNuevaPartida(): Guid y Salir(): void; la mecánica de locks sigue propuesta y pendiente de ensayo.

Partida conserva las reglas de dominio y se amplía con CapturarEstado/Restaurar y AplicarEleccion(jugador: TipoProceso, personaje: Personaje, atributo: TipoAtributo?): void. El coordinador administra esperas y solicitudes; el bucle Jugar anterior debe adaptarse a ese avance por pasos y a la restauración, sin ejecutar de nuevo Iniciar al recuperar. Ninguna de estas firmas está implementada.

## Revisión y verificaciones pendientes

El usuario aprobó JSON para Accion/Estado y Estado como captura completa del juego. Las firmas y el mecanismo de publicación pendiente son propuestas técnicas documentadas para revisión. La reducción de TipoAccion a tres mensajes es propuesta de simplificación, no una decisión previamente confirmada por el usuario.

Pendiente: revisión de los contratos DTO y de los nuevos tres mensajes; revisión visual del UML; implementación futura en Taller solo con autorización; ensayo en Windows/SMB de reserva de roles, sesión compartida, caída de red, reemplazo de estado.json, lectura durante anexado y recuperación en cada punto de guardado/publicación. La carpeta y permisos reales todavía no están disponibles. No se considera completada esa verificación por validar XML o JSON documental. El plan de casos completo corresponde a actividad 7.

Los cambios se mantienen en rama docs/ y requieren revisión de compañero antes de integrarse. No se publican ni integran automáticamente.

Verificación documental del 13 de noviembre de 2026: XML de ambos diagramas y enlaces locales válidos; git diff --check sin errores de contenido. No se ejecutaron build ni tests porque no se modificó código. Sin exportación visual de UMLet ni comprobación Windows/SMB.
