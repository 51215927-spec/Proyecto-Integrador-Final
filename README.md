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
- [Diagrama UML](docs/diagramas/UML.uxf): modelo de dominio actualizado para abrir con UMLet; integración del protocolo pendiente de la actividad 6.
- [Guía de colaboración](CONTRIBUTING.md): organización del trabajo, ramas, commits, revisión de cambios y [metodología TDD](CONTRIBUTING.md#desarrollo-guiado-por-pruebas-tdd).

## Instalación de herramientas y dependencias

Esta guía prepara una computadora para compilar y ejecutar la base actual. Está orientada a Windows, el entorno previsto para jugar. Para consultar solamente la documentación no hace falta instalar estas herramientas.

### 1. Instalar Git

Descargá [Git para Windows](https://git-scm.com/downloads/win) y ejecutá el instalador. Git permite descargar el repositorio y sus bibliotecas auxiliares. Después abrí una terminal nueva (PowerShell o Git Bash) y comprobá:

```bash
git --version
```

El comando debe mostrar una versión de Git. Si no se reconoce, cerrá y volvé a abrir la terminal después de instalarlo.

### 2. Instalar el SDK de .NET 8

Entrá a la [descarga oficial de .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0). En la sección **SDK**, elegí el instalador para Windows y la arquitectura de tu equipo: normalmente x64; usá Arm64 si tu equipo utiliza esa arquitectura. Ejecutá el instalador y abrí una terminal nueva.

El **SDK** contiene las herramientas para compilar el código y los componentes para ejecutarlo, incluido ASP.NET Core. Para trabajar con este repositorio elegí el SDK, no solamente la descarga llamada Runtime. El proyecto mantiene `net8.0` por decisión del equipo. Referencia consultada: SDK 8.0.428, que incluye los runtimes 8.0.34; el número del SDK es distinto del número del runtime.

Comprobá la instalación:

```bash
dotnet --list-sdks
dotnet --list-runtimes
```

La primera lista debe incluir un SDK `8.0.xxx`. La segunda debe incluir `Microsoft.NETCore.App 8.0.x` y `Microsoft.AspNetCore.App 8.0.x`. Podés tener otras versiones instaladas al mismo tiempo. Si `dotnet` no se reconoce, abrí otra terminal; si sigue sin aparecer, revisá la instalación según la [guía oficial para Windows](https://learn.microsoft.com/en-us/dotnet/core/install/windows).

Para Linux o macOS, seleccioná el instalador o las instrucciones correspondientes en la misma página de descarga de .NET 8.

### 3. Descargar el proyecto con sus bibliotecas

En la terminal, ubicáte en la carpeta donde querés guardar el proyecto y ejecutá:

```bash
git clone --recurse-submodules https://github.com/51215927-spec/proyecto-integrador-final.git
cd proyecto-integrador-final
```

Si ya habías descargado el repositorio, entrá a su carpeta y ejecutá solamente:

```bash
git submodule update --init --recursive
```

Esto recupera DataBases y Utilidades en `Mis-Librerias/`, usando la versión registrada por el proyecto. Son bibliotecas del repositorio; no se instalan mediante NuGet.

### 4. Restaurar los paquetes

Desde la carpeta que contiene `Proyecto-Integrador-Final.sln`, ejecutá:

```bash
dotnet restore Proyecto-Integrador-Final.sln
```

Este comando descarga las versiones de los paquetes declaradas en los archivos de proyecto, incluyendo xUnit y las herramientas de pruebas. Necesitás conexión a Internet si todavía no están descargados. No hace falta instalar esos paquetes uno por uno. Consultá la [documentación de dotnet restore](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-restore) si falla la descarga.

### 5. Comprobar que la base funciona

Ejecutá estos comandos uno después del otro:

```bash
dotnet build Proyecto-Integrador-Final.sln
dotnet test Proyecto-Integrador-Final.sln
dotnet run --project src/Proyecto-Integrador-Final
```

La compilación debe terminar sin errores. Actualmente existe una única prueba vacía y el programa muestra `Hello, World!`: estos resultados comprueban la preparación básica, no el funcionamiento del juego.

### Dependencias previstas para REST

El diseño REST acordado utiliza ASP.NET Core, HttpClient, System.Text.Json, SQLite y Entity Framework Core. HttpClient y System.Text.Json forman parte de .NET; ASP.NET Core se incluye con el SDK. EF Core y su proveedor SQLite se incorporarán como paquetes del futuro proyecto servidor, con versiones estables compatibles de la línea 8.0 (referencia acordada: 8.0.34).

Todavía no existe ese servidor ni están declarados esos paquetes en la solución. Por eso, la restauración actual no instala EF Core ni prepara una base de datos. Cuando se autorice Taller y se incorporen sus referencias, se actualizará esta guía con los pasos para crear la base y ejecutar el servidor. SQLite se utilizará dentro del servidor; no se prevé instalar un servicio de base de datos independiente. Las herramientas de migraciones de EF Core también quedan para esa etapa.

En el diseño acordado, primero se abre el servidor y, cuando está disponible, se abren los clientes. La implementación y los comandos de inicio específicos siguen pendientes.

## Cómo usarlo

Para consultar la documentación, empezá por la consigna y las reglas; luego revisá el plan y la arquitectura. Para ejecutar la base de código, seguí la guía de instalación anterior.

Antes de modificar el proyecto, seguí la [guía de colaboración](CONTRIBUTING.md). Los cambios se proponen mediante un Pull Request y requieren revisión de un compañero antes de integrarse a `main`.

## Autores

- [51215927-spec](https://github.com/51215927-spec)
- [50509098-JC](https://github.com/50509098-JC)
- [Chino18cm](https://github.com/Chino18cm)
