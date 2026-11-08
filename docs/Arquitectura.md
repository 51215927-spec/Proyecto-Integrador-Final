# Arquitectura

[UML](diagramas/UML.uxf)

## Estado del diseño

El UML es una propuesta de análisis de TI; todavía no hay implementación de estas clases. La revisión del 8 de noviembre de 2026 y los pendientes se registran en [Plan](Plan.md).

El modelo separa los datos de Personaje y la consulta delegada desde Carta; Mazo administra la colección común y Jugador conserva sus cartas y marcador. Partida coordina Ronda y Combate. ControlTurno regula quién puede actuar; IInterfaz separa la interacción de consola y ArchivoPartida representa el intercambio de acciones mediante Accion. Estas responsabilidades se desprenden del diagrama y requieren completar su justificación y sus contratos.

Las reglas de empate a favor de quien no eligió, dos combates para ganar una ronda y 15 rondas para ganar una partida coinciden entre el avance revisado y el UML.

## Decisiones por cerrar

- Unificar los métodos del borrador con las firmas y responsabilidades del UML antes de especificarlos o implementarlos.
- Precisar el retorno de las cartas no jugadas al terminar una ronda anticipadamente y la formación del mazo inicial.
- El UML propone dos rutas de acciones (propia y rival), mientras la consigna pide un archivo de partida compartida. La cantidad de archivos y el protocolo necesitan resolución y revisión con SSI; no se consideran aprobados.
- Completar identidad de partida, detección de duplicados, sincronización, acceso concurrente y tratamiento de fallos. Las excepciones y los estados enumerados en el UML son un punto de partida, no un protocolo completo.
