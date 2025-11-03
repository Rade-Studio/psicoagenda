# Guía de Arquitectura - PsicoAgenda

## Introducción

Esta guía explica cómo está organizado el proyecto PsicoAgenda. Si nunca has programado, imagina que construir software es como construir una casa: cada parte tiene su lugar y su función específica.

## ¿Qué es una Arquitectura en Capas?

La arquitectura en capas es como una cebolla: tiene varias capas concéntricas donde cada una tiene una responsabilidad específica. Las capas externas pueden usar las internas, pero las internas nunca usan las externas. Esto mantiene el código organizado y fácil de mantener.

```
┌─────────────────────────────────────┐
│   CAPA API (Más Externa)            │
│   Punto de entrada de la aplicación │
└──────────────┬──────────────────────┘
               │ usa
               ▼
┌─────────────────────────────────────┐
│   CAPA APPLICATION                   │
│   Reglas de negocio y transformación │
└──────────────┬──────────────────────┘
               │ usa
               ▼
┌─────────────────────────────────────┐
│   CAPA INFRASTRUCTURE                │
│   Implementación de servicios       │
└──────────────┬──────────────────────┘
               │ usa
               ▼
┌─────────────────────────────────────┐
│   CAPA PERSISTENCE                   │
│   Acceso a base de datos             │
└──────────────┬──────────────────────┘
               │ usa
               ▼
┌─────────────────────────────────────┐
│   CAPA DOMAIN (Más Interna)          │
│   Entidades y contratos base         │
└─────────────────────────────────────┘
```

## Las 5 Capas del Proyecto

### 1. Capa Domain (PsicoAgenda.Domain)

**¿Qué es?** Es el núcleo del proyecto, la capa más interna e importante. Contiene las "cosas reales" de tu negocio.

**¿Qué contiene?**
- **Models**: Las entidades del negocio (Paciente, Cita, Sesión, etc.)
- **Interfaces**: Contratos que definen qué operaciones se pueden hacer (sin especificar cómo)
- **Enums**: Valores fijos que pueden tener ciertos campos (como EstadoCita, ModoCita)

**Analogía**: Si fuera una empresa, esta capa sería como la "definición de qué es un empleado, un cliente, un producto" antes de pensar en cómo se manejan.

**Archivos importantes:**
- `Models/Paciente.cs` - Define qué es un paciente
- `Models/Cita.cs` - Define qué es una cita
- `Interfaces/IRepository.cs` - Define qué operaciones se pueden hacer con datos
- `Interfaces/IUnitOfWork.cs` - Define cómo se agrupan las operaciones de datos

**Regla importante**: Esta capa NO puede usar ninguna otra capa. Es independiente.

---

### 2. Capa Application (PsicoAgenda.Application)

**¿Qué es?** Aquí se define QUÉ hace la aplicación, pero no CÓMO lo hace específicamente.

**¿Qué contiene?**
- **Dtos**: Objetos de transferencia de datos. Son versiones "simplificadas" de las entidades para comunicarse con el exterior
- **Interfaces**: Define qué servicios existen (por ejemplo: IPacienteService)
- **Mappers**: Transforman datos de un formato a otro (por ejemplo: de Paciente a PacienteRespuesta)

**Analogía**: Es como el menú de un restaurante. Te dice qué platos hay disponibles, pero no te dice cómo se cocinan exactamente.

**Archivos importantes:**
- `Dtos/Pacientes/PacienteCreacion.cs` - Datos necesarios para crear un paciente
- `Dtos/Pacientes/PacienteRespuesta.cs` - Datos que se devuelven al consultar un paciente
- `Interfaces/IPacienteService.cs` - Define qué puede hacer el servicio de pacientes
- `Mappers/PacienteProfile.cs` - Convierte entre diferentes tipos de objetos

**Regla importante**: Esta capa puede usar Domain, pero NO puede usar Infrastructure, Persistence o Api.

---

### 3. Capa Infrastructure (PsicoAgenda.Infrastructure)

**¿Qué es?** Aquí se implementa el CÓMO. Es donde realmente se ejecuta la lógica de los servicios.

**¿Qué contiene?**
- **Services**: Implementaciones concretas de los servicios definidos en Application
- **Lógica de negocio**: La forma específica en que se procesan los datos

**Analogía**: Es como la cocina del restaurante. Aquí es donde realmente se cocinan los platos del menú.

**Archivos importantes:**
- `Services/PacienteService.cs` - Implementación real del servicio de pacientes
- `DependencyInjection.cs` - Registra los servicios para que estén disponibles

**Regla importante**: Esta capa puede usar Domain y Application, pero NO puede usar Api directamente.

---

### 4. Capa Persistence (PsicoAgenda.Persistence)

**¿Qué es?** Todo lo relacionado con guardar y recuperar información de la base de datos.

**¿Qué contiene?**
- **Context**: La conexión con la base de datos (AppDbContext)
- **Repositories**: Clases que manejan las operaciones de base de datos (crear, leer, actualizar, eliminar)
- **UnitOfWork**: Agrupa múltiples operaciones de base de datos para ejecutarlas como una sola transacción

**Analogía**: Es como el almacén de un negocio. Aquí es donde se guardan y se buscan las cosas.

**Archivos importantes:**
- `Context/AppDbContext.cs` - Configuración de la conexión a la base de datos
- `Repositories/Repository.cs` - Operaciones básicas de base de datos
- `UnitOfWork/UnitOfWorkManager.cs` - Maneja las transacciones de base de datos

**Regla importante**: Esta capa puede usar Domain, pero NO puede usar Application, Infrastructure o Api.

---

### 5. Capa Api (PsicoAgenda.Api)

**¿Qué es?** Es el punto de entrada de la aplicación. Es como la "cara" que ve el mundo exterior.

**¿Qué contiene?**
- **Controllers**: Reciben las peticiones HTTP (GET, POST, PUT, DELETE) y las procesan
- **Program.cs**: Configuración inicial de la aplicación (qué servicios usar, qué rutas hay, etc.)

**Analogía**: Es como la recepción de un hotel. Aquí llegan los clientes (peticiones HTTP) y se les dirige al lugar correcto.

**Archivos importantes:**
- `Controllers/PacientesController.cs` - Maneja las peticiones relacionadas con pacientes
- `Program.cs` - Configuración y arranque de la aplicación

**Regla importante**: Esta capa puede usar todas las demás capas.

---

## Diagrama Completo de Dependencias

```
┌─────────────────────────────────────────────────────────┐
│                      API                                │
│  • Recibe peticiones HTTP                              │
│  • Controllers                                          │
└────────────────┬────────────────────────────────────────┘
                 │ puede usar
                 ├──────────────────────────────────────────┐
                 │                                          │
                 ▼                                          ▼
┌──────────────────────────────────┐  ┌──────────────────────────────┐
│     APPLICATION                   │  │    INFRASTRUCTURE            │
│  • Define QUÉ hacer              │  │  • Implementa CÓMO hacer     │
│  • DTOs, Interfaces, Mappers     │  │  • Servicios concretos       │
└────────────────┬──────────────────┘  └────────────────┬─────────────┘
                 │                                       │
                 │ puede usar                            │ puede usar
                 │                                       │
                 ▼                                       ▼
┌──────────────────────────────────┐  ┌──────────────────────────────┐
│        DOMAIN                     │  │     PERSISTENCE              │
│  • Entidades del negocio         │  │  • Acceso a base de datos    │
│  • Contratos base                │  │  • Repositorios              │
│  • NO depende de nada            │  │  • Unit of Work              │
└──────────────────────────────────┘  └──────────────────────────────┘
                 ▲                           ▲
                 │                           │
                 └───────────────────────────┘
                    ambos usan DOMAIN
```

## Reglas de Dependencias (Muy Importante)

Para mantener el código organizado, hay reglas estrictas sobre qué capa puede usar qué:

1. **Domain**: No puede usar NADA. Es completamente independiente.
2. **Application**: Solo puede usar Domain.
3. **Infrastructure**: Puede usar Domain y Application.
4. **Persistence**: Solo puede usar Domain.
5. **Api**: Puede usar TODAS las demás capas.

**¿Por qué estas reglas?** Porque así el código es más fácil de cambiar. Si necesitas cambiar la base de datos, solo tocas Persistence. Si necesitas cambiar cómo se exponen los datos, solo tocas Api. Y así sucesivamente.

## Ejemplo Práctico: Consultar un Paciente

Imaginemos que alguien hace una petición para ver los datos de un paciente:

1. **Api (PacientesController)**: Recibe la petición HTTP GET
2. **Application (IPacienteService)**: El controller usa la interfaz para pedir el paciente
3. **Infrastructure (PacienteService)**: La implementación real busca el paciente usando...
4. **Persistence (UnitOfWork/Repository)**: ...que a su vez consulta...
5. **Persistence (AppDbContext)**: ...la base de datos PostgreSQL
6. **Application (Mapper)**: Los datos se transforman del formato de base de datos al formato de respuesta
7. **Api (Controller)**: Se devuelve la respuesta HTTP al cliente

Para más detalles sobre este flujo, consulta [FLUJO_EJECUCION.md](FLUJO_EJECUCION.md).

## Estructura de Carpetas del Proyecto

```
psicoagenda/
├── PsicoAgenda.Api/              ← Capa API
│   ├── Controllers/
│   └── Program.cs
│
├── PsicoAgenda.Application/      ← Capa Application
│   ├── Dtos/
│   ├── Interfaces/
│   └── Mappers/
│
├── PsicoAgenda.Infrastructure/  ← Capa Infrastructure
│   └── Services/
│
├── PsicoAgenda.Persistence/     ← Capa Persistence
│   ├── Context/
│   ├── Repositories/
│   └── UnitOfWork/
│
└── PsicoAgenda.Domain/           ← Capa Domain
    ├── Models/
    └── Interfaces/
```

## Resumen

- **Domain**: Define QUÉ son las cosas del negocio
- **Application**: Define QUÉ se puede hacer
- **Infrastructure**: Implementa CÓMO se hace
- **Persistence**: Maneja DÓNDE se guarda
- **Api**: Expone TODO al mundo exterior

Cada capa tiene su responsabilidad clara, lo que hace el código más fácil de entender, mantener y modificar.

## Próximos Pasos

- [Flujo de Ejecución](FLUJO_EJECUCION.md) - Ver cómo fluye una petición por todas las capas
- [Inyección de Dependencias](INYECCION_DEPENDENCIAS.md) - Entender cómo se conectan las piezas
- [Agregar una Funcionalidad](AGREGAR_FUNCIONALIDAD.md) - Guía paso a paso para agregar nuevas características
- [Referencias del Proyecto](REFERENCIAS_PROYECTO.md) - Glosario y términos técnicos

