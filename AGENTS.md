# Instrucciones para agentes

## 1. Idioma y alcance del proyecto

- Responder siempre al usuario en español.
- El alcance actual del proyecto se limita a **TI (análisis y diseño)**. Mantener Taller de Programación (implementación) como trabajo futuro hasta que el usuario autorice explícitamente avanzar con esa etapa.
- No implementar funcionalidades, crear código de producción ni adelantar trabajo de Taller de Programación mientras el alcance esté limitado a TI.
- Diferenciar claramente entre requisitos definidos, decisiones de diseño, trabajo pendiente y funcionalidades efectivamente implementadas.
- No considerar implementado algo por el simple hecho de que esté documentado.

## 2. Contexto y documentación

Antes de modificar el alcance, la arquitectura o la estructura del proyecto, leer:

- `README.md`
- `docs/Consigna.md`
- `docs/Plan.md`
- `docs/Arquitectura.md`
- `CONTRIBUTING.md`

La fuente principal de requisitos es `docs/Consigna.md`. En caso de contradicción, no asumir qué interpretación es correcta: identificar el conflicto y solicitar una definición cuando sea necesario.

Aplicar el skill `desarrollo-documentado` durante el trabajo en este proyecto:

- Consultar el contexto y el estado real del repositorio antes de planificar.
- Mantener `docs/Plan.md` y `docs/Arquitectura.md` actualizados según las decisiones vigentes, exclusivamente mediante tareas documentales en ramas `docs/`.
- Realizar cambios acotados, coherentes y revisables.
- Verificar los aspectos afectados por cada modificación.
- Registrar decisiones y resultados relevantes cuando aporten trazabilidad.
- Respetar la estructura documental existente y evitar duplicaciones innecesarias.

### Separación de cambios documentales

- Modificar documentación únicamente desde ramas cuyo nombre comience con `docs/`. Esta regla incluye `AGENTS.md`, `README.md`, `CONTRIBUTING.md` y los documentos de `docs/`.
- No modificar `docs/Plan.md` ni marcar o tachar tareas completadas desde ramas de código o pruebas.
- Realizar las actualizaciones documentales derivadas de una tarea en una rama `docs/` independiente, con commits exclusivamente documentales. No mezclar código y documentación en un mismo commit o PR.
- Si una tarea de código requiere actualizar el plan, la arquitectura u otro documento, informar el pendiente al usuario y realizar la actualización en la tarea documental separada.
- Esta separación prevalece sobre las indicaciones generales de actualizar documentación al completar una tarea y busca evitar conflictos entre ramas.

## 3. Descripción del proyecto

El proyecto consiste en desarrollar un juego de cartas «Guerra» de consola, utilizando C# y .NET 8.

La consigna contempla:

- Personajes con seis atributos.
- Cartas y mazos.
- Reglas y desarrollo de las partidas.
- Coordinación entre dos computadoras mediante un archivo compartido en una red local.

### Validaciones de personajes

Cada personaje deberá cumplir las siguientes restricciones:

- La suma de sus seis atributos debe ser exactamente 3000.
- Cada atributo debe estar comprendido entre 250 y 750, inclusive.
- Al menos un atributo debe ser igual o superior a 650.
- Al menos un atributo debe ser igual o inferior a 350.

Estas restricciones forman parte de los requisitos y deben mantenerse presentes durante el análisis, diseño e implementación.

## 4. Estructura del repositorio

Respetar la siguiente organización:

- `src/`: código fuente de la aplicación.
- `test/`: proyectos y código de pruebas automatizadas.
- `docs/`: documentación del proyecto.

Al agregar documentación, enlazarla desde `README.md` cuando corresponda.

Mantener la documentación y el código coherentes. Una modificación arquitectónica relevante debe reflejarse en la documentación correspondiente.

### Bibliotecas auxiliares

- `Mis-Librerias/` contiene bibliotecas auxiliares independientes, con sus propios proyectos y documentación.
- Consultar los README de las bibliotecas antes de utilizarlas.
- `Mis-Librerias/DataBases viejo/` contiene código anterior y no forma parte de la solución actual.
- No asumir que el comportamiento de una biblioteca es conocido sin consultar su implementación o documentación pertinente.

### Estado inicial

El programa principal todavía contiene el ejemplo inicial `Hello, World!` y las pruebas también son iniciales.

No asumir que existe funcionalidad implementada previamente sin comprobarlo en el repositorio.

## 5. Gestión de Git y GitHub

### 5.1. Sincronización previa

Antes de comenzar una nueva tarea de GitHub:

1. Comprobar el estado del repositorio y los cambios locales.
2. Actualizar las referencias remotas mediante `git fetch`.
3. Revisar el estado de `main` respecto de `origin/main`.
4. Sincronizar `main` mediante avance rápido (`fast-forward`) cuando sea posible.
5. Crear la rama de trabajo desde el `main` actualizado.

Si existen divergencias, conflictos o cambios locales pendientes:

- Analizar su origen y contenido.
- Conservar los cambios locales y remotos.
- No utilizar operaciones destructivas para forzar la sincronización.
- No descartar, sobrescribir ni integrar cambios sin la revisión correspondiente.
- Informar al usuario si es necesaria una intervención antes de continuar.

Nunca asumir que una sincronización se realizó correctamente sin verificar el resultado.

### 5.2. Ramas

Cada tarea de GitHub debe desarrollarse en una rama nueva.

Utilizar el siguiente formato:

`tipo/descripcion-corta`

Tipos permitidos:

- `feature/`: nuevas funcionalidades.
- `fix/`: correcciones.
- `docs/`: documentación.
- `refactor/`: refactorizaciones.
- `test/`: pruebas automatizadas.

La descripción debe estar en minúsculas, sin tildes y con palabras separadas por guiones.

Ejemplos:

- `feature/sistema-de-combate`
- `fix/validacion-de-atributos`
- `docs/actualizar-arquitectura`
- `refactor/organizacion-de-clases`
- `test/pruebas-de-personajes`

No trabajar directamente sobre `main`.

### Trabajo en paralelo con worktrees

- Utilizar `git worktree` para trabajar en tareas simultáneas: cada tarea debe tener su propia rama y directorio de trabajo.
- Guardar los worktrees en directorios hermanos del repositorio principal con el formato `../worktree-<nombre-de-la-rama>`, sustituyendo `/` por `-`. Por ejemplo, la rama `docs/protocolo-partida-web` utiliza `../worktree-docs-protocolo-partida-web`. No ubicarlos en `/tmp` ni dentro del directorio del repositorio principal.
- Crear cada rama nueva desde `main` actualizado, respetando los tipos de rama y la separación de documentación, código y pruebas.
- Antes de crear o reutilizar un worktree, consultar `git worktree list`, el estado de los directorios implicados y las referencias remotas.
- No cambiar la rama de un directorio utilizado por otra tarea ni compartir un mismo worktree entre agentes o tareas que escriban simultáneamente.
- Coordinar los cambios sobre archivos comunes y revisar los resultados antes de integrarlos; los worktrees no evitan conflictos al integrar ramas.
- Mantener commits, verificaciones y revisión de PR independientes por tarea. El uso de worktrees no autoriza implementar fuera de alcance ni omitir la revisión requerida.
- Al terminar, retirar un worktree únicamente si está limpio y su trabajo está conservado. No usar eliminación forzada ni descartar cambios pendientes.
- Cuando una integración sea final y la tarea quede cerrada, eliminar su rama local, su rama remota y su worktree. No aplicar esta limpieza a una sincronización parcial ni a una rama con trabajo en curso.
- Antes de limpiar, verificar que la integración esté completa en la rama de destino y publicada en el remoto, que el worktree esté limpio y que no queden commits o cambios pendientes de conservar. Si hubo squash o rebase, comprobar la equivalencia de los cambios antes de borrar.
- Retirar primero el worktree limpio y después eliminar las ramas local y remota; comprobar el resultado con `git worktree list` y las referencias de Git. No usar borrado forzado como sustituto de verificar la integración. Si alguna comprobación falla, conservar el trabajo e informar el pendiente.

### 5.3. Commits

Utilizar el siguiente formato:

`tipo: descripcion breve`

Tipos permitidos:

- `feat`
- `fix`
- `docs`
- `refactor`
- `test`

Los mensajes deben ser concretos, estar en minúsculas, no llevar tildes y no terminar en punto.

Ejemplos:

- `feat: agregar validacion de personajes`
- `fix: corregir calculo de atributos`
- `docs: actualizar arquitectura`
- `refactor: simplificar gestion de mazos`
- `test: agregar pruebas de personajes`

### 5.4. Pull Requests y colaboración

- Seguir las convenciones de colaboración descritas en `CONTRIBUTING.md`.
- No publicar ni integrar cambios sin la revisión prevista en ese documento.
- No aprobar ni integrar un PR propio cuando se requiera la revisión de un compañero.
- Mantener los PR acotados y describir claramente los cambios realizados.
- No considerar finalizada una tarea que requiere revisión hasta que esta se haya completado.

### 5.5. Archivos PDF de referencia

- Los PDF aportados como referencia son exclusivamente para consulta local.
- No agregarlos al repositorio ni subirlos a GitHub.
- Revisarlos y trasladar al plan los avances, requisitos y pendientes pertinentes.
- Una vez completada la actualización documental, eliminarlos únicamente cuando el usuario lo solicite.

## 6. Verificación del proyecto

Utilizar los siguientes comandos para comprobar el estado de la solución:

Compilación:

`dotnet build Proyecto-Integrador-Final.sln`

Pruebas automatizadas:

`dotnet test Proyecto-Integrador-Final.sln`

La compilación y las pruebas deben ejecutarse según corresponda al alcance y al tipo de modificación.

- Durante TI, no implementar código únicamente para conseguir que compile o pasen las pruebas.
- Durante Taller de Programación, verificar la compilación y ejecutar las pruebas pertinentes.
- No afirmar que una verificación fue exitosa sin haber ejecutado el comando correspondiente.
- Informar los errores encontrados y distinguir entre errores preexistentes y errores introducidos por los cambios realizados.

---

## 7. Metodología de desarrollo: TDD

Esta metodología se aplicará exclusivamente cuando el usuario autorice avanzar con Taller de Programación.

Todo el desarrollo de código deberá seguir **TDD (Test-Driven Development)**, utilizando xUnit para las pruebas automatizadas.

El proceso seguirá el ciclo RED-GREEN-REFACTOR.

### 7.1. Definición de la estructura

Antes de implementar la lógica:

- Crear las clases, atributos, constructores y firmas de los métodos necesarios, respetando la arquitectura definida.
- Los métodos que todavía no tengan implementación deberán lanzar `NotImplementedException`.
- No agregar lógica de producción en esta etapa.

Esta etapa establece la estructura inicial del código, pero no reemplaza la escritura previa de pruebas para cada comportamiento.

### 7.2. Escritura de pruebas (RED)

- Desarrollar los tests en los proyectos ubicados en `test/`, utilizando xUnit.
- Escribir las pruebas antes de implementar la lógica correspondiente.
- Contemplar los casos esperados, casos límite, valores inválidos y excepciones.
- Utilizar `[Fact]` para escenarios individuales y `[Theory]` con `[InlineData]` cuando resulte conveniente probar múltiples conjuntos de datos.
- Ejecutar los tests y comprobar que fallen por la ausencia de la implementación o por el comportamiento todavía no satisfecho.

### 7.3. Implementación (GREEN)

- Implementar únicamente la lógica necesaria para satisfacer los tests existentes.
- Ejecutar las pruebas después de cada modificación relevante.
- Modificar el código hasta conseguir que todas las pruebas correspondientes pasen.
- Evitar agregar funcionalidades que no estén respaldadas por requisitos o comportamientos definidos.

### 7.4. Refactorización (REFACTOR)

- Mejorar la estructura, legibilidad, mantenibilidad y calidad del código.
- No modificar el comportamiento esperado durante la refactorización.
- Ejecutar nuevamente los tests después de cada refactorización.
- No considerar terminada una refactorización mientras existan pruebas fallidas.

### 7.5. Mantenimiento

Toda modificación posterior deberá conservar el comportamiento previamente validado.

- Los cambios en funcionalidades existentes deben mantener los tests anteriores, salvo que los requisitos hayan cambiado.
- Las nuevas funcionalidades deben incorporar sus correspondientes pruebas.
- Las modificaciones de comportamiento deben reflejarse en los tests afectados.
- Ejecutar la suite completa antes de finalizar una tarea de desarrollo.

### 7.6. Reglas de TDD

- No implementar lógica de producción antes de escribir los tests correspondientes.
- No eliminar, desactivar ni modificar pruebas simplemente para conseguir que pasen.
- Si un test parece incorrecto, analizarlo y justificar cualquier modificación según los requisitos.
- No considerar terminado un desarrollo mientras existan pruebas fallidas.
- No reemplazar pruebas automatizadas por comprobaciones manuales.
- No utilizar mocks innecesariamente: emplearlos cuando sea necesario aislar dependencias externas o colaboradoras.
- Mantener los tests independientes entre sí y evitar que dependan del orden de ejecución.
- No confundir cobertura de código con calidad de las pruebas.

---

## 8. Criterios generales de trabajo

- Priorizar la corrección y la coherencia con la consigna por encima de la velocidad.
- No inventar requisitos ni tomar decisiones arquitectónicas importantes sin fundamento.
- No realizar modificaciones ajenas a la tarea solicitada.
- Evitar cambios innecesarios en archivos que no estén relacionados con el objetivo.
- Ante una ambigüedad que pueda afectar significativamente el diseño o la implementación, consultar antes de asumir.
- Mantener informado al usuario sobre decisiones relevantes, problemas encontrados y verificaciones realizadas.
- Diferenciar siempre entre trabajo propuesto, trabajo realizado y trabajo pendiente.
