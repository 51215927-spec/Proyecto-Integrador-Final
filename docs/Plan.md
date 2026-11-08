# Plan de trabajo — Juego Guerra

Plan basado en [la consigna](Consigna.md). Marcar una tarea cuando su resultado esté disponible y revisado. Las actividades de TI definen los contratos que se usarán en Taller; las decisiones pendientes deben resolverse antes de implementar el componente correspondiente.

## Estado revisado — 8 de noviembre de 2026

Alcance actual: únicamente TI (análisis y diseño). Taller queda pendiente y fuera del trabajo actual. El código todavía contiene `Hello, World!` y una prueba vacía; el diseño documentado no está implementado.

Se revisaron el documento de avance aportado por el usuario y el [UML existente](diagramas/UML.uxf). El documento se utilizó como referencia local y no forma parte de la entrega del repositorio. Cada casilla de TI refleja esa revisión: `[x]` indica un resultado comprobado, `[ ]` indica trabajo pendiente y **En curso** señala un avance parcial con el cierre que falta. Los resultados de personajes se limitan a Lysander y Reched; no acreditan las fichas del resto del equipo. Completar una tarea de diseño no implica aprobar el entregable completo ni implementar código.

| Actividad de TI | Estado y evidencia | Qué falta para cerrarla |
|---|---|---|
| 1. Personajes | Dos fichas con nombre, descripción, seis valores, total y atributos principal y menor. Ambos personajes cumplen las restricciones. | Comparar distribuciones alternativas, justificar explícitamente la estrategia y completar o verificar las referencias visuales y las fichas restantes del equipo. |
| 2. Requerimientos | Reglas principales, mazo común, datos de ejecución y una propuesta de comunicación redactados. | Definir la mano inicial, completar errores y respuestas, separar requisitos funcionales y técnicos y resolver las precisiones indicadas abajo. |
| 3. Modelo de clases | UML elaborado con clases, atributos privados, firmas y relaciones. | Justificar responsabilidades y decisiones, revisar coherencia con las reglas y cerrar las observaciones de arquitectura. |
| 4. Métodos | Borrador de Partida, Jugador y Mazo. | Unificar las firmas con el UML y completar contratos, validaciones, excepciones y diagramas de algoritmos. |
| 5. Archivos de personajes | Solo se indica un límite de ASCII: «máximo 25 líneas y 120 caracteres». | Aclarar si 120 es por línea o en total; definir sintaxis, campos, descripción, ASCII, errores y ejemplos válidos e inválidos. |
| 6. Archivo de partida | Propuesta de archivo común de personajes y un archivo de mensajes por computadora; UML con rutas propia y rival. | Acordar con la consigna y SSI el uso de un archivo compartido o justificar la alternativa; especificar formato, identificación, lecturas, escrituras, concurrencia y recuperación de errores. |
| 7. Pruebas | No se encontró un plan de casos desarrollado en el material revisado. | Diseñar condiciones iniciales, datos y resultados esperados para casos normales, límites y errores. |

### Verificación de personajes

Valores en orden: Fuerza, Velocidad, Resistencia, Inteligencia, Energía y Habilidad.

| Personaje | Valores | Total | Principal / menor | Resultado |
|---|---|---|---|---|
| Lysander | 750, 550, 500, 250, 500, 550 | 3000 | Fuerza / Inteligencia | Cumple rango 250–750 y umbrales 650 y 350. |
| Reched | 250, 700, 350, 750, 450, 500 | 3000 | Inteligencia / Fuerza | Cumple rango 250–750 y umbrales 650 y 350. |

### Reglas ya redactadas

- Participan dos jugadores y se reparten tres cartas a cada uno desde un mazo común.
- La mano elige el primer atributo; ambos jugadores eligen su carta y la muestran simultáneamente. Gana el mayor valor y, ante empate, quien no eligió el atributo.
- El ganador del combate elige el atributo siguiente. La ronda o «mano» se gana con dos combates ganados, por lo que puede terminar sin jugar la tercera carta.
- Al terminar la ronda se devuelven las cartas jugadas al mazo, se mezcla, se reparte nuevamente y se alterna la mano. Gana la partida quien alcanza 15 rondas ganadas.

### Precisiones pendientes antes de cerrar el diseño

- Determinar cómo se elige la primera mano y confirmar que «mano» como jugador inicial y como ronda no genere ambigüedades en los contratos.
- Aclarar el destino de las cartas no jugadas cuando la ronda termina tras dos combates y el orden de devolución. El UML ofrece `EntregarCartas`, pero no reemplaza una regla escrita.
- Definir cómo se reúnen las cartas iniciales y qué ocurre si hay menos de seis disponibles. Las dos fichas revisadas no acreditan por sí solas un mazo suficiente.
- Precisar el orden de elección de atributo y carta y cómo se garantiza que una computadora no revele su carta antes de que ambas elecciones estén registradas.
- Alinear las firmas del borrador con el UML: `Partida.Jugar(Jugador, Jugador): void` frente a constructor con jugadores y `Jugar(): Jugador`; elecciones en Jugador frente a `IInterfaz`; `Mazo.GetMano()` frente a `Repartir(cantidad)`.
- Separar los archivos iniciales de personajes del intercambio de acciones de la partida; definir identidad de partida, turno, ronda y combate, acciones duplicadas o fuera de turno y escrituras incompletas.

### Orden de trabajo para TI

1. Cerrar las precisiones de reglas y requerimientos de la actividad 2, completando en paralelo las justificaciones de personajes.
2. Revisar el UML y justificar sus decisiones en Arquitectura (actividad 3).
3. Completar las especificaciones de métodos según el modelo acordado (actividad 4).
4. Definir los formatos de personajes y partida compartida con sus errores y ejemplos (actividades 5 y 6).
5. Completar el plan de pruebas y revisar que todos los entregables de TI sean coherentes (actividad 7).

Estamos consolidando requerimientos y modelo de clases (actividades 2 y 3), con un inicio de especificación de métodos (actividad 4). Ningún entregable completo de TI se considera cerrado solo por contar con un borrador.

## TI — Análisis de Sistemas

### Actividad 1 — Análisis y diseño de los personajes

- [x] Escribir el nombre y la descripción de Lysander y Reched (fichas del documento revisado).
- [ ] Completar o verificar la imagen o referencia visual de ambos personajes.
- [ ] Proponer distintas distribuciones para Fuerza, Velocidad, Resistencia, Inteligencia, Energía y Habilidad.
- [ ] Comparar las fortalezas y debilidades de las distribuciones propuestas.
- [x] Elegir una distribución para Lysander y Reched (valores registrados en la tabla de verificación).
- [ ] Justificar explícitamente la estrategia elegida y su relación con las fortalezas y debilidades.
- [x] Comprobar que los seis valores de ambos personajes estén entre 250 y 750 y sumen exactamente 3000.
- [x] Comprobar en ambos personajes que al menos un atributo sea mayor o igual a 650 y otro menor o igual a 350; ninguno tiene seis valores de 500.
- [x] Identificar el atributo principal y el de menor valor de ambos personajes.
- [ ] **En curso:** completar y revisar las dos fichas con alternativas, comparación y justificación; ya contienen datos, valores y total. Verificar las fichas restantes del equipo antes de cerrar la actividad.

### Actividad 2 — Relevamiento de requerimientos

- [x] Escribir el objetivo del juego: alcanzar 15 rondas ganadas.
- [x] Identificar a los dos participantes y sus acciones: elegir atributo cuando corresponde y elegir una carta de su mano.
- [x] Definir el reparto de tres cartas a cada jugador desde un mazo común y la mezcla entre rondas.
- [ ] Definir cómo se forma el mazo inicial y qué ocurre si tiene menos de seis cartas.
- [ ] Definir quién es mano al iniciar la partida.
- [x] Definir quién elige el atributo: la mano en el primer combate y el ganador del combate en el siguiente.
- [x] Especificar la comparación por el atributo elegido y la victoria de ronda al alcanzar dos combates ganados.
- [x] Resolver la regla de empate: gana quien no eligió el atributo.
- [x] Definir que las cartas jugadas vuelven al mazo común al terminar la ronda.
- [ ] Precisar el destino de las cartas no jugadas y el orden de devolución de todas las cartas.
- [x] Definir la alternancia de la mano entre rondas y la elección del atributo por el ganador entre combates.
- [ ] Precisar el orden de elección de atributo y carta y el mecanismo para mostrar ambas cartas simultáneamente.
- [x] Establecer la victoria y finalización normal de la partida al alcanzar 15 rondas ganadas.
- [x] Enumerar los datos de ejecución: mazo, cartas de cada jugador, cartas jugadas, combates ganados y rondas ganadas.
- [ ] **En curso:** completar los datos de intercambio; hay una propuesta de mensajes de acciones y un modelo Accion, pero falta el protocolo y su identidad de partida.
- [ ] **En curso:** completar errores y respuestas; el documento identifica sobrescritura y atributos inválidos, y el UML agrega excepciones de personaje, archivo, acción y mazo.
- [ ] Separar los requerimientos funcionales de las condiciones técnicas de ejecución.
- [ ] Revisar y entregar el documento de requerimientos y reglas.

### Actividad 3 — Modelo de clases

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

- [ ] **En curso:** definir datos y formato del archivo compartido; Accion propone jugador, tipo, ronda, combate y valor, pero faltan sintaxis y acuerdo sobre uno o dos archivos.
- [ ] **En curso:** completar la identificación de partida, jugadores, estado y turno; ControlTurno y EstadoPartida ya modelan estados, actores pendientes y números de ronda y combate, pero falta identidad de partida y su representación en el archivo.
- [ ] **En curso:** especificar el registro de acciones; el UML propone Registrar y cinco tipos de acción, pero no define formato, confirmación ni duplicados.
- [ ] **En curso:** determinar cuándo lee y actualiza cada computadora; hay rutas propia y rival, contador de líneas leídas y LeerAccionesDelRival, sin frecuencia ni secuencia de operaciones acordadas.
- [x] Definir la regla de autorización del turno en el UML: al elegir atributo actúa solo quien elige; al elegir cartas actúan ambos una vez cada uno; las acciones fuera de turno, ronda o combate se rechazan con AccionInvalidaException.
- [ ] Especificar el algoritmo que valida los datos recibidos y conserva el estado ante una acción inválida o repetida.
- [ ] Elegir y documentar el mecanismo para evitar accesos concurrentes inconsistentes.
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
