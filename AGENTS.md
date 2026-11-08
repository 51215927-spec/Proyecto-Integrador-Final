# Instrucciones para agentes

- Responder siempre al usuario en español.
- Leer `README.md`, `docs/Consigna.md`, `docs/Plan.md`, `docs/Arquitectura.md` y `CONTRIBUTING.md` antes de cambiar el alcance o la estructura del proyecto. La consigna es la fuente de requisitos; no dar por implementado algo que solo está documentado.
- El proyecto principal es un juego de cartas «Guerra» de consola en C# y .NET 8. La consigna requiere personajes con seis atributos, cartas y mazos, reglas de partida y coordinación entre dos computadoras mediante un archivo compartido en red local.
- Para cada personaje, validar que los seis atributos sumen exactamente 3000, que cada uno esté entre 250 y 750, que alguno sea al menos 650 y que alguno sea como máximo 350.
- Guardar código de la aplicación en `src/`, pruebas en `test/` y documentación del proyecto en `docs/`. Al agregar documentación, enlazarla desde `README.md`. Mantener cambios de código y documentación coherentes.
- `Mis-Librerias/` contiene bibliotecas auxiliares independientes con su propia documentación. Consultar sus README antes de usarlas; `Mis-Librerias/DataBases viejo/` es código anterior y no forma parte de la solución actual.
- El programa principal todavía contiene el ejemplo inicial `Hello, World!`; las pruebas también son iniciales. Verificar el trabajo con `dotnet build Proyecto-Integrador-Final.sln` y, cuando corresponda, `dotnet test Proyecto-Integrador-Final.sln`.
- Seguir las convenciones de ramas, commits y revisión descritas en `CONTRIBUTING.md` si se trabaja con GitHub. No publicar ni integrar cambios sin la revisión prevista allí.
