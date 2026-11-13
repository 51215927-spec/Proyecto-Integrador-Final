# Proyecto integrador final

Repositorio de un proyecto académico de juego de cartas para dos jugadores, previsto como aplicación de consola en C# y .NET 8. Reúne los requisitos, el diseño y la base de código para organizar el trabajo del equipo y su futura implementación.

El juego consiste en enfrentar personajes comparando sus atributos. Cada jugador recibe tres cartas por ronda; gana la ronda quien consigue dos combates y la partida quien alcanza 15 rondas ganadas. Las reglas completas están en [Reglas del juego](docs/Reglas-del-juego.md).

## Estado actual

El trabajo actual se limita a **TI: análisis y diseño**. La implementación en Taller de Programación queda para una etapa posterior.

Las actividades 1 a 5 de TI están completadas en su documentación por confirmación del usuario. Quedan pendientes el protocolo de partida y el plan de pruebas (actividades 6 y 7).

La aplicación todavía muestra `Hello, World!`. Existen implementaciones parciales de Personaje y Mazo y una única prueba vacía, sin comprobaciones de comportamiento. El juego aún no está integrado. El plan registra también los diseños de comunicación disponibles en ramas pendientes de revisión e integración. Consultá el [plan de trabajo](docs/Plan.md) para conocer los avances y pendientes.

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
- [Tecnologías REST](docs/Tecnologias-rest.md): selección acordada para la alternativa web, alcance de persistencia y decisiones pendientes.
- [Diagrama UML](docs/diagramas/UML.uxf): modelo de dominio actualizado para abrir con UMLet; integración del protocolo pendiente de la actividad 6.
- [Guía de colaboración](CONTRIBUTING.md): organización del trabajo, ramas, commits, revisión de cambios y [metodología TDD](CONTRIBUTING.md#desarrollo-guiado-por-pruebas-tdd).

## Cómo usarlo

Para consultar la documentación, empezá por la consigna y las reglas; luego revisá el plan y la arquitectura.

Para compilar y ejecutar la base actual del proyecto, necesitás el **SDK de .NET 8**:

```bash
git clone --recurse-submodules https://github.com/51215927-spec/Proyecto-Integrador-Final.git
cd proyecto-integrador-final
dotnet build Proyecto-Integrador-Final.sln
dotnet run --project src/Proyecto-Integrador-Final
```

Si clonaste sin `--recurse-submodules`, recuperá la versión de las bibliotecas registrada por el proyecto con `git submodule update --init --recursive`. DataBases y Utilidades están referenciadas por la aplicación e incluidas en la solución principal; la carga de personajes y la comunicación todavía no están conectadas al programa.

La ejecución muestra únicamente `Hello, World!`. Para ejecutar la prueba inicial vacía:

```bash
dotnet test Proyecto-Integrador-Final.sln
```

Antes de modificar el proyecto, seguí la [guía de colaboración](CONTRIBUTING.md). Los cambios se proponen mediante un Pull Request y requieren revisión de un compañero antes de integrarse a `main`.

## Autores

- [51215927-spec](https://github.com/51215927-spec)
- [50509098-JC](https://github.com/50509098-JC)
- [Chino18cm](https://github.com/Chino18cm)
