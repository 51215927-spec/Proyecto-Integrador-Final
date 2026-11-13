# Tecnologías de la alternativa REST

## Decisión y alcance

El usuario confirmó la selección tecnológica durante la discusión del 13 de noviembre de 2026 y precisó que la base de datos también conserva permanentemente los personajes. Esta decisión corresponde a TI: no autoriza implementación, instalación de dependencias ni migración del proyecto. Complementa el borrador REST disponible en la rama `docs/protocolo-partida-web`; no aprueba todos sus contratos ni sustituye el requisito académico de archivo compartido.

## Tecnologías seleccionadas

| Tecnología | Responsabilidad prevista |
|---|---|
| C# y .NET | Lenguaje y plataforma para dominio, servidor y clientes de consola. |
| ASP.NET Core con Minimal APIs | Exponer las operaciones REST y delegar las reglas al dominio. |
| HttpClient | Realizar solicitudes HTTP desde los clientes de consola. |
| System.Text.Json | Serializar y deserializar los DTO de intercambio JSON. |
| SQLite | Persistir el catálogo de personajes y los datos de las partidas en una base local al servidor. |
| Entity Framework Core con proveedor SQLite | Acceso a datos, configuración del modelo persistente, migraciones y transacciones. |
| xUnit | Pruebas automatizadas de reglas e integración durante Taller, siguiendo TDD. |

REST es el estilo de comunicación; HTTP y JSON son el protocolo y el formato de intercambio. La selección cubre la base tecnológica prevista. Los mecanismos concretos de identidad, despliegue y recuperación se definirán con sus contratos; no se agregan paquetes por anticipado.

## Persistencia

La base de datos no es solamente una copia para emergencias: es el almacenamiento permanente del catálogo. Conservará nombre, descripción, ASCII y los seis atributos de cada personaje. Se mantienen las restricciones del dominio: suma exactamente 3000, cada atributo entre 250 y 750 inclusive, al menos uno mayor o igual a 650 y al menos uno menor o igual a 350.

También se prevé conservar participantes, estado, orden del mazo, manos, rondas, combates, elecciones, resultados, marcadores y control de operaciones de las partidas. Las tablas, relaciones, restricciones y límites de las transacciones todavía deben diseñarse. Elegir SQLite y EF Core no implica que el modelo o la recuperación estén implementados.

El servidor será el único componente que acceda a SQLite; los clientes consultarán la API. La base quedará en el disco local del servidor, sin compartirse como archivo entre clientes.

Queda pendiente definir cómo ingresa y se mantiene el catálogo: importación del CSV existente, altas y modificaciones desde el sistema, o una combinación. No se incorpora un editor de personajes por inferencia. También debe definirse cómo conservar los datos usados por una partida para que una modificación posterior del catálogo no cambie sus reglas o resultados. El contrato Personaje.Configurar de DataBases sigue perteneciendo al diseño anterior y deberá conciliarse al adaptar el modelo REST; no se mantienen dos fuentes de persistencia autoritativas por defecto.

## Versiones y ejecución pendientes

La plataforma del repositorio sigue siendo .NET 8. .NET 10 LTS se propuso en la conversación, pero no se acordó expresamente una migración. Falta confirmar la versión permitida por la materia, los SDK disponibles y las versiones compatibles de ASP.NET Core, EF Core y su proveedor SQLite. No se cambian los archivos de proyecto en esta tarea.

La propuesta de ejecución es un servidor y dos clientes de consola; servidor y un cliente pueden compartir computadora. Deben confirmarse esa distribución, dirección, puerto, HTTP/HTTPS y configuración de la red. La consulta periódica del estado sigue siendo una propuesta; su intervalo y la presencia de clientes no quedan aprobados por elegir las bibliotecas.

## Referencias consultadas durante la discusión

- [Minimal APIs de ASP.NET Core](https://dotnet.microsoft.com/en-us/learn/back-end-web-dev).
- [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview).
- [Proveedor SQLite de EF Core](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/).
- [Política de soporte de .NET](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core).

Las referencias explican las tecnologías; no acreditan implementación o verificaciones del proyecto. Las tareas vigentes están en [Plan](Plan.md#tareas-vigentes-de-la-alternativa-rest).
