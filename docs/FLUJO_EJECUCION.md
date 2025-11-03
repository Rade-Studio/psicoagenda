# Flujo de Ejecución - Cómo Funciona una Petición

## Introducción

Este documento explica paso a paso qué sucede cuando alguien hace una petición a la aplicación. Usaremos un ejemplo real: **obtener los datos de un paciente por su ID**.

## Ejemplo: GET /api/pacientes/{id}

Imagina que un usuario quiere ver la información de un paciente específico. Hace una petición HTTP GET a la URL: `GET /api/pacientes/11111111-1111-1111-1111-111111111111`

## Diagrama General del Flujo

```
Cliente (Navegador/App)
    │
    │ HTTP GET /api/pacientes/{id}
    ▼
┌─────────────────────────────────────────┐
│ 1. API - PacientesController            │ ← Recibe la petición
│    • Valida la URL y parámetros        │
│    • Llama al servicio                 │
└────────────────┬────────────────────────┘
                 │
                 │ _pacienteService.ObtenerPaciente(id)
                 ▼
┌─────────────────────────────────────────┐
│ 2. APPLICATION - IPacienteService       │ ← Interfaz (contrato)
│    • Define QUÉ se puede hacer         │
│    • NO contiene código real           │
└────────────────┬────────────────────────┘
                 │
                 │ Implementación real
                 ▼
┌─────────────────────────────────────────┐
│ 3. INFRASTRUCTURE - PacienteService      │ ← Implementación
│    • Usa UnitOfWork para obtener datos │
│    • Aplica reglas de negocio          │
└────────────────┬────────────────────────┘
                 │
                 │ unitOfWork.Pacientes.SeleccionarPorId(id)
                 ▼
┌─────────────────────────────────────────┐
│ 4. PERSISTENCE - UnitOfWorkManager      │ ← Coordinador
│    • Maneja múltiples repositorios     │
│    • Agrupa operaciones                │
└────────────────┬────────────────────────┘
                 │
                 │ Repository.SeleccionarPorId(id)
                 ▼
┌─────────────────────────────────────────┐
│ 5. PERSISTENCE - Repository             │ ← Acceso a datos
│    • Ejecuta consultas                 │
│    • Usa DbContext                      │
└────────────────┬────────────────────────┘
                 │
                 │ dbContext.Set<Paciente>().FirstOrDefaultAsync()
                 ▼
┌─────────────────────────────────────────┐
│ 6. PERSISTENCE - AppDbContext            │ ← Conexión BD
│    • Traduce a SQL                      │
│    • Consulta PostgreSQL                │
└────────────────┬────────────────────────┘
                 │
                 │ SELECT * FROM Pacientes WHERE Id = ...
                 ▼
┌─────────────────────────────────────────┐
│ 7. PostgreSQL Database                  │ ← Base de datos
│    • Ejecuta la consulta                │
│    • Retorna los datos                  │
└────────────────┬────────────────────────┘
                 │
                 │ Datos del paciente
                 ▼
┌─────────────────────────────────────────┐
│ 8. PERSISTENCE - Repository             │ ← Recibe datos
│    • Devuelve entidad Domain            │
└────────────────┬────────────────────────┘
                 │
                 │ Entidad Paciente
                 ▼
┌─────────────────────────────────────────┐
│ 9. INFRASTRUCTURE - PacienteService      │ ← Recibe entidad
│    • Aplica lógica de negocio           │
│    • Usa Mapper para transformar        │
└────────────────┬────────────────────────┘
                 │
                 │ mapper.Map<PacienteRespuesta>(paciente)
                 ▼
┌─────────────────────────────────────────┐
│ 10. APPLICATION - PacienteProfile        │ ← Transformación
│     • Convierte Paciente → DTO          │
│     • Mapea campos                      │
└────────────────┬────────────────────────┘
                 │
                 │ PacienteRespuesta (DTO)
                 ▼
┌─────────────────────────────────────────┐
│ 11. INFRASTRUCTURE - PacienteService    │ ← Retorna DTO
│     • Devuelve DTO al controller        │
└────────────────┬────────────────────────┘
                 │
                 │ PacienteRespuesta
                 ▼
┌─────────────────────────────────────────┐
│ 12. API - PacientesController           │ ← Retorna HTTP
│     • Convierte a respuesta HTTP        │
│     • Status 200 OK + JSON              │
└────────────────┬────────────────────────┘
                 │
                 │ HTTP 200 OK + JSON
                 ▼
Cliente (Navegador/App)
```

## Paso a Paso Detallado

### Paso 1: La Petición Llega al Controller

**Archivo**: `PsicoAgenda.Api/Controllers/PacientesController.cs`

```csharp
[HttpGet("{id}")]
public async Task<ActionResult<PacienteRespuesta>> Get(Guid id, CancellationToken cancellationToken)
{
    // Este método se ejecuta cuando llega: GET /api/pacientes/{id}
    // El framework ASP.NET Core automáticamente extrae el {id} de la URL
    // y lo pasa como parámetro
    
    var paciente = await _pacienteService.ObtenerPaciente(id, cancellationToken);
    // ↑ Aquí llama al servicio. Veamos qué pasa dentro...
    
    return Ok(paciente);
    // ↑ Finalmente devuelve la respuesta HTTP 200 OK con los datos
}
```

**¿Qué hace?**
- Recibe la petición HTTP GET
- Extrae el ID del paciente de la URL
- Llama al servicio `_pacienteService` para obtener el paciente
- Devuelve los datos como respuesta HTTP

**Datos en este punto**: `id = 11111111-1111-1111-1111-111111111111`

---

### Paso 2: El Controller Usa la Interfaz del Servicio

**Archivo**: `PsicoAgenda.Application/Interfaces/IPacienteService.cs`

```csharp
public interface IPacienteService
{
    Task<PacienteRespuesta> ObtenerPaciente(Guid id, CancellationToken cancellationToken);
    // ↑ Esto es un CONTRATO. Dice "debe existir un método que haga esto",
    // pero NO dice CÓMO se hace
}
```

**¿Qué hace?**
- Define qué debe poder hacer el servicio (el contrato)
- NO contiene código real, solo la definición
- El controller depende de esta interfaz, no de la implementación

**Analogía**: Es como un menú de restaurante que lista los platos, pero no las recetas.

---

### Paso 3: La Implementación Real del Servicio

**Archivo**: `PsicoAgenda.Infrastructure/Services/PacienteService.cs`

```csharp
public class PacienteService(IUnitOfWork unitOfWork, IMapper mapper) : IPacienteService
{
    // ↑ Recibe dos dependencias inyectadas:
    //   - unitOfWork: Para acceder a los datos
    //   - mapper: Para transformar objetos
    
    public async Task<PacienteRespuesta> ObtenerPaciente(Guid id, CancellationToken cancellationToken)
    {
        // 1. Buscar el paciente en la base de datos
        var paciente = await unitOfWork.Pacientes.SeleccionarPorId(id, cancellationToken);
        //    ↑ Usa el repositorio de pacientes del UnitOfWork
        //    Esto aún no va a la BD, solo prepara la consulta
        
        // 2. Transformar de Paciente (entidad) a PacienteRespuesta (DTO)
        return mapper.Map<PacienteRespuesta>(paciente);
        //    ↑ El mapper convierte automáticamente:
        //      - Paciente.PrimerNombre → PacienteRespuesta.Nombre
        //      - Paciente.Email → PacienteRespuesta.Email
        //      etc.
    }
}
```

**¿Qué hace?**
- Busca el paciente usando el repositorio
- Transforma la entidad del dominio a un DTO usando AutoMapper
- Retorna el DTO

**Datos en este punto**: `paciente` es una instancia de `Paciente` (entidad del dominio)

---

### Paso 4: El UnitOfWork Coordina el Acceso a Datos

**Archivo**: `PsicoAgenda.Persistence/UnitOfWork/UnitOfWorkManager.cs`

```csharp
public class UnitOfWorkManager : IUnitOfWork
{
    public IRepository<Paciente> Pacientes { get; }
    // ↑ Expone un repositorio para cada entidad
    
    // Cuando se llama a unitOfWork.Pacientes, se usa este repositorio
}
```

**¿Qué hace?**
- Centraliza el acceso a todos los repositorios
- Permite agrupar múltiples operaciones en una transacción
- En este caso, solo necesita el repositorio de Pacientes

**Datos en este punto**: Se pasa el `id` al repositorio

---

### Paso 5: El Repository Ejecuta la Consulta

**Archivo**: `PsicoAgenda.Persistence/Repositories/Repository.cs`

```csharp
public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntidadBase
{
    private readonly AppDbContext dbContext;
    // ↑ Tiene acceso al contexto de base de datos
    
    public async Task<TEntity?> SeleccionarPorId(Guid id, CancellationToken cancellationToken)
    {
        // Busca en la tabla correspondiente a TEntity (en este caso, Paciente)
        return await dbContext.Set<TEntity>()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        //     ↑ Esto crea una consulta SQL, pero aún no la ejecuta
    }
}
```

**¿Qué hace?**
- Crea una consulta usando Entity Framework Core
- La consulta busca una entidad por su ID
- Aún no ejecuta nada, solo prepara la consulta

**Datos en este punto**: Se prepara la consulta SQL equivalente a:
```sql
SELECT * FROM "Pacientes" WHERE "Id" = '11111111-1111-1111-1111-111111111111'
```

---

### Paso 6: El DbContext Traduce a SQL

**Archivo**: `PsicoAgenda.Persistence/Context/AppDbContext.cs`

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Paciente> Pacientes { get; set; }
    // ↑ Define qué tabla existe en la base de datos
    
    // Entity Framework Core automáticamente:
    // 1. Traduce la consulta LINQ a SQL
    // 2. Establece conexión con PostgreSQL
    // 3. Ejecuta la consulta
}
```

**¿Qué hace?**
- Entity Framework Core toma la consulta LINQ
- La traduce a SQL de PostgreSQL
- Abre una conexión a la base de datos
- Ejecuta la consulta

**Datos en este punto**: Se envía el SQL a PostgreSQL

---

### Paso 7: PostgreSQL Ejecuta la Consulta

**Base de datos PostgreSQL**

```sql
SELECT * FROM "Pacientes" 
WHERE "Id" = '11111111-1111-1111-1111-111111111111';
```

PostgreSQL:
1. Busca en la tabla `Pacientes`
2. Encuentra la fila con ese ID
3. Retorna todos los campos de esa fila

**Datos retornados** (ejemplo):
```
Id: 11111111-1111-1111-1111-111111111111
PrimerNombre: "Ana"
SegundoNombre: "Ramírez"
Email: "ana@mail.com"
Telefono: "3001111111"
FechaNacimiento: 1995-05-10
...
```

---

### Paso 8-9: Los Datos Vuelven por las Capas

Los datos regresan por las mismas capas, pero en orden inverso:

1. **PostgreSQL** → retorna datos al **AppDbContext**
2. **AppDbContext** → crea objeto `Paciente` y lo retorna al **Repository**
3. **Repository** → retorna `Paciente` al **UnitOfWork**
4. **UnitOfWork** → retorna `Paciente` al **PacienteService**

**Datos en este punto**: Objeto `Paciente` con todos sus datos

---

### Paso 10: El Mapper Transforma los Datos

**Archivo**: `PsicoAgenda.Application/Mappers/PacienteProfile.cs`

```csharp
public class PacienteProfile : Profile
{
    public PacienteProfile()
    {
        CreateMap<Paciente, PacienteRespuesta>()
            // Define cómo convertir Paciente → PacienteRespuesta
            
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            // ↑ Campo Id: se copia directamente
            
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.PrimerNombre))
            // ↑ Campo Nombre: toma el PrimerNombre del Paciente
            
            .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.SegundoNombre))
            // ↑ Campo Apellidos: toma el SegundoNombre del Paciente
            
            // ... más mapeos
    }
}
```

**¿Qué hace?**
- AutoMapper toma el objeto `Paciente`
- Lo transforma según las reglas definidas
- Crea un nuevo objeto `PacienteRespuesta`

**Transformación**:
```
Paciente (entidad dominio)          →    PacienteRespuesta (DTO)
─────────────────────────────────        ────────────────────────
Id: Guid                              →    Id: Guid
PrimerNombre: "Ana"                   →    Nombre: "Ana"
SegundoNombre: "Ramírez"              →    Apellidos: "Ramírez"
Email: "ana@mail.com"                 →    Email: "ana@mail.com"
Telefono: "3001111111"               →    Telefono: "3001111111"
FechaNacimiento: 1995-05-10          →    FechaNacimiento: 1995-05-10
```

**¿Por qué se hace esto?** Porque:
- La entidad `Paciente` tiene campos internos que no queremos exponer
- El DTO `PacienteRespuesta` tiene una estructura más simple para el cliente
- Separar la estructura interna de la estructura externa es una buena práctica

---

### Paso 11: El Servicio Retorna el DTO

**Archivo**: `PsicoAgenda.Infrastructure/Services/PacienteService.cs`

```csharp
return mapper.Map<PacienteRespuesta>(paciente);
// Retorna el DTO transformado al controller
```

**Datos en este punto**: Objeto `PacienteRespuesta` listo para enviar

---

### Paso 12: El Controller Devuelve la Respuesta HTTP

**Archivo**: `PsicoAgenda.Api/Controllers/PacientesController.cs`

```csharp
return Ok(paciente);
// ↑ Convierte el DTO a JSON y envía HTTP 200 OK
```

**¿Qué hace?**
- Serializa el DTO a formato JSON
- Envía la respuesta HTTP con código 200 (OK)
- Incluye los datos en el cuerpo de la respuesta

**Respuesta HTTP final**:
```
HTTP/1.1 200 OK
Content-Type: application/json

{
  "id": "11111111-1111-1111-1111-111111111111",
  "nombre": "Ana",
  "apellidos": "Ramírez",
  "email": "ana@mail.com",
  "telefono": "3001111111",
  "fechaNacimiento": "1995-05-10T00:00:00"
}
```

---

## Flujo Completo en una Imagen Mental

```
1. Cliente hace petición HTTP
   ↓
2. Controller recibe y valida
   ↓
3. Controller llama a servicio (interfaz)
   ↓
4. Servicio (implementación) busca datos
   ↓
5. UnitOfWork coordina repositorios
   ↓
6. Repository prepara consulta
   ↓
7. DbContext traduce a SQL
   ↓
8. PostgreSQL ejecuta y retorna datos
   ↑
9. DbContext recibe datos
   ↑
10. Repository devuelve entidad
   ↑
11. UnitOfWork devuelve entidad
   ↑
12. Servicio transforma con Mapper
   ↑
13. Controller serializa a JSON
   ↑
14. Cliente recibe respuesta HTTP
```

## Resumen del Flujo

1. **API** recibe la petición HTTP
2. **Application** define el contrato (interfaz)
3. **Infrastructure** implementa la lógica
4. **Persistence** accede a los datos
5. **Database** almacena y retorna datos
6. Los datos vuelven transformándose en cada capa
7. **API** devuelve la respuesta HTTP final

## Conceptos Clave

### ¿Por qué tantas capas?

Cada capa tiene una responsabilidad única:
- **API**: Comunicación con el exterior
- **Application**: Definición de reglas
- **Infrastructure**: Implementación de reglas
- **Persistence**: Acceso a datos
- **Domain**: Entidades del negocio

Esto hace el código más fácil de:
- **Mantener**: Cambios en una capa no afectan otras
- **Probar**: Se puede probar cada capa independientemente
- **Entender**: Cada archivo tiene un propósito claro

### ¿Qué pasa si algo falla?

Si hay un error en cualquier paso:
1. El error "sube" por las capas hasta el controller
2. El controller puede capturar el error y devolver un mensaje apropiado
3. O el framework ASP.NET Core devuelve un error HTTP estándar

## Otro Ejemplo: Crear un Paciente (POST)

Para ver cómo funciona un flujo de escritura (crear datos), el proceso es similar pero:

1. El cliente envía datos JSON en el cuerpo de la petición
2. El controller recibe un DTO `PacienteCreacion`
3. El servicio transforma el DTO a entidad `Paciente`
4. El repositorio agrega la entidad al contexto
5. Se llama a `GuardarCambios()` para persistir en la BD
6. Se retorna una confirmación

## Próximos Pasos

- [Guía de Arquitectura](GUIA_ARQUITECTURA.md) - Entender las capas del proyecto
- [Inyección de Dependencias](INYECCION_DEPENDENCIAS.md) - Cómo se conectan las piezas
- [Agregar una Funcionalidad](AGREGAR_FUNCIONALIDAD.md) - Crear nuevos endpoints

