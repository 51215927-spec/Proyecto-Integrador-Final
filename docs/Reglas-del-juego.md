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

El CSV de personajes contiene los datos iniciales; su función es distinta de la del archivo compartido que coordina las acciones y el estado de una partida. La estructura del CSV y el tratamiento de datos inválidos o duplicados todavía deben especificarse.

Se reparten tres cartas a cada jugador desde un mazo común. J1 es el jugador mano de la primera ronda.

Al terminar cada ronda, el rol de jugador mano pasa al otro jugador, independientemente de quién haya ganado: J1 en la primera ronda, J2 en la segunda, J1 en la tercera, y así sucesivamente hasta terminar la partida.

## Combates

1. El jugador mano elige el atributo del primer combate de la ronda.
2. Cada jugador selecciona una carta de su mano para enfrentarla con la del rival. Ambas cartas se muestran al mismo tiempo.
3. Se comparan los valores del atributo elegido en las dos cartas. Gana el combate quien tenga el valor más alto y suma un combate ganado en la ronda.
4. Si los valores son iguales, gana el combate quien no eligió el atributo de ese combate.
5. Si la ronda continúa, el ganador del combate elige el atributo del siguiente.

Las cartas utilizadas se registran como cartas jugadas y no se vuelven a utilizar durante esa ronda. Elegir el atributo después de ganar un combate no cambia quién es el jugador mano de la ronda.

El orden exacto entre la selección de carta del jugador que elige el atributo y el anuncio de ese atributo todavía debe precisarse. También falta definir cómo se garantiza la revelación simultánea entre las dos computadoras.

## Victoria de ronda y nuevo reparto

Gana la ronda el primer jugador que consigue dos combates ganados. Se le suma una ronda ganada a su marcador de partida.

Cada ronda tiene hasta tres combates. Si un jugador gana los dos primeros, la ronda termina inmediatamente y no se juega el tercero.

Al terminar la ronda, las cartas jugadas vuelven al mazo común. Antes de comenzar la siguiente ronda se mezcla el mazo, se reparten nuevamente tres cartas a cada jugador y se alterna el jugador mano. El conteo de combates ganados comienza nuevamente para la nueva ronda; las rondas ganadas se acumulan durante la partida.

Todavía debe definirse el destino de las cartas no jugadas cuando la ronda termina después de dos combates y el orden de devolución de las cartas.

## Victoria de partida

La partida termina cuando un jugador alcanza 15 rondas ganadas. Ese jugador es el ganador y no se inicia otra ronda.

## Decisiones pendientes

- Definir qué sucede si el CSV compartido no puede leerse o contiene menos de seis cartas válidas disponibles para repartir, y cómo se tratan los personajes duplicados.
- Precisar el destino de las cartas no jugadas al finalizar anticipadamente una ronda y el orden de devolución de todas las cartas.
- Precisar el orden de elección de atributo y carta, y el mecanismo que permite mostrar ambas cartas simultáneamente.

La comunicación entre computadoras, las respuestas ante errores y la separación entre requisitos funcionales y condiciones técnicas siguen pendientes en la actividad 2 del [Plan](Plan.md). Este reglamento no cierra por sí solo el documento completo de requerimientos.
