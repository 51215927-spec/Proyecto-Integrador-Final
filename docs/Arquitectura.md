# Arquitectura

[UML](diagramas/UML.uxf)

## Estado del diseño

El UML es una propuesta de análisis de TI; todavía no hay implementación de estas clases. La revisión del 8 de noviembre de 2026 y los pendientes se registran en [Plan](Plan.md).

El UML existente separa Personaje y Carta, pero el usuario decidió unificarlos: carta y personaje son el mismo elemento, representado por Personaje. No se incorporará una clase que solo envuelva una referencia al personaje y un string. El UML y sus contratos todavía deben adaptarse a esta decisión; no se ha decidido trasladar el ASCII Art a otra clase.

Mazo administra la colección común de personajes y Jugador conserva su mano y marcador. Partida coordina Ronda y Combate. ControlTurno regula quién puede actuar; IInterfaz separa la interacción de consola y ArchivoPartida representa el intercambio de acciones mediante Accion. Estas responsabilidades requieren completar su justificación y sus contratos.

El mazo inicial proviene de un CSV compartido generado por un sistema externo al cliente de los jugadores. Ese sistema de carga queda fuera del alcance del cliente. El CSV es una fuente de datos iniciales y no sustituye al archivo de intercambio de la partida; faltan definir su formato, lectura y validaciones.

La unificación de Carta y Personaje es una decisión del usuario que difiere de la separación y delegación explícitas de la etapa 3 de Taller en la consigna. Se conserva la consigna sin modificar y se deberá revisar esta diferencia con el docente antes de la entrega; Taller permanece fuera del alcance actual.

Las reglas de empate a favor de quien no eligió, dos combates para ganar una ronda y 15 rondas para ganar una partida coinciden entre el avance revisado y el UML.

Según la decisión confirmada por el usuario, J1 es el jugador mano de la primera ronda y el rol se alterna entre J1 y J2 al finalizar cada ronda, independientemente del ganador. El jugador mano elige el primer atributo; después la elección se alterna entre jugadores en cada combate, independientemente del ganador y sin modificar ese rol durante la ronda. Esta decisión reemplaza la regla anterior basada en el ganador; el UML debe revisarse para reflejarla. Carta y atributo permanecen ocultos al rival hasta que ambos hayan elegido. La terminología y el traslado pendiente al documento de entrega se registran en [Plan](Plan.md#decisión-confirmada--jugador-mano-y-terminología); falta comprobar su coherencia con los contratos del modelo.

## Decisiones por cerrar

- Unificar los métodos del borrador con las firmas y responsabilidades del UML antes de especificarlos o implementarlos.
- Adaptar el UML y sus firmas a Personaje como único concepto de carta y revisar la diferencia con la consigna.
- Ajustar el modelo a la restauración del mazo completo después de cada ronda, incluidas las cartas no jugadas. No se permiten personajes repetidos; con menos de seis cartas válidas se informa el error y no se inicia la partida. Falta especificar la detección y respuesta ante duplicados del CSV.
- El UML propone dos rutas de acciones (propia y rival), mientras la consigna pide un archivo de partida compartida. La cantidad de archivos y el protocolo necesitan resolución y revisión con SSI; no se consideran aprobados.
- Completar identidad de partida, detección de duplicados, sincronización, acceso concurrente y tratamiento de fallos. Las excepciones y los estados enumerados en el UML son un punto de partida, no un protocolo completo.
