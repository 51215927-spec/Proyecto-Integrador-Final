## Estructura del proyecto

El repositorio está organizado en carpetas para separar el código fuente, las pruebas y la documentación.

```text
Proyecto-Integrador-Final/
│
├── docs/
│   ├── Consigna.md
│   ├── Arquitectura.md
│   ├── Plan.md
│   └── Guia-Git.md
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
├── .gitignore
├── Proyecto-Integrador-Final.sln
└── README.md
```

### ¿Dónde guardar cada archivo?

| Ubicación | Contenido |
|---|---|
| `src/` | Código fuente de la aplicación. |
| `test/` | Proyectos de pruebas automatizadas. |
| `docs/` | Documentación del proyecto, consignas, arquitectura, planificación y guías. |
| `.gitignore` | Archivos y carpetas que Git debe ignorar. |
| `.sln` | Solución de Visual Studio que agrupa los proyectos. |
| `README.md` | Presentación del proyecto e índice de documentación. |

### Reglas para organizar los archivos

- Guardar el código de la aplicación dentro de `src/`, no directamente en la raíz del repositorio.
- Guardar las pruebas dentro de `test/`.
- Guardar los documentos Markdown (`.md`) dentro de `docs/`, excepto el `README.md` principal.
- No subir carpetas generadas automáticamente, como `bin/` y `obj/`.
- Si se agrega un nuevo proyecto de código, ubicarlo dentro de `src/` o `test/`, según corresponda.
- Si se agrega documentación, incluir un enlace a ella en el `README.md`.

### Enlaces a la documentación

- [Arquitectura](docs/Arquitectura.md)
- 
