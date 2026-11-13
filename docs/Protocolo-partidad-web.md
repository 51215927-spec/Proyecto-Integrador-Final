# Protocolo de partida web — API REST

## Estado y alcance

Borrador de TI iniciado el 13 de noviembre de 2026. El usuario eligió REST para explorar la comunicación web. No hay implementación REST del juego ni verificaciones entre computadoras. El usuario confirmó net8.0, ASP.NET Core con Minimal APIs, HttpClient, System.Text.Json, SQLite, EF Core y xUnit; la referencia estable acordada para ASP.NET Core y EF Core es 8.0.34. La selección no implica dependencias instaladas ni contratos completos.

Se conserva el [protocolo por archivos](Protocolo-partida-archivos.md) como alternativa independiente. Sus locks, historiales y reglas de continuidad no se trasladan automáticamente al diseño web. La [consigna](Consigna.md) exige un archivo compartido: adoptar REST como sustituto para la entrega requiere resolver esa diferencia con el docente. Este borrador no modifica la consigna ni autoriza Taller.

## Organización propuesta

Dos clientes de consola se comunican con una API ASP.NET Core mediante HTTP y JSON. El servidor dirige el juego y es la única autoridad sobre cartas, turnos, resultados y marcadores. Cada cliente recibe exclusivamente la información que puede ver y devuelve las elecciones solicitadas.

EF Core administrará la persistencia en una base SQLite local al servidor, tanto del catálogo permanente de personajes (nombre, descripción, ASCII y seis atributos) como de los datos de partidas. Los clientes acceden exclusivamente a la API. La selección está acordada; el esquema, las transacciones y el mantenimiento del catálogo siguen pendientes.

El usuario dio por terminado el paso 2: dos computadoras Windows; la primera ejecuta servidor y cliente J1 como procesos separados, y la segunda cliente J2. Primero se abre el servidor y, cuando está disponible, los clientes. La comprobación de las instalaciones se posterga hasta disponer de los equipos; no se presenta como realizada.

La API y el dominio tienen responsabilidades distintas: la API identifica al jugador y valida el contrato recibido; el dominio decide si la elección es válida y resuelve el juego; la persistencia confirma los cambios. Los DTO de red no son entidades EF ni objetos vivos de Partida.

## Conexión acordada — paso 3

Para las pruebas en la red local se utiliza HTTP, puerto TCP 5000 y dirección del servidor configurable en cada cliente. Ejemplo ilustrativo: `http://192.168.1.20:5000`; la IP no está asignada ni comprobada. La ubicación concreta de esa configuración y su carga se completarán al especificar los contratos.

Localhost identifica la computadora del propio proceso: sirve para probar clientes y servidor en el mismo equipo. El cliente de otra computadora debe usar la dirección de red del servidor. Este debe aceptar conexiones en una interfaz accesible desde esa red, no limitarse a localhost; no hay configuración implementada que lo acredite.

Si el servidor no está disponible, el cliente informa y ofrece reintentar o salir. Conserva una elección pendiente y no la transforma en una operación nueva al reintentar. La confirmación de una elección cuya respuesta se perdió se resolverá mediante los contratos de operaciones e idempotencia de los pasos 5 y 7; no se asume que falló por un error de conexión.

HTTP no cifra el intercambio. Su elección se limita al entorno local de prueba acordado; no se define publicación en Internet. Primero se probará la comunicación para determinar si existe un bloqueo, sin asumir que sea necesario abrir el firewall. De requerirse una excepción en un equipo escolar, se coordinará con el responsable y se limitará al servicio, puerto y equipos participantes. No se requiere desactivar el firewall ni abrir puertos del router hacia Internet.

### Evidencia y pruebas postergadas

El usuario informó que dos procesos se comunican correctamente mediante localhost. Es una observación aportada por el usuario, sin comprobación reproducida por el agente; no acredita REST implementado en este repositorio ni conectividad entre equipos.

Por falta de acceso a las computadoras, quedan pendientes: verificar SDK y runtimes, IP real, disponibilidad de TCP 5000, escucha en red, conexión entre equipos, permisos y posibles bloqueos del firewall/red escolar. También se deben probar servidor ausente, pérdida de conexión y reintentos sin duplicación. Se conservan como tareas de verificación del plan, sin reabrir el cierre confirmado del paso 2. Los pasos REST mantienen su numeración 1 a 14.

## Identidad y sesiones

Propuesta: cada partida tiene un UUID; cada participante tiene un identificador y una credencial de reincorporación. El rol J1/J2 se asigna o reserva en el servidor, con mecanismo todavía por definir. El cuerpo enviado por el cliente no puede atribuirse libremente otro rol. La credencial se envía mediante un encabezado de autorización y no forma parte de las URL ni de las presentaciones.

Debe definirse quién administra el servidor, quién crea e inicia partidas, cómo se distribuyen las credenciales y cómo se evita ocupar dos veces un rol. El nombre visible no sirve como autenticación. Una instancia nueva del cliente no debe desplazar silenciosamente a otra conectada.

## Recursos y operaciones propuestas

Prefijo de versión: `/api/v1`. Las rutas representan recursos; los nombres y contratos son iniciales.

| Método y ruta | Finalidad | Resultado inicial propuesto |
|---|---|---|
| `POST /partidas` | Crear la partida desde la administración autorizada. | `201 Created`, identificación y cabecera Location. |
| `POST /partidas/{id}/participantes` | Incorporarse a una plaza disponible. | `201 Created`, identidad y credencial. |
| `POST /partidas/{id}/inicio` | Solicitar inicio explícito con ambos jugadores preparados. | `201 Created` al registrar el inicio. |
| `GET /partidas/{id}/presentacion` | Consultar la vista del participante autenticado. | `200 OK`, presentación completa. |
| `POST /partidas/{id}/elecciones` | Registrar la respuesta a una solicitud pendiente. | `201 Created`, recibo de aceptación. |
| `GET /partidas/{id}/elecciones/{idOperacion}` | Consultar el recibo propio tras perder una respuesta HTTP. | `200 OK` o `404 Not Found`. |
| `PUT /partidas/{id}/participantes/me/presencia` | Renovar la presencia del cliente autenticado. | `204 No Content`. |

La administración requiere permisos distintos de los de jugador. El acceso a recibos y presentaciones se limita a su titular. Crear recursos y registrar el inicio también requiere protección contra reintentos duplicados; su contrato de idempotencia debe completarse antes de implementar.

## Presentación y elecciones

La presentación propuesta contiene `idPartida`, `revision`, `fase`, rol y nombre propios, mano visible, marcador, resultado público y `solicitud` opcional. Una solicitud incluye `idSolicitud`, clase de entrada y opciones permitidas. No expone la mano rival ni su carta o atributo elegidos antes de la revelación. Los detalles exactos de cada DTO quedan por especificar.

Ejemplo ilustrativo de una elección, con identificadores de ejemplo:

```json
{
  "idOperacion": "fd222e81-d696-4c1a-95eb-6b5e5ce67f31",
  "idSolicitud": "9d421cf5-f1fe-4dc1-88b9-0bb11832dced",
  "revisionEsperada": 12,
  "idPersonaje": 7,
  "atributo": "Fuerza"
}
```

El rol procede de la identidad autenticada. El atributo solo se admite cuando la solicitud lo exige. El servidor verifica partida, solicitud vigente, propietario de la carta y opciones ofrecidas. Los nombres de atributos corresponden a los seis definidos en la consigna. El tratamiento del nombre del jugador y las demás clases de entrada debe completar este contrato.

`revisionEsperada` identifica la versión del juego vista por el cliente. La renovación de presencia no debe invalidar elecciones incrementando esa revisión. Falta definir con precisión qué transiciones cambian la revisión y si una modificación compatible permite conservar la solicitud.

## Secuencia de un combate

1. El servidor prepara una solicitud para quien elige carta y atributo. El rival recibe una presentación de espera.
2. El elector envía su elección. El servidor la valida y confirma antes de responder.
3. El servidor solicita la carta del rival sin revelar carta ni atributo del elector, conforme a las reglas documentadas.
4. Al confirmar la segunda elección, resuelve el combate y prepara el resultado público para ambos.
5. Los clientes consultan y muestran el resultado. La necesidad de confirmar su lectura antes de avanzar queda pendiente.

Se mantienen las [reglas del juego](Reglas-del-juego.md): alternancia del elector, desempate a favor de quien no eligió atributo, dos combates para ganar la ronda y quince rondas para ganar la partida. El protocolo no cambia las restricciones de personajes: total 3000, rango inclusivo 250–750 y umbrales 650/350.

## Consultas y presencia

Propuesta inicial: consultar la presentación cada segundo mientras se espera, con una sola consulta pendiente por cliente y timeout configurable. El intervalo se evaluará en red. Cada respuesta es una vista completa; el cliente puede comparar la revisión para evitar redibujar sin cambios. La captura de teclado no debe impedir las consultas de presencia.

HTTP no demuestra que un programa siga activo entre solicitudes. Se propone una renovación periódica de presencia y un plazo de vencimiento medido por el servidor; ambos valores quedan pendientes. Un timeout local tampoco prueba que el servidor haya cerrado.

Debe decidirse cuándo se pausa el juego, si se conserva el diálogo de espera cada cinco segundos y qué significa abandonar. No se puede aplicar directamente la regla anterior de «si todos cierran, la partida deja de ser recuperable»: el servidor web necesita una definición observable de sesión y abandono.

## Concurrencia, reintentos y persistencia

Las modificaciones de una misma partida deben aplicarse de forma serializada y confirmarse de manera atómica. Propuesta: guardar en una transacción el estado resultante, la solicitud consumida, la revisión y el recibo de la operación. El mecanismo concreto con EF Core/SQLite y su modelo de datos quedan pendientes.

El cliente genera `idOperacion` antes del primer envío y lo conserva durante los reintentos. Una misma operación del mismo participante y partida, con idéntico contenido, devuelve su recibo sin volver a consumir cartas ni sumar victorias. El mismo identificador con otro contenido produce conflicto. La comprobación de duplicados precede al rechazo por revisión antigua para poder recuperar un éxito previo.

Una operación nueva sobre una solicitud consumida o revisión incompatible se rechaza sin efectos. Un error de red no autoriza al cliente a asumir que la elección falló: consulta el recibo o reenvía la misma operación. El estado persistido es autoritativo; no se anuncia éxito antes de confirmar la transacción.

El reparto completo debe confirmarse antes de mostrar las manos. Tras reiniciar el servidor no se vuelve a mezclar una partida ya guardada. Falta definir retención de recibos, recuperación de credenciales y política de continuidad tras reinicios. La base conservaría catálogo utilizado, orden del mazo, manos, ronda, combates, solicitudes y marcadores; el esquema relacional frente a una captura JSON aún debe evaluarse. La importación inicial del CSV/DataBases también queda pendiente.

## Respuestas de error

| Estado HTTP | Uso propuesto |
|---|---|
| `400 Bad Request` | JSON inválido, campos obligatorios ausentes o formato incorrecto. |
| `401 Unauthorized` | Credencial ausente o inválida. |
| `403 Forbidden` | Identidad válida sin permiso para la operación. |
| `404 Not Found` | Recurso inexistente o no accesible conforme a la política de visibilidad. |
| `409 Conflict` | Turno, solicitud o revisión incompatible; operación reutilizada con otro contenido. |
| `422 Unprocessable Content` | Elección bien formada que incumple las opciones permitidas. |
| `503 Service Unavailable` | Servicio temporalmente incapaz de procesar la operación. |

Se propone un cuerpo uniforme con `codigo`, `mensaje` e identificador de seguimiento, sin detalles internos. El cliente conserva su entrada ante fallos de red y obtiene una presentación actualizada ante conflictos. Los errores no revelan elecciones ocultas. El contrato definitivo de errores y las reglas de reintento se revisarán conjuntamente.

## Pendientes de diseño y verificación

- Resolver la compatibilidad con la entrega; la pila tecnológica y net8.0 ya están confirmados. Verificar las instalaciones y las condiciones reales de ejecución cuando estén disponibles los equipos.
- Definir administración, identidad, incorporación, presencia, abandono y recuperación.
- Precisar DTO, rutas, respuestas, estados y transiciones, incluyendo inicio y fin de ronda/partida.
- Diseñar persistencia, transacciones e idempotencia de todas las operaciones que modifican datos.
- Actualizar los contratos de componentes y el UML para la alternativa web, sin presentar el UML de archivos como diseño REST.
- Diseñar casos de solicitudes simultáneas, respuestas perdidas, reintentos, vistas antiguas y cortes durante el guardado.
- Planificar pruebas en dos computadoras y revisión de compañero. Su ejecución técnica corresponde a una etapa autorizada; no está realizada.

## Referencias técnicas

- [Diseño de APIs REST — Microsoft](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design).
- [APIs con ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet/apis).
- [Proveedor SQLite para EF Core](https://learn.microsoft.com/es-es/ef/core/providers/sqlite/).

Estas referencias describen capacidades tecnológicas; no acreditan que el diseño propuesto esté implementado o probado.
