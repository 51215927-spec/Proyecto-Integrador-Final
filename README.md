# Proyecto integrador final

Repositorio de un proyecto académico de juego de cartas para dos jugadores, previsto como aplicación de consola en C# y .NET 8. Reúne los requisitos, el diseño y la base de código para organizar el trabajo del equipo y su futura implementación.

El juego consiste en enfrentar personajes comparando sus atributos. Cada jugador recibe tres cartas por ronda; gana la ronda quien consigue dos combates y la partida quien alcanza 15 rondas ganadas. Las reglas completas están en [Reglas del juego](docs/Reglas-del-juego.md).

## Estado actual

El trabajo actual se limita a **TI: análisis y diseño**. La implementación en Taller de Programación queda para una etapa posterior.

Las actividades 1 a 4 de TI están completadas en su documentación. Quedan pendientes los formatos de personajes, el protocolo de partida y el plan de pruebas (actividades 5 a 7).

La aplicación todavía muestra `Hello, World!` y existe una única prueba vacía, sin validaciones del juego. El juego aún no está implementado. Consultá el [plan de trabajo](docs/Plan.md) para conocer los avances y pendientes.

## Contenido del repositorio

| Ubicación | Contenido |
|---|---|
| [`src/`](src/) | Código de la aplicación de consola. |
| [`test/`](test/) | Proyecto de pruebas automatizadas. |
| [`docs/`](docs/) | Requisitos, reglas, planificación y diseño. |
| [`Mis-Librerias/`](Mis-Librerias/) | Submódulo de bibliotecas auxiliares independientes, con documentación propia. |

## Documentación

- [Consigna](docs/Consigna.md): requisitos y entregables del proyecto.
- [Reglas del juego](docs/Reglas-del-juego.md): funcionamiento esperado del sistema.
- [Plan de trabajo](docs/Plan.md): estado de las actividades y próximos pasos.
- [Arquitectura](docs/Arquitectura.md): decisiones de diseño y especificaciones por clase y método, con características, contratos y justificación.
- [Diagrama UML](docs/diagramas/UML.uxf): modelo de dominio actualizado para abrir con UMLet; integración del protocolo pendiente de la actividad 6.
- [Guía de colaboración](CONTRIBUTING.md): organización del trabajo, ramas, commits y revisión de cambios.

## Cómo usarlo

Para consultar la documentación, empezá por la consigna y las reglas; luego revisá el plan y la arquitectura.

Para compilar y ejecutar la base actual del proyecto, necesitás el **SDK de .NET 8**:

```bash
git clone --recurse-submodules https://github.com/51215927-spec/Proyecto-Integrador-Final.git
cd proyecto-integrador-final
dotnet build Proyecto-Integrador-Final.sln
dotnet run --project src/Proyecto-Integrador-Final
```

Si clonaste sin `--recurse-submodules`, recuperá la versión de las bibliotecas registrada por el proyecto con `git submodule update --init --recursive`. Las bibliotecas todavía no están referenciadas por los proyectos ni incluidas en la solución principal; compilar esta solución no las verifica.

La ejecución muestra únicamente `Hello, World!`. Para ejecutar la prueba inicial vacía:

```bash
dotnet test Proyecto-Integrador-Final.sln
```

Antes de modificar el proyecto, seguí la [guía de colaboración](CONTRIBUTING.md). Los cambios se proponen mediante un Pull Request y requieren revisión de un compañero antes de integrarse a `main`.

## Autores

- [51215927-spec](https://github.com/51215927-spec)
- [50509098-JC](https://github.com/50509098-JC)
- [Chino18cm](https://github.com/Chino18cm)
