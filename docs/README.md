# Documentación Técnica - PsicoAgenda

## Bienvenido

Esta documentación está diseñada para explicar el proyecto PsicoAgenda de forma clara y accesible, incluso si no tienes experiencia en programación. Cada documento usa analogías del mundo real y evita jerga técnica innecesaria.

## ¿Por Dónde Empezar?

### Si eres nuevo en el proyecto

1. **Empieza aquí**: [Guía de Arquitectura](GUIA_ARQUITECTURA.md)
   - Entiende cómo está organizado el proyecto
   - Aprende sobre las capas y sus responsabilidades

2. **Luego**: [Flujo de Ejecución](FLUJO_EJECUCION.md)
   - Ve cómo funciona una petición de principio a fin
   - Entiende cómo interactúan las capas

3. **Después**: [Inyección de Dependencias](INYECCION_DEPENDENCIAS.md)
   - Aprende cómo se conectan las piezas del sistema
   - Entiende por qué se usan interfaces

4. **Opcional**: [UnitOfWork y Repository](UNITOFWORK_REPOSITORY.md)
   - Entiende cómo se manejan los datos y transacciones
   - Ve cómo se relacionan estos patrones

### Si quieres agregar funcionalidad

1. **Sigue la guía**: [Agregar una Funcionalidad](AGREGAR_FUNCIONALIDAD.md)
   - Proceso paso a paso con ejemplo completo
   - Checklist y buenas prácticas

### Si necesitas consultar términos

- **Glosario**: [Referencias del Proyecto](REFERENCIAS_PROYECTO.md)
  - Definiciones de términos técnicos
  - Patrones de diseño utilizados
  - Tecnologías y librerías

---

## Documentos Disponibles

### 📚 [Guía de Arquitectura](GUIA_ARQUITECTURA.md)

**¿Qué encontrarás?**
- Explicación de la arquitectura en capas (Clean Architecture)
- Descripción detallada de cada capa:
  - Domain (núcleo del negocio)
  - Application (lógica de aplicación)
  - Infrastructure (implementaciones)
  - Persistence (acceso a datos)
  - Api (punto de entrada)
- Diagramas visuales de la estructura
- Reglas de dependencias entre capas

**Cuándo leerlo**: Si necesitas entender la estructura general del proyecto.

---

### 🔄 [Flujo de Ejecución](FLUJO_EJECUCION.md)

**¿Qué encontrarás?**
- Flujo completo desde una petición HTTP hasta la respuesta
- Ejemplo real paso a paso: GET /api/pacientes/{id}
- Qué archivos se ejecutan en cada paso
- Diagrama visual del flujo completo
- Explicación de cada transformación de datos

**Cuándo leerlo**: Si quieres entender qué sucede cuando se hace una petición al sistema.

---

### 🔌 [Inyección de Dependencias](INYECCION_DEPENDENCIAS.md)

**¿Qué encontrarás?**
- ¿Qué es la inyección de dependencias? (explicado simple)
- ¿Por qué usar interfaces en lugar de clases directas?
- Cómo funciona en este proyecto
- Ejemplos prácticos con código comentado
- Dónde se registran las dependencias
- Tipos de ciclo de vida (Scoped, Singleton, Transient)
- Ventajas: testabilidad, flexibilidad, mantenibilidad

**Cuándo leerlo**: Si necesitas entender cómo se conectan las piezas del sistema o por qué se usan interfaces.

---

### 🗂️ [UnitOfWork y Repository](UNITOFWORK_REPOSITORY.md)

**¿Qué encontrarás?**
- ¿Qué son los patrones UnitOfWork y Repository?
- Cómo funcionan en el proyecto
- Relación entre Repository y UnitOfWork
- Ejemplos prácticos paso a paso
- Cuándo usar transacciones
- Diferencias clave entre ambos patrones
- Buenas prácticas y errores comunes

**Cuándo leerlo**: Si necesitas entender cómo se accede y guarda la información en la base de datos, o cómo funcionan las transacciones.

---

### ➕ [Agregar una Funcionalidad](AGREGAR_FUNCIONALIDAD.md)

**¿Qué encontrarás?**
- Orden paso a paso para agregar nueva funcionalidad
- Checklist completo de archivos a crear/modificar
- Ejemplo completo: implementación del módulo de Citas
- Código comentado línea por línea
- Buenas prácticas y convenciones
- Errores comunes y cómo solucionarlos

**Cuándo leerlo**: Cuando necesites agregar una nueva característica al sistema.

---

### 📖 [Referencias del Proyecto](REFERENCIAS_PROYECTO.md)

**¿Qué encontrarás?**
- Glosario completo de términos técnicos:
  - Entidad, DTO, Repository, UnitOfWork, Service, Controller
  - Interface, Mapper, DbContext, Enum, Dependency Injection
- Explicación de patrones de diseño:
  - Repository Pattern
  - Unit of Work Pattern
  - Dependency Injection Pattern
  - Clean Architecture
- Tecnologías utilizadas:
  - .NET, ASP.NET Core
  - Entity Framework Core
  - AutoMapper
  - PostgreSQL
- Estructura de carpetas explicada
- Convenciones de nombres
- Conceptos de base de datos

**Cuándo leerlo**: Cuando encuentres un término técnico que no entiendes o necesites una referencia rápida.

---

## Ruta de Aprendizaje Recomendada

### Para Entender el Proyecto (30-45 minutos)

1. **15 min** - [Guía de Arquitectura](GUIA_ARQUITECTURA.md)
   - Lee las secciones de cada capa
   - Revisa los diagramas

2. **20 min** - [Flujo de Ejecución](FLUJO_EJECUCION.md)
   - Sigue el ejemplo paso a paso
   - Revisa el diagrama del flujo

3. **10 min** - [Inyección de Dependencias](INYECCION_DEPENDENCIAS.md)
   - Lee las secciones principales
   - Entiende el concepto básico

### Para Agregar Funcionalidad (1-2 horas)

1. **15 min** - Revisa [Agregar una Funcionalidad](AGREGAR_FUNCIONALIDAD.md)
   - Lee el checklist completo
   - Revisa el ejemplo de Citas

2. **1-2 horas** - Implementa tu funcionalidad
   - Sigue el checklist paso a paso
   - Consulta [Referencias](REFERENCIAS_PROYECTO.md) si encuentras términos desconocidos

### Para Consulta Rápida

- **Referencias del Proyecto](REFERENCIAS_PROYECTO.md)** - Búsqueda rápida de términos
- **Agregar Funcionalidad](AGREGAR_FUNCIONALIDAD.md)** - Checklist rápido

---

## Estructura de la Documentación

```
docs/
├── README.md                    ← Estás aquí (índice)
├── GUIA_ARQUITECTURA.md         ← Estructura del proyecto
├── FLUJO_EJECUCION.md           ← Cómo funciona una petición
├── INYECCION_DEPENDENCIAS.md    ← Cómo se conectan las piezas
├── UNITOFWORK_REPOSITORY.md     ← Patrones de acceso a datos
├── AGREGAR_FUNCIONALIDAD.md     ← Guía para agregar features
└── REFERENCIAS_PROYECTO.md      ← Glosario y términos
```

---

## Convenciones Usadas en Esta Documentación

### Iconos

- 📚 **Documentos principales** - Guías completas
- 🔄 **Flujos** - Cómo funciona algo
- 🔌 **Conceptos técnicos** - Explicaciones de patrones
- 🗂️ **Patrones de datos** - Acceso a datos y transacciones
- ➕ **Guías prácticas** - Cómo hacer algo
- 📖 **Referencias** - Consultas rápidas

### Formato

- **Código en bloques**: Ejemplos de código real del proyecto
- **Analogías**: Explicaciones usando situaciones del mundo real
- **Diagramas ASCII**: Visualizaciones simples en texto
- **Checklists**: Listas de verificación para seguir

---

## Preguntas Frecuentes

### ¿Necesito saber programar para entender esta documentación?

No necesariamente. Esta documentación está escrita para personas sin conocimientos de programación, usando analogías y explicaciones simples. Sin embargo, tener conocimientos básicos ayudará.

### ¿Dónde empiezo si solo quiero entender el proyecto?

Empieza con la [Guía de Arquitectura](GUIA_ARQUITECTURA.md) y luego el [Flujo de Ejecución](FLUJO_EJECUCION.md).

### ¿Dónde empiezo si quiero agregar una funcionalidad?

Ve directamente a [Agregar una Funcionalidad](AGREGAR_FUNCIONALIDAD.md) y sigue el checklist.

### ¿Qué hago si encuentro un término que no entiendo?

Consulta el [Glosario en Referencias del Proyecto](REFERENCIAS_PROYECTO.md).

### ¿La documentación está completa?

Esta documentación cubre los conceptos fundamentales. A medida que el proyecto crezca, se pueden agregar más ejemplos y casos específicos.

---

## Contribuir a la Documentación

Si encuentras algo confuso o faltante en la documentación:

1. Identifica qué documento necesita mejora
2. Sugiere cambios específicos (ej: "necesito más ejemplos en la sección X")
3. Proporciona feedback sobre qué no quedó claro

La documentación debe ser útil para personas sin conocimientos técnicos previos.

---

## Recursos Adicionales

### Documentación Oficial (Avanzado)

Si ya entiendes los conceptos básicos y quieres profundizar:

- [Documentación de .NET](https://learn.microsoft.com/dotnet/)
- [Documentación de ASP.NET Core](https://learn.microsoft.com/aspnet/core)
- [Entity Framework Core](https://learn.microsoft.com/ef/core)
- [AutoMapper](https://docs.automapper.org/)

### Conceptos Relacionados

- **Clean Architecture**: Arquitectura en capas
- **SOLID Principles**: Principios de diseño orientado a objetos
- **Repository Pattern**: Patrón de acceso a datos
- **Dependency Injection**: Inyección de dependencias

---

## Contacto y Soporte

Para preguntas sobre:
- **Arquitectura del proyecto**: Revisa [Guía de Arquitectura](GUIA_ARQUITECTURA.md)
- **Cómo funciona algo**: Revisa [Flujo de Ejecución](FLUJO_EJECUCION.md)
- **Acceso a datos**: Revisa [UnitOfWork y Repository](UNITOFWORK_REPOSITORY.md)
- **Términos técnicos**: Revisa [Referencias del Proyecto](REFERENCIAS_PROYECTO.md)
- **Agregar funcionalidad**: Revisa [Agregar una Funcionalidad](AGREGAR_FUNCIONALIDAD.md)

---

## Próximos Pasos

1. **Si eres nuevo**: Empieza con [Guía de Arquitectura](GUIA_ARQUITECTURA.md)
2. **Si quieres entender el flujo**: Lee [Flujo de Ejecución](FLUJO_EJECUCION.md)
3. **Si quieres entender acceso a datos**: Lee [UnitOfWork y Repository](UNITOFWORK_REPOSITORY.md)
4. **Si quieres agregar código**: Sigue [Agregar una Funcionalidad](AGREGAR_FUNCIONALIDAD.md)
5. **Si tienes dudas de términos**: Consulta [Referencias del Proyecto](REFERENCIAS_PROYECTO.md)

---

**¡Bienvenido a PsicoAgenda!** 🧠

Esta documentación está aquí para ayudarte. No dudes en consultarla cuando tengas dudas.

