# Plan de trabajo — Juego Guerra

Este plan organiza los entregables de [la consigna](Consigna.md). Las tareas se consideran pendientes hasta que exista el documento, código o prueba correspondiente y el equipo lo haya revisado. No fija fechas ni responsables individuales: deben acordarse entre los integrantes.

## 1. Análisis y diseño (TI)

| Orden | Trabajo | Entregable y criterio de cierre |
|---|---|---|
| 1 | Definir los personajes. Proponer y comparar distribuciones, elegir una estrategia y justificarla con la descripción visual como contexto. | Una ficha por personaje con nombre, descripción, seis atributos, total, atributo principal, atributo menor e imagen o referencia. Cada valor está entre 250 y 750; la suma es 3000; alguno es ≥ 650 y alguno ≤ 350. |
| 2 | Relevar y acordar las reglas. Precisar objetivo, participantes, cartas y mazos, desarrollo de la ronda, elección del atributo, empates, turnos y condiciones de victoria o finalización. Separar requisitos funcionales de condiciones técnicas y registrar errores esperados. | Documento de requerimientos y reglas sin decisiones de juego ambiguas para quien implemente. |
| 3 | Diseñar el modelo de objetos. Definir responsabilidades, datos encapsulados, relaciones, métodos, parámetros, retornos y delegaciones de Personaje, Carta, Mazo, Partida y las clases adicionales necesarias. | Diagrama UML y justificación de las decisiones principales; actualizar [Arquitectura](Arquitectura.md). |
| 4 | Especificar los métodos relevantes. Detallar entradas, salidas, precondiciones, resultados, validaciones y errores; usar diagramas de Nassi-Shneiderman cuando aclaren un algoritmo. | Contratos que permitan implementar las reglas sin inventarlas. |
| 5 | Definir el formato común de los archivos de personajes. Precisar cómo reconocer nombre, seis atributos, descripción y ASCII Art o su referencia; resolver datos faltantes, duplicados, inválidos y errores de lectura. | Especificación del formato y ejemplos válidos e inválidos. |
| 6 | Diseñar el archivo de partida compartida para dos computadoras en red local. Definir estructura, identificadores, estado, turno, acciones, momentos de lectura y escritura, errores de acceso y control de concurrencia. | Especificación del protocolo de intercambio y de la recuperación ante fallos, diferenciada de los archivos individuales de personajes. |
| 7 | Diseñar los casos de prueba a partir de los requisitos y contratos. | Plan de pruebas con condiciones iniciales, datos y resultado esperado para casos normales, límites y errores; aplicar caja negra y blanca según corresponda. |

Antes de implementar cada componente, los Analistas deben resolver con el equipo de Taller las ambigüedades o dificultades técnicas detectadas y actualizar la especificación. En particular, la consigna no determina por sí sola cómo se resuelven los empates, el orden de las cartas ganadas ni el mecanismo concreto de acceso concurrente: esas decisiones deben quedar documentadas antes de programarlas.

## 2. Implementación (Taller de Programación)

| Orden | Trabajo | Criterio de cierre |
|---|---|---|
| 1 | Implementar `Personaje` con nombre, seis atributos y ASCII Art, según el modelo acordado. | Los datos quedan encapsulados y se verifican las restricciones de puntajes y las consultas previstas. |
| 2 | Leer los archivos de personajes y construir objetos válidos. | Se procesan ejemplos válidos y se informan archivos inexistentes, incompletos o inválidos según la especificación. |
| 3 | Implementar `Carta` mediante su relación con `Personaje`. | La carta consulta los datos necesarios sin duplicar los atributos del personaje. |
| 4 | Implementar `Mazo`: agregar, mezclar, repartir, extraer, retirar, incorporar cartas ganadas y consultar cantidad. | Las operaciones y sus límites, incluido el mazo vacío, cumplen los contratos. |
| 5 | Implementar las reglas de ronda y partida. | Se elige un atributo, se comparan cartas, se resuelven empates, se asignan cartas y turnos y se detecta el fin de la partida según las reglas acordadas. |
| 6 | Implementar el intercambio mediante el archivo compartido. | Dos procesos consultan y actualizan el estado sin aceptar acciones de un turno incorrecto; se manejan errores de acceso y escritura y los conflictos previstos. |
| 7 | Integrar la aplicación de consola. | Ambas computadoras pueden iniciar y completar una partida con personajes cargados desde archivos. |

Guardar la aplicación en `src/`, las pruebas en `test/` y las especificaciones y evidencias de diseño en `docs/`. Mantener sincronizados el UML, los contratos, las reglas y el código cuando cambie una decisión.

## 3. Verificación y entrega

- Cubrir con pruebas los casos definidos en TI: personaje válido, suma distinta de 3000, atributos fuera de rango o sin especialización, archivo incompleto, mazo vacío, empate, jugador sin cartas, archivo compartido inaccesible y turno inesperado.
- Ejecutar `dotnet build Proyecto-Integrador-Final.sln` y, cuando existan pruebas aplicables, `dotnet test Proyecto-Integrador-Final.sln`. Registrar los resultados y corregir los fallos antes de integrar cambios.
- Realizar una prueba de punta a punta con dos computadoras en la red local y el archivo compartido configurado; verificar inicio, rondas, finalización y al menos una interrupción de acceso.
- Revisar que la entrega final incluya todas las fichas, requerimientos y reglas, UML, contratos y diagramas de algoritmos, formatos de personajes, protocolo del archivo compartido, plan de pruebas y aplicación integrada.
- Preparar la defensa: cada integrante debe poder explicar la estrategia de su personaje, las clases y métodos en los que trabajó y la relación entre diseño, implementación y pruebas.

Los cambios para GitHub se proponen en ramas y Pull Requests conforme a [CONTRIBUTING](../CONTRIBUTING.md); la integración requiere revisión de un compañero.
