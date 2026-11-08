# Plan de trabajo — Juego Guerra

Plan basado en [la consigna](Consigna.md). Marcar una tarea cuando su resultado esté disponible y revisado. Las actividades de TI definen los contratos que se usarán en Taller; las decisiones pendientes deben resolverse antes de implementar el componente correspondiente.

## TI — Análisis de Sistemas

### Actividad 1 — Análisis y diseño de los personajes

- [ ] Escribir el nombre y la descripción de cada personaje e incorporar su imagen o referencia visual.
- [ ] Proponer distintas distribuciones para Fuerza, Velocidad, Resistencia, Inteligencia, Energía y Habilidad.
- [ ] Comparar las fortalezas y debilidades de las distribuciones propuestas.
- [ ] Elegir una distribución y justificar la estrategia del personaje.
- [ ] Comprobar que los seis valores estén entre 250 y 750 y sumen exactamente 3000.
- [ ] Comprobar que al menos un atributo sea mayor o igual a 650 y otro menor o igual a 350; rechazar seis valores de 500.
- [ ] Identificar el atributo principal y el de menor valor.
- [ ] Completar y revisar una ficha de análisis por personaje con los datos anteriores y el total.

### Actividad 2 — Relevamiento de requerimientos

- [ ] Escribir el objetivo del juego y las responsabilidades de los dos participantes.
- [ ] Definir cómo se forma el mazo inicial y cómo se reparten las cartas.
- [ ] Definir quién inicia la partida y quién elige el atributo de cada enfrentamiento.
- [ ] Especificar cómo se comparan los atributos y cómo se determina el ganador de una ronda.
- [ ] Resolver la regla de empate y documentarla.
- [ ] Definir dónde se colocan las cartas utilizadas y las cartas ganadas y en qué orden.
- [ ] Definir cómo se asigna el siguiente turno.
- [ ] Establecer las condiciones de victoria y de finalización de la partida.
- [ ] Enumerar los datos que deben conservarse durante la ejecución y los que deben intercambiar las computadoras.
- [ ] Enumerar los errores que debe contemplar el sistema y la respuesta esperada para cada uno.
- [ ] Separar los requerimientos funcionales de las condiciones técnicas de ejecución.
- [ ] Revisar y entregar el documento de requerimientos y reglas.

### Actividad 3 — Modelo de clases

- [ ] Definir las responsabilidades de Personaje, Carta, Mazo y los objetos que administrarán la partida.
- [ ] Identificar y justificar las clases adicionales necesarias.
- [ ] Definir los atributos de cada clase y cuáles deben quedar encapsulados.
- [ ] Definir los métodos de cada clase con sus parámetros y valores de retorno.
- [ ] Definir las relaciones entre las clases y las operaciones que se delegarán en otros objetos.
- [ ] Elaborar el diagrama de clases UML.
- [ ] Documentar las decisiones de diseño en [Arquitectura](Arquitectura.md) y comprobar su coherencia con el UML.

### Actividad 4 — Especificación de métodos

- [ ] Identificar los métodos relevantes del modelo de clases.
- [ ] Escribir el nombre y la responsabilidad de cada método.
- [ ] Especificar sus parámetros de entrada y su valor de retorno, cuando corresponda.
- [ ] Indicar las precondiciones y el resultado esperado de cada método.
- [ ] Definir sus validaciones y el tratamiento de situaciones excepcionales.
- [ ] Elaborar diagramas de Nassi-Schneiderman para los algoritmos que los necesiten.
- [ ] Revisar con Taller que los contratos permitan implementar los métodos sin inventar reglas.

### Actividad 5 — Formato de los archivos de personajes

- [ ] Definir la estructura del archivo de texto y cómo se reconoce cada dato.
- [ ] Especificar el formato del nombre y de los seis atributos obligatorios.
- [ ] Definir cómo se incorpora o referencia la descripción y el ASCII Art.
- [ ] Documentar las validaciones de rango, suma y especialización de los atributos.
- [ ] Definir la respuesta ante campos faltantes, duplicados o incorrectos.
- [ ] Definir la respuesta ante un archivo inexistente o que no se puede leer.
- [ ] Preparar un archivo válido y ejemplos inválidos para cada error previsto.
- [ ] Revisar y entregar la especificación del formato común.

### Actividad 6 — Archivo de partida compartida

- [ ] Definir los datos y el formato del archivo de texto compartido.
- [ ] Definir cómo se identifican la partida, los jugadores, su estado y el turno vigente.
- [ ] Especificar cómo se registra la acción de cada jugador.
- [ ] Determinar cuándo lee y cuándo actualiza el archivo cada computadora.
- [ ] Definir cómo se detectan y rechazan datos que no corresponden al turno esperado.
- [ ] Elegir y documentar el mecanismo para evitar accesos concurrentes inconsistentes.
- [ ] Definir la respuesta ante archivo inaccesible, lectura fallida o escritura incompleta.
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
