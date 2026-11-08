# Reglas del juego Guerra

Este documento reúne las reglas confirmadas por el equipo y las restricciones de personajes establecidas en la [consigna](Consigna.md). Las decisiones todavía pendientes se indican al final y se siguen en el [Plan](Plan.md). Forma parte del análisis de TI; no implica que el juego esté implementado.

## Conceptos

| Término | Significado |
|---|---|
| Partida | Juego completo entre dos jugadores. Finaliza cuando uno alcanza 15 rondas ganadas. |
| Ronda | Serie de hasta tres combates. Finaliza cuando un jugador gana dos combates. Es lo que coloquialmente se llama «mano» en el truco. |
| Combate | Enfrentamiento de una carta de cada jugador, comparando un atributo elegido. |
| Jugador mano | Jugador que elige el atributo del primer combate de la ronda. |
| Mano de cartas | Conjunto de cartas disponibles de un jugador. Al comenzar cada ronda, contiene tres cartas. |
| Mazo común | Conjunto de cartas desde el que se reparte a ambos jugadores. |

## Participantes y objetivo

Participan dos jugadores, identificados como J1 y J2. El objetivo es ser el primero en alcanzar 15 rondas ganadas.

Cada jugador elige la carta de su mano que utilizará en cada combate. Cuando le corresponde, también elige el atributo por el que se comparan las cartas.

## Personajes y atributos

Carta y personaje designan el mismo elemento del juego. En el modelo se utilizará Personaje, sin una clase Carta adicional. Cada personaje tiene seis atributos: Fuerza, Velocidad, Resistencia, Inteligencia, Energía y Habilidad. Todos pueden utilizarse en los combates.

Los valores de cada personaje deben cumplir estas condiciones:

- Sumar exactamente 3000 puntos entre los seis atributos.
- Tener cada atributo entre 250 y 750 puntos, inclusive.
- Tener al menos un atributo de 650 puntos o más.
- Tener al menos un atributo de 350 puntos o menos.
- No asignar 500 puntos a los seis atributos.

## Preparación y jugador mano

Un sistema externo al cliente de los jugadores carga todos los personajes en un archivo CSV compartido. Ese archivo proporciona los personajes con los que se forma el mazo inicial. La carga y edición del catálogo mediante ese sistema externo quedan fuera del alcance del cliente.

El CSV de personajes contiene los datos iniciales; su función es distinta de la del archivo compartido que coordina las acciones y el estado de una partida. No se permiten personajes repetidos en el mazo. La estructura del CSV, el criterio para reconocer un personaje duplicado y la respuesta ante registros inválidos o duplicados todavía deben especificarse.

Si hay menos de seis cartas válidas disponibles, el sistema muestra el error «No se puede jugar porque no hay suficientes cartas» y no permite iniciar la partida.

Se reparten tres cartas a cada jugador desde un mazo común. J1 es el jugador mano de la primera ronda.

Al terminar cada ronda, el rol de jugador mano pasa al otro jugador, independientemente de quién haya ganado: J1 en la primera ronda, J2 en la segunda, J1 en la tercera, y así sucesivamente hasta terminar la partida.

## Combates

1. En el primer combate de la ronda comienza el jugador mano. Mira sus cartas y elige qué carta jugar y qué atributo utilizar.
2. El otro jugador elige una de sus cartas sin conocer la carta ni el atributo elegidos por su contrincante.
3. Cuando ambos han elegido, se revelan las dos cartas y el atributo con el que se enfrentan.
4. Se comparan los valores del atributo elegido en las dos cartas. Gana el combate quien tenga el valor más alto y suma un combate ganado en la ronda.
5. Si los valores son iguales, gana el combate quien no eligió el atributo de ese combate.
6. Si la ronda continúa, quien no eligió el atributo pasa a elegir carta y atributo en el siguiente combate, independientemente de quién haya ganado.

Las cartas utilizadas se registran como cartas jugadas y no se vuelven a utilizar durante esa ronda. La elección del atributo se alterna entre jugadores en cada combate y no cambia quién es el jugador mano de la ronda.

Esta alternancia reemplaza la regla anterior que asignaba la elección del siguiente atributo al ganador del combate. Falta especificar el mecanismo de comunicación que mantenga ocultas las elecciones hasta que ambos jugadores hayan elegido.

## Victoria de ronda y nuevo reparto

Gana la ronda el primer jugador que consigue dos combates ganados. Se le suma una ronda ganada a su marcador de partida.

Cada ronda tiene hasta tres combates. Si un jugador gana los dos primeros, la ronda termina inmediatamente y no se juega el tercero.

Al terminar cada ronda se restaura el mazo completo: vuelven todas las cartas repartidas, tanto las jugadas como las que quedaron sin jugar. No se acumulan cartas ganadas ni se pierden cartas entre rondas. Antes de comenzar la siguiente ronda se mezcla el mazo completo, se reparten nuevamente tres cartas a cada jugador y se alterna el jugador mano. El conteo de combates ganados comienza nuevamente para la nueva ronda; las rondas ganadas se acumulan durante la partida.

## Victoria de partida

La partida termina cuando un jugador alcanza 15 rondas ganadas. Ese jugador es el ganador y no se inicia otra ronda.

## Decisiones pendientes

- Definir qué sucede si el CSV compartido no puede leerse o contiene datos inválidos.
- Especificar cómo se identifica un personaje repetido y qué respuesta se da si el CSV contiene duplicados; la prohibición de repetidos ya está confirmada.
- Definir el mecanismo de comunicación para mantener ocultas las elecciones y revelar las cartas y el atributo cuando ambos hayan elegido.

La comunicación entre computadoras, las respuestas ante errores y la separación entre requisitos funcionales y condiciones técnicas siguen pendientes en la actividad 2 del [Plan](Plan.md). Este reglamento no cierra por sí solo el documento completo de requerimientos.
