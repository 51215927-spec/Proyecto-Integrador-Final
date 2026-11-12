# Instrucciones para agentes

- Responder siempre al usuario en español.
- El alcance actual se limita a TI (análisis y diseño). Mantener Taller de Programación como trabajo futuro hasta que el usuario indique avanzar con la implementación.
- No agregar a Git ni subir a GitHub los PDF aportados como referencia. Revisarlos localmente y trasladar los avances y pendientes pertinentes al plan; eliminarlos cuando el usuario lo solicite y se haya completado la actualización.
- Antes de comenzar una nueva tarea de GitHub, actualizar las referencias remotas y sincronizar `main` mediante avance rápido, conservando los cambios locales. Crear la rama de la tarea desde el `main` actualizado; si hay divergencias, revisarlas sin sobrescribir trabajo ni integrar cambios sin la revisión requerida.
- Leer `README.md`, `docs/Consigna.md`, `docs/Plan.md`, `docs/Arquitectura.md` y `CONTRIBUTING.md` antes de cambiar el alcance o la estructura del proyecto. La consigna es la fuente de requisitos; no dar por implementado algo que solo está documentado.
- El proyecto principal es un juego de cartas «Guerra» de consola en C# y .NET 8. La consigna requiere personajes con seis atributos, cartas y mazos, reglas de partida y coordinación entre dos computadoras mediante un archivo compartido en red local.
- Para cada personaje, validar que los seis atributos sumen exactamente 3000, que cada uno esté entre 250 y 750, que alguno sea al menos 650 y que alguno sea como máximo 350.
- Guardar código de la aplicación en `src/`, pruebas en `test/` y documentación del proyecto en `docs/`. Al agregar documentación, enlazarla desde `README.md`. Mantener cambios de código y documentación coherentes.
- `Mis-Librerias/` contiene bibliotecas auxiliares independientes con su propia documentación. Consultar sus README antes de usarlas; `Mis-Librerias/DataBases viejo/` es código anterior y no forma parte de la solución actual.
- El programa principal todavía contiene el ejemplo inicial `Hello, World!`; las pruebas también son iniciales. Verificar el trabajo con `dotnet build Proyecto-Integrador-Final.sln` y, cuando corresponda, `dotnet test Proyecto-Integrador-Final.sln`.
- Seguir las convenciones de ramas, commits y revisión descritas en `CONTRIBUTING.md` si se trabaja con GitHub. No publicar ni integrar cambios sin la revisión prevista allí.
- Para cada tarea de GitHub, usar una rama nueva con formato `tipo/descripcion-corta`: `feature/`, `fix/`, `docs/`, `refactor/` o `test/`; escribir la descripción en minúsculas, sin tildes y con guiones. Usar commits con formato `tipo: descripcion breve`: `feat`, `fix`, `docs`, `refactor` o `test`, con un mensaje concreto, en minúsculas y sin punto final. No trabajar directamente en `main` ni integrar un PR sin revisión de un compañero.
- Aplicar el skill `desarrollo-documentado` al trabajar en este proyecto: consultar el contexto y el estado real antes de planificar, mantener `docs/Plan.md` y `docs/Arquitectura.md` acordes con las decisiones vigentes, implementar cambios acotados y revisables, verificar el comportamiento afectado y registrar decisiones o resultados relevantes cuando aporten trazabilidad. Respetar la estructura existente del repositorio y evitar documentación duplicada.
- Para el desarrollo, se utilisara la metodologia TDD (Test-Driven Development). Los pasos seran los siguentes:
  1. Generar las firmas de los metodos pero no sus firmas (metodo() => throw new notimplementedexception()).
  2. Generar los test en `test/` con xUnit.
  3. Hacer y rehacer los metodos hasta que pasen las pruebas.
  4. Cada modificacion posterios debe pasar todos los test y los nuevos que requiera.
