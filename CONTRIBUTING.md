# Reglas para la colaboración del proyecto

## Guía de Git y GitHub

Esta guía explica cómo trabajar en equipo usando Git y GitHub. Vamos a usar GitHub Flow: cada integrante trabaja en su propia rama y propone sus cambios mediante un Pull Request.

### 1. Preparar el proyecto

La primera vez, descargá el proyecto:

```bash
git clone --recurse-submodules https://github.com/51215927-spec/Proyecto-Integrador-Final.git
cd Proyecto-Integrador-Final
```

Configurá tu nombre y correo para identificar tus commits:

```bash
git config --global user.name "Tu nombre"
git config --global user.email "Tu correo de GitHub"
```

### 2. Comandos básicos

| Comando | Función |
|---|---|
| `git status` | Muestra los cambios y la rama actual. |
| `git add .` | Prepara todos los cambios para guardarlos. |
| `git commit -m "mensaje"` | Guarda los cambios preparados en un commit. |
| `git push` | Sube los commits a GitHub. |
| `git pull --ff-only` | Descarga e integra los cambios de GitHub. |
| `git branch` | Muestra las ramas locales. |
| `git switch main` | Cambia a la rama principal. |
| `git switch -c nombre-rama` | Crea una rama y cambia a ella. |
| `git merge main` | Integra los cambios de `main` en la rama actual. |

**Importante:** `git add` y `git commit` guardan los cambios en tu computadora. `git push` los sube a GitHub.

### 3. Nomenclatura de ramas y commits

Para que todos trabajemos de la misma manera, vamos a usar nombres claros y consistentes.

#### Ramas

Usamos este formato:

```text
tipo/descripcion-corta
```

Tipos de ramas:

| Tipo | Cuándo usarlo | Ejemplo |
|---|---|---|
| `feature/` | Agregar una funcionalidad. | `feature/registro-usuarios` |
| `fix/` | Corregir un error. | `fix/error-login` |
| `docs/` | Agregar o modificar documentación. | `docs/guia-git` |
| `refactor/` | Mejorar el código sin cambiar su comportamiento. | `refactor/validacion-usuarios` |
| `test/` | Agregar o modificar pruebas. | `test/registro-usuarios` |

Reglas:

- Usar minúsculas y guiones para separar palabras.
- No usar espacios, tildes ni caracteres especiales.
- Describir la tarea, no el nombre de quien la hace.
- Crear una rama nueva para cada tarea.

#### Commits

Usamos este formato:

```text
tipo: descripcion breve
```

Tipos de commits:

| Tipo | Cuándo usarlo | Ejemplo |
|---|---|---|
| `feat` | Nueva funcionalidad. | `feat: agregar registro de usuarios` |
| `fix` | Corrección de un error. | `fix: corregir validacion del login` |
| `docs` | Cambios en documentación. | `docs: agregar guia de Git` |
| `refactor` | Mejorar el código sin cambiar su comportamiento. | `refactor: simplificar validacion` |
| `test` | Agregar o modificar pruebas. | `test: agregar pruebas de registro` |

Reglas:

- Escribir el mensaje en infinitivo o describiendo la acción realizada.
- Usar minúsculas y evitar puntos al final.
- Hacer commits pequeños, que representen un cambio concreto.
- No usar mensajes genéricos como `cambios`, `arreglos` o `actualizacion`.

### 4. Trabajar en una tarea

Antes de empezar, comprobá `git status` y conservá cualquier cambio local pendiente. Actualizá las referencias remotas, sincronizá `main` por avance rápido y creá una rama propia. Si hay divergencias, revisalas antes de continuar sin sobrescribir trabajo local:

```bash
git fetch origin
git switch main
git merge --ff-only origin/main
git switch -c feature/registro-usuarios
```

Trabajá en los archivos del proyecto. Cuando quieras guardar y compartir tus cambios:

```bash
git status
git add .
git commit -m "feat: agregar registro de usuarios"
git push -u origin feature/registro-usuarios
```

La primera vez se usa `git push -u origin ...` para publicar la rama. Después, en esa misma rama, alcanza con:

```bash
git push
```

### 5. GitHub Flow y Pull Requests

En este proyecto vamos a usar GitHub Flow:

1. `main` es la rama principal. No trabajes directamente ahí.
2. Para cada tarea, creá una rama nueva desde `main`.
3. Hacé cambios y guardalos con commits.
4. Subí tu rama a GitHub con `git push`.
5. En GitHub, abrí un Pull Request (PR) desde tu rama hacia `main`.
6. Un compañero revisa los cambios. Si hay comentarios, corregí el código y volvé a hacer `git add`, `git commit` y `git push`. El PR se actualiza solo.
7. Cuando esté revisado, se integra el PR a `main`.
8. Después de integrarlo, actualizá tu `main` local antes de empezar otra tarea.

#### ¿Qué es un Pull Request?

Un Pull Request es una propuesta para integrar los cambios de una rama en otra. En nuestro caso, se usa para pedir que revisen tu trabajo antes de incorporarlo a `main`.

El PR no sube los cambios por sí solo: primero tenés que hacer `git push`.

#### Cómo crear un Pull Request

##### Paso 1. Subir los cambios a GitHub

Antes de crear un PR, guardá tus cambios y subí tu rama:

```bash
git add .
git commit -m "feat: agregar registro de usuarios"
git push
```

Si es la primera vez que subís esa rama, usá:

```bash
git push -u origin feature/registro-usuarios
```

##### Paso 2. Abrir el Pull Request

1. Entrá al repositorio en GitHub.
2. Hacé clic en la pestaña `Pull requests`.
3. Presioná `New pull request`.
4. Elegí las ramas que querés comparar:
   - `base`: `main`, donde querés incorporar los cambios.
   - `compare`: `feature/registro-usuarios`, tu rama de trabajo.
5. Revisá que aparezcan los cambios correctos y presioná `Create pull request`.

También podés entrar a la página principal del repositorio. Si GitHub muestra el botón `Compare & pull request` después de subir tu rama, podés usarlo directamente.

##### Paso 3. Completar la descripción

El título debe explicar brevemente qué cambios contiene el PR.

En la descripción, explicá qué hiciste y qué debería revisar tu compañero.

Ejemplo de título:

```text
feat: agregar registro de usuarios
```

Ejemplo de descripción:

```markdown
## Cambios realizados

- Se agregó el registro de usuarios.
- Se incorporó la validación de datos.

## Pruebas

- Se verificó el registro con datos válidos.
- Se verificó el rechazo de datos incorrectos.
```

Cuando termines, presioná `Create pull request`.

##### Paso 4. Solicitar una revisión

Una vez creado el PR, un compañero debe revisar los cambios.

Para solicitar una revisión:

1. Abrí el PR.
2. En la sección `Reviewers`, seleccioná al compañero que debe revisar tu trabajo.
3. Si es necesario, agregá comentarios o explicaciones en la descripción.

El revisor debe comprobar que los cambios funcionen correctamente y que no afecten otras partes del proyecto.

##### Paso 5. Corregir los cambios si es necesario

Si el revisor encuentra un problema, puede dejar comentarios en el PR.

Para corregirlo, volvé a tu computadora y trabajá en la misma rama:

```bash
git add .
git commit -m "fix: corregir validacion de usuarios"
git push
```

Los nuevos commits se incorporan automáticamente al PR existente. No es necesario crear otro.

El revisor puede volver a comprobar los cambios.

##### Paso 6. Integrar el PR a main

Cuando el revisor apruebe los cambios, el PR puede integrarse a `main`.

1. Entrá al PR.
2. Comprobá que no haya conflictos ni problemas pendientes.
3. Presioná `Merge pull request`.
4. Confirmá la integración con `Confirm merge`.

Una vez integrado, los cambios de tu rama pasan a formar parte de `main`.

**Importante:** No integres tu propio PR sin la revisión correspondiente. Si el PR todavía no fue aprobado, esperá a que tu compañero lo revise.

##### Paso 7. Actualizar tu computadora

Después de que el PR se integre, actualizá tu rama principal:

```bash
git switch main
git pull --ff-only
```

Ahora tu computadora tiene los cambios que se incorporaron al repositorio. Podés empezar una nueva tarea creando otra rama.

#### Resumen del proceso

```text
Crear una rama
      |
      v
Hacer cambios y commits
      |
      v
Subir la rama a GitHub
      |
      v
Crear el Pull Request
      |
      v
Revisión de un compañero
      |
      v
¿Hay correcciones?
   /         \
 Sí           No
 |             |
 v             |
Corregir       |
y subir        |
los cambios    |
 |             |
 └──────┬──────┘
        v
Integrar el PR a main
        |
        v
Actualizar main local
```

### Imagen: GitHub Flow

![Diagrama de GitHub Flow: crear una rama, hacer commits, abrir un Pull Request, revisar y unir los cambios](https://user-images.githubusercontent.com/6351798/48032310-63842400-e114-11e8-8db0-06dc0504dcb5.png)

[Ver la guía oficial de GitHub Flow](https://docs.github.com/en/get-started/using-github/github-flow)

### 6. Si main recibió cambios mientras trabajabas

Guardá tu trabajo con un commit y traé los cambios de `main` a tu rama:

```bash
git switch main
git pull --ff-only
git switch feature/registro-usuarios
git merge main
```

Si Git avisa que hay conflictos, no borres archivos ni elijas cambios al azar: avisale al compañero con quien estás trabajando para resolverlos juntos.

### 7. Resumen rápido

**Antes de empezar:** `git status` → `git fetch origin` → `git switch main` → `git merge --ff-only origin/main` → crear tu rama.

**Para guardar y compartir:** `git add .` → `git commit` → `git push`.

**Para integrar a `main`:** abrir un Pull Request en GitHub y esperar la revisión.

## Estructura del proyecto

El repositorio está organizado en carpetas para separar el código fuente, las pruebas y la documentación.

```text
Proyecto-Integrador-Final/
│
├── docs/
│   ├── Consigna.md
│   ├── Arquitectura.md
│   ├── Plan.md
│   ├── Reglas-del-juego.md
│   └── diagramas/
│       └── UML.uxf
│
├── src/
│   └── Proyecto-Integrador-Final/
│       ├── Program.cs
│       └── ...
│
├── test/
│   └── Proyecto-Integrador-Final.Tests/
│       └── ...
│
├── Mis-Librerias/             Submódulo independiente
├── AGENTS.md
├── CONTRIBUTING.md
├── .gitignore
├── .gitmodules
├── Proyecto-Integrador-Final.sln
└── README.md
```

### ¿Dónde guardar cada archivo?

| Ubicación | Contenido |
|---|---|
| `src/` | Código fuente de la aplicación. |
| `test/` | Proyectos de pruebas automatizadas. |
| `docs/` | Documentación del proyecto, consignas, arquitectura, planificación y guías. |
| `Mis-Librerias/` | Submódulo independiente; sus bibliotecas aún no están referenciadas por la aplicación. |
| `CONTRIBUTING.md` | Guía de colaboración de este repositorio. |
| `AGENTS.md` | Instrucciones de trabajo para agentes. |
| `.gitignore` | Archivos y carpetas que Git debe ignorar. |
| `.sln` | Solución de Visual Studio que agrupa los proyectos. |
| `README.md` | Presentación del proyecto e índice de documentación. |

### Reglas para organizar los archivos

- Guardar el código de la aplicación dentro de `src/`, no directamente en la raíz del repositorio.
- Guardar las pruebas dentro de `test/`.
- Guardar la documentación del proyecto en `docs/`. En la raíz se mantienen `README.md`, `CONTRIBUTING.md` y `AGENTS.md`; las bibliotecas del submódulo conservan sus propios README.
- No subir carpetas generadas automáticamente, como `bin/` y `obj/`.
- Si se agrega un nuevo proyecto de código, ubicarlo dentro de `src/` o `test/`, según corresponda.
- Si se agrega documentación, incluir un enlace a ella en el `README.md`.
