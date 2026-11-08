# Juego Guerra — Requerimientos y reglas del juego

## Objetivo y participantes

Participan dos jugadores, J1 y J2. Gana la partida quien alcanza primero 15 rondas ganadas. Cada jugador utiliza un cliente para elegir sus cartas y, cuando le corresponde, el atributo del combate. El servidor analiza las elecciones y comunica los resultados.

## Conceptos

| Término | Significado |
|---|---|
| Partida | Juego completo entre J1 y J2, hasta que uno gana 15 rondas. |
| Ronda | Serie de hasta tres combates; la gana quien consigue dos combates ganados. |
| Combate | Enfrentamiento de una carta de cada jugador mediante un atributo elegido. |
| Jugador mano | Quien elige carta y atributo en el primer combate de la ronda. |
| Mano de cartas | Cartas disponibles de un jugador; recibe tres al comenzar cada ronda. |
| Mazo común | Conjunto completo de personajes con el que se realizan los repartos. |

## Requerimientos funcionales

### Personajes y preparación

Carta y personaje son el mismo elemento. Cada personaje tiene nombre, representación ASCII y seis atributos: Fuerza, Velocidad, Resistencia, Inteligencia, Energía y Habilidad. Los atributos deben sumar exactamente 3000 puntos; cada uno debe estar entre 250 y 750, al menos uno debe ser de 650 o más y al menos uno de 350 o menos. No se permiten seis valores de 500.

Un sistema externo carga los personajes en un CSV compartido y garantiza que no haya duplicados. El juego obtiene de ese archivo los personajes del mazo inicial. La carga y administración del catálogo quedan fuera del cliente de los jugadores.

Se reparten tres cartas a cada jugador desde el mazo común. J1 es el jugador mano de la primera ronda. Al terminar cada ronda, ese rol pasa al otro jugador, independientemente del ganador: J1, J2, J1, J2, hasta finalizar la partida.

### Desarrollo de los combates

1. En el primer combate comienza el jugador mano. Mira sus cartas y elige qué carta jugar y qué atributo utilizar.
2. El rival elige una carta de su mano sin que la interfaz le muestre la carta ni el atributo seleccionados por el contrincante.
3. Cuando ambos han elegido, se muestran las dos cartas y el atributo del combate.
4. Gana el combate quien tenga el valor más alto en ese atributo. Si hay empate, gana quien no eligió el atributo. Se suma un combate ganado al vencedor.
5. Si la ronda continúa, quien no eligió el atributo pasa a elegir carta y atributo en el siguiente combate, independientemente del resultado anterior.

Las cartas jugadas no vuelven a utilizarse durante la misma ronda. La elección del atributo se alterna en cada combate y no cambia quién es el jugador mano de esa ronda. Se acepta que un jugador pueda consultar las elecciones manualmente en los archivos compartidos; no se garantiza su secreto frente a ese acceso.

### Finalización de ronda y partida

Gana la ronda quien consigue dos combates ganados y suma una ronda a su marcador. Si gana los dos primeros combates, no se juega el tercero.

Al finalizar la ronda se restaura el mazo completo, incluidas las cartas no jugadas. No se acumulan cartas ganadas ni se pierden cartas entre rondas. Antes de la siguiente ronda se mezcla el mazo, se reparten tres cartas a cada jugador, se alterna el jugador mano y se reinician los contadores de combates ganados. Las rondas ganadas se acumulan durante toda la partida.

Cuando un jugador alcanza 15 rondas ganadas, se comunica el ganador y finaliza la partida sin iniciar otra ronda.

### Información conservada durante la ejecución

- Datos de los personajes y composición del mazo completo.
- Identificación de J1 y J2 y cartas disponibles de cada uno.
- Cartas jugadas durante la ronda.
- Ronda y combate actuales.
- Jugador mano y jugador al que corresponde elegir el atributo.
- Carta elegida por cada jugador, atributo seleccionado y si ambos han completado su elección.
- Combates ganados por cada jugador en la ronda y rondas ganadas en la partida.
- Resultados de combates y rondas y ganador de la partida cuando corresponda.
- Estado de la partida y si ha finalizado normalmente o por un error.

### Comunicación y validación

Cada cliente publica sus elecciones en sus propios archivos. El servidor lee esas elecciones, analiza el combate y escribe mensajes con los resultados. Los clientes leen los mensajes del servidor y muestran las cartas y el atributo cuando ambos jugadores han elegido.

Los clientes son responsables de que los datos enviados al servidor sean válidos. El servidor realiza las comprobaciones al iniciar la partida. El CSV de personajes proporciona los datos iniciales y cumple una función distinta de los archivos que intercambian elecciones y resultados durante el juego.

### Situaciones de error

| Situación | Respuesta |
|---|---|
| Menos de seis cartas válidas para repartir | Mostrar «No se puede jugar porque no hay suficientes cartas» y terminar la ejecución sin iniciar la partida. |
| CSV inexistente, ilegible o con datos inválidos | Terminar la ejecución del juego. |
| Fallo de acceso, lectura o escritura de los archivos de comunicación | Terminar la ejecución del juego. |
| El servidor no puede continuar | Finalizar la partida y la ejecución de los clientes y del servidor. |
| Cualquier otro error al iniciar o durante la partida | Terminar la ejecución del juego completo, sin recuperación ni continuación de la partida. |

## Condiciones técnicas de ejecución

- Aplicación de consola desarrollada en C# y .NET 8.
- Dos clientes, uno por jugador en cada una de las dos computadoras, y un proceso servidor.
- Conexión mediante una red local y acceso a los archivos compartidos necesarios.
- CSV de personajes preparado por el sistema externo de carga.
- Cada proceso escribe exclusivamente sus propios archivos y solo lee los archivos de los demás. Esta separación evita que distintos procesos sobrescriban un mismo archivo.
