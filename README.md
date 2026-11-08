# Guerra — Proyecto integrador final

**Personajes con fortalezas y debilidades. Tres cartas para decidir. Un atributo para enfrentarse.**

Guerra es un proyecto académico de juego de cartas para dos jugadores, pensado para ejecutarse en consola con C# y .NET 8. Cada carta representa un personaje y sus seis atributos; la estrategia consiste en elegir qué carta jugar y cuándo aprovechar sus mejores características.

La propuesta conecta el análisis de sistemas con la programación, el diseño de personajes y la comunicación en red local: pasar de una idea de juego a una solución que el equipo pueda explicar, construir y verificar.

## La idea del juego

Los personajes tienen Fuerza, Velocidad, Resistencia, Inteligencia, Energía y Habilidad. Todos cuentan con **3000 puntos**, pero deben distribuirlos con fortalezas y debilidades: cada atributo vale entre 250 y 750, al menos uno alcanza 650 y al menos uno queda en 350 o menos.

Así, la elección no depende de tener una carta superior en todo, sino de decidir qué personaje conviene enfrentar con el atributo en juego.

### Una partida en pocas líneas

1. Cada jugador recibe tres cartas de un mazo común.
2. Quien elige el atributo selecciona también su carta; el rival elige la suya sin conocer esas elecciones. Luego se revelan ambas cartas y el atributo.
3. Gana el combate el valor más alto. En caso de empate, gana quien no eligió el atributo.
4. Quien gana dos combates gana la ronda. El mazo completo se reúne, se mezcla y se vuelve a repartir.
5. Gana la partida quien alcanza **15 rondas ganadas**.

J1 elige el primer atributo de la primera ronda. La elección del atributo se alterna en cada combate y el jugador que comienza se alterna entre rondas, independientemente de los resultados.

Las [reglas del juego](docs/Reglas-del-juego.md) desarrollan estos conceptos y los requisitos completos.

## Estado actual

**Etapa actual: TI — análisis y diseño.** La implementación en Taller de Programación queda como trabajo futuro.

Las actividades de personajes y requerimientos tienen sus entregables documentados. El modelo de clases está en revisión; faltan completar contratos de métodos, formatos de archivos, protocolo de comunicación y casos de prueba. El detalle de avances y pendientes está en el [plan de trabajo](docs/Plan.md).

La aplicación todavía contiene el ejemplo inicial `Hello, World!` y las pruebas son iniciales: **el juego aún no se puede jugar**. Las funcionalidades descritas en este README corresponden al sistema previsto.

## Qué buscamos aprender

- Convertir una consigna en reglas y requisitos precisos.
- Diseñar clases con responsabilidades claras y contratos verificables.
- Fundamentar el equilibrio y la estrategia de los personajes.
- Diseñar el intercambio de acciones y resultados mediante archivos compartidos en red local.
- Colaborar con ramas, Pull Requests y revisión entre compañeros.

La solución prevista utiliza dos clientes y un servidor. Cada proceso escribe sus propios archivos y lee los que le corresponden de los demás. El catálogo inicial de personajes proviene de un CSV generado por un sistema externo; su función es distinta de la de los archivos de comunicación de la partida.

## Autores

<!-- Completar los nombres y reemplazar los textos de perfil por enlaces de GitHub. Agregar filas según la cantidad de integrantes. -->

| Integrante | Perfil de GitHub |
|---|---|
| [Nombre y apellido] | [Perfil de GitHub] |
| [Nombre y apellido] | [Perfil de GitHub] |

## Explorar el repositorio

| Ubicación | Contenido |
|---|---|
| [`src/`](src/) | Proyecto de la aplicación de consola. |
| [`test/`](test/) | Proyecto de pruebas automatizadas, todavía iniciales. |
| [`docs/`](docs/) | Consigna, reglas, planificación y diseño. |
| [`Mis-Librerias/`](Mis-Librerias/) | Submódulo de bibliotecas auxiliares independientes, con documentación propia. |

### Documentación

| Documento | Para qué consultarlo |
|---|---|
| [Consigna](docs/Consigna.md) | Requisitos y entregables de las materias. |
| [Reglas del juego](docs/Reglas-del-juego.md) | Funcionamiento esperado y requerimientos del sistema. |
| [Plan de trabajo](docs/Plan.md) | Avances, pendientes y orden de las actividades. |
| [Arquitectura](docs/Arquitectura.md) | Responsabilidades, decisiones de diseño y revisiones pendientes. |
| [Diagrama UML](docs/diagramas/UML.uxf) | Modelo de clases en formato UMLet, pendiente de ajustes. |
| [Guía de colaboración](CONTRIBUTING.md) | Organización del repositorio y flujo de Git y GitHub. |

## Preparar el entorno

Para compilar el proyecto se necesita el **SDK de .NET 8**.

```bash
git clone --recurse-submodules https://github.com/51215927-spec/Proyecto-Integrador-Final.git
cd Proyecto-Integrador-Final
dotnet build Proyecto-Integrador-Final.sln
dotnet test Proyecto-Integrador-Final.sln
```

Estos comandos comprueban la base actual de la solución; todavía no validan las reglas del juego. Los requisitos de red y archivos compartidos corresponden a la futura ejecución del juego.

## Colaborar

Antes de comenzar, consultá la [guía de colaboración](CONTRIBUTING.md): actualizá `main`, creá una rama para la tarea y proponé los cambios mediante un Pull Request. La integración requiere la revisión de un compañero.

Para continuar el trabajo de TI, el próximo paso es revisar el modelo de clases y su coherencia con las reglas acordadas, siguiendo el [plan](docs/Plan.md).
