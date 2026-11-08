# Proyecto integrador: Juego Guerra

## TI — Análisis de Sistemas

### 1. Propósito

En esta materia trabajarán en el análisis y diseño del sistema que permitirá implementar el juego Juego Guerra.

El objetivo es transformar una descripción del problema en una solución documentada, definiendo las reglas del juego, la información que deberá administrar el programa, las responsabilidades de sus componentes y las condiciones necesarias para verificar su correcto funcionamiento.

Algunos estudiantes asumirán el rol de Analistas y tendrán la responsabilidad adicional de coordinar el análisis, elaborar las especificaciones y acompañar a sus compañeros durante el desarrollo en Taller de Programación.

### 2. Actividad 1 — Análisis y diseño de los personajes

Cada alumno deberá analizar las características de su personaje y definir la distribución de puntos que utilizará durante los enfrentamientos.

Todos los personajes competirán bajo las mismas reglas. Por eso, deberán respetarse los siguientes requisitos:

- Cada personaje tendrá los siguientes seis atributos:

| Atributo | Descripción |
|---|---|
| Fuerza | Representa la potencia física o capacidad de ataque del personaje. |
| Velocidad | Representa la rapidez de movimiento y reacción. |
| Resistencia | Representa la capacidad para soportar ataques o situaciones adversas. |
| Inteligencia | Representa la capacidad de análisis y estrategia. |
| Energía | Representa el poder o capacidad energética del personaje. |
| Habilidad | Representa la destreza, precisión y dominio de sus capacidades. |

- Todos estos atributos serán utilizados posteriormente durante los combates.
- La suma de los seis atributos deberá ser exactamente de 3.000 puntos. Cada atributo tendrá un valor mínimo de 250 y un máximo de 750 puntos.

Además, se deberán cumplir las siguientes condiciones:

- Al menos un atributo deberá tener 650 puntos o más.
- Al menos un atributo deberá tener 350 puntos o menos.
- No se permitirá asignar 500 puntos a los seis atributos.

El objetivo no es distribuir los puntos al azar, sino tomar una decisión de diseño fundamentada. Aumentar el valor de un atributo implica disponer de menos puntos para los restantes. Cada alumno deberá analizar cómo esa decisión influye en las fortalezas, las debilidades y la estrategia de su personaje.

Para ello deberán:

- Comprender qué representa cada atributo.
- Proponer distintas distribuciones posibles.
- Comparar las alternativas y evaluar sus ventajas y desventajas.
- Seleccionar una distribución y justificar la decisión.
- Verificar que la distribución cumpla todas las restricciones.
- Identificar el atributo principal y el atributo de menor valor.

La imagen y la descripción visual del personaje se trabajarán en Arte. En TI deberán considerar esas características como contexto para fundamentar las decisiones sobre los atributos, sin que la apariencia determine automáticamente los puntajes.

Entregable: ficha de análisis del personaje con los seis valores, el total, el atributo principal, el atributo de menor valor y una justificación de la estrategia elegida.

### 3. Actividad 2 — Relevamiento de requerimientos

Analicen el funcionamiento esperado del juego e identifiquen las necesidades que deberá satisfacer la aplicación.

Deberán documentar:

- El objetivo del juego.
- Los participantes y sus responsabilidades.
- Las reglas de los enfrentamientos.
- La administración de cartas y mazos.
- Las condiciones para ganar una ronda y una partida.
- La información que debe conservarse durante la ejecución.
- Las necesidades de comunicación entre las dos computadoras.
- Las situaciones de error que el sistema debe contemplar.

Distingan los requerimientos funcionales de las condiciones técnicas necesarias para que el sistema pueda ejecutarse.

Entregable: documento de requerimientos y reglas del juego.

### 4. Actividad 3 — Modelo de clases

A partir de los requerimientos, identifiquen las clases necesarias para representar el problema.

Para cada clase deberán determinar:

- Su responsabilidad dentro del sistema.
- Los atributos que necesita.
- Los métodos que debe ofrecer.
- Los parámetros y valores de retorno de cada método.
- Las relaciones con las demás clases.
- La información que debe mantener encapsulada.
- Las operaciones que deben delegarse en otros objetos.

El modelo deberá contemplar, como mínimo, la representación de los personajes, las cartas, los mazos y los elementos necesarios para administrar una partida. La identificación y justificación de las clases adicionales será responsabilidad del equipo de análisis.

Elaboren un diagrama de clases UML que represente la solución propuesta.

Entregable: diagrama de clases UML y justificación de las principales decisiones de diseño.

### 5. Actividad 4 — Especificación de métodos

Para cada método relevante, documenten su comportamiento esperado.

La especificación deberá indicar:

- Nombre y responsabilidad.
- Parámetros de entrada.
- Valor de retorno, si corresponde.
- Precondiciones.
- Resultado esperado.
- Validaciones y situaciones excepcionales.

Utilicen los diagramas de NassI-Schneiderman cuando corresponda representar la lógica de los algoritmos.

Las especificaciones deben ser suficientemente precisas para que un compañero pueda implementar el método sin tener que inventar las reglas del problema.

Entregable: especificaciones de métodos y diagramas correspondientes.

### 6. Actividad 5 — Formato de los archivos de personajes

Cada alumno proporcionará un archivo de texto con los datos de su personaje. El sistema deberá poder interpretar esos datos y utilizarlos para construir un objeto Personaje.

El equipo de análisis deberá definir:

- La estructura del archivo.
- Los datos obligatorios y su formato.
- La representación del nombre y de los seis atributos.
- La forma de incorporar o referenciar la descripción y el ASCII Art, según las decisiones del diseño.
- Las reglas para validar los valores de los atributos.
- El tratamiento de datos faltantes, duplicados o incorrectos.
- El comportamiento esperado cuando no se puede leer un archivo.

El formato deberá ser común para todos los personajes, de modo que el programa pueda procesarlos de manera uniforme.

No alcanza con definir qué datos se necesitan: deben especificar cómo se reconocerá cada dato dentro del archivo.

Entregable: especificación del formato del archivo y ejemplos válidos e inválidos.

### 7. Actividad 6 — Archivo de partida compartida

El juego deberá permitir que dos computadoras participen de una misma partida utilizando un archivo compartido en la red local.

Analicen qué información necesita conocer cada jugador y qué información debe intercambiarse durante la partida.

Definan:

- Qué datos deben almacenarse.
- Cuándo se actualiza el archivo.
- Cuándo debe consultar su contenido cada computadora.
- Cómo se identifica el estado de la partida y el turno correspondiente.
- Cómo se registra la acción de cada jugador.
- Qué ocurre cuando el archivo no está disponible.
- Cómo se evitan inconsistencias cuando dos procesos intentan acceder al recurso.

La solución deberá ser coherente con las posibilidades técnicas trabajadas en SSI y con las operaciones que se implementarán en Taller de Programación.

Tengan presente que el archivo compartido durante la partida cumple una función diferente de los archivos individuales que contienen los datos iniciales de los personajes.

Entregable: especificación del contenido y del funcionamiento del archivo compartido.

### 8. Actividad 7 — Casos de prueba

Diseñen pruebas que permitan verificar que el sistema cumple con los requerimientos.

Incluyan casos normales, situaciones límite y errores posibles, por ejemplo:

- Personaje con todos sus datos válidos.
- Distribución de puntos cuyo total no sea 3.000.
- Atributo fuera del rango permitido.
- Distribución que no cumpla las condiciones de especialización.
- Archivo de personaje incompleto.
- Mazo sin cartas disponibles.
- Empate en un enfrentamiento.
- Jugador que se queda sin cartas.
- Archivo compartido inaccesible.
- Información de una partida que no corresponde al turno esperado.

Para cada caso indiquen las condiciones iniciales, los datos de prueba y el resultado esperado.

Utilicen análisis de caja negra y caja blanca según corresponda.

Entregable: plan de pruebas con resultados esperados.

### 9. Responsabilidades de los Analistas

Los estudiantes designados como Analistas deberán coordinar la elaboración de los entregables, mantener actualizada la documentación y comunicar las decisiones al equipo de Taller de Programación.

También deberán analizar las observaciones de los programadores. Si una especificación es ambigua, incompleta o técnicamente inviable, deberán revisar el diseño y documentar los cambios.

La función de Analista no reemplaza la participación individual en las actividades de aprendizaje y programación. Todos los estudiantes deberán comprender el funcionamiento del sistema y poder explicar las decisiones tomadas.

### 10. Entrega final de TI

La entrega incluirá:

- Las fichas de análisis de los personajes.
- El documento de requerimientos y reglas del juego.
- El diagrama de clases UML.
- Las especificaciones de métodos y los diagramas de algoritmos correspondientes.
- La definición del formato de los archivos de personajes.
- El diseño del archivo de partida compartida.
- El plan de pruebas.

Durante la defensa deberán justificar las decisiones de análisis y diseño, explicar las estrategias de asignación de puntos y demostrar cómo la solución propuesta responde a los requerimientos del juego.

#### Modelo de Ficha de análisis:

Al finalizar esta etapa deberán entregar una ficha con la siguiente información:

Nombre del personaje:

Descripción:

Fuerza: __________

Velocidad: __________

Resistencia: __________

Inteligencia: __________

Energía: __________

Habilidad: __________

Total: __________ / 3.000

Atributo principal: __________________

Atributo más bajo: __________________

Imagen del personaje:

---

# Proyecto integrador: Juego Guerra

## Taller de Programación — C#

### 1. Propósito

En Taller deberán implementar en C# el sistema diseñado en TI.

El trabajo consiste en transformar las especificaciones en clases, métodos y funcionalidades que puedan probarse e integrarse hasta obtener una aplicación funcional entre dos computadoras conectadas a una red local.

La implementación deberá respetar las responsabilidades definidas para cada clase y los contratos establecidos por los Analistas.

### 2. Etapa 1 — Implementación de Personaje

A partir del diseño elaborado en TI, implementen la clase que represente a un personaje.

La clase deberá almacenar la información definida en el análisis, incluyendo el nombre, los seis atributos numéricos y la representación ASCII Art.

Deberán implementar el constructor y las operaciones de consulta que se hayan especificado.

Verifiquen que los objetos puedan representar correctamente los personajes creados por los alumnos y que la información se mantenga encapsulada.

Entregable: clase Personaje implementada y pruebas de funcionamiento.

### 3. Etapa 2 — Lectura de archivos de personajes

Implementen el mecanismo para leer los archivos de texto entregados por los alumnos.

El programa deberá interpretar la estructura acordada en TI, validar los datos y construir los objetos correspondientes.

Contemplen los errores previstos en el análisis, como archivos inexistentes, información incompleta o valores inválidos.

La lectura del archivo deberá estar organizada de acuerdo con las responsabilidades definidas en el diseño. Eviten concentrar toda la lógica de lectura, validación y creación de objetos en un único método sin una justificación adecuada.

Entregable: funcionalidad de carga de personajes y pruebas con archivos válidos e inválidos.

### 4. Etapa 3 — Implementación de Carta

Implementen la clase que represente una carta del juego.

La carta deberá utilizar un objeto Personaje para obtener la información que necesita mostrar o utilizar durante un enfrentamiento.

Apliquen composición y delegación cuando corresponda al diseño. Eviten duplicar en Carta los datos que ya pertenecen a Personaje.

Entregable: clase Carta y pruebas de su relación con Personaje.

### 5. Etapa 4 — Administración del mazo

Implementen la clase responsable de administrar las cartas de un mazo.

De acuerdo con las especificaciones de TI, deberá permitir realizar las operaciones necesarias para:

- Incorporar cartas.
- Mezclar las cartas.
- Repartirlas entre los jugadores.
- Obtener la carta que participa de un enfrentamiento.
- Retirar cartas utilizadas.
- Incorporar las cartas ganadas.
- Consultar la cantidad de cartas disponibles.

Las operaciones deberán contemplar las situaciones límite previstas en el análisis.

Entregable: clase de administración del mazo y pruebas de sus operaciones.

### 6. Etapa 5 — Implementación de las reglas

Implementen las funcionalidades necesarias para desarrollar una partida.

El sistema deberá permitir seleccionar el atributo con el que se enfrentan las cartas, comparar los valores, determinar al ganador de la ronda, administrar las cartas involucradas y establecer quién tiene el siguiente turno.

La aplicación deberá reconocer las condiciones de finalización y comunicar el resultado de la partida.

Entregable: funcionalidades de juego integradas y pruebas de los distintos escenarios.

### 7. Etapa 6 — Comunicación entre computadoras

Implementen el mecanismo de intercambio de información definido en TI, utilizando el archivo de texto compartido configurado en SSI.

El programa deberá poder consultar y actualizar el estado de la partida de acuerdo con las reglas de intercambio acordadas.

Contemplen los errores de acceso y las situaciones de concurrencia previstas en el diseño. La implementación no deberá asumir que el archivo siempre está disponible o que una operación de escritura necesariamente termina correctamente.

Entregable: comunicación implementada y pruebas realizadas con dos computadoras.

### 8. Etapa 7 — Integración

Integren las clases y funcionalidades desarrolladas por los distintos integrantes del equipo.

Verifiquen que los componentes respeten las interfaces y responsabilidades definidas. Si una especificación presenta un problema, comuníquenlo a los Analistas antes de modificar unilateralmente el diseño.

La aplicación deberá poder iniciarse en ambas computadoras y completar una partida utilizando los personajes entregados por los alumnos.

Entregable: aplicación integrada y código fuente organizado.

### 9. Evaluación

Se evaluará la correcta implementación de las especificaciones, el uso de programación orientada a objetos, el encapsulamiento, la composición, la delegación, la modularización, la validación de datos, la calidad de las pruebas y la integración del sistema.

Durante la defensa, cada estudiante deberá explicar las clases y los métodos en los que trabajó, justificar las decisiones de implementación y demostrar que comprende cómo se relacionan los componentes.
