# Cómo Agregar una Nueva Funcionalidad - Guía Paso a Paso

## Introducción

Esta guía te muestra el orden exacto en que debes crear los archivos al agregar una nueva funcionalidad al proyecto. Usaremos el ejemplo de **Citas** para ilustrar todo el proceso.

## Orden de Implementación (Checklist)

Sigue estos pasos en orden. No saltes pasos, ya que cada uno depende del anterior.

### ✅ Checklist Completo

- [ ] **Paso 1**: Crear/Verificar modelo en Domain/Models
- [ ] **Paso 2**: Crear/Verificar enums en Domain/Enums (si aplica)
- [ ] **Paso 3**: Verificar interfaces en Domain/Interfaces (IRepository, IUnitOfWork)
- [ ] **Paso 4**: Crear DTOs en Application/Dtos
- [ ] **Paso 5**: Crear interface de servicio en Application/Interfaces
- [ ] **Paso 6**: Crear mapper en Application/Mappers
- [ ] **Paso 7**: Implementar servicio en Infrastructure/Services
- [ ] **Paso 8**: Registrar servicio en Infrastructure/DependencyInjection.cs
- [ ] **Paso 9**: Actualizar IUnitOfWork en Domain/Interfaces
- [ ] **Paso 10**: Actualizar UnitOfWorkManager en Persistence/UnitOfWork
- [ ] **Paso 11**: Actualizar AppDbContext en Persistence/Context
- [ ] **Paso 12**: Crear Controller en Api/Controllers
- [ ] **Paso 13**: Crear migración de base de datos (si es nueva entidad)

---

## Ejemplo Completo: Módulo de Citas

Vamos a ver cómo implementar el módulo completo de Citas, paso a paso.

### Paso 1: Crear el Modelo en Domain/Models

**Archivo**: `PsicoAgenda.Domain/Models/Cita.cs`

```csharp
using PsicoAgenda.Domain.Enums;

namespace PsicoAgenda.Domain.Models
{
    public class Cita : EntidadBase
    // ↑ Hereda de EntidadBase, que incluye Id, FechaCreacion, etc.
    {
        // Propiedades específicas de Cita
        public Guid PacienteId { get; set; }
        // ↑ ID del paciente al que pertenece esta cita
        
        public DateTime FechaInicio { get; set; }
        // ↑ Cuándo empieza la cita
        
        public DateTime FechaFin { get; set; }
        // ↑ Cuándo termina la cita
        
        public ModoCita Modo { get; set; }
        // ↑ Presencial, Virtual, o Híbrido
        
        public EstadoCita Estado { get; set; }
        // ↑ Pendiente, Confirmada, Cancelada, etc.
        
        public string? UbicacionLink { get; set; }
        // ↑ Link para citas virtuales (opcional)
        
        public string? Notas { get; set; }
        // ↑ Notas adicionales (opcional)
        
        // Relación con Paciente
        public Paciente? Paciente { get; set; }
        // ↑ Navegación a la entidad relacionada
    }
}
```

**¿Qué es esto?**
- Define qué es una "Cita" en el negocio
- Hereda de `EntidadBase` para tener Id, fechas de auditoría automáticas
- Define todas las propiedades que tiene una cita

**Reglas importantes:**
- Debe heredar de `EntidadBase`
- Las propiedades opcionales usan `?` (nullable)
- Las relaciones se definen como propiedades navegables

---

### Paso 2: Crear/Verificar Enums (si aplica)

**Archivo**: `PsicoAgenda.Domain/Enums/ModoCita.cs`

```csharp
namespace PsicoAgenda.Domain.Enums
{
    public enum ModoCita
    {
        Presencial,  // En persona
        Virtual,     // Por video llamada
        Hibrido      // Combinación de ambos
    }
}
```

**Archivo**: `PsicoAgenda.Domain/Enums/EstadoCita.cs`

```csharp
namespace PsicoAgenda.Domain.Enums
{
    public enum EstadoCita
    {
        Pendiente,   // Creada pero no confirmada
        Confirmada,  // El paciente confirmó asistencia
        Cancelada,   // Cancelada por alguna razón
        Completada,  // La cita ya se realizó
        NoAsistio    // El paciente no asistió
    }
}
```

**¿Qué es esto?**
- Define valores fijos que puede tener un campo
- Evita usar strings como "Pendiente" y usar valores erróneos como "pendiente" o "PENDIENTE"

---

### Paso 3: Verificar Interfaces del Domain

**Archivo**: `PsicoAgenda.Domain/Interfaces/IUnitOfWork.cs`

Ya debe estar actualizado (lo verificamos más adelante). Debe incluir:
```csharp
public interface IUnitOfWork
{
    IRepository<Cita> Citas { get; }
    // ... otros repositorios
}
```

**¿Qué es esto?**
- Define que el UnitOfWork debe tener acceso a Citas
- Ya debería estar si seguiste el patrón del proyecto

---

### Paso 4: Crear DTOs en Application/Dtos

Los DTOs son versiones "simplificadas" de las entidades para comunicarse con el exterior.

#### DTO para Crear una Cita

**Archivo**: `PsicoAgenda.Application/Dtos/Citas/CitaCreacion.cs`

```csharp
using PsicoAgenda.Domain.Enums;

namespace PsicoAgenda.Application.Dtos.Citas;

public record CitaCreacion(
    Guid PacienteId,           // ID del paciente
    DateTime FechaInicio,      // Cuándo empieza
    DateTime FechaFin,         // Cuándo termina
    ModoCita Modo,             // Presencial, Virtual, etc.
    string? UbicacionLink,     // Link opcional para virtuales
    string? Notas              // Notas opcionales
);
```

#### DTO para Respuesta (al consultar una Cita)

**Archivo**: `PsicoAgenda.Application/Dtos/Citas/CitaRespuesta.cs`

```csharp
using PsicoAgenda.Domain.Enums;

namespace PsicoAgenda.Application.Dtos.Citas;

public record CitaRespuesta(
    Guid Id,                   // ID de la cita
    Guid PacienteId,           // ID del paciente
    string NombrePaciente,     // Nombre del paciente (incluido para conveniencia)
    DateTime FechaInicio,      // Cuándo empieza
    DateTime FechaFin,         // Cuándo termina
    ModoCita Modo,             // Presencial, Virtual, etc.
    EstadoCita Estado,         // Estado actual
    string? UbicacionLink,     // Link opcional
    string? Notas              // Notas opcionales
);
```

**¿Qué es esto?**
- `CitaCreacion`: Datos mínimos necesarios para crear una cita
- `CitaRespuesta`: Datos que se devuelven al consultar (puede incluir información adicional como el nombre del paciente)

**¿Por qué no usar la entidad directamente?**
- Separación de responsabilidades
- Control sobre qué datos exponer
- Posibilidad de incluir datos calculados o relacionados

---

### Paso 5: Crear Interface de Servicio

**Archivo**: `PsicoAgenda.Application/Interfaces/ICitaService.cs`

```csharp
using PsicoAgenda.Application.Dtos.Citas;

namespace PsicoAgenda.Application.Interfaces;

public interface ICitaService
{
    // Obtener una cita por ID
    Task<CitaRespuesta> ObtenerCita(Guid id, CancellationToken cancellationToken);
    
    // Crear una nueva cita
    Task<Guid> CrearCita(CitaCreacion request, CancellationToken cancellationToken);
    
    // Obtener todas las citas de un paciente
    Task<IEnumerable<CitaRespuesta>> ObtenerCitasPorPaciente(
        Guid pacienteId, 
        CancellationToken cancellationToken
    );
    
    // Actualizar una cita
    Task ActualizarCita(Guid id, CitaCreacion request, CancellationToken cancellationToken);
    
    // Cancelar una cita
    Task CancelarCita(Guid id, CancellationToken cancellationToken);
}
```

**¿Qué es esto?**
- Define QUÉ operaciones puede hacer el servicio de citas
- NO contiene código real, solo la firma de los métodos
- Define los contratos que debe cumplir cualquier implementación

---

### Paso 6: Crear Mapper

**Archivo**: `PsicoAgenda.Application/Mappers/CitaProfile.cs`

```csharp
using AutoMapper;
using PsicoAgenda.Application.Dtos.Citas;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Application.Mappers;

public class CitaProfile : Profile
{
    public CitaProfile()
    {
        // Mapeo: Cita (entidad) → CitaRespuesta (DTO)
        CreateMap<Cita, CitaRespuesta>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
            .ForMember(dest => dest.NombrePaciente, opt => opt.MapFrom(src => 
                src.Paciente != null ? src.Paciente.PrimerNombre : string.Empty))
            // ↑ Si el paciente está cargado, toma su nombre
            .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
            .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin))
            .ForMember(dest => dest.Modo, opt => opt.MapFrom(src => src.Modo))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
            .ForMember(dest => dest.UbicacionLink, opt => opt.MapFrom(src => src.UbicacionLink))
            .ForMember(dest => dest.Notas, opt => opt.MapFrom(src => src.Notas));
        
        // Mapeo: CitaCreacion (DTO) → Cita (entidad)
        CreateMap<CitaCreacion, Cita>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            // ↑ El ID se genera automáticamente, no viene en la creación
            .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
            .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
            .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin))
            .ForMember(dest => dest.Modo, opt => opt.MapFrom(src => src.Modo))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => EstadoCita.Pendiente))
            // ↑ Al crear, siempre empieza como Pendiente
            .ForMember(dest => dest.UbicacionLink, opt => opt.MapFrom(src => src.UbicacionLink))
            .ForMember(dest => dest.Notas, opt => opt.MapFrom(src => src.Notas))
            .ForMember(dest => dest.Paciente, opt => opt.Ignore());
            // ↑ La relación con Paciente no se mapea directamente
    }
}
```

**¿Qué es esto?**
- Define cómo convertir entre entidades y DTOs
- AutoMapper usa estas reglas para transformar objetos automáticamente

---

### Paso 7: Implementar el Servicio

**Archivo**: `PsicoAgenda.Infrastructure/Services/CitaService.cs`

```csharp
using AutoMapper;
using PsicoAgenda.Application.Dtos.Citas;
using PsicoAgenda.Application.Interfaces;
using PsicoAgenda.Domain.Enums;
using PsicoAgenda.Domain.Interfaces;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Infrastructure.Services;

public class CitaService : ICitaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    // Constructor: recibe dependencias inyectadas
    public CitaService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<CitaRespuesta> ObtenerCita(Guid id, CancellationToken cancellationToken)
    {
        // 1. Buscar la cita en la base de datos, incluyendo el paciente relacionado
        var cita = await _unitOfWork.Citas.SeleccionarPorEIncluir(
            c => c.Id == id,
            cancellationToken,
            c => c.Include(cita => cita.Paciente)
        );
        
        if (cita == null)
        {
            throw new KeyNotFoundException($"Cita con ID {id} no encontrada");
        }
        
        // 2. Transformar de entidad a DTO
        return _mapper.Map<CitaRespuesta>(cita);
    }
    
    public async Task<Guid> CrearCita(CitaCreacion request, CancellationToken cancellationToken)
    {
        // 1. Verificar que el paciente existe
        var paciente = await _unitOfWork.Pacientes.SeleccionarPorId(
            request.PacienteId, 
            cancellationToken
        );
        
        if (paciente == null)
        {
            throw new KeyNotFoundException($"Paciente con ID {request.PacienteId} no encontrado");
        }
        
        // 2. Transformar DTO a entidad
        var cita = _mapper.Map<Cita>(request);
        cita.Estado = EstadoCita.Pendiente; // Estado inicial
        
        // 3. Guardar en la base de datos
        await _unitOfWork.Citas.Crear(cita);
        await _unitOfWork.GuardarCambios(cancellationToken);
        
        // 4. Retornar el ID de la cita creada
        return cita.Id;
    }
    
    public async Task<IEnumerable<CitaRespuesta>> ObtenerCitasPorPaciente(
        Guid pacienteId, 
        CancellationToken cancellationToken)
    {
        // 1. Buscar todas las citas del paciente, incluyendo datos del paciente
        var citas = await _unitOfWork.Citas.SeleccionarTodosPorEIncluir(
            c => c.PacienteId == pacienteId,
            c => c.Include(cita => cita.Paciente)
        );
        
        // 2. Transformar a DTOs
        return _mapper.Map<IEnumerable<CitaRespuesta>>(citas);
    }
    
    public async Task ActualizarCita(Guid id, CitaCreacion request, CancellationToken cancellationToken)
    {
        // 1. Buscar la cita existente
        var cita = await _unitOfWork.Citas.SeleccionarPorId(id, cancellationToken);
        
        if (cita == null)
        {
            throw new KeyNotFoundException($"Cita con ID {id} no encontrada");
        }
        
        // 2. Actualizar propiedades (excepto Estado, que se maneja por separado)
        cita.PacienteId = request.PacienteId;
        cita.FechaInicio = request.FechaInicio;
        cita.FechaFin = request.FechaFin;
        cita.Modo = request.Modo;
        cita.UbicacionLink = request.UbicacionLink;
        cita.Notas = request.Notas;
        
        // 3. Guardar cambios
        _unitOfWork.Citas.Actualizar(cita);
        await _unitOfWork.GuardarCambios(cancellationToken);
    }
    
    public async Task CancelarCita(Guid id, CancellationToken cancellationToken)
    {
        // 1. Buscar la cita
        var cita = await _unitOfWork.Citas.SeleccionarPorId(id, cancellationToken);
        
        if (cita == null)
        {
            throw new KeyNotFoundException($"Cita con ID {id} no encontrada");
        }
        
        // 2. Cambiar estado a Cancelada
        cita.Estado = EstadoCita.Cancelada;
        
        // 3. Guardar cambios
        _unitOfWork.Citas.Actualizar(cita);
        await _unitOfWork.GuardarCambios(cancellationToken);
    }
}
```

**¿Qué es esto?**
- La implementación REAL del servicio
- Aquí está toda la lógica de negocio
- Usa el UnitOfWork para acceder a los datos
- Usa el Mapper para transformar objetos

---

### Paso 8: Registrar el Servicio

**Archivo**: `PsicoAgenda.Infrastructure/DependencyInjection.cs`

```csharp
using Microsoft.Extensions.DependencyInjection;
using PsicoAgenda.Application.Interfaces;
using PsicoAgenda.Infrastructure.Services;

namespace PsicoAgenda.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPacienteService, PacienteService>();
        services.AddScoped<ICitaService, CitaService>();
        // ↑ Agregar esta línea para registrar el nuevo servicio
    }
}
```

**¿Qué es esto?**
- Registra que cuando alguien pida `ICitaService`, se debe usar `CitaService`
- Permite que el sistema de inyección de dependencias funcione

---

### Paso 9: Actualizar IUnitOfWork

**Archivo**: `PsicoAgenda.Domain/Interfaces/IUnitOfWork.cs`

Verificar que incluya el repositorio de Citas:

```csharp
public interface IUnitOfWork : IDisposable
{
    IRepository<Cita> Citas { get; }
    IRepository<Paciente> Pacientes { get; }
    // ... otros repositorios
}
```

**Nota**: Si el modelo Cita ya existía, esto ya debería estar hecho.

---

### Paso 10: Actualizar UnitOfWorkManager

**Archivo**: `PsicoAgenda.Persistence/UnitOfWork/UnitOfWorkManager.cs`

Verificar que incluya el repositorio de Citas:

```csharp
public class UnitOfWorkManager : IUnitOfWork
{
    public IRepository<Cita> Citas { get; }
    public IRepository<Paciente> Pacientes { get; }
    // ... otros repositorios
    
    public UnitOfWorkManager(
        AppDbContext context,
        IRepository<Cita> citas,
        IRepository<Paciente> pacientes,
        // ... otros repositorios
    )
    {
        Citas = citas;
        Pacientes = pacientes;
        // ... asignar otros repositorios
    }
}
```

**Nota**: Si el modelo Cita ya existía, esto ya debería estar hecho.

---

### Paso 11: Actualizar AppDbContext

**Archivo**: `PsicoAgenda.Persistence/Context/AppDbContext.cs`

Verificar que incluya el DbSet de Citas:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Cita> Citas { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    // ... otros DbSets
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuración de Cita
        modelBuilder.Entity<Cita>(c =>
        {
            c.HasKey(c => c.Id);
            c.HasOne(c => c.Paciente)
             .WithMany()
             .HasForeignKey(c => c.PacienteId);
        });
        
        // Configuración de enums
        modelBuilder.Entity<Cita>().Property(c => c.Modo).HasConversion<string>();
        modelBuilder.Entity<Cita>().Property(c => c.Estado).HasConversion<string>();
        
        // ... otras configuraciones
    }
}
```

**¿Qué es esto?**
- `DbSet<Cita>`: Define que existe una tabla de Citas en la base de datos
- `OnModelCreating`: Configura cómo se mapea la entidad a la tabla
  - Define la clave primaria
  - Define la relación con Paciente (Foreign Key)
  - Configura los enums para guardarlos como strings

**Nota**: Si el modelo Cita ya existía, esto ya debería estar hecho.

---

### Paso 12: Crear el Controller

**Archivo**: `PsicoAgenda.Api/Controllers/CitasController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using PsicoAgenda.Application.Dtos.Citas;
using PsicoAgenda.Application.Interfaces;

namespace PsicoAgenda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
// ↑ Esto hace que las rutas sean: /api/Citas
public class CitasController : ControllerBase
{
    private readonly ICitaService _citaService;
    
    // Constructor: recibe el servicio inyectado
    public CitasController(ICitaService citaService)
    {
        _citaService = citaService;
    }
    
    // GET /api/citas/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CitaRespuesta>> Get(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var cita = await _citaService.ObtenerCita(id, cancellationToken);
        return Ok(cita);
    }
    
    // POST /api/citas
    [HttpPost]
    public async Task<ActionResult> Post(
        [FromBody] CitaCreacion request,
        CancellationToken cancellationToken)
    {
        var id = await _citaService.CrearCita(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }
    
    // GET /api/citas/paciente/{pacienteId}
    [HttpGet("paciente/{pacienteId}")]
    public async Task<ActionResult<IEnumerable<CitaRespuesta>>> GetPorPaciente(
        Guid pacienteId,
        CancellationToken cancellationToken)
    {
        var citas = await _citaService.ObtenerCitasPorPaciente(pacienteId, cancellationToken);
        return Ok(citas);
    }
    
    // PUT /api/citas/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(
        Guid id,
        [FromBody] CitaCreacion request,
        CancellationToken cancellationToken)
    {
        await _citaService.ActualizarCita(id, request, cancellationToken);
        return NoContent();
    }
    
    // DELETE /api/citas/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _citaService.CancelarCita(id, cancellationToken);
        return NoContent();
    }
}
```

**¿Qué es esto?**
- Expone los endpoints HTTP para manejar citas
- Cada método HTTP (GET, POST, PUT, DELETE) mapea a un método del servicio
- Valida y formatea las respuestas HTTP

---

### Paso 13: Crear Migración (si es una entidad nueva)

Si es la primera vez que creas esta entidad en la base de datos, necesitas crear una migración:

**Comando en la terminal**:
```bash
dotnet ef migrations add AgregarTablaCitas --project PsicoAgenda.Persistence --startup-project PsicoAgenda.Api
```

**¿Qué hace esto?**
- Crea un archivo de migración que contiene los cambios SQL necesarios
- El archivo se crea en `PsicoAgenda.Persistence/Migrations/`

**Aplicar la migración**:
```bash
dotnet ef database update --project PsicoAgenda.Persistence --startup-project PsicoAgenda.Api
```

**¿Qué hace esto?**
- Aplica los cambios en la base de datos real
- Crea las tablas y relaciones necesarias

---

## Resumen del Proceso

```
1. Domain/Models        → Define QUÉ es la entidad
2. Domain/Enums        → Define valores fijos (si aplica)
3. Application/Dtos    → Define formatos de entrada/salida
4. Application/Interfaces → Define QUÉ puede hacer el servicio
5. Application/Mappers → Define cómo transformar objetos
6. Infrastructure/Services → Implementa CÓMO hacerlo
7. Infrastructure/DI   → Registra el servicio
8. Persistence         → Configura acceso a datos (si es nuevo)
9. Api/Controllers     → Expone endpoints HTTP
10. Migración          → Crea tablas en BD (si es nuevo)
```

## Buenas Prácticas

### 1. Sigue el Orden
No saltes pasos. Cada paso depende del anterior.

### 2. Nombres Consistentes
- Entidad: `Cita` (singular, PascalCase)
- DTOs: `CitaCreacion`, `CitaRespuesta` (singular + acción/resultado)
- Servicio: `ICitaService`, `CitaService`
- Controller: `CitasController` (plural)
- Ruta: `/api/citas` (plural, minúsculas)

### 3. Manejo de Errores
Siempre valida que los datos existan antes de usarlos:
```csharp
var paciente = await _unitOfWork.Pacientes.SeleccionarPorId(id, cancellationToken);
if (paciente == null)
{
    throw new KeyNotFoundException($"Paciente con ID {id} no encontrado");
}
```

### 4. Usa CancellationToken
Siempre incluye `CancellationToken cancellationToken` en métodos asíncronos para permitir cancelación.

### 5. Guarda los Cambios
Después de crear, actualizar o eliminar, siempre llama a:
```csharp
await _unitOfWork.GuardarCambios(cancellationToken);
```

## Errores Comunes y Soluciones

### Error: "No se puede resolver ICitaService"
**Causa**: No registraste el servicio en `DependencyInjection.cs`
**Solución**: Agrega `services.AddScoped<ICitaService, CitaService>();`

### Error: "La entidad Cita no está en el DbContext"
**Causa**: Falta `DbSet<Cita>` en `AppDbContext`
**Solución**: Agrega `public DbSet<Cita> Citas { get; set; }`

### Error: "No se puede convertir CitaCreacion a Cita"
**Causa**: Falta el mapper o está mal configurado
**Solución**: Verifica `CitaProfile.cs` y que esté registrado en `Application/DependencyInjection.cs`

### Error: "Repository<Cita> no está disponible"
**Causa**: Falta `Citas` en `IUnitOfWork` o `UnitOfWorkManager`
**Solución**: Agrega el repositorio en ambos lugares

## Próximos Pasos

- [Guía de Arquitectura](GUIA_ARQUITECTURA.md) - Entender las capas
- [Flujo de Ejecución](FLUJO_EJECUCION.md) - Ver cómo fluye una petición
- [Inyección de Dependencias](INYECCION_DEPENDENCIAS.md) - Entender cómo se conectan las piezas
- [Referencias del Proyecto](REFERENCIAS_PROYECTO.md) - Glosario de términos

