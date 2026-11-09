# Plan de trabajo — Juego Guerra

Plan basado en [la consigna](Consigna.md). Marcar una tarea cuando su resultado esté disponible y revisado. Las actividades de TI definen los contratos que se usarán en Taller; las decisiones pendientes deben resolverse antes de implementar el componente correspondiente. Por decisión del usuario, no se utilizarán diagramas Nassi-Schneiderman (NS); el comportamiento de los métodos se documentará por escrito con sus entradas, resultados, precondiciones y validaciones. La consigna original se conserva como referencia sin modificar.

## Estado revisado — 8 de noviembre de 2026

Alcance actual: únicamente TI (análisis y diseño). Taller queda pendiente y fuera del trabajo actual. El código todavía contiene `Hello, World!` y una prueba vacía; el diseño documentado no está implementado.

Se revisaron el documento de avance aportado por el usuario y el [UML existente](diagramas/UML.uxf). El documento se utilizó como referencia local y no forma parte de la entrega del repositorio. Cada casilla de TI refleja esa revisión y las confirmaciones posteriores del usuario: `[x]` indica un resultado comprobado o confirmado, `[ ]` indica trabajo pendiente y **En curso** señala un avance parcial con el cierre que falta. Los valores verificados de personajes corresponden a Lysander y Reched. El usuario confirmó que la actividad 1 está completada; se retiran los pendientes de alternativas, comparación, justificación y verificación de otras fichas como condiciones para reabrirla. La imagen se trabaja en Arte. Las demás actividades conservan su estado de revisión. Completar una tarea de diseño no implica implementar código.

| Actividad de TI | Estado y evidencia | Qué falta para cerrarla |
|---|---|---|
| 1. Personajes | Completada por confirmación del usuario. Lysander y Reched tienen nombre, descripción, seis valores, total y atributos principal y menor; ambos cumplen las restricciones. | Sin pendientes para esta actividad. |
| 2. Requerimientos | Documento completo y revisado en [Reglas del juego](Reglas-del-juego.md), listo para copiar a la entrega. Incluye reglas, participantes, datos de ejecución, comunicación, errores y separación de requisitos funcionales y técnicos. | Sin tareas de redacción pendientes. La aceptación del PR y la revisión del compañero siguen pendientes; el protocolo detallado corresponde a la actividad 6. |
| 3. Modelo de clases | Dominio actualizado en UML y Arquitectura, con Datos y bibliotecas; firmas y responsabilidades revisadas. | Integración del protocolo tras actividad 6, revisión docente y de un compañero. |
| 4. Métodos | Borrador de Partida, Jugador y Mazo. | Unificar las firmas con el UML y completar contratos, validaciones y excepciones. |
| 5. Archivos de personajes | Solo se indica un límite de ASCII: «máximo 25 líneas y 120 caracteres». | Aclarar si 120 es por línea o en total; definir sintaxis, campos, descripción, ASCII, errores y ejemplos válidos e inválidos. |
| 6. Archivo de partida | Dos clientes y servidor; intercambio mediante Datos y DataBases. | Definir registros, identificación, secuencia, permisos, publicación y cierre coordinado. Verificar funcionamiento en red local. |
| 7. Pruebas | No se encontró un plan de casos desarrollado en el material revisado. | Diseñar condiciones iniciales, datos y resultados esperados para casos normales, límites y errores. |

### Verificación de personajes

Valores en orden: Fuerza, Velocidad, Resistencia, Inteligencia, Energía y Habilidad.

| Personaje | Valores | Total | Principal / menor | Resultado |
|---|---|---|---|---|
| Lysander | 750, 550, 500, 250, 500, 550 | 3000 | Fuerza / Inteligencia | Cumple rango 250–750 y umbrales 650 y 350. |
| Reched | 250, 700, 350, 750, 450, 500 | 3000 | Inteligencia / Fuerza | Cumple rango 250–750 y umbrales 650 y 350. |

### Reglas ya redactadas

- Participan dos jugadores y se reparten tres cartas a cada uno desde un mazo común.
- El jugador mano elige el primer atributo; ambos jugadores eligen su carta y la muestran simultáneamente. Gana el mayor valor y, ante empate, quien no eligió el atributo del combate.
- La elección del atributo se alterna entre los jugadores en cada combate, independientemente del ganador. La ronda se gana con dos combates ganados, por lo que puede terminar sin jugar la tercera carta.
- Al terminar la ronda se devuelven las cartas jugadas al mazo, se mezcla y se reparte nuevamente. Gana la partida quien alcanza 15 rondas ganadas.

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
- Restauración completa del mazo documentada en UML y Arquitectura; completar la especificación detallada en la actividad 4.
- El mazo inicial se forma a partir del CSV compartido que genera un sistema externo al cliente. Los repetidos están prohibidos y menos de seis cartas válidas impiden iniciar la partida con un mensaje de error. El sistema externo de carga impide duplicados. Falta definir el formato en la actividad 5.
- Especificar cómo se mantienen ocultas carta y atributo hasta registrar ambas elecciones; el orden de elección ya está definido en Reglas-del-juego.md.
- En la actividad 4 usar las firmas vigentes del UML y Arquitectura para sustituir las diferencias del borrador anterior.
- Separar los archivos iniciales de personajes del intercambio de acciones de la partida; definir identidad de partida, turno, ronda y combate, acciones duplicadas o fuera de turno y escrituras incompletas.

### Orden de trabajo para TI

1. Actividades 1 y 2 completadas en su documentación. La actividad 2 está lista para copiar a la entrega y para revisión del PR.
2. Revisar el UML y justificar sus decisiones en Arquitectura (actividad 3).
3. Completar las especificaciones de métodos según el modelo acordado (actividad 4).
4. Definir los formatos de personajes y partida compartida con sus errores y ejemplos (actividades 5 y 6).
5. Completar el plan de pruebas y revisar que todos los entregables de TI sean coherentes (actividad 7).

Las actividades 1 y 2 tienen sus entregables documentados. Sigue pendiente revisar el modelo de clases (actividad 3), con el dominio y dependencias actualizados y la integración de comunicación pendiente. La actividad 4 conserva un borrador por adecuar. Los demás entregables de TI no se consideran cerrados solo por contar con un borrador.

## TI — Análisis de Sistemas

### Actividad 1 — Análisis y diseño de los personajes

**Completada por confirmación del usuario.** Se conserva el registro de los datos y las validaciones revisadas, sin tareas adicionales pendientes para esta actividad.

- [x] Escribir el nombre y la descripción de Lysander y Reched (fichas del documento revisado).
- [x] Elegir una distribución para Lysander y Reched (valores registrados en la tabla de verificación).
- [x] Comprobar que los seis valores de ambos personajes estén entre 250 y 750 y sumen exactamente 3000.
- [x] Comprobar en ambos personajes que al menos un atributo sea mayor o igual a 650 y otro menor o igual a 350; ninguno tiene seis valores de 500.
- [x] Identificar el atributo principal y el de menor valor de ambos personajes.
- [x] Dar por completada la actividad 1 según la confirmación del usuario.

### Actividad 2 — Relevamiento de requerimientos

- [x] Escribir el objetivo del juego: alcanzar 15 rondas ganadas.
- [x] Identificar a los dos participantes y sus acciones: elegir atributo cuando corresponde y elegir una carta de su mano.
- [x] Definir el reparto de tres cartas a cada jugador desde un mazo común y la mezcla entre rondas.
- [x] Definir el origen del mazo inicial: personajes cargados en un CSV compartido por un sistema externo al cliente de los jugadores.
- [x] Registrar que carta y personaje son el mismo elemento y que no se utilizará una clase Carta adicional.
- [x] Definir que menos de seis cartas válidas impiden iniciar la partida y muestran el error «No se puede jugar porque no hay suficientes cartas».
- [x] Prohibir personajes repetidos en el mazo.
- [x] Asignar la prevención de personajes duplicados al sistema externo de carga, fuera del cliente y del servidor de partida.
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
- [x] Completar los datos de ejecución: personajes y mazo, cartas disponibles y jugadas, jugadores, ronda, combate, jugador mano, elección de atributo, elecciones realizadas, marcadores, resultados y estado de partida.
- [x] Definir la comunicación general: dos clientes publican sus elecciones; el servidor las lee, analiza y publica mensajes de resultados. Cada proceso solo escribe sus archivos y lee los de los demás.
- [x] Asignar al cliente la validación de sus datos antes del envío y al servidor el tratamiento de errores al iniciar la partida.
- [x] Definir responsabilidades y política general de errores: el cliente garantiza envíos válidos; cualquier error termina el juego completo, incluido el fallo del servidor. Los tipos de acciones, secuencias y formatos se difieren a la actividad 6.
- [x] Definir la respuesta ante errores iniciales y de ejecución: finalizar el juego completo, sin recuperación. Con menos de seis cartas se informa además el mensaje ya acordado.
- [x] Separar los requerimientos funcionales de las condiciones técnicas de ejecución en Reglas-del-juego.md.
- [x] Revisar la cobertura de la consigna y preparar el documento de requerimientos y reglas, listo para que el usuario lo copie a su entrega. No se afirma que ya haya realizado la copia ni presentado la entrega.

El PR de esta actividad permanecerá en borrador mientras existan tareas pendientes de la actividad 2. El usuario indicó que solo lo aceptará cuando todas estén realizadas; antes de solicitar su revisión final, comprobar cada casilla y la actualización del documento de entrega. La integración requiere además la revisión de un compañero prevista en CONTRIBUTING.md.

### Actividad 3 — Modelo de clases

**En curso:** modelo de dominio actualizado según la recapitulación aceptada por el usuario. No hay implementación. El detalle vigente está en [Arquitectura](Arquitectura.md).

- [x] Representar Personaje, Mazo, Jugador, Partida, Ronda y Combate con atributos privados, firmas tipadas y relaciones.
- [x] Adaptar todas las referencias al concepto unificado de Personaje.
- [x] Asignar coordinación del dominio al servidor y presentación a los clientes, sin comunicación directa entre rivales.
- [x] Incorporar Datos estática y las dependencias de DataBases sin duplicar la lectura CSV.
- [x] Describir InterfazConsola delegando entradas, menús y presentación en Utilidades.
- [x] Justificar responsabilidades, encapsulamiento, delegación y decisiones en Arquitectura.
- [x] Reflejar J1 como mano inicial, alternancia por ronda y combate, empate, dos victorias por ronda y 15 rondas por partida.
- [x] Documentar recuperación de todas las cartas y ocultación en interfaz hasta completar ambas elecciones.
- [x] Unificar las firmas de referencia en UML y Arquitectura; adecuar los borradores anteriores en la actividad 4.
- [ ] Completar entidades, relaciones y firmas de integración del intercambio cuando se defina el protocolo en la actividad 6.
- [ ] Revisar con el docente la diferencia entre Personaje unificado y la separación requerida para Taller.
- [ ] Revisar con SSI y el docente el esquema de archivos compartidos y verificar la biblioteca en red local.
- [ ] Obtener revisión de un compañero antes de integrar el PR y cerrar la actividad.

### Actividad 4 — Especificación de métodos

- [x] Identificar en el UML los métodos vigentes de Personaje, Mazo, Jugador, Partida, Ronda, Combate, Datos e InterfazConsola.
- [ ] **En curso:** completar la responsabilidad de cada método; el borrador describe Jugar, la elección de atributo, la elección de carta y el reparto, pero no cubre todo el UML.
- [ ] **En curso:** unificar parámetros y retornos del borrador con el UML y completar las firmas incompletas.
- [ ] **En curso:** completar precondiciones y resultados; el borrador ya limita la elección a los seis atributos y a las cartas disponibles, y describe el reparto de tres cartas.
- [ ] Completar validaciones y excepciones por método a partir de los contratos vigentes y las excepciones de DataBases y Utilidades.
- [ ] Revisar con Taller que los contratos permitan implementar los métodos sin inventar reglas.

### Actividad 5 — Formato de los archivos de personajes

- [ ] Especificar el CSV compartido generado por el sistema externo y su relación con los archivos individuales de personajes exigidos por la consigna, incluyendo campos, ASCII Art, duplicados y errores de lectura.
- [ ] Definir la estructura del archivo de texto y cómo se reconoce cada dato.
- [ ] Especificar el formato del nombre y de los seis atributos obligatorios.
- [ ] **En curso:** definir cómo se incorpora o referencia la descripción y el ASCII Art; solo está escrito el límite de 25 líneas y 120 caracteres, cuya interpretación falta precisar.
- [x] Documentar las restricciones numéricas: seis atributos entre 250 y 750, total 3000, uno de al menos 650 y otro de como máximo 350, sin seis valores de 500 (documento revisado y consigna).
- [ ] Especificar cómo se aplican esas restricciones al leer el archivo y cómo se informa cada incumplimiento.
- [ ] Definir la respuesta ante campos faltantes, duplicados o incorrectos.
- [ ] Definir la respuesta ante un archivo inexistente o que no se puede leer.
- [ ] Preparar un archivo válido y ejemplos inválidos para cada error previsto.
- [ ] Revisar y entregar la especificación del formato común.

### Actividad 6 — Archivo de partida compartida

Los registros de elecciones y publicaciones se definirán en esta actividad. El acceso a archivos se realiza mediante Datos y DataBases. La validez de los envíos corresponde al cliente y cualquier fallo de ejecución finaliza el juego completo, sin recuperación.

- [x] Definir un escritor exclusivo por archivo: cada cliente escribe sus elecciones y el servidor sus publicaciones; los demás procesos leen.
- [ ] Definir las entidades de intercambio y sus campos, cantidad de archivos, rutas, permisos y configuración en DataBases.
- [ ] Especificar la identificación de partida, jugadores, estado, ronda, combate y jugador autorizado a elegir.
- [ ] Definir cómo se relacionan los registros de intercambio con los personajes del catálogo.
- [ ] Determinar cuándo cada proceso ejecuta Cargar<T> y Guardar<T> y qué tablas puede guardar.
- [ ] Especificar la secuencia de elecciones y la publicación de resultados sin revelar elecciones anticipadamente en la interfaz.
- [ ] Definir cómo se reconocen registros ya procesados, repetidos o correspondientes a otra partida, ronda o combate.
- [ ] Especificar publicación de mensajes completos y coherencia entre archivos, considerando que Guardar reemplaza una tabla completa y no hay transacciones entre archivos.
- [ ] Especificar la creación inicial de las tablas y cuáles requieren archivos existentes al cargarse.
- [ ] Definir la detección de fallos y el cierre coordinado de clientes y servidor, sin recuperación.
- [ ] Verificar con SSI el acceso, permisos, bloqueos y reemplazo de archivos en la red local.
- [ ] Completar las entidades y firmas de integración pendientes en el UML y Arquitectura.
- [ ] Entregar la especificación de intercambio, diferenciándola del catálogo inicial de personajes.

### Actividad 7 — Casos de prueba

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

Etapas conservadas como referencia futura. No se implementarán en el alcance actual, limitado a TI.

### Etapa 1 — Implementación de Personaje

- [ ] Crear la clase Personaje en `src/` conforme al modelo acordado.
- [ ] Implementar el constructor con nombre, seis atributos y ASCII Art.
- [ ] Implementar las validaciones de los atributos según los contratos de TI.
- [ ] Implementar las operaciones de consulta conservando el encapsulamiento.
- [ ] Agregar en `test/` pruebas de personajes válidos y de cada restricción de puntajes.

### Etapa 2 — Carga de personajes con Datos y DataBases

- [ ] Referenciar DataBases e implementar Personaje.Configurar con el formato definido en TI.
- [ ] Crear Datos estática con su Modelo e Iniciar conforme al diseño.
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

- [ ] Crear Mazo con su colección privada de Personaje.
- [ ] Implementar Mezclar, Repartir, Devolver y GetCantidad conforme a las firmas del UML.
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

## Resultado de la revisión del modelo — 8 de noviembre de 2026

Se actualizó el UML y se justificaron contratos y responsabilidades en Arquitectura, incluyendo consultas de combate y ronda necesarias para presentar resultados y recuperar cartas sin exponer colecciones privadas. Se comprobó el XML del diagrama y se revisaron firmas, tipos y dependencias contra las bibliotecas. `git diff --check` no encontró problemas y `dotnet build Proyecto-Integrador-Final.sln` terminó con cero errores y advertencias. Esta compilación solo verifica la base inicial existente; no acredita implementación ni funcionamiento del juego. No se agregaron pruebas de código para un cambio documental. La revisión visual en UMLet y la integración del protocolo siguen pendientes; el PR se prepara en borrador para revisión de un compañero.

Corrección de notación solicitada por el usuario: referencias y colecciones se muestran solo como asociaciones, con roles separados, multiplicidad y visibilidad explícita. Datos.Modelo conserva lectura pública, escritura privada y carácter estático en la relación.
