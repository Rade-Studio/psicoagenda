# Inyección de Dependencias - Cómo se Conectan las Piezas

## Introducción

La inyección de dependencias es un concepto fundamental en este proyecto. Aunque suena complicado, en realidad es una forma muy simple de organizar el código. Este documento explica qué es, por qué se usa, y cómo funciona específicamente en PsicoAgenda.

## ¿Qué es la Inyección de Dependencias?

### Analogía Simple: El Restaurante

Imagina que eres dueño de un restaurante:

**❌ SIN inyección de dependencias:**
```
Cocinero necesita ingredientes
→ Cocinero va al supermercado él mismo
→ Cocinero cocina
```

Problema: Si cambia el supermercado, el cocinero tiene que cambiar todo su proceso.

**✅ CON inyección de dependencias:**
```
Restaurante tiene una "proveeduría" central
→ La proveeduría le entrega los ingredientes al cocinero
→ Cocinero solo cocina
```

Ventaja: Si cambia el proveedor, solo cambias la proveeduría. El cocinero no se entera.

### En Programación

**Inyección de dependencias** significa que cuando una clase necesita usar otra clase, no la crea ella misma. En su lugar, alguien externo (el "inyector") se la proporciona.

## Ejemplo Real del Proyecto

Veamos cómo funciona en nuestro código:

### Ejemplo 1: Controller Necesita un Servicio

**Archivo**: `PsicoAgenda.Api/Controllers/PacientesController.cs`

```csharp
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;
    // ↑ Esta es una DEPENDENCIA: el controller necesita el servicio
    
    public PacientesController(IPacienteService pacienteService)
    //                          ↑ El servicio se INYECTA aquí
    {
        _pacienteService = pacienteService;
        // ↑ Se guarda para usarlo después
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id)
    {
        var paciente = await _pacienteService.ObtenerPaciente(id);
        //              ↑ Usa el servicio inyectado
        return Ok(paciente);
    }
}
```

**¿Qué está pasando?**
1. El controller **NO crea** el `PacienteService` él mismo
2. Recibe el servicio como parámetro en el constructor
3. El framework ASP.NET Core automáticamente crea el servicio y se lo pasa

**¿Quién crea el servicio?** Lo veremos más adelante en "Registro de Dependencias".

---

## ¿Por qué Usar Interfaces en Lugar de Clases Directas?

Esta es una pregunta muy importante. Vamos a verlo con ejemplos:

### ❌ Problema: Usar la Clase Directa

```csharp
public class PacientesController : ControllerBase
{
    private readonly PacienteService _pacienteService;
    //                  ↑ Depende directamente de la clase concreta
    
    public PacientesController()
    {
        _pacienteService = new PacienteService();
        //                  ↑ Crea la instancia directamente
    }
}
```

**Problemas de este enfoque:**

1. **No se puede cambiar fácilmente**
   - Si quieres usar otro servicio (ej: `PacienteServiceV2`), tienes que cambiar el controller
   - Si quieres usar un servicio de prueba (mock), no puedes

2. **Es difícil de probar**
   - No puedes "simular" el servicio para hacer pruebas unitarias
   - Estás forzado a usar la implementación real siempre

3. **Acoplamiento fuerte**
   - El controller está "casado" con esa clase específica
   - Cualquier cambio en `PacienteService` puede romper el controller

### ✅ Solución: Usar una Interfaz

```csharp
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;
    //                  ↑ Depende de la INTERFAZ, no de la clase
    
    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
        // ↑ Recibe cualquier cosa que implemente IPacienteService
    }
}
```

**Ventajas de este enfoque:**

1. **Flexibilidad**
   - Puedes cambiar la implementación sin tocar el controller
   - El controller solo necesita saber "qué puede hacer", no "cómo se hace"

2. **Fácil de probar**
   - Puedes crear un "servicio falso" (mock) que implemente `IPacienteService`
   - Las pruebas son más rápidas y no dependen de servicios externos

3. **Desacoplamiento**
   - El controller no sabe qué clase concreta está usando
   - Solo sabe que hay algo que puede hacer las operaciones definidas en la interfaz

### Analogía: El Conductor y el Vehículo

**❌ Sin interfaz:**
```
Conductor sabe que tiene un Ford Focus específico
→ Si el auto se descompone, el conductor tiene que esperar ese modelo
→ Si quiere usar otro auto, tiene que aprender a manejarlo diferente
```

**✅ Con interfaz:**
```
Conductor sabe que tiene un "Vehículo" (interfaz) con:
  - Volante
  - Pedal de aceleración
  - Pedal de freno
  
→ Puede usar cualquier auto que tenga esos controles
→ Puede cambiar de Ford a Toyota sin problemas
→ Para pruebas, puede usar un simulador que también tenga esos controles
```

---

## ¿Cómo Funciona en Nuestro Proyecto?

El proyecto usa el sistema de inyección de dependencias de .NET. El proceso tiene 3 pasos:

### Paso 1: Definir la Interfaz (Application)

**Archivo**: `PsicoAgenda.Application/Interfaces/IPacienteService.cs`

```csharp
public interface IPacienteService
{
    // Define QUÉ puede hacer, pero no CÓMO
    Task<PacienteRespuesta> ObtenerPaciente(Guid id, CancellationToken cancellationToken);
    Task CrearPaciente(PacienteCreacion request);
}
```

**¿Qué es esto?**
- Un contrato: dice "cualquier clase que implemente esto debe tener estos métodos"
- No tiene código real, solo la firma de los métodos

### Paso 2: Implementar la Clase (Infrastructure)

**Archivo**: `PsicoAgenda.Infrastructure/Services/PacienteService.cs`

```csharp
public class PacienteService : IPacienteService
//                           ↑ Implementa la interfaz
{
    // Aquí está el código REAL
    public async Task<PacienteRespuesta> ObtenerPaciente(Guid id, CancellationToken cancellationToken)
    {
        // Implementación real...
    }
}
```

**¿Qué es esto?**
- La implementación concreta de la interfaz
- Aquí está todo el código que realmente hace el trabajo

### Paso 3: Registrar la Relación (DependencyInjection)

**Archivo**: `PsicoAgenda.Infrastructure/DependencyInjection.cs`

```csharp
public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPacienteService, PacienteService>();
        //        ↑ Interfaz     ↑ Implementación
        // Esto dice: "Cuando alguien pida IPacienteService,
        //             dale una instancia de PacienteService"
    }
}
```

**¿Qué es esto?**
- Le dice al sistema: "Cuando encuentres `IPacienteService`, usa `PacienteService`"
- Es como registrar en un catálogo: "Bajo el nombre 'IPacienteService', busca la clase 'PacienteService'"

### Paso 4: Configurar en el Arranque (Program.cs)

**Archivo**: `PsicoAgenda.Api/Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

// Registrar todas las dependencias
builder.Services.AddApplication();
// ↑ Registra mappers, etc.

builder.Services.AddPersistence();
// ↑ Registra repositorios, UnitOfWork

builder.Services.AddInfrastructure();
// ↑ Registra servicios (IPacienteService → PacienteService)

var app = builder.Build();
```

**¿Qué es esto?**
- Al arrancar la aplicación, se registran todas las dependencias
- El framework crea un "contenedor" que sabe cómo crear cada cosa

---

## El Flujo Completo de Inyección

```
1. Programa inicia (Program.cs)
   ↓
2. Se registran dependencias (DependencyInjection.cs)
   "IPacienteService → PacienteService"
   ↓
3. Alguien pide IPacienteService (Controller constructor)
   ↓
4. El sistema busca en el registro
   ↓
5. Encuentra que IPacienteService = PacienteService
   ↓
6. Crea una instancia de PacienteService
   ↓
7. Se la inyecta al Controller
   ↓
8. Controller puede usarla
```

---

## Tipos de Ciclo de Vida (Lifetimes)

Cuando registras una dependencia, puedes elegir cuánto tiempo vive:

### 1. Scoped (Ámbito de Solicitud)

```csharp
services.AddScoped<IPacienteService, PacienteService>();
```

**¿Qué significa?**
- Se crea UNA instancia por cada petición HTTP
- Si en una petición múltiples clases piden `IPacienteService`, todas comparten la misma instancia
- Al terminar la petición, se destruye

**Analogía**: En un restaurante, cada mesa (petición) tiene su propio mozo (instancia). Diferentes mesas tienen diferentes mozos.

**Cuándo usar**: Para servicios que manejan datos (como nuestro `PacienteService`)

### 2. Singleton (Único)

```csharp
services.AddSingleton<IConfigurationService, ConfigurationService>();
```

**¿Qué significa?**
- Se crea UNA sola instancia para toda la aplicación
- Todas las peticiones comparten la misma instancia
- Vive mientras la aplicación esté corriendo

**Analogía**: El chef del restaurante es el mismo para todas las mesas.

**Cuándo usar**: Para servicios que no cambian y no tienen estado (configuraciones, caché compartido)

### 3. Transient (Transitorio)

```csharp
services.AddTransient<IEmailValidator, EmailValidator>();
```

**¿Qué significa?**
- Se crea una NUEVA instancia cada vez que se pide
- Cada clase que la pide recibe su propia instancia

**Analogía**: Cada vez que necesitas un plato nuevo, te dan uno limpio.

**Cuándo usar**: Para servicios ligeros que se usan una vez y se descartan

---

## Ejemplo Completo: Cadena de Dependencias

Veamos cómo se inyectan múltiples dependencias en cadena:

### Controller → Service → UnitOfWork → Repository

```csharp
// 1. Controller necesita Service
public class PacientesController(IPacienteService pacienteService)
{
    // pacienteService se inyecta automáticamente
}

// 2. Service necesita UnitOfWork y Mapper
public class PacienteService(IUnitOfWork unitOfWork, IMapper mapper) : IPacienteService
{
    // unitOfWork y mapper se inyectan automáticamente
}

// 3. UnitOfWork necesita Repositories
public class UnitOfWorkManager(
    AppDbContext context,
    IRepository<Paciente> pacientes,
    // ... más repositorios
) : IUnitOfWork
{
    // context y repositorios se inyectan automáticamente
}

// 4. Repository necesita DbContext
public class Repository<TEntity>(AppDbContext dbContext) : IRepository<TEntity>
{
    // dbContext se inyecta automáticamente
}
```

**¿Cómo funciona esto?**
1. El framework ve que `PacientesController` necesita `IPacienteService`
2. Busca cómo crear `IPacienteService` → encuentra `PacienteService`
3. Ve que `PacienteService` necesita `IUnitOfWork` y `IMapper`
4. Busca cómo crear cada uno (recursivamente)
5. Crea toda la cadena de dependencias
6. Inyecta todo en el lugar correcto

**Ventaja**: No tienes que crear nada manualmente. El framework lo hace todo automáticamente.

---

## Ubicación de los Registros de Dependencias

Cada capa tiene su propio archivo `DependencyInjection.cs`:

### Application Layer
**Archivo**: `PsicoAgenda.Application/DependencyInjection.cs`
```csharp
public static void AddApplication(this IServiceCollection services)
{
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    // ↑ Registra AutoMapper para mapeo de objetos
}
```

### Persistence Layer
**Archivo**: `PsicoAgenda.Persistence/DependencyInjection.cs`
```csharp
public static void AddPersistence(this IServiceCollection services)
{
    services.AddScoped<IUnitOfWork, UnitOfWorkManager>();
    // ↑ Registra el UnitOfWork
    
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    // ↑ Registra el repositorio genérico para todas las entidades
}
```

### Infrastructure Layer
**Archivo**: `PsicoAgenda.Infrastructure/DependencyInjection.cs`
```csharp
public static void AddInfrastructure(this IServiceCollection services)
{
    services.AddScoped<IPacienteService, PacienteService>();
    // ↑ Registra el servicio de pacientes
    // Cuando se agreguen más servicios, se registran aquí
}
```

### Api Layer (Program.cs)
**Archivo**: `PsicoAgenda.Api/Program.cs`
```csharp
builder.Services.AddDbContext<AppDbContext>(db =>
{
    db.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// ↑ Registra el contexto de base de datos

builder.Services.AddApplication();
builder.Services.AddPersistence();
builder.Services.AddInfrastructure();
// ↑ Registra todas las dependencias de todas las capas
```

---

## Ventajas de la Inyección de Dependencias

### 1. Testabilidad (Fácil de Probar)

**Sin inyección:**
```csharp
// Difícil de probar: siempre usa la base de datos real
var service = new PacienteService();
var paciente = await service.ObtenerPaciente(id);
```

**Con inyección:**
```csharp
// Fácil de probar: puedes usar un servicio "falso"
var mockService = new MockPacienteService(); // Servicio de prueba
var controller = new PacientesController(mockService);
var paciente = await controller.Get(id);
```

### 2. Flexibilidad

Puedes cambiar implementaciones sin tocar el código que las usa:
```csharp
// Cambiar de implementación es solo cambiar el registro
services.AddScoped<IPacienteService, PacienteServiceV2>();
// ↑ Cambias aquí y TODO el código sigue funcionando
```

### 3. Mantenibilidad

El código es más claro porque:
- Cada clase declara explícitamente qué necesita
- No hay creación oculta de objetos
- Es fácil ver las dependencias

### 4. Desacoplamiento

Las clases no dependen de implementaciones concretas, solo de contratos (interfaces).

---

## Resumen

1. **Inyección de dependencias**: El framework proporciona las dependencias automáticamente
2. **Interfaces**: Se usan en lugar de clases para mayor flexibilidad
3. **Registro**: Se registra en `DependencyInjection.cs` de cada capa
4. **Ciclo de vida**: Scoped (por petición), Singleton (único), Transient (nuevo cada vez)
5. **Ventajas**: Testabilidad, flexibilidad, mantenibilidad, desacoplamiento

## Próximos Pasos

- [Guía de Arquitectura](GUIA_ARQUITECTURA.md) - Ver cómo se organizan las capas
- [Flujo de Ejecución](FLUJO_EJECUCION.md) - Cómo fluyen las dependencias en una petición
- [Agregar una Funcionalidad](AGREGAR_FUNCIONALIDAD.md) - Cómo registrar nuevas dependencias

