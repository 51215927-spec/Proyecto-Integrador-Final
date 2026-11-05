# Guía de Git y GitHub

Esta guía explica cómo trabajar en equipo usando Git y GitHub. Vamos a usar GitHub Flow: cada integrante trabaja en su propia rama y propone sus cambios mediante un Pull Request.

## 1. Preparar el proyecto

La primera vez, descargá el proyecto:

```bash
git clone https://github.com/51215927-spec/Proyecto-Integrador-Final.git
cd Proyecto-Integrador-Final
```

Configurá tu nombre y correo para identificar tus commits:

```bash
git config --global user.name "Tu nombre"
git config --global user.email "Tu correo de GitHub"
```

## 2. Comandos básicos

| Comando | Función |
|---|---|
| `git status` | Muestra los cambios y la rama actual. |
| `git add .` | Prepara todos los cambios para guardarlos. |
| `git commit -m "mensaje"` | Guarda los cambios preparados en un commit. |
| `git push` | Sube los commits a GitHub. |
| `git pull` | Descarga e integra los cambios de GitHub. |
| `git branch` | Muestra las ramas locales. |
| `git switch main` | Cambia a la rama principal. |
| `git switch -c nombre-rama` | Crea una rama y cambia a ella. |
| `git merge main` | Integra los cambios de `main` en la rama actual. |

**Importante:** `git add` y `git commit` guardan los cambios en tu computadora. `git push` los sube a GitHub.

## 3. Nomenclatura de ramas y commits

Para que todos trabajemos de la misma manera, vamos a usar nombres claros y consistentes.

### Ramas

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

### Commits

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

## 4. Trabajar en una tarea

Antes de empezar, actualizá `main` y creá una rama propia:

```bash
git switch main
git pull
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

## 5. GitHub Flow y Pull Requests

En este proyecto vamos a usar GitHub Flow:

1. `main` es la rama principal. No trabajes directamente ahí.
2. Para cada tarea, creá una rama nueva desde `main`.
3. Hacé cambios y guardalos con commits.
4. Subí tu rama a GitHub con `git push`.
5. En GitHub, abrí un Pull Request (PR) desde tu rama hacia `main`.
6. Un compañero revisa los cambios. Si hay comentarios, corregí el código y volvé a hacer `git add`, `git commit` y `git push`. El PR se actualiza solo.
7. Cuando esté revisado, se integra el PR a `main`.
8. Después de integrarlo, actualizá tu `main` local antes de empezar otra tarea.

### ¿Qué es un Pull Request?

Un Pull Request es una propuesta para integrar los cambios de una rama en otra. En nuestro caso, se usa para pedir que revisen tu trabajo antes de incorporarlo a `main`.

El PR no sube los cambios por sí solo: primero tenés que hacer `git push`.

### Imagen: GitHub Flow

![Diagrama de GitHub Flow: crear una rama, hacer commits, abrir un Pull Request, revisar y unir los cambios](https://user-images.githubusercontent.com/6351798/48032310-63842400-e114-11e8-8db0-06dc0504dcb5.png)

[Ver la guía oficial de GitHub Flow](https://docs.github.com/en/get-started/using-github/github-flow)

## 6. Si `main` recibió cambios mientras trabajabas

Guardá tu trabajo con un commit y traé los cambios de `main` a tu rama:

```bash
git switch main
git pull
git switch feature/registro-usuarios
git merge main
```

Si Git avisa que hay conflictos, no borres archivos ni elijas cambios al azar: avisale al compañero con quien estás trabajando para resolverlos juntos.

## 7. Resumen rápido

**Antes de empezar:** `git switch main` → `git pull` → crear tu rama.

**Para guardar y compartir:** `git add .` → `git commit` → `git push`.

**Para integrar a `main`:** abrir un Pull Request en GitHub y esperar la revisión.