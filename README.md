# Proyecto integrador final

Repositorio de un proyecto académico de juego de cartas para dos jugadores, previsto como aplicación de consola en C# y .NET 8. Reúne los requisitos, el diseño y la base de código para organizar el trabajo del equipo y su futura implementación.

El juego consiste en enfrentar personajes comparando sus atributos. Cada jugador recibe tres cartas por ronda; gana la ronda quien consigue dos combates y la partida quien alcanza 15 rondas ganadas. Las reglas completas están en [Reglas del juego](docs/Reglas-del-juego.md).

## Estado actual

El trabajo actual se limita a **TI: análisis y diseño**. La implementación en Taller de Programación queda para una etapa posterior.

Las actividades 1 a 5 de TI están completadas en su documentación por confirmación del usuario. La actividad 6 conserva la propuesta por archivos y agrega un borrador alternativo REST, ambos pendientes de revisión; la actividad 7 sigue pendiente.

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
- [Protocolo de partida por archivos](docs/Protocolo-partida-archivos.md): acciones JSON, DTO Estado completo, recuperación y verificaciones pendientes.
- [Protocolo de partida web](docs/Protocolo-partidad-web.md): borrador REST, operaciones HTTP, concurrencia y decisiones pendientes.
- [UML de comunicación](docs/diagramas/Comunicacion.uxf): ampliación propuesta para la actividad 6.
- [Arquitectura](docs/Arquitectura.md): decisiones de diseño y especificaciones por clase y método, con características, contratos y justificación.
- [Tecnologías REST](docs/Tecnologias-rest.md): selección acordada para la alternativa web, alcance de persistencia y decisiones pendientes.
- [Diagrama UML](docs/diagramas/UML.uxf): modelo de dominio actualizado para abrir con UMLet; integración del protocolo pendiente de la actividad 6.
- [Guía de colaboración](CONTRIBUTING.md): organización del trabajo, ramas, commits, revisión de cambios y [metodología TDD](CONTRIBUTING.md#desarrollo-guiado-por-pruebas-tdd).

## Instalación de herramientas y dependencias

Esta guía prepara una computadora para compilar y ejecutar la base actual. Está orientada a Windows, el entorno previsto para jugar. Para consultar solamente la documentación no hace falta instalar estas herramientas.

### 1. Instalar Git y el SDK de .NET 8 desde PowerShell

Abrí **PowerShell** y ejecutá estos comandos uno por uno. WinGet es la herramienta de Windows que descarga e instala programas; aceptá la solicitud de permisos si aparece.

```powershell
winget --version
winget install --id Git.Git --exact --source winget --accept-source-agreements --accept-package-agreements
winget install --id Microsoft.DotNet.SDK.8 --exact --source winget --accept-source-agreements --accept-package-agreements
```

Si `winget` no se reconoce, abrí la página de **Instalador de aplicación** en Microsoft Store con este comando, instalalo o actualizalo y volvé a abrir PowerShell antes de repetir los comandos:

```powershell
Start-Process "ms-windows-store://pdp/?ProductId=9NBLGGH4NNS1"
```

Los identificadores de instalación corresponden a las guías oficiales de [Git para Windows](https://git-scm.com/install/windows) y [.NET en Windows](https://learn.microsoft.com/en-us/dotnet/core/install/windows). El SDK incluye las herramientas para compilar y los componentes para ejecutar .NET y ASP.NET Core. No hace falta instalar los runtimes por separado para desarrollar.

### 2. Abrir una nueva ventana de PowerShell y verificar

Cerrá PowerShell y abrilo otra vez para que reconozca los programas instalados. Ejecutá:

```powershell
git --version
dotnet --list-sdks
dotnet --list-runtimes
```

Debe aparecer una versión de Git, un SDK `8.0.xxx` y los runtimes `Microsoft.NETCore.App 8.0.x` y `Microsoft.AspNetCore.App 8.0.x`. El proyecto mantiene `net8.0` por decisión del equipo. Los números de versión del SDK y del runtime son distintos; pueden coexistir con otras versiones.

### 3. Descargar el proyecto con sus bibliotecas

En la nueva ventana de PowerShell, estos comandos crean una carpeta `proyectos` dentro de tu carpeta personal y descargan el repositorio allí. Si ya lo descargaste, usá la alternativa de abajo:

```powershell
New-Item -ItemType Directory -Path "$env:USERPROFILE\proyectos" -Force | Out-Null
Set-Location "$env:USERPROFILE\proyectos"
git clone --recurse-submodules https://github.com/51215927-spec/proyecto-integrador-final.git
Set-Location .\proyecto-integrador-final
```

Si ya habías descargado el repositorio, ejecutá lo siguiente reemplazando la ruta del ejemplo por la ubicación real de tu copia:

```powershell
Set-Location "C:\ruta\a\proyecto-integrador-final"
git submodule update --init --recursive
```

Esto recupera DataBases y Utilidades en `Mis-Librerias/`, usando la versión registrada por el proyecto. Son bibliotecas del repositorio; no se instalan mediante NuGet.

### 4. Restaurar los paquetes

Desde la carpeta que contiene `Proyecto-Integrador-Final.sln`, ejecutá:

```powershell
dotnet restore Proyecto-Integrador-Final.sln
```

Este comando descarga las versiones de los paquetes declaradas en los archivos de proyecto, incluyendo xUnit y las herramientas de pruebas. Necesitás conexión a Internet si todavía no están descargados. No hace falta instalar esos paquetes uno por uno. Consultá la [documentación de dotnet restore](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-restore) si falla la descarga.

### 5. Comprobar que la base funciona

Ejecutá estos comandos uno después del otro:

```powershell
dotnet build Proyecto-Integrador-Final.sln
dotnet test Proyecto-Integrador-Final.sln
dotnet run --project src/Proyecto-Integrador-Final
```

La compilación debe terminar sin errores. Actualmente existe una única prueba vacía y el programa muestra `Hello, World!`: estos resultados comprueban la preparación básica, no el funcionamiento del juego.

### Dependencias previstas para REST

El diseño REST acordado utiliza ASP.NET Core, HttpClient, System.Text.Json, SQLite y Entity Framework Core. HttpClient y System.Text.Json forman parte de .NET; ASP.NET Core se incluye con el SDK. EF Core y su proveedor SQLite se incorporarán como paquetes del futuro proyecto servidor, con versiones estables compatibles de la línea 8.0 (referencia acordada: 8.0.34).

Todavía no existe ese servidor ni están declarados esos paquetes en la solución. Por eso, la restauración actual no instala EF Core ni prepara una base de datos. Cuando se autorice Taller y se incorporen sus referencias, se actualizará esta guía con los pasos para crear la base y ejecutar el servidor. SQLite se utilizará dentro del servidor; no se prevé instalar un servicio de base de datos independiente. Las herramientas de migraciones de EF Core también quedan para esa etapa.

#### Comandos de PowerShell para EF Core y SQLite (etapa futura)

Estos son los comandos de instalación previstos. Se ejecutarán cuando Taller esté autorizado y exista el proyecto servidor; no deben aplicarse al proyecto de consola actual. Cuando llegue esa etapa, PowerShell pedirá la ruta real del archivo `.csproj` del servidor, para no depender de un nombre todavía sin definir:

```powershell
$proyectoServidor = Read-Host "Ruta del archivo .csproj del servidor"
if (-not (Test-Path -LiteralPath $proyectoServidor -PathType Leaf)) {
    throw "No existe el archivo de proyecto indicado."
}
dotnet add "$proyectoServidor" package Microsoft.EntityFrameworkCore --version 8.0.34
dotnet add "$proyectoServidor" package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.34
dotnet add "$proyectoServidor" package Microsoft.EntityFrameworkCore.Design --version 8.0.34
dotnet restore "$proyectoServidor"
dotnet list "$proyectoServidor" package
```

El proveedor SQLite incluye las dependencias necesarias para usar la base desde .NET. No hace falta instalar un servidor SQLite por separado. Los clientes de consola no necesitan estos paquetes de persistencia.

Para instalar la herramienta de migraciones de EF Core por primera vez:

```powershell
dotnet tool install --global dotnet-ef --version 8.0.34
dotnet ef --version
```

Si `dotnet-ef` ya estaba instalada y necesitás seleccionar esta versión, usá esta alternativa al comando de instalación:

```powershell
dotnet tool update --global dotnet-ef --version 8.0.34
```

Si la versión global existente pertenece a otra línea y la herramienta rechaza el cambio, conservála y revisá una instalación local para este proyecto; no hace falta desinstalar herramientas usadas por otros proyectos. Crear tablas y migraciones requiere diseñar e implementar primero el modelo de datos; instalar los paquetes no crea la base del juego.

Sintaxis contrastada con las referencias oficiales del [proveedor SQLite](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/) y las [herramientas de EF Core](https://learn.microsoft.com/en-us/ef/core/cli/dotnet).

En el diseño acordado, primero se abre el servidor y, cuando está disponible, se abren los clientes. La implementación y los comandos de inicio específicos siguen pendientes.

## Cómo usarlo

Para consultar la documentación, empezá por la consigna y las reglas; luego revisá el plan y la arquitectura. Para ejecutar la base de código, seguí la guía de instalación anterior.

Antes de modificar el proyecto, seguí la [guía de colaboración](CONTRIBUTING.md). Los cambios se proponen mediante un Pull Request y requieren revisión de un compañero antes de integrarse a `main`.

## Autores

- [51215927-spec](https://github.com/51215927-spec)
- [50509098-JC](https://github.com/50509098-JC)
- [Chino18cm](https://github.com/Chino18cm)
