# Plan de trabajo — Juego Guerra

Plan basado en [la consigna](Consigna.md). Marcar una tarea cuando su resultado esté disponible y revisado. Las actividades de TI definen los contratos que se usarán en Taller; las decisiones pendientes deben resolverse antes de implementar el componente correspondiente. Por decisión del usuario, no se utilizarán diagramas Nassi-Schneiderman (NS); el comportamiento de los métodos se documentará por escrito con sus entradas, resultados, precondiciones y validaciones. La consigna original se conserva como referencia sin modificar.

## Estado revisado — 13 de noviembre de 2026

Alcance actual: únicamente TI (análisis y diseño). Taller queda pendiente y fuera del trabajo actual. El punto de entrada todavía muestra `Hello, World!` y la única prueba está vacía. Existen implementaciones parciales de Personaje y Mazo, sin pruebas de comportamiento; el juego y la comunicación no están implementados. Este registro de código existente no autoriza avanzar con Taller.

Se revisaron el documento de avance aportado por el usuario y el [UML existente](diagramas/UML.uxf). El documento se utilizó como referencia local y no forma parte de la entrega del repositorio. Cada casilla de TI refleja esa revisión y las confirmaciones posteriores del usuario: `[x]` indica un resultado comprobado o confirmado, `[ ]` indica trabajo pendiente y **En curso** señala un avance parcial con el cierre que falta. Los valores verificados de personajes corresponden a Lysander y Reched. El usuario confirmó que la actividad 1 está completada; se retiran los pendientes de alternativas, comparación, justificación y verificación de otras fichas como condiciones para reabrirla. La imagen se trabaja en Arte. Las demás actividades conservan su estado de revisión. Completar una tarea de diseño no implica implementar código.

| Actividad de TI | Estado y evidencia | Qué falta para cerrarla |
|---|---|---|
| 1. Personajes | Completada por confirmación del usuario. Lysander y Reched tienen nombre, descripción, seis valores, total y atributos principal y menor; ambos cumplen las restricciones. | Sin pendientes para esta actividad. |
| 2. Requerimientos | Documento completo y revisado en [Reglas del juego](Reglas-del-juego.md), listo para copiar a la entrega. Incluye reglas, participantes, datos de ejecución, comunicación, errores y separación de requisitos funcionales y técnicos. | Sin tareas de redacción pendientes. El [PR #7](https://github.com/51215927-spec/Proyecto-Integrador-Final/pull/7) está integrado; el protocolo detallado corresponde a la actividad 6. |
| 3. Modelo de clases | Completada por confirmación del usuario. UML y Arquitectura revisados, con firmas, relaciones, responsabilidades y justificación. | Sin pendientes propios. El modelado de comunicación y su integración se realizan en la actividad 6. |
| 4. Métodos | Completada por confirmación del usuario. Las 58 especificaciones están redactadas en Arquitectura, con un párrafo por método y sin validaciones repetidas. | Sin pendientes propios. Los formatos y la integración corresponden a las actividades 5 y 6. |
| 5. Archivos de personajes | Completada por confirmación del usuario. Formato de DataBases, columnas, descripción, ASCII y errores documentados en Arquitectura; UML y contratos de Personaje actualizados. | Sin condiciones pendientes para el cierre aceptado. Conversión al CSV postergada; ejemplos propios no elaborados. |
| 6. Archivo de partida | En curso en ramas no integradas: protocolo por archivos JSON, fotografía de estado y UML de comunicación; además, borrador alternativo REST. El detalle se registra debajo. | Revisar contratos, UML y viabilidad en red; resolver compatibilidad académica de REST si se adopta. Revisión de compañero e integración pendientes. |
| 7. Pruebas | No se encontró un plan de casos desarrollado en el material revisado. | Diseñar condiciones iniciales, datos y resultados esperados para casos normales, límites y errores. |

### Verificación de personajes

Valores en orden: Fuerza, Velocidad, Resistencia, Inteligencia, Energía y Habilidad.

|  | Valores | Total | Principal / menor | Resultado |
|---|---|---|---|---|
| Lysander | 750, 550, 500, 250, 500, 550 | 3000 | Fuerza / Inteligencia | Cumple rango 250–750 y umbrales 650 y 350. |
| Reched | 250, 700, 350, 750, 450, 500 | 3000 | Inteligencia / Fuerza | Cumple rango 250–750 y umbrales 650 y 350. |

### Reglas ya redactadas

- Participan dos jugadores y se reparten tres cartas a cada uno desde un mazo común.
- El jugador mano elige el primer atributo; ambos jugadores eligen su carta y la muestran simultáneamente. Gana el mayor valor y, ante empate, quien no eligió el atributo del combate.
- La elección del atributo se alterna entre los jugadores en cada combate, independientemente del ganador. La ronda se gana con dos combates ganados, por lo que puede terminar sin jugar la tercera carta.
- Al terminar la ronda se restaura el mazo completo, incluidas las cartas no jugadas. Si nadie alcanzó 15 rondas ganadas, se mezcla y se reparte nuevamente; de lo contrario finaliza la partida.

### Decisión confirmada — jugador mano y terminología

El usuario confirmó que al iniciar la partida se identifican J1 y J2. J1 es el jugador mano de la primera ronda. Al terminar cada ronda, el rol pasa al otro jugador: J1, J2, J1, J2, hasta finalizar la partida, independientemente de quién gane la ronda. Dentro de una ronda, la elección del atributo se alterna entre los jugadores en cada combate, independientemente del ganador; esto no cambia quién es el jugador mano de esa ronda.

El documento de entrega debe distinguir estos conceptos:

- **Partida:** juego completo; termina cuando un jugador alcanza 15 rondas ganadas.
- **Ronda:** serie de hasta tres combates; termina cuando un jugador gana dos combates. Equivale a lo que coloquialmente se llama «mano» en el truco.
- **Combate:** enfrentamiento de una carta de cada jugador mediante un atributo elegido.
- **Jugador mano:** jugador que elige el atributo del primer combate de la ronda.
- **Mano de cartas:** cartas disponibles de un jugador; no equivale a una ronda ni al rol de jugador mano.

Esta decisión y las reglas confirmadas están redactadas en [Reglas del juego](Reglas-del-juego.md). El usuario indicó que copiará este archivo a su documento de entrega; se completó y revisó el contenido necesario para la actividad 2, sin dar por realizada esa copia externa.

### Precisiones pendientes antes de cerrar el diseño

- Contratos de dominio revisados frente a las reglas; falta completar su integración con el protocolo de la actividad 6.
- Restauración completa del mazo especificada en UML y Arquitectura, incluidas las cartas no jugadas y la devolución única al cerrar la ronda.
- El mazo inicial se forma a partir del CSV compartido que genera un sistema externo al cliente. Los repetidos están prohibidos y menos de seis cartas válidas impiden iniciar la partida con un mensaje de error. El usuario garantiza que el catálogo preparado no tendrá duplicados. El formato está documentado en Arquitectura, actividad 5; la conversión se definirá después.
- Especificar cómo se mantienen ocultas carta y atributo hasta registrar ambas elecciones; el orden de elección ya está definido en Reglas-del-juego.md.
- Las especificaciones de la actividad 4 se redactan directamente desde el UML y Arquitectura. Se corrige la referencia anterior a un borrador: el usuario confirmó que no existe.
- Separar los archivos iniciales de s del intercambio de acciones de la partida; definir identidad de partida, turno, ronda y combate, acciones duplicadas o fuera de turno y escrituras incompletas.

### Orden de trabajo para TI

1. Actividades 1 y 2 completadas en su documentación e integradas mediante los PR #6 y #7. La actividad 2 está lista para copiar a la entrega; esa copia externa no se ha comprobado.
2. Actividad 3 completada: UML y justificación revisados. La ampliación del modelo para comunicación corresponde a la actividad 6.
3. Actividad 4 completada: especificaciones de métodos revisadas y aceptadas por el usuario.
4. Actividad 5 completada por confirmación del usuario. Continuar con el protocolo de partida compartida (actividad 6).
5. Completar el plan de pruebas y revisar que todos los entregables de TI sean coherentes (actividad 7).

Las actividades 1, 2, 3 y 4 tienen sus entregables documentados; el usuario confirmó el cierre de las actividades 3 y 4. El modelado e integración de comunicación se completarán en la actividad 6. Los demás entregables de TI no se consideran cerrados solo por contar con documentación parcial.

## TI — Análisis de Sistemas

### Actividad 1 — Análisis y diseño de los personajes

**Completada por confirmación del usuario.** Se conserva el registro de los datos y las validaciones revisadas, sin tareas adicionales pendientes para esta actividad.

- [x] Escribir el nombre y la descripción de Lysander y Reched (fichas del documento revisado).
- [x] Elegir una distribución para Lysander y Reched (valores registrados en la tabla de verificación).
- [x] Comprobar que los seis valores de ambos s estén entre 250 y 750 y sumen exactamente 3000.
- [x] Comprobar en ambos s que al menos un atributo sea mayor o igual a 650 y otro menor o igual a 350; ninguno tiene seis valores de 500.
- [x] Identificar el atributo principal y el de menor valor de ambos s.
- [x] Dar por completada la actividad 1 según la confirmación del usuario.

### Actividad 2 — Relevamiento de requerimientos

- [x] Escribir el objetivo del juego: alcanzar 15 rondas ganadas.
- [x] Identificar a los dos participantes y sus acciones: elegir atributo cuando corresponde y elegir una carta de su mano.
- [x] Definir el reparto de tres cartas a cada jugador desde un mazo común y la mezcla entre rondas.
- [x] Definir el origen del mazo inicial: s cargados en un CSV compartido por un sistema externo al cliente de los jugadores.
- [x] Registrar que carta y  son el mismo elemento y que no se utilizará una clase Carta adicional.
- [x] Definir que menos de seis cartas válidas impiden iniciar la partida y muestran el error «No se puede jugar porque no hay suficientes cartas».
- [x] Prohibir s repetidos en el mazo.
- [x] Asignar la prevención de s duplicados al sistema externo de carga, fuera del cliente y del servidor de partida.
- [x] Definir quién es jugador mano al iniciar la partida: J1, según confirmación del usuario.
- [x] Reunir las reglas confirmadas en [Reglas del juego](Reglas-del-juego.md), diferenciando partida, ronda, combate, jugador mano y mano de cartas y señalando las decisiones pendientes.
- [x] Preparar Reglas-del-juego.md como contenido de entrega que copiará el usuario, con J1 como jugador mano inicial y alternancia J1/J2 al terminar cada ronda.
- [x] Diferenciar en todo el documento partida, ronda, combate, jugador mano y mano de cartas; usar «ronda ganada» y aclarar que se juegan hasta tres combates.
- [x] Definir quién elige el atributo: el jugador mano en el primer combate; después se alterna entre jugadores, independientemente del ganador. Reemplaza la regla anterior basada en el ganador.
- [x] Especificar la comparación por el atributo elegido y la victoria de ronda al alcanzar dos combates ganados.
- [x] Resolver la regla de empate: gana quien no eligió el atributo.
- [x] Definir que las cartas jugadas vuelven al mazo común al terminar la ronda.
- [x] Definir la restauración del mazo completo al terminar cada ronda, incluidas las cartas no jugadas; se mezcla antes del nuevo reparto.
- [x] Definir la alternancia del jugador mano entre rondas y del jugador que elige el atributo entre combates.
- [x] Definir la secuencia: quien comienza el combate elige carta y atributo; el rival elige sin conocer esas elecciones; luego se revelan ambas cartas y el atributo.
- [x] Limitar la ocultación de elecciones a la interfaz: se revelan cuando ambos han elegido; se acepta que los archivos puedan consultarse manualmente. No se exige una garantía de secreto frente a ese acceso.
- [x] Establecer la victoria y finalización normal de la partida al alcanzar 15 rondas ganadas.
- [x] Completar los datos de ejecución: s y mazo, cartas disponibles y jugadas, jugadores, ronda, combate, jugador mano, elección de atributo, elecciones realizadas, marcadores, resultados y estado de partida.
- [x] Definir la comunicación general: dos clientes publican sus elecciones; el servidor las lee, analiza y publica mensajes de resultados. Cada proceso solo escribe sus archivos y lee los de los demás.
- [x] Asignar al cliente la validación de sus datos antes del envío y al servidor el tratamiento de errores al iniciar la partida.
- [x] Definir responsabilidades y política general de errores: el cliente garantiza envíos válidos; cualquier error termina el juego completo, incluido el fallo del servidor. Los tipos de acciones, secuencias y formatos se difieren a la actividad 6.
- [x] Definir la respuesta ante errores iniciales y de ejecución: finalizar el juego completo, sin recuperación. Con menos de seis cartas se informa además el mensaje ya acordado.
- [x] Separar los requerimientos funcionales de las condiciones técnicas de ejecución en Reglas-del-juego.md.
- [x] Revisar la cobertura de la consigna y preparar el documento de requerimientos y reglas, listo para que el usuario lo copie a su entrega. No se afirma que ya haya realizado la copia ni presentado la entrega.

El [PR #7](https://github.com/51215927-spec/Proyecto-Integrador-Final/pull/7) de esta actividad se integró el 8 de noviembre de 2026. Las tareas documentales están completas; no se da por realizada la copia externa al documento de entrega. Los nuevos PR mantienen la revisión de un compañero prevista en [CONTRIBUTING](../CONTRIBUTING.md).

### Actividad 3 — Modelo de clases

**Completada por confirmación del usuario.** El UML y su justificación están revisados. No hay implementación. El detalle vigente está en [Arquitectura](Arquitectura.md). Las tareas de modelado e integración de comunicación se trasladan a la actividad 6.

- [x] Representar , Mazo, Jugador, Partida, Ronda y Combate con atributos privados, firmas tipadas y relaciones.
- [x] Adaptar todas las referencias al concepto unificado de .
- [x] Asignar coordinación del dominio al servidor y presentación a los clientes, sin comunicación directa entre rivales.
- [x] Incorporar Datos estática y las dependencias de DataBases sin duplicar la lectura CSV.
- [x] Definir en Datos constantes de rutas y Configurar(TipoProceso), con identidad fija de servidor, cliente J1 o cliente J2 antes de Iniciar.
- [x] Describir InterfazConsola delegando entradas, menús y presentación en Utilidades.
- [x] Justificar responsabilidades, encapsulamiento, delegación y decisiones en Arquitectura.
- [x] Reflejar J1 como mano inicial, alternancia por ronda y combate, empate, dos victorias por ronda y 15 rondas por partida.
- [x] Conservar el jugador mano solo en Ronda; Partida calcula el rol al construir cada ronda según su número.
- [x] Revisar consultas necesarias para la presentación, multiplicidades de inicialización y contratos que evitan registrar combates repetidos o ajenos.
- [x] Documentar recuperación de todas las cartas y ocultación en interfaz hasta completar ambas elecciones.
- [x] Unificar las firmas de referencia en UML y Arquitectura, base de las especificaciones de la actividad 4.
- [x] Revisar la notación frente a UML 2.5.1, exportar con UMLet y comprobar que las 16 relaciones conecten a bordes sin atravesar cajas.
- [x] Dar por terminada la actividad 3 según confirmación del usuario; trasladar el modelado de comunicación a la actividad 6.

### Actividad 4 — Especificación de métodos

Completada por confirmación del usuario el 9 de noviembre de 2026, después de revisar y simplificar las especificaciones de [Arquitectura](Arquitectura.md). El código incluido es ilustrativo y no constituye implementación. No se utilizan diagramas NS, según la decisión registrada.

- [x] Identificar las clases y métodos vigentes del UML, incluidas las operaciones externas representadas.
- [x] Organizar Arquitectura con una sección por clase y una subsección por método o constructor.
- [x] Explicar responsabilidad y motivo de existencia de cada método.
- [x] Precisar accesibilidad, carácter de instancia o estático, parámetros y retorno.
- [x] Especificar precondiciones, resultados, efectos, validaciones y errores, incluidas las dependencias de DataBases y Utilidades.
- [x] Describir las secuencias de combate, ronda, partida y recuperación de cartas, con un ejemplo ilustrativo de comparación.
- [x] Distinguir las especificaciones actuales de los formatos y protocolo pendientes de las actividades 5 y 6.
- [x] Revisar las especificaciones con el usuario y resolver observaciones antes de dar la actividad por cerrada.

Verificación documental: las 58 operaciones del UML tienen una subsección de especificación, incluidos constructores y contratos externos. `git diff --check` no detectó errores y `dotnet build Proyecto-Integrador-Final.sln` terminó sin errores ni advertencias. La compilación verifica la base existente, no acredita implementación del diseño. Se precisó la retirada de cartas desde Combate.SetCarta y su recuperación única al cerrar la ronda. Formatos de archivos, protocolo e integración permanecen pendientes en las actividades 5 y 6.

### Actividad 5 — Formato de los archivos de personajes

**Completada por confirmación del usuario.** El cierre corresponde a la especificación acordada en Arquitectura y a la actualización del modelo; no implica implementación.

- [x] Definir estructura y reconocimiento de datos mediante el formato propio de DataBases y columnas correspondientes a los campos de Personaje.
- [x] Especificar nombre y seis atributos, con las restricciones numéricas y su validación al construir Personaje.
- [x] Incorporar descripcion al campo privado, constructor, GetDescripcion y columna del archivo: opcional, sin límite y ausente como texto vacío ("").
- [x] Definir ASCII: máximo 25 líneas de 120 caracteres por línea y sin tabulaciones. Se entrega con contenido, sin validar texto vacío; se mantiene el rechazo de null.
- [x] Documentar errores de formato, campos faltantes o incorrectos, archivos inexistentes e ilegibles y finalización sin recuperación.
- [x] Registrar que el usuario garantiza el catálogo sin duplicados y que la conversión al CSV se definirá después.
- [x] Registrar el cierre por confirmación del usuario.

No se elaboraron ejemplos propios válidos e inválidos; el cierre confirmado no los presenta como realizados ni los conserva como condición para reabrir esta actividad. La presentación de la descripción en consola sigue sin definir y no condiciona este cierre. La conversión al CSV queda postergada por indicación del usuario.

<a id="actividad-6--archivo-de-partida-compartida"></a>

# Actividad 6

**En curso, con avances fuera de main.** La lista que sigue describe la base integrada anterior; no debe interpretarse como ausencia de trabajo en las ramas de protocolos. Revisión del 13 de noviembre de 2026:

| Rama revisada | Evidencia disponible | Pendientes declarados |
|---|---|---|
| `docs/protocolo-partida-compartida` (`70302da`) | `docs/Protocolo-partida.md` y `docs/diagramas/Comunicacion.uxf`: roles, historiales JSON con un escritor por historial, Estado completo, catálogo estable, propuesta de publicación y recuperación; contratos y UML ampliados. | Revisar propuesta de tres mensajes y captura/restauración; revisión visual del UML; verificar Windows/SMB con rutas y permisos reales; revisión de compañero antes de integrar. |
| `docs/protocolo-partida-web` (`c807a42`) | Conserva la alternativa por archivos en `docs/Protocolo-partida-archivos.md` y agrega el borrador REST `docs/Protocolo-partidad-web.md`. | Resolver con el docente la compatibilidad con la consigna; confirmar tecnologías, sesiones, rutas/DTO/estados HTTP, persistencia, concurrencia, presencia y recuperación; adaptar UML y diseñar verificaciones. |

Los documentos se encuentran en los worktrees hermanos de cada rama; aún no existen en main. Ambas ramas contienen el main revisado (`dcfc9eb`) y coinciden con sus referencias remotas. No se integran alternativas ni se da por aprobada una propuesta técnica por estar redactada.

El protocolo por archivos registra decisiones del usuario del 12 de noviembre que sustituyen el cierre inmediato: suspensión ante ausencia, espera de cinco segundos y recuperación mientras sobreviva un proceso. También separa Personaje CSV/DataBases de Accion y Estado JSON. Estas decisiones están documentadas en la rama; la política anterior que permanece debajo y en Arquitectura corresponde a la base integrada y deberá conciliarse al revisar e integrar el protocolo. El borrador REST no reemplaza automáticamente el requisito académico de archivo compartido.

**Criterio de cierre:** revisar los contratos y diagramas de la alternativa que corresponda a la entrega, verificar las condiciones reales de SSI y completar la revisión de compañero. La actividad 7 sigue pendiente; describir escenarios del protocolo no equivale a entregar el plan de pruebas.

## Archivos

Lista de la base integrada, pendiente de conciliación con el protocolo por archivos.

Los registros de elecciones y publicaciones se definirán en esta actividad. El acceso a archivos se realiza mediante Datos y DataBases. La validez de los envíos corresponde al cliente y cualquier fallo de ejecución finaliza el juego completo, sin recuperación.

- [x] Definir un escritor exclusivo por archivo: cada cliente escribe sus elecciones y el servidor sus publicaciones; los demás procesos leen.
- [ ] Definir las entidades de intercambio y sus campos, cantidad de archivos, rutas, permisos y configuración en DataBases.
- [ ] Especificar la identificación de partida, jugadores, estado, ronda, combate y jugador autorizado a elegir.
- [ ] Definir cómo se relacionan los registros de intercambio con los s del catálogo.
- [ ] Determinar cuándo cada proceso ejecuta Cargar<T> y Guardar<T> y qué tablas puede guardar.
- [ ] Especificar la secuencia de elecciones y la publicación de resultados sin revelar elecciones anticipadamente en la interfaz.
- [ ] Definir cómo se reconocen registros ya procesados, repetidos o correspondientes a otra partida, ronda o combate.
- [ ] Especificar publicación de mensajes completos y coherencia entre archivos, considerando que Guardar reemplaza una tabla completa y no hay transacciones entre archivos.
- [ ] Especificar la creación inicial de las tablas y cuáles requieren archivos existentes al cargarse.
- [ ] Definir la detección de fallos y el cierre coordinado de clientes y servidor, sin recuperación.
- [ ] Verificar con SSI el acceso, permisos, bloqueos y reemplazo de archivos en la red local.
- [ ] Incorporar al UML y Arquitectura las entidades de elecciones de los clientes y publicaciones del servidor, con atributos, relaciones y configuración en Datos.
- [ ] Definir los métodos que conectan los registros de intercambio con Partida y los clientes, conforme al protocolo acordado.
- [ ] Revisar la coherencia de esas ampliaciones con las reglas y el modelo de dominio ya cerrado en la actividad 3.
- [ ] Entregar la especificación de intercambio, diferenciándola del catálogo inicial de s.

## REST

Las tareas se presentan con casillas, sin numeración. Las decisiones del protocolo se redactan en `docs/protocolo-partida-web`; esta reorganización en `docs/actualizar-tareas` modifica únicamente el plan y conserva el seguimiento anterior.

- [ ] **Acordar con el docente si REST sustituye el archivo compartido.** Pendiente.
- [x] **Definir versiones y distribución de ejecución.** Terminado por confirmación del usuario: net8.0; ASP.NET Core y EF Core estables de la línea 8.0 (referencia 8.0.34); ambas computadoras Windows; servidor y cliente J1 en la primera y cliente J2 en la segunda. Primero se abre el servidor y después los clientes. La comprobación de instalaciones queda pendiente en las verificaciones, sin reabrir esta tarea.
- [ ] **Definir conexión y disponibilidad del servidor.** Decisiones redactadas: HTTP en red local, TCP 5000 y dirección configurable. Ante indisponibilidad se informa y se permite reintentar o salir, conservando una elección pendiente sin tratarla como una jugada nueva. Pruebas entre equipos pendientes.
- [ ] **Definir administración, incorporación, J1/J2, credenciales y reincorporación.** Pendiente.
- [ ] **Completar rutas, DTO, respuestas HTTP y transiciones del juego.** Pendiente.
- [ ] **Definir consultas, presencia, pausa, abandono y recuperación.** Pendiente.
- [ ] **Diseñar idempotencia, revisiones y concurrencia.** Pendiente.
- [ ] **Diseñar tablas, relaciones y transacciones de personajes y partidas.** Pendiente. SQLite conserva permanentemente el catálogo, además del estado de partidas.
- [ ] **Definir incorporación y mantenimiento de personajes.** Pendiente.
- [ ] **Preservar datos de personajes en partidas iniciadas ante cambios del catálogo.** Pendiente.
- [ ] **Adaptar Arquitectura y UML a REST y conciliar la persistencia anterior.** Pendiente.
- [ ] **Diseñar casos de prueba y vincularlos con la actividad 7.** Pendiente.
- [ ] **Planificar y registrar la verificación en dos computadoras.** Pendiente; las verificaciones concretas se mantienen debajo.
- [ ] **Conciliar documentos y completar revisión antes de integrar.** Pendiente.

**Evidencia disponible:** el usuario informó comunicación correcta entre dos procesos mediante localhost. No aportó una ejecución reproducible del proyecto ni una prueba entre computadoras; no se presenta como implementación del juego o validación de la red escolar. No tiene los equipos disponibles para ejecutar las comprobaciones ahora.

**Verificaciones pendientes de instalaciones y comunicación entre computadoras:**

- [ ] Comprobar SDK y runtimes .NET/ASP.NET Core 8 en ambas computadoras y registrar resultados.
- [ ] Comprobar dirección real del servidor y disponibilidad del puerto TCP 5000.
- [ ] Verificar que el servidor acepte conexiones por su dirección de red y no solamente por localhost.
- [ ] Probar los dos clientes distribuidos en las dos computadoras con HTTP y dirección configurable.
- [ ] Comprobar si el firewall o la red escolar bloquean la comunicación; si hace falta una excepción, coordinarla con el responsable y limitarla al servicio y equipos participantes, sin desactivar el firewall ni abrir el router a Internet.
- [ ] Verificar servidor no iniciado, pérdida de conexión, reintento y salida sin duplicar elecciones; vincular estos escenarios con el diseño de idempotencia y concurrencia.
- [ ] Registrar condiciones de la red, configuración utilizada y resultados; no marcar exitosas pruebas todavía no ejecutadas.

# Actividad 7 — Casos de prueba

- [ ] Definir condiciones iniciales, datos y resultado esperado para cada caso de prueba.
- [ ] Diseñar casos para un personaje válido y para un total distinto de 3000.
- [ ] Diseñar casos para los límites 250 y 750 y para valores fuera de rango.
- [ ] Diseñar casos que comprueben los umbrales 650 y 350 y rechacen distribuciones sin especialización.
- [ ] Diseñar casos de archivos de personajes incompletos, duplicados, incorrectos e ilegibles.
- [ ] Diseñar casos para un mazo vacío y para las operaciones de mezcla y reparto.
- [ ] Diseñar casos de ronda ganada, empate y cambio de turno.
- [ ] Diseñar un caso de jugador sin cartas y finalización de partida.
- [ ] Diseñar casos de archivo compartido inaccesible y acceso concurrente.
- [ ] Diseñar un caso de información que no corresponde al turno esperado.
- [ ] Indicar qué pruebas usan caja negra y cuáles requieren caja blanca.
- [ ] Revisar y entregar el plan de pruebas con sus resultados esperados.

## Taller de Programación — C#

Etapas conservadas como referencia futura. No se implementarán en el alcance actual, limitado a TI. Las casillas marcadas registran código ya presente, no etapas terminadas ni cumplimiento de TDD: no hay pruebas de comportamiento que lo acrediten.

### Etapa 1 — Implementación de Personaje

- [ ] Completar Personaje conforme al modelo acordado: la clase existe, pero faltan descripcion y GetDescripcion.
- [x] Implementar el constructor con nombre, seis atributos y ASCII Art.
- [x] Implementar las validaciones de los atributos según los contratos de TI.
- [x] Implementar GetNombre, GetAscii y GetAtributo conservando el encapsulamiento. GetDescripcion permanece pendiente en la tarea de completar Personaje.
- [ ] Agregar en `test/` pruebas de personajes válidos y de cada restricción de puntajes.

### Etapa 2 — Carga de personajes con Datos y DataBases

- [x] Referenciar DataBases y Utilidades desde la aplicación e incluir sus proyectos en la solución.
- [ ] Implementar Personaje.Configurar con el formato definido en TI: el método existe, pero su cuerpo está vacío.
- [ ] Crear Datos estática con sus constantes de rutas, Configurar, GetTipoProceso, Modelo e Iniciar conforme al diseño.
- [ ] Fijar las rutas absolutas reales del recurso de SSI y configurar el rol antes de inicializar el modelo.
- [ ] Cargar el catálogo obligatorio mediante Modelo.Cargar<Personaje>(exigirArchivo: true).
- [ ] Obtener los personajes de la tabla cargada para construir el mazo inicial.
- [ ] Mantener la validación de puntajes en el constructor de Personaje y la interpretación del CSV en DataBases.
- [ ] Aplicar la finalización prevista para archivos inexistentes, ilegibles o inválidos.
- [ ] Probar la carga con los ejemplos de la actividad 5, sin reimplementar el lector CSV.

### Etapa 3 — Jugador e interacción de consola

- [ ] Crear Jugador con nombre, mano de personajes y marcador de rondas ganadas.
- [ ] Implementar recepción, consulta, retiro y entrega de personajes sin exponer la colección interna.
- [ ] Implementar InterfazConsola utilizando Funciones, DecoracionConsola y MyConsole de Utilidades.
- [ ] Seleccionar un Personaje de la mano disponible mediante PedirOpcionMenu y el atributo mediante PedirEnum<TipoAtributo>.
- [ ] Presentar mano, marcador, esperas, resultados y errores según los contratos de TI.
- [ ] Comprobar que la interfaz no revele las elecciones rivales hasta completar ambas elecciones.

### Etapa 4 — Administración del mazo

- [x] Crear Mazo con su colección privada de Personaje.
- [x] Implementar Mezclar, Repartir, Devolver y GetCantidad conforme a las firmas del UML.
- [ ] Comprobar que el reparto retire los elementos entregados y respete su disponibilidad.
- [ ] Restaurar el mazo completo al cerrar cada ronda, incluyendo personajes jugados y no jugados, sin pérdidas ni repeticiones.
- [ ] Impedir iniciar una partida con menos de seis personajes válidos y mostrar el mensaje acordado.
- [ ] Probar mazo vacío, reparto insuficiente, mezcla y recuperación completa.

### Etapa 5 — Implementación de las reglas

- [ ] Implementar Partida, Ronda y Combate con los contratos acordados en TI.
- [ ] Iniciar con J1 como jugador mano y repartir tres personajes a cada jugador.
- [ ] Registrar carta y atributo del elector y después la carta del rival según la secuencia definida.
- [ ] Resolver cada combate por el atributo elegido; ante empate gana quien no eligió el atributo.
- [ ] Retirar de la mano los personajes jugados para impedir reutilizarlos durante la ronda.
- [ ] Alternar el elector de atributo entre combates, independientemente del ganador.
- [ ] Cerrar la ronda al alcanzar dos combates ganados y sumar una ronda al marcador del vencedor.
- [ ] Recuperar todos los personajes y alternar el jugador mano al terminar cada ronda.
- [ ] Finalizar al alcanzar 15 rondas ganadas, sin iniciar otra ronda.
- [ ] Ejecutar las pruebas de combates, empates, alternancia, cierre anticipado de ronda y fin de partida.

### Etapa 6 — Comunicación entre computadoras

- [ ] Implementar las entidades y configuraciones de intercambio definidas en la actividad 6 de TI.
- [ ] Comprobar el acceso al recurso compartido desde los dos clientes y el servidor.
- [ ] Cargar y publicar mediante Datos.Modelo, guardando solo las tablas propias de cada proceso.
- [ ] Implementar la secuencia de elecciones, análisis en el servidor y presentación de resultados en los clientes.
- [ ] Aplicar identificación y control de registros procesados conforme al protocolo acordado.
- [ ] Comprobar bloqueos y publicación de archivos completos con DataBases en la red local.
- [ ] Implementar el cierre coordinado ante fallos, sin recuperación de la partida.
- [ ] Probar el intercambio entre dos computadoras, registros repetidos o fuera de secuencia y pérdida de acceso; registrar los resultados.

### Etapa 7 — Integración

- [ ] Integrar la carga de personajes, mazo, jugadores, reglas, consola y comunicación en la aplicación de consola.
- [ ] Comprobar que las firmas y responsabilidades implementadas coincidan con los contratos de TI.
- [ ] Resolver con los Analistas las diferencias detectadas y actualizar la documentación afectada.
- [ ] Ejecutar `dotnet build Proyecto-Integrador-Final.sln` y corregir los errores de compilación.
- [ ] Ejecutar `dotnet test Proyecto-Integrador-Final.sln` y corregir los fallos de las pruebas.
- [ ] Iniciar la aplicación en ambas computadoras con los personajes entregados por los alumnos.
- [ ] Completar una partida en red local y comprobar su resultado en ambas computadoras.
- [ ] Registrar los resultados del plan de pruebas y resolver los fallos pendientes.
- [ ] Revisar que el código esté en `src/`, las pruebas en `test/` y la documentación en `docs/`.
- [ ] Comprobar que estén disponibles todos los entregables de TI y el código fuente integrado.
- [ ] Preparar la defensa de las decisiones de diseño, estrategias de personajes, clases y métodos de cada integrante.

Los Analistas coordinan los entregables y comunican los cambios a Taller. Los cambios para GitHub siguen las ramas, los commits y la revisión por un compañero establecidos en [CONTRIBUTING](../CONTRIBUTING.md).

## Revisión del estado real — 13 de noviembre de 2026

Se revisaron main (`dcfc9eb`), las dos ramas de protocolos, los worktrees limpios, las referencias remotas tras `git fetch`, el código, los proyectos y el submódulo. Main ya estaba sincronizado con origin/main y no requirió actualización. Los avances de las ramas se registran sin integrarlos ni alterar sus directorios.

Personaje contiene constructor, las cuatro restricciones numéricas y consultas de nombre, ASCII y atributo. No contiene descripción ni configuración de persistencia efectiva. Mazo contiene constructor, mezcla Fisher–Yates, reparto, devolución y consulta de cantidad. Program conserva el ejemplo inicial; UnitTest1.Test1 no tiene aserciones. No existen todavía Jugador, Partida, Ronda, Combate, Datos ni InterfazConsola en src. DataBases y Utilidades sí están referenciadas e incluidas en la solución, aunque no hay flujo de carga ni juego conectado a ellas.

Verificación: `dotnet build Proyecto-Integrador-Final.sln` terminó sin errores; se observó CA1416 en Utilidades/MyConsole.cs por una API de consola exclusiva de Windows. La primera ejecución simultánea de compilación y pruebas produjo reintentos MSB3026 de copia; se repitieron secuencialmente para eliminar esa interferencia. `dotnet test Proyecto-Integrador-Final.sln` pasó una única prueba vacía, lo que no valida reglas ni comunicación. `git diff --check` verificó los cambios documentales.

Se mantienen los cierres confirmados de las actividades 1 a 5 y los pendientes de la actividad 7. Los registros históricos de compilación y diseño que siguen describen sus revisiones originales; no sustituyen esta comprobación ni acreditan el comportamiento del juego.

## Revisión de documentación — 9 de noviembre de 2026

Se contrastaron los diez documentos Markdown del repositorio y del submódulo con el código, los proyectos, el UML y el estado de los PR #6, #7, #9 y #10 en GitHub. Se actualizaron las referencias a PR ya integrados, la estructura publicada y el flujo de sincronización de ramas. README y Arquitectura aclaran que las bibliotecas todavía no están referenciadas por la aplicación ni incluidas en la solución principal. La consigna original y las decisiones de diseño se conservan; las actividades 5 a 7 siguen pendientes. No se detectaron correcciones necesarias en los README del submódulo para esta revisión y se conservó su versión registrada.

Verificación: los 33 enlaces locales apuntan a archivos o directorios existentes; el XML del UML es válido y sus 58 operaciones tienen 58 subsecciones de especificación en Arquitectura. `git diff --check` no encontró problemas. La solución compiló sin advertencias ni errores y `dotnet test Proyecto-Integrador-Final.sln --no-build` ejecutó correctamente la única prueba existente, que está vacía. La ejecución de la aplicación mostró `Hello, World!`. Estos resultados verifican la base inicial; no acreditan funcionamiento del juego, pruebas de las bibliotecas ni comunicación en red.

## Resultado de la revisión del modelo — 8 de noviembre de 2026

Se actualizó el UML y se justificaron contratos y responsabilidades en Arquitectura, incluyendo consultas de combate y ronda necesarias para presentar resultados y recuperar cartas sin exponer colecciones privadas. Se comprobó el XML del diagrama y se revisaron firmas, tipos y dependencias contra las bibliotecas. `git diff --check` no encontró problemas y `dotnet build Proyecto-Integrador-Final.sln` terminó con cero errores y advertencias. Esta compilación solo verifica la base inicial existente; no acredita implementación ni funcionamiento del juego. No se agregaron pruebas de código para un cambio documental. La revisión visual se completó mediante exportación con UMLet 15.1 y se verificaron geométricamente las 16 relaciones, sin tramos que atraviesen cajas y con extremos conectados a bordes. Se consultó UML 2.5.1 y se corrigió la notación de enumeraciones, miembros estáticos y realización de interfaz. La integración del protocolo sigue pendiente; el PR #9 del modelo ya está integrado desde el 8 de noviembre de 2026 (hora de Argentina).

Corrección de notación solicitada por el usuario: referencias y colecciones se muestran solo como asociaciones, con roles separados, multiplicidad y visibilidad explícita. Datos.Modelo conserva lectura pública, escritura privada y carácter estático en la relación.

Por indicación del usuario, se retiraron las cuatro flechas de dependencia `.>`; el uso de las bibliotecas y entidades se explica en Arquitectura. Las asociaciones y la realización de la interfaz de DataBases se conservan.

Datos se amplió con constantes de rutas y configuración explícita de TipoProceso. Los valores reales de red y las entidades de intercambio siguen pendientes; no se implementó código.

Se ajustaron las dimensiones de las cajas al texto y se verificaron las exportaciones PNG y SVG de UMLet: sin textos recortados ni relaciones atravesando cajas.

Se compactó la distribución de clases y se recalcularon los recorridos de las relaciones, conservando las cajas ajustadas al texto. La exportación de UMLet se revisó visualmente.

Revisión final del dominio: jugador mano pertenece solo a Ronda; Combate ofrece GetParticipantes para presentar sus elecciones y Datos.Modelo tiene multiplicidad 0..1 hasta inicializarse. Se revisaron firmas, encapsulamiento y coherencia de reglas. Por decisión posterior del usuario, la actividad 3 se da por terminada y el modelado de entidades, relaciones y firmas de intercambio queda asignado exclusivamente a la actividad 6. Los detalles de métodos, formato del catálogo y pruebas permanecen en sus actividades respectivas; la revisión de un compañero sigue siendo requisito para integrar el PR.

Cierre de la actividad 3 por confirmación del usuario: sin tareas propias pendientes. El modelado de elecciones y publicaciones, su configuración en Datos y los métodos de integración de clientes y Partida quedan en la actividad 6. El cierre documental quedó incorporado mediante el PR #9; no implica implementación.

## Ampliación documental de Personaje — actividad 5

Por indicación del usuario se agregó descripcion: string al modelo de Personaje y a sus columnas, se amplió el constructor y se especificó GetDescripcion. La descripción es opcional y no tiene límite de longitud por confirmación del usuario; su ausencia se representa con texto vacío ("") y su presentación sigue pendiente de decisión. La conversión al CSV se resolverá posteriormente y el usuario garantiza un catálogo preparado sin duplicados. No se implementó código. Posteriormente el usuario confirmó el cierre de la actividad 5 con esta especificación. Los registros anteriores de 58 operaciones corresponden a la revisión previa a esta ampliación.
