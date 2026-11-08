# Plan de trabajo — Juego Guerra

Plan basado en [la consigna](Consigna.md). Marcar una tarea cuando su resultado esté disponible y revisado. Las actividades de TI definen los contratos que se usarán en Taller; las decisiones pendientes deben resolverse antes de implementar el componente correspondiente.

## Estado revisado — 8 de noviembre de 2026

Alcance actual: únicamente TI (análisis y diseño). Taller queda pendiente y fuera del trabajo actual. El código todavía contiene `Hello, World!` y una prueba vacía; el diseño documentado no está implementado.

Se revisaron el documento de avance aportado por el usuario y el [UML existente](diagramas/UML.uxf). El documento se utilizó como referencia local y no forma parte de la entrega del repositorio. Cada casilla de TI refleja esa revisión y las confirmaciones posteriores del usuario: `[x]` indica un resultado comprobado o confirmado, `[ ]` indica trabajo pendiente y **En curso** señala un avance parcial con el cierre que falta. Los valores verificados de personajes corresponden a Lysander y Reched. El usuario confirmó que la actividad 1 está completada; se retiran los pendientes de alternativas, comparación, justificación y verificación de otras fichas como condiciones para reabrirla. La imagen se trabaja en Arte. Las demás actividades conservan su estado de revisión. Completar una tarea de diseño no implica implementar código.

| Actividad de TI | Estado y evidencia | Qué falta para cerrarla |
|---|---|---|
| 1. Personajes | Completada por confirmación del usuario. Lysander y Reched tienen nombre, descripción, seis valores, total y atributos principal y menor; ambos cumplen las restricciones. | Sin pendientes para esta actividad. |
| 2. Requerimientos | Reglas principales, mazo común, datos de ejecución y una propuesta de comunicación redactados. Confirmado: J1 comienza como jugador mano y el rol se alterna entre rondas. | Actualizar la terminología y las reglas del documento de entrega, completar errores y respuestas, separar requisitos funcionales y técnicos y resolver las precisiones indicadas abajo. |
| 3. Modelo de clases | UML elaborado con clases, atributos privados, firmas y relaciones. | Justificar responsabilidades y decisiones, revisar coherencia con las reglas y cerrar las observaciones de arquitectura. |
| 4. Métodos | Borrador de Partida, Jugador y Mazo. | Unificar las firmas con el UML y completar contratos, validaciones, excepciones y diagramas de algoritmos. |
| 5. Archivos de personajes | Solo se indica un límite de ASCII: «máximo 25 líneas y 120 caracteres». | Aclarar si 120 es por línea o en total; definir sintaxis, campos, descripción, ASCII, errores y ejemplos válidos e inválidos. |
| 6. Archivo de partida | Confirmados dos clientes y un servidor, con archivos de escritura exclusiva por proceso; UML todavía con rutas propia y rival. | Revisar con la consigna y SSI el esquema confirmado de varios archivos compartidos; especificar formato, identificación, lecturas, escrituras, concurrencia y recuperación de errores. |
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

Esta decisión y las reglas confirmadas están redactadas en [Reglas del juego](Reglas-del-juego.md). Todavía falta trasladar y revisar la redacción en el documento de entrega del usuario, si se mantiene por separado.

### Precisiones pendientes antes de cerrar el diseño

- Aplicar la decisión de J1 como jugador mano inicial y la alternancia entre rondas al documento de entrega. Unificar la terminología de partida, ronda, combate, jugador mano y mano de cartas, y comprobar su coherencia con los contratos.
- Ajustar los contratos del UML a la restauración del mazo completo al terminar cada ronda, incluidas las cartas no jugadas.
- El mazo inicial se forma a partir del CSV compartido que genera un sistema externo al cliente. Los repetidos están prohibidos y menos de seis cartas válidas impiden iniciar la partida con un mensaje de error. El sistema externo de carga impide duplicados. Falta definir el formato en la actividad 5.
- Especificar cómo se mantienen ocultas carta y atributo hasta registrar ambas elecciones; el orden de elección ya está definido en Reglas-del-juego.md.
- Alinear las firmas del borrador con el UML: `Partida.Jugar(Jugador, Jugador): void` frente a constructor con jugadores y `Jugar(): Jugador`; elecciones en Jugador frente a `IInterfaz`; `Mazo.GetMano()` frente a `Repartir(cantidad)`.
- Separar los archivos iniciales de personajes del intercambio de acciones de la partida; definir identidad de partida, turno, ronda y combate, acciones duplicadas o fuera de turno y escrituras incompletas.

### Orden de trabajo para TI

1. Cerrar las precisiones de reglas y requerimientos de la actividad 2. La actividad 1 está completada.
2. Revisar el UML y justificar sus decisiones en Arquitectura (actividad 3).
3. Completar las especificaciones de métodos según el modelo acordado (actividad 4).
4. Definir los formatos de personajes y partida compartida con sus errores y ejemplos (actividades 5 y 6).
5. Completar el plan de pruebas y revisar que todos los entregables de TI sean coherentes (actividad 7).

La actividad 1 está completada. Estamos consolidando requerimientos y modelo de clases (actividades 2 y 3), con un inicio de especificación de métodos (actividad 4). Los demás entregables de TI no se consideran cerrados solo por contar con un borrador.

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
- [ ] Actualizar el documento de entrega con J1 como jugador mano inicial y la alternancia J1/J2 al terminar cada ronda, independientemente de su ganador.
- [ ] Diferenciar en todo el documento partida, ronda, combate, jugador mano y mano de cartas; sustituir «mano ganada» por «ronda ganada» y aclarar que se juegan hasta tres combates.
- [x] Definir quién elige el atributo: el jugador mano en el primer combate; después se alterna entre jugadores, independientemente del ganador. Reemplaza la regla anterior basada en el ganador.
- [x] Especificar la comparación por el atributo elegido y la victoria de ronda al alcanzar dos combates ganados.
- [x] Resolver la regla de empate: gana quien no eligió el atributo.
- [x] Definir que las cartas jugadas vuelven al mazo común al terminar la ronda.
- [x] Definir la restauración del mazo completo al terminar cada ronda, incluidas las cartas no jugadas; se mezcla antes del nuevo reparto.
- [x] Definir la alternancia del jugador mano entre rondas y del jugador que elige el atributo entre combates.
- [x] Definir la secuencia: quien comienza el combate elige carta y atributo; el rival elige sin conocer esas elecciones; luego se revelan ambas cartas y el atributo.
- [x] Limitar la ocultación de elecciones a la interfaz: se revelan cuando ambos han elegido; se acepta que los archivos puedan consultarse manualmente. No se exige una garantía de secreto frente a ese acceso.
- [x] Establecer la victoria y finalización normal de la partida al alcanzar 15 rondas ganadas.
- [x] Enumerar los datos de ejecución: mazo, cartas de cada jugador, cartas jugadas, combates ganados y rondas ganadas.
- [x] Definir la comunicación general: dos clientes publican sus elecciones; el servidor las lee, analiza y publica mensajes de resultados. Cada proceso solo escribe sus archivos y lee los de los demás.
- [x] Asignar al cliente la validación de sus datos antes del envío y al servidor el tratamiento de errores al iniciar la partida.
- [x] Definir responsabilidades y política general de errores: el cliente garantiza envíos válidos; cualquier error termina el juego completo, incluido el fallo del servidor. Los tipos de acciones, secuencias y formatos se difieren a la actividad 6.
- [x] Definir la respuesta ante errores iniciales y de ejecución: finalizar el juego completo, sin recuperación. Con menos de seis cartas se informa además el mensaje ya acordado.
- [ ] Separar los requerimientos funcionales de las condiciones técnicas de ejecución.
- [ ] Revisar y entregar el documento de requerimientos y reglas.

El PR de esta actividad permanecerá en borrador mientras existan tareas pendientes de la actividad 2. El usuario indicó que solo lo aceptará cuando todas estén realizadas; antes de solicitar su revisión final, comprobar cada casilla y la actualización del documento de entrega. La integración requiere además la revisión de un compañero prevista en CONTRIBUTING.md.

### Actividad 3 — Modelo de clases

- [ ] Adaptar el UML a dos clientes y un servidor: reemplazar lectura directa del rival, asignar análisis y resultados al servidor y revisar ControlTurno, ArchivoPartida y Accion.

- [ ] Adaptar el UML, sus relaciones y contratos a la decisión de unificar Carta y Personaje. Las casillas históricas sobre Carta describen el diagrama anterior, no una aprobación del diseño vigente.
- [ ] Revisar con el docente la diferencia entre esta decisión y la separación explícita de Carta y Personaje que establece la consigna para Taller; conservar Taller fuera del alcance actual.
- [x] Definir las responsabilidades principales de Personaje, Carta, Mazo y los objetos de partida (UML y descripción en Arquitectura).
- [x] Identificar las clases adicionales: Jugador, Partida, Ronda, Combate, ControlTurno, LectorPersonaje, ArchivoPartida, Accion e interfaz de consola, con sus enumeraciones.
- [ ] Justificar las clases adicionales y las decisiones de separación de responsabilidades.
- [x] Definir los atributos de las clases y su encapsulamiento privado en el UML.
- [ ] **En curso:** completar las firmas del modelo; la mayoría incluye parámetros y retornos, pero el constructor de Accion no tipa sus parámetros y existen diferencias con el borrador de métodos.
- [x] Definir las relaciones principales y la delegación de Carta en Personaje (UML y Arquitectura).
- [x] Elaborar el diagrama de clases UML (disponible en `docs/diagramas/UML.uxf`; revisión final pendiente).
- [ ] **En curso:** completar la justificación de decisiones en [Arquitectura](Arquitectura.md) y cerrar la revisión de coherencia; ya se documentaron responsabilidades y diferencias pendientes.

### Actividad 4 — Especificación de métodos

- [x] Identificar los métodos del modelo de clases en el UML, incluidos los de partida, ronda, combate, cartas, mazo, personajes, turnos, archivos e interfaz.
- [ ] **En curso:** completar la responsabilidad de cada método; el borrador describe Jugar, la elección de atributo, la elección de carta y el reparto, pero no cubre todo el UML.
- [ ] **En curso:** unificar parámetros y retornos del borrador con el UML y completar las firmas incompletas.
- [ ] **En curso:** completar precondiciones y resultados; el borrador ya limita la elección a los seis atributos y a las cartas disponibles, y describe el reparto de tres cartas.
- [ ] **En curso:** completar validaciones y excepciones por método; el UML ya enumera cuatro excepciones y el rechazo de acciones fuera de turno, ronda o combate.
- [ ] Elaborar diagramas de Nassi-Schneiderman para los algoritmos que los necesiten.
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

Los tipos de acciones se planificarán en esta actividad, según decisión del usuario; los enumerados actualmente en el UML son antecedentes, no un protocolo aprobado. La validez de los envíos corresponde al cliente y cualquier error finaliza el juego completo.

- [ ] Especificar cómo se detecta un fallo del servidor y cómo se comunica y coordina la finalización de ambos clientes y del servidor, sin recuperación de la partida.

- [x] Definir un escritor exclusivo por archivo: cada cliente escribe sus acciones y el servidor sus mensajes y resultados; los demás procesos acceden en modo lectura.
- [ ] Definir cantidad, rutas, permisos, datos y formato de los archivos de clientes y servidor; revisar Accion y agregar los mensajes de resultados.
- [ ] **En curso:** completar la identificación de partida, jugadores, estado y turno; ControlTurno y EstadoPartida ya modelan estados, actores pendientes y números de ronda y combate, pero falta identidad de partida y su representación en el archivo.
- [ ] **En curso:** especificar el registro de acciones; el UML propone Registrar y cinco tipos de acción, pero no define formato, confirmación ni duplicados.
- [ ] **En curso:** determinar cuándo lee y actualiza cada computadora; hay rutas propia y rival, contador de líneas leídas y LeerAccionesDelRival, sin frecuencia ni secuencia de operaciones acordadas.
- [x] Definir la regla de autorización del turno en el UML: al elegir atributo actúa solo quien elige; al elegir cartas actúan ambos una vez cada uno; las acciones fuera de turno, ronda o combate se rechazan con AccionInvalidaException.
- [ ] Especificar el algoritmo que valida los datos recibidos y conserva el estado ante una acción inválida o repetida.
- [ ] Completar la regla de escritor exclusivo con publicación y lectura de mensajes completos, confirmaciones y prevención de procesamiento duplicado.
- [ ] **En curso:** completar la respuesta ante archivo inaccesible, lectura fallida o escritura incompleta; el UML propone EstaDisponible y ArchivoNoDisponibleException, pero falta la política de recuperación.
- [ ] Revisar con SSI la viabilidad del mecanismo en la red local y los permisos necesarios.
- [ ] Entregar la especificación del archivo compartido, diferenciándolo de los archivos iniciales de personajes.

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

### Etapa 2 — Lectura de archivos de personajes

- [ ] Implementar la lectura del formato de texto definido en TI.
- [ ] Interpretar los campos y validar los datos obligatorios y duplicados.
- [ ] Construir objetos Personaje a partir de los datos válidos.
- [ ] Implementar las respuestas previstas para archivos inexistentes, ilegibles o inválidos.
- [ ] Separar lectura, validación y construcción según las responsabilidades del diseño.
- [ ] Probar la carga con los archivos válidos e inválidos de la actividad 5.

### Etapa 3 — Implementación de Carta

Referencia de la consigna para trabajo futuro. La decisión vigente del usuario unifica Carta y Personaje; las tareas siguientes no deben ejecutarse sin resolver esa diferencia y actualizar el modelo.

- [ ] Crear la clase Carta con una referencia a Personaje según el UML.
- [ ] Implementar las consultas de información que necesita mostrar la carta.
- [ ] Implementar la consulta del atributo usado en el enfrentamiento mediante delegación.
- [ ] Comprobar que Carta no duplique los datos que pertenecen a Personaje.
- [ ] Probar que las consultas de Carta devuelvan la información de su personaje.

### Etapa 4 — Administración del mazo

- [ ] Crear la clase Mazo y su almacenamiento de cartas según el diseño.
- [ ] Implementar la incorporación de cartas y la consulta de cantidad disponible.
- [ ] Implementar la mezcla y comprobar que conserve las cartas originales.
- [ ] Implementar el reparto entre los jugadores según las reglas acordadas.
- [ ] Implementar la obtención y el retiro de la carta que participa de una ronda.
- [ ] Implementar la incorporación de las cartas ganadas en el orden especificado.
- [ ] Probar las operaciones con un mazo vacío, una carta y varias cartas.

### Etapa 5 — Implementación de las reglas

- [ ] Implementar la preparación de una partida y la asignación del turno inicial.
- [ ] Implementar la selección del atributo y la validación de la elección.
- [ ] Comparar los valores del atributo en las cartas enfrentadas.
- [ ] Determinar el ganador de la ronda y aplicar la regla de empate acordada.
- [ ] Transferir las cartas involucradas según el resultado de la ronda.
- [ ] Asignar el turno siguiente según las reglas.
- [ ] Detectar las condiciones de finalización y comunicar el resultado de la partida.
- [ ] Ejecutar las pruebas de rondas, empates, turnos y jugadores sin cartas.

### Etapa 6 — Comunicación entre computadoras

- [ ] Comprobar desde ambas computadoras el acceso al archivo compartido configurado en SSI.
- [ ] Implementar la lectura del estado de partida según el formato acordado.
- [ ] Implementar el registro de acciones y la actualización del estado compartido.
- [ ] Validar la identidad de la partida y el turno antes de aceptar una acción.
- [ ] Implementar el mecanismo acordado para evitar inconsistencias por acceso concurrente.
- [ ] Manejar los errores de acceso, lectura y escritura según la especificación.
- [ ] Probar el intercambio entre dos computadoras y registrar los resultados.
- [ ] Probar la pérdida de acceso y el rechazo de información de un turno incorrecto.

### Etapa 7 — Integración

- [ ] Integrar la carga de personajes, cartas, mazos, reglas y comunicación en la aplicación de consola.
- [ ] Comprobar que las interfaces implementadas coincidan con los contratos de TI.
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
