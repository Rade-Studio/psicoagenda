# Referencias y Glosario del Proyecto

## Introducción

Este documento contiene un glosario de términos técnicos, explicaciones de patrones de diseño, tecnologías utilizadas y la estructura del proyecto. Todo explicado de forma simple para personas sin conocimientos técnicos previos.

---

## Glosario de Términos Técnicos

### Entidad (Entity)

**¿Qué es?** Una entidad es la representación de un "objeto real" del negocio en código.

**Ejemplo**: `Paciente` es una entidad que representa a un paciente real en el sistema.

**Archivo ejemplo**: `PsicoAgenda.Domain/Models/Paciente.cs`

**Características**:
- Tiene propiedades que describen sus características
- Tiene un identificador único (ID)
- Se guarda en la base de datos
- Hereda de `EntidadBase` para tener campos comunes (Id, FechaCreacion, etc.)

---

### DTO (Data Transfer Object)

**¿Qué es?** Un objeto que se usa SOLO para transferir datos entre capas. No contiene lógica de negocio.

**¿Por qué existe?** Para separar la estructura interna (entidad) de la estructura externa (lo que se expone).

**Ejemplo**: 
- `Paciente` (entidad) tiene `PrimerNombre` y `SegundoNombre`
- `PacienteRespuesta` (DTO) tiene `Nombre` y `Apellidos` (formato más amigable)

**Archivos ejemplo**: 
- `PsicoAgenda.Application/Dtos/Pacientes/PacienteCreacion.cs`
- `PsicoAgenda.Application/Dtos/Pacientes/PacienteRespuesta.cs`

**Tipos comunes**:
- **Creación** (`PacienteCreacion`): Datos necesarios para crear algo
- **Respuesta** (`PacienteRespuesta`): Datos que se devuelven al consultar algo

---

### Repository (Repositorio)

**¿Qué es?** Una clase que maneja todas las operaciones de base de datos para una entidad específica.

**¿Qué hace?**
- Crear registros (INSERT)
- Leer registros (SELECT)
- Actualizar registros (UPDATE)
- Eliminar registros (DELETE)

**Analogía**: Es como un bibliotecario que sabe dónde está cada libro y cómo buscarlo.

**Archivo ejemplo**: `PsicoAgenda.Persistence/Repositories/Repository.cs`

**Patrón**: Repository Pattern - Separa la lógica de acceso a datos del resto del código.

---

### Unit of Work (Unidad de Trabajo)

**¿Qué es?** Un patrón que agrupa múltiples operaciones de base de datos para ejecutarlas como una sola unidad.

**¿Por qué se usa?**
- Permite hacer múltiples cambios y guardarlos todos juntos
- Si algo falla, se puede cancelar todo (rollback)
- Mantiene consistencia de datos

**Analogía**: Es como un carrito de compras. Agregas varios productos y al final pagas todo junto. Si cambias de opinión, puedes cancelar toda la compra.

**Archivo ejemplo**: `PsicoAgenda.Persistence/UnitOfWork/UnitOfWorkManager.cs`

**Operaciones típicas**:
- `GuardarCambios()`: Guarda todos los cambios pendientes
- `IniciarTransaccion()`: Inicia una transacción (todo o nada)
- `ConfirmarTransaccion()`: Confirma los cambios
- `CancelarTransaccion()`: Cancela los cambios si algo salió mal

---

### Service (Servicio)

**¿Qué es?** Una clase que contiene la lógica de negocio. Coordina entre diferentes partes del sistema.

**¿Qué hace?**
- Aplica reglas de negocio
- Valida datos
- Orquestra operaciones entre repositorios
- Transforma datos usando mappers

**Analogía**: Es como el gerente de un negocio. Coordina entre diferentes departamentos para completar una tarea.

**Archivos ejemplo**: 
- Interfaz: `PsicoAgenda.Application/Interfaces/IPacienteService.cs`
- Implementación: `PsicoAgenda.Infrastructure/Services/PacienteService.cs`

---

### Controller (Controlador)

**¿Qué es?** Una clase que recibe las peticiones HTTP y las procesa.

**¿Qué hace?**
- Recibe peticiones HTTP (GET, POST, PUT, DELETE)
- Valida los datos de entrada
- Llama a los servicios apropiados
- Formatea las respuestas HTTP

**Analogía**: Es como la recepción de un hotel. Recibe a los huéspedes (peticiones) y los dirige al lugar correcto (servicios).

**Archivo ejemplo**: `PsicoAgenda.Api/Controllers/PacientesController.cs`

---

### Interface (Interfaz)

**¿Qué es?** Un contrato que define QUÉ debe poder hacer una clase, pero NO cómo lo hace.

**¿Por qué se usa?**
- Permite cambiar implementaciones fácilmente
- Facilita las pruebas (puedes crear implementaciones "falsas")
- Desacopla el código

**Analogía**: Es como el menú de un restaurante. Te dice qué platos hay, pero no las recetas.

**Archivo ejemplo**: `PsicoAgenda.Application/Interfaces/IPacienteService.cs`

**Características**:
- Solo contiene firmas de métodos (nombre, parámetros, tipo de retorno)
- NO contiene código real
- Una clase "implementa" una interfaz cuando tiene todos los métodos definidos

---

### Mapper (Mapeador)

**¿Qué es?** Una clase que transforma objetos de un tipo a otro.

**¿Por qué se usa?**
- Transforma entidades del dominio a DTOs
- Evita escribir código repetitivo de transformación
- Mantiene la lógica de transformación centralizada

**Herramienta**: AutoMapper - Librería que hace esto automáticamente.

**Archivo ejemplo**: `PsicoAgenda.Application/Mappers/PacienteProfile.cs`

**Ejemplo de transformación**:
```
Paciente (entidad)          →    PacienteRespuesta (DTO)
────────────────────              ────────────────────
PrimerNombre: "Ana"        →    Nombre: "Ana"
SegundoNombre: "Ramírez"   →    Apellidos: "Ramírez"
```

---

### DbContext (Contexto de Base de Datos)

**¿Qué es?** Una clase que representa la conexión y configuración con la base de datos.

**¿Qué hace?**
- Define qué tablas existen (DbSet)
- Configura cómo se mapean las entidades a tablas
- Ejecuta consultas SQL
- Maneja transacciones

**Herramienta**: Entity Framework Core

**Archivo ejemplo**: `PsicoAgenda.Persistence/Context/AppDbContext.cs`

**Componentes principales**:
- `DbSet<T>`: Representa una tabla en la base de datos
- `OnModelCreating()`: Configura cómo se mapean las entidades

---

### Enum (Enumeración)

**¿Qué es?** Un tipo de dato que define un conjunto fijo de valores posibles.

**¿Por qué se usa?**
- Evita usar strings que pueden tener errores ("Pendiente" vs "pendiente" vs "PENDIENTE")
- El compilador ayuda a detectar errores
- Hace el código más claro y seguro

**Ejemplo**: `EstadoCita` puede ser: Pendiente, Confirmada, Cancelada, Completada, NoAsistio

**Archivos ejemplo**: 
- `PsicoAgenda.Domain/Enums/EstadoCita.cs`
- `PsicoAgenda.Domain/Enums/ModoCita.cs`

---

### Dependency Injection (Inyección de Dependencias)

**¿Qué es?** Un patrón donde las dependencias (objetos que una clase necesita) se proporcionan desde afuera en lugar de crearlas internamente.

**Ventajas**:
- Facilita las pruebas
- Permite cambiar implementaciones fácilmente
- Desacopla el código

**Ver más**: [Inyección de Dependencias](INYECCION_DEPENDENCIAS.md)

---

### CancellationToken

**¿Qué es?** Un objeto que permite cancelar operaciones asíncronas.

**¿Por qué se usa?**
- Si el cliente cancela la petición, se puede detener la operación
- Evita desperdiciar recursos
- Mejora la experiencia del usuario

**Ejemplo**: Si un usuario hace una petición pero cierra el navegador, la operación se cancela automáticamente.

---

## Patrones de Diseño Utilizados

### Repository Pattern

**¿Qué es?** Un patrón que abstrae el acceso a datos. En lugar de acceder directamente a la base de datos, se usa un repositorio.

**Ventajas**:
- Código más limpio y organizado
- Fácil de cambiar la fuente de datos
- Fácil de probar (puedes crear repositorios "falsos")

**Implementación**: `PsicoAgenda.Persistence/Repositories/Repository.cs`

---

### Unit of Work Pattern

**¿Qué es?** Un patrón que agrupa múltiples operaciones de repositorio en una sola transacción.

**Ventajas**:
- Mantiene consistencia de datos
- Permite rollback si algo falla
- Centraliza el guardado de cambios

**Implementación**: `PsicoAgenda.Persistence/UnitOfWork/UnitOfWorkManager.cs`

---

### Dependency Injection Pattern

**¿Qué es?** Un patrón donde las dependencias se proporcionan externamente.

**Ventajas**:
- Desacoplamiento
- Testabilidad
- Flexibilidad

**Implementación**: Sistema nativo de .NET Core

**Ver más**: [Inyección de Dependencias](INYECCION_DEPENDENCIAS.md)

---

### Clean Architecture / Onion Architecture

**¿Qué es?** Una arquitectura en capas donde las capas internas no dependen de las externas.

**Ventajas**:
- Código mantenible
- Fácil de probar
- Independencia de frameworks

**Implementación**: Capas Domain → Application → Infrastructure → Persistence → Api

**Ver más**: [Guía de Arquitectura](GUIA_ARQUITECTURA.md)

---

## Tecnologías y Librerías

### .NET

**¿Qué es?** Una plataforma de desarrollo de Microsoft para crear aplicaciones.

**Versión utilizada**: .NET (la más reciente)

**Usos en el proyecto**:
- Framework base para la aplicación
- Lenguaje C# para escribir código
- Sistema de inyección de dependencias

---

### ASP.NET Core

**¿Qué es?** Un framework para crear APIs web y aplicaciones web.

**Usos en el proyecto**:
- Crear controladores REST
- Manejar peticiones HTTP
- Configurar middleware

---

### Entity Framework Core

**¿Qué es?** Un ORM (Object-Relational Mapping) que permite trabajar con bases de datos usando objetos de C# en lugar de SQL directo.

**Ventajas**:
- No necesitas escribir SQL manualmente
- Type-safe (el compilador detecta errores)
- Migrations automáticas

**Usos en el proyecto**:
- `AppDbContext` para conectarse a la base de datos
- Repositorios usan EF Core para consultas
- Migrations para crear/actualizar esquema de BD

---

### AutoMapper

**¿Qué es?** Una librería que mapea objetos de un tipo a otro automáticamente.

**Ventajas**:
- Reduce código repetitivo
- Centraliza la lógica de mapeo
- Fácil de mantener

**Usos en el proyecto**:
- Transformar entidades a DTOs
- Transformar DTOs a entidades
- Archivos Profile definen las reglas

---

### PostgreSQL

**¿Qué es?** Una base de datos relacional (RDBMS) de código abierto.

**Características**:
- Relacional: datos organizados en tablas
- SQL: usa el lenguaje SQL estándar
- Robusta: usada en producción por muchas empresas

**Usos en el proyecto**:
- Almacenar todas las entidades (Pacientes, Citas, Sesiones, etc.)
- Entity Framework Core se conecta a PostgreSQL

---

## Estructura de Carpetas Explicada

```
psicoagenda/
│
├── PsicoAgenda.Domain/           ← Capa más interna
│   ├── Models/                   → Entidades del negocio
│   │   ├── Paciente.cs
│   │   ├── Cita.cs
│   │   └── EntidadBase.cs       → Clase base con Id y fechas
│   │
│   ├── Interfaces/               → Contratos base
│   │   ├── IRepository.cs       → Operaciones de datos genéricas
│   │   └── IUnitOfWork.cs       → Unidad de trabajo
│   │
│   └── Enums/                    → Valores fijos
│       ├── EstadoCita.cs
│       └── ModoCita.cs
│
├── PsicoAgenda.Application/      ← Lógica de aplicación
│   ├── Dtos/                     → Objetos de transferencia
│   │   └── Pacientes/
│   │       ├── PacienteCreacion.cs
│   │       └── PacienteRespuesta.cs
│   │
│   ├── Interfaces/              → Contratos de servicios
│   │   └── IPacienteService.cs
│   │
│   ├── Mappers/                 → Transformaciones
│   │   └── PacienteProfile.cs
│   │
│   └── DependencyInjection.cs   → Registro de servicios Application
│
├── PsicoAgenda.Infrastructure/  ← Implementaciones
│   ├── Services/                → Servicios concretos
│   │   └── PacienteService.cs
│   │
│   └── DependencyInjection.cs  → Registro de servicios Infrastructure
│
├── PsicoAgenda.Persistence/     ← Acceso a datos
│   ├── Context/                 → Conexión BD
│   │   └── AppDbContext.cs
│   │
│   ├── Repositories/           → Acceso a datos
│   │   └── Repository.cs
│   │
│   ├── UnitOfWork/             → Unidad de trabajo
│   │   └── UnitOfWorkManager.cs
│   │
│   ├── Migrations/              → Cambios de esquema BD
│   │   └── [archivos de migración]
│   │
│   └── DependencyInjection.cs → Registro de Persistence
│
└── PsicoAgenda.Api/            ← Capa más externa
    ├── Controllers/            → Endpoints HTTP
    │   ├── PacientesController.cs
    │   └── HealthController.cs
    │
    ├── Program.cs              → Configuración y arranque
    └── appsettings.json        → Configuración (conexiones, etc.)
```

---

## Convenciones de Nombres

### Clases
- **Singular y PascalCase**: `Paciente`, `Cita`, `PacienteService`
- **Controllers en plural**: `PacientesController`, `CitasController`

### Interfaces
- **Prefijo "I"**: `IPacienteService`, `IRepository`, `IUnitOfWork`

### Archivos
- **Un archivo por clase**: `Paciente.cs` contiene la clase `Paciente`
- **Carpetas por funcionalidad**: `Dtos/Pacientes/`, `Services/`

### DTOs
- **Creación**: `PacienteCreacion`, `CitaCreacion`
- **Respuesta**: `PacienteRespuesta`, `CitaRespuesta`

### Métodos
- **PascalCase**: `ObtenerPaciente`, `CrearCita`
- **Verbos descriptivos**: `Obtener`, `Crear`, `Actualizar`, `Eliminar`

---

## Tipos de Datos Comunes

### Guid

**¿Qué es?** Un identificador único global (Global Unique Identifier).

**Ejemplo**: `11111111-1111-1111-1111-111111111111`

**¿Por qué se usa?**
- Único a nivel mundial
- No se puede adivinar fácilmente
- No requiere una secuencia de base de datos

**Uso en el proyecto**: IDs de entidades (Paciente.Id, Cita.Id)

---

### DateTime

**¿Qué es?** Un tipo de dato que representa fecha y hora.

**Ejemplo**: `1995-05-10 14:30:00`

**Uso en el proyecto**: Fechas de nacimiento, fechas de citas, fechas de auditoría

---

### string?

**¿Qué es?** Un string que puede ser `null` (opcional).

**¿Por qué el "?"?** Indica que el valor es opcional y puede no tener valor.

**Uso en el proyecto**: Campos opcionales como `SegundoNombre`, `Email`, `Notas`

---

## Conceptos de Base de Datos

### Tabla

**¿Qué es?** Una estructura que almacena datos en filas y columnas.

**Analogía**: Como una hoja de cálculo Excel.

**Ejemplo**: Tabla `Pacientes` tiene columnas: Id, PrimerNombre, Email, etc.

---

### Primary Key (Clave Primaria)

**¿Qué es?** Un campo único que identifica cada fila.

**Ejemplo**: `Id` en la tabla `Pacientes`

**Características**:
- Debe ser único
- No puede ser nulo
- Una tabla tiene solo una clave primaria

---

### Foreign Key (Clave Foránea)

**¿Qué es?** Un campo que referencia a la clave primaria de otra tabla.

**Ejemplo**: `PacienteId` en la tabla `Citas` referencia al `Id` de `Pacientes`

**Propósito**: Establecer relaciones entre tablas

---

### Migración

**¿Qué es?** Un archivo que describe cambios en el esquema de la base de datos.

**¿Por qué se usa?**
- Versiona los cambios de BD
- Se puede aplicar o revertir fácilmente
- Permite que todos trabajen con el mismo esquema

**Ejemplo**: Crear tabla `Citas`, agregar columna `Email` a `Pacientes`

---

## Resumen de Conceptos Clave

| Concepto | ¿Qué es? | Archivo Ejemplo |
|----------|----------|----------------|
| **Entidad** | Objeto del negocio | `Domain/Models/Paciente.cs` |
| **DTO** | Objeto de transferencia | `Application/Dtos/Pacientes/` |
| **Repository** | Acceso a datos | `Persistence/Repositories/Repository.cs` |
| **UnitOfWork** | Unidad de transacción | `Persistence/UnitOfWork/UnitOfWorkManager.cs` |
| **Service** | Lógica de negocio | `Infrastructure/Services/PacienteService.cs` |
| **Controller** | Endpoint HTTP | `Api/Controllers/PacientesController.cs` |
| **Interface** | Contrato | `Application/Interfaces/IPacienteService.cs` |
| **Mapper** | Transformación | `Application/Mappers/PacienteProfile.cs` |
| **DbContext** | Conexión BD | `Persistence/Context/AppDbContext.cs` |

---

## Recursos Adicionales

- [Guía de Arquitectura](GUIA_ARQUITECTURA.md) - Entender las capas
- [Flujo de Ejecución](FLUJO_EJECUCION.md) - Cómo fluye una petición
- [Inyección de Dependencias](INYECCION_DEPENDENCIAS.md) - Cómo se conectan las piezas
- [Agregar Funcionalidad](AGREGAR_FUNCIONALIDAD.md) - Guía paso a paso

---

## Notas Finales

Este glosario está diseñado para ser una referencia rápida. Si encuentras un término que no está aquí o necesitas más explicación, consulta los otros documentos de la guía o pregunta al equipo de desarrollo.

