# PROYECTO INTEGRADOR: Juego Guerra

## 3.º año – Informática

### Presentación

El proyecto integrador **Juego Guerra** propone desarrollar un juego que combine programación, análisis de sistemas, administración de archivos y diseño visual.
A lo largo del proyecto, trabajarán en las distintas etapas de construcción del juego, aplicando los conocimientos y las herramientas de cada materia. El objetivo es lograr un producto integrado en el que las diferentes partes funcionen de manera coordinada.

### Organización del proyecto

El proyecto se desarrolla de manera articulada entre cuatro materias:

- **TI — Análisis de sistemas:** Definición del problema, análisis de los requerimientos y modelado de la solución.
- **Taller de Programación:** Diseño e implementación de la lógica del juego mediante POO (C#).

---

## 🟢 TI — Análisis de Sistemas

### 1. Propósito

Transformar una descripción del problema en una solución documentada, definen las reglas del juego, la información a administrar, las responsabilidades de sus componentes y las condiciones de verificación.

### 2. Actividad 1 — Análisis y diseño de los personajes

Cada alumno deberá analizar las características de su personaje y definir la distribución de puntos:

- **Seis atributos:** Fuerza, Velocidad, Resistencia, Inteligencia, Energía y Habilidad.

- **Restricciones de puntaje:**
  - La suma exacta debe ser **3.000 puntos**.
  - Cada atributo tendrá un mínimo de **250** y un máximo de **750 puntos**.
  - Al menos un atributo deberá tener **650 puntos o más**.
  - Al menos un atributo deberá tener **350 puntos o menos**.
  - **No** se permite asignar 500 puntos a todos los atributos.

**Entregable:** Ficha de análisis del personaje con los seis valores, el total, el atributo principal, el atributo de menor valor y la justificación.

### 3. Actividad 2 — Relevamiento de requerimientos

Documentar el objetivo del juego, participantes, reglas, administración de cartas/mazos, condiciones de victoria, comunicación en red local y manejo de errores.

**Entregable:** Documento de requerimientos y reglas del juego.

### 4. Actividad 3 — Modelo de clases

Diseñar el diagrama de clases UML contemplando la representación de personajes, cartas, mazos y administración de la partida.

**Entregable:** Diagrama de clases UML y justificación de diseño.

### 5. Actividad 4 — Especificación de métodos

Documentar el comportamiento esperado de cada método (parámetros, retornos, precondiciones, validaciones, diagramas Nassi-Shneiderman).

**Entregable:** Especificaciones de métodos y diagramas correspondientes.

### 6. Actividad 5 — Formato de los archivos de personajes

Definir la estructura del archivo de texto que contendrá los datos del personaje para ser interpretado por el programa.

**Entregable:** Especificación del formato de archivo con ejemplos válidos e inválidos.

### 7. Actividad 6 — Archivo de partida compartida

Definir la estructura y dinámica del archivo compartido en la red local para coordinar la partida entre dos computadoras.

**Entregable:** Especificación del contenido y funcionamiento del archivo compartido.

### 8. Actividad 7 — Casos de prueba

Diseñar pruebas de caja negra y caja blanca contemplando casos normales, límites y de error.

**Entregable:** Plan de pruebas con resultados esperados.

---

## ⚫ Taller de Programación

### Etapas de Implementación

1. **Etapa 1 — Implementación de Personaje:** Clase `Personaje` con atributos y representación ASCII Art.
2. **Etapa 2 — Lectura de archivos de personajes:** Carga, parsing y validación de archivos de texto.
3. **Etapa 3 — Implementación de Carta:** Clase `Carta` vinculada a `Personaje` mediante composición.
4. **Etapa 4 — Administración del mazo:** Mezclar, repartir, consultar y administrar cartas.
5. **Etapa 5 — Implementación de las reglas:** Comparación de atributos, turnos, lógica de enfrentamiento y victoria.
6. **Etapa 6 — Comunicación entre computadoras:** Sincronización de la partida leyendo/escribiendo el archivo compartido en red local.
7. **Etapa 7 — Integración:** Unificación de componentes en una aplicación funcional de consola.

---
