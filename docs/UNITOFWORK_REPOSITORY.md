# Patrón UnitOfWork y Repository - Guía Completa

## Introducción

Este documento explica en detalle cómo funcionan los patrones **UnitOfWork** y **Repository** en el proyecto PsicoAgenda, y cómo se relacionan entre sí. Estos patrones son fundamentales para el acceso a datos en la aplicación.

## ¿Qué Problema Resuelven?

### Sin estos patrones (problemas)

**Problema 1: Código desorganizado**
```csharp
// Código directo sin patrones - MALO
var paciente = dbContext.Pacientes.Find(id);
var cita = dbContext.Citas.Add(nuevaCita);
dbContext.SaveChanges(); // ¿Cuándo guardar?
```

**Problema 2: Múltiples guardados**
```csharp
// Si necesitas hacer múltiples operaciones:
dbContext.Pacientes.Add(paciente);
dbContext.SaveChanges(); // Guarda 1

dbContext.Citas.Add(cita);
dbContext.SaveChanges(); // Guarda 2

// Si falla el segundo guardado, el primero ya se guardó
// ¡Datos inconsistentes!
```

**Problema 3: Difícil de probar**
- Dependes directamente de Entity Framework
- No puedes "simular" el acceso a datos
- Las pruebas requieren base de datos real

### Con estos patrones (soluciones)

**Solución 1: Código organizado**
```csharp
// Con Repository y UnitOfWork - BUENO
await unitOfWork.Pacientes.Crear(paciente);
await unitOfWork.Citas.Crear(cita);
await unitOfWork.GuardarCambios(); // Guarda todo junto
```

**Solución 2: Transacciones seguras**
```csharp
// Todo o nada
await unitOfWork.IniciarTransaccion();
await unitOfWork.Pacientes.Crear(paciente);
await unitOfWork.Citas.Crear(cita);
await unitOfWork.GuardarCambios();
await unitOfWork.ConfirmarTransaccion(); // Si algo falla, se cancela todo
```

**Solución 3: Fácil de probar**
- Puedes crear implementaciones "falsas" de los repositorios
- Las pruebas no necesitan base de datos

---

## ¿Qué es el Patrón Repository?

### Concepto Simple

El **Repository** es como un "cajero especializado" para cada tipo de entidad. Cada entidad tiene su propio repositorio que sabe cómo guardarla, buscarla, actualizarla y eliminarla.

**Analogía del Restaurante:**
- **Repository de Pacientes**: Como un mozo especializado solo en servir platos principales
- **Repository de Citas**: Como un mozo especializado solo en servir postres
- Cada uno sabe exactamente cómo manejar su tipo de objeto

### En el Proyecto

**Archivo**: `PsicoAgenda.Persistence/Repositories/Repository.cs`

```csharp
public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntidadBase
{
    private readonly AppDbContext dbContext;
    
    // Operaciones básicas que TODA entidad puede hacer:
    
    // 1. CREAR
    public async Task Crear(TEntity entity)
    {
        await dbContext.Set<TEntity>().AddAsync(entity);
        // ↑ Agrega a memoria, pero NO guarda aún
    }
    
    // 2. LEER (por ID)
    public async Task<TEntity?> SeleccionarPorId(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        // ↑ Busca y retorna inmediatamente (consulta a BD)
    }
    
    // 3. LEER (todos)
    public async Task<IEnumerable<TEntity>> SeleccionarTodos(CancellationToken cancellationToken)
    {
        return await dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }
    
    // 4. LEER (con condición)
    public async Task<TEntity?> SeleccionarPor(Expression<Func<TEntity, bool>> predicate, ...)
    {
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    // 5. ACTUALIZAR
    public void Actualizar(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
        // ↑ Marca para actualizar, pero NO guarda aún
    }
    
    // 6. ELIMINAR
    public void Eliminar(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
        // ↑ Marca para eliminar, pero NO elimina aún
    }
}
```

### Características Importantes del Repository

1. **Es Genérico**: `Repository<TEntity>` funciona para cualquier entidad
   - `Repository<Paciente>` para pacientes
   - `Repository<Cita>` para citas
   - `Repository<Sesion>` para sesiones

2. **Solo Prepara Operaciones**: NO guarda directamente
   - `Crear()` agrega a memoria
   - `Actualizar()` marca para actualizar
   - `Eliminar()` marca para eliminar
   - **Pero NO llama a `SaveChanges()`**

3. **Lee Inmediatamente**: Las operaciones de lectura sí van a la base de datos
   - `SeleccionarPorId()` consulta la BD inmediatamente
   - `SeleccionarTodos()` consulta la BD inmediatamente

---

## ¿Qué es el Patrón UnitOfWork?

### Concepto Simple

El **UnitOfWork** es como un "coordinador" que:
- Agrupa todos los repositorios en un solo lugar
- Controla CUÁNDO se guardan todos los cambios
- Maneja transacciones (todo o nada)

**Analogía del Restaurante:**
- **UnitOfWork**: Es el mesero principal que coordina a todos los mozos especializados
- Él toma todos los pedidos de diferentes mesas
- Al final, va a la cocina y entrega TODO junto
- Si algo está mal, puede cancelar TODO el pedido

### En el Proyecto

**Archivo**: `PsicoAgenda.Persistence/UnitOfWork/UnitOfWorkManager.cs`

```csharp
public class UnitOfWorkManager : IUnitOfWork
{
    private readonly AppDbContext context;
    
    // Tiene acceso a TODOS los repositorios
    public IRepository<Paciente> Pacientes { get; }
    public IRepository<Cita> Citas { get; }
    public IRepository<Sesion> Sesiones { get; }
    // ... más repositorios
    
    // Método clave: GUARDAR TODOS LOS CAMBIOS
    public async Task<int> GuardarCambios(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
        // ↑ Aquí es donde REALMENTE se guarda en la base de datos
    }
    
    // Manejo de transacciones
    public async Task IniciarTransaccion()
    {
        _transaction = await context.Database.BeginTransactionAsync();
    }
    
    public async Task ConfirmarTransaccion()
    {
        await _transaction!.CommitAsync();
        // ↑ Confirma todos los cambios (todo o nada)
    }
    
    public async Task CancelarTransaccion()
    {
        await _transaction!.RollbackAsync();
        // ↑ Cancela todos los cambios si algo salió mal
    }
}
```

### Características Importantes del UnitOfWork

1. **Centraliza los Repositorios**: Un solo lugar para acceder a todos
   ```csharp
   // En lugar de tener múltiples repositorios separados:
   var repoPacientes = new Repository<Paciente>();
   var repoCitas = new Repository<Cita>();
   
   // Tienes uno solo que los agrupa:
   unitOfWork.Pacientes.Crear(...);
   unitOfWork.Citas.Crear(...);
   ```

2. **Controla el Guardado**: Solo hay un método `GuardarCambios()`
   ```csharp
   // Puedes hacer múltiples operaciones
   await unitOfWork.Pacientes.Crear(paciente1);
   await unitOfWork.Pacientes.Crear(paciente2);
   await unitOfWork.Citas.Crear(cita);
   
   // Y guardar TODO junto en una sola operación
   await unitOfWork.GuardarCambios();
   ```

3. **Maneja Transacciones**: Permite operaciones "todo o nada"
   ```csharp
   await unitOfWork.IniciarTransaccion();
   // ... múltiples operaciones ...
   await unitOfWork.ConfirmarTransaccion(); // O CancelarTransaccion()
   ```

---

## ¿Cómo se Relacionan Repository y UnitOfWork?

### Relación Visual

```
┌─────────────────────────────────────────┐
│         SERVICE (PacienteService)       │
│  • Lógica de negocio                   │
└──────────────┬──────────────────────────┘
               │ usa
               ▼
┌─────────────────────────────────────────┐
│         UNIT OF WORK                     │
│  • Coordinador de repositorios          │
│  • Controla cuándo guardar             │
│  • Maneja transacciones                 │
│                                         │
│  ┌──────────────────────────────────┐ │
│  │  Pacientes (Repository<Paciente>)│ │
│  └──────────────────────────────────┘ │
│  ┌──────────────────────────────────┐ │
│  │  Citas (Repository<Cita>)        │ │
│  └──────────────────────────────────┘ │
│  ┌──────────────────────────────────┐ │
│  │  Sesiones (Repository<Sesion>)    │ │
│  └──────────────────────────────────┘ │
└──────────────┬──────────────────────────┘
               │ cada repositorio usa
               ▼
┌─────────────────────────────────────────┐
│       REPOSITORY<TEntity> (genérico)    │
│  • Operaciones CRUD                     │
│  • NO guarda (solo prepara)            │
└──────────────┬──────────────────────────┘
               │ usa
               ▼
┌─────────────────────────────────────────┐
│         APPDBCONTEXT                     │
│  • Conexión a base de datos             │
│  • Ejecuta consultas SQL               │
└─────────────────────────────────────────┘
```

### Flujo de Trabajo Real

**Ejemplo: Crear un paciente y una cita relacionada**

```csharp
// 1. Service usa UnitOfWork
public async Task CrearPacienteConCita(CrearRequest request)
{
    // 2. UnitOfWork expone repositorios
    // Crear paciente
    var paciente = mapper.Map<Paciente>(request.Paciente);
    await unitOfWork.Pacientes.Crear(paciente);
    // ↑ Repository prepara (agrega a memoria), NO guarda
    
    // Crear cita
    var cita = mapper.Map<Cita>(request.Cita);
    cita.PacienteId = paciente.Id;
    await unitOfWork.Citas.Crear(cita);
    // ↑ Repository prepara (agrega a memoria), NO guarda
    
    // 3. UnitOfWork guarda TODO junto
    await unitOfWork.GuardarCambios();
    // ↑ AQUÍ es donde realmente se ejecuta:
    //    - INSERT INTO Pacientes ...
    //    - INSERT INTO Citas ...
    //    TODO en la misma operación
}
```

### ¿Por Qué Esta Separación?

**Repository (Operaciones Individuales)**
- Se enfoca en UNA entidad a la vez
- Proporciona operaciones CRUD estándar
- No sabe de otras entidades

**UnitOfWork (Coordinación)**
- Conoce TODAS las entidades
- Coordina múltiples repositorios
- Controla el momento de guardar
- Maneja transacciones complejas

---

## Ejemplos Prácticos en el Proyecto

### Ejemplo 1: Operación Simple (Solo Lectura)

**Código en Service:**
```csharp
public async Task<PacienteRespuesta> ObtenerPaciente(Guid id, CancellationToken cancellationToken)
{
    // 1. Usa UnitOfWork para acceder al repositorio
    var paciente = await unitOfWork.Pacientes.SeleccionarPorId(id, cancellationToken);
    //    ↑ UnitOfWork.Pacientes → Repository<Paciente>
    //    ↑ SeleccionarPorId() → consulta BD inmediatamente
    
    // 2. Transforma y retorna
    return mapper.Map<PacienteRespuesta>(paciente);
}
```

**Flujo:**
```
Service → UnitOfWork.Pacientes → Repository<Paciente> → DbContext → PostgreSQL
         (coordinador)          (operaciones)        (conexión)    (datos)
```

**No necesita GuardarCambios()** porque es solo lectura.

---

### Ejemplo 2: Operación de Escritura (Crear)

**Código en Service:**
```csharp
public async Task CrearPaciente(PacienteCreacion request)
{
    // 1. Transforma DTO a entidad
    var paciente = mapper.Map<Paciente>(request);
    
    // 2. Usa Repository a través de UnitOfWork
    await unitOfWork.Pacientes.Crear(paciente);
    //    ↑ Esto solo prepara (agrega a memoria)
    //    ↑ NO guarda en la BD aún
    
    // 3. IMPORTANTE: Guardar cambios
    await unitOfWork.GuardarCambios();
    //    ↑ AQUÍ es donde realmente se guarda en PostgreSQL
    //    ↑ Ejecuta: INSERT INTO Pacientes ...
}
```

**Flujo:**
```
Service → UnitOfWork.Pacientes.Crear() → Repository.Crear()
                                           ↓ (prepara en memoria)
Service → UnitOfWork.GuardarCambios() → DbContext.SaveChanges()
                                           ↓ (ejecuta SQL)
                                         PostgreSQL
```

**Nota crítica**: Sin `GuardarCambios()`, los datos NO se guardan en la base de datos.

---

### Ejemplo 3: Operación Compleja (Múltiples Entidades)

**Escenario**: Crear un paciente y su primera cita al mismo tiempo.

**Código en Service:**
```csharp
public async Task CrearPacienteConPrimeraCita(CrearRequest request)
{
    // 1. Crear paciente
    var paciente = mapper.Map<Paciente>(request.Paciente);
    await unitOfWork.Pacientes.Crear(paciente);
    // ↑ Preparado, pero NO guardado
    
    // 2. Crear cita relacionada
    var cita = mapper.Map<Cita>(request.Cita);
    cita.PacienteId = paciente.Id;
    await unitOfWork.Citas.Crear(cita);
    // ↑ Preparado, pero NO guardado
    
    // 3. Guardar TODO junto
    await unitOfWork.GuardarCambios();
    // ↑ Ejecuta:
    //    INSERT INTO Pacientes ...
    //    INSERT INTO Citas ...
    //    TODO en una sola operación
    
    // Si algo falla aquí, NADA se guarda (consistencia)
}
```

**Ventajas:**
- Si falla la creación de la cita, el paciente tampoco se crea (consistencia)
- Una sola operación a la base de datos (más eficiente)
- Todo o nada (transacción implícita)

---

### Ejemplo 4: Con Transacción Explícita

**Escenario**: Operación que debe ser completamente atómica (todo o nada).

**Código en Service:**
```csharp
public async Task TransferirCitaDePaciente(Guid citaId, Guid nuevoPacienteId)
{
    try
    {
        // 1. Iniciar transacción explícita
        await unitOfWork.IniciarTransaccion();
        
        // 2. Buscar la cita
        var cita = await unitOfWork.Citas.SeleccionarPorId(citaId, cancellationToken);
        if (cita == null) throw new Exception("Cita no encontrada");
        
        // 3. Actualizar paciente de la cita
        cita.PacienteId = nuevoPacienteId;
        unitOfWork.Citas.Actualizar(cita);
        
        // 4. Actualizar historial (ejemplo)
        // ... otras operaciones ...
        
        // 5. Guardar cambios
        await unitOfWork.GuardarCambios();
        
        // 6. Confirmar transacción
        await unitOfWork.ConfirmarTransaccion();
        // ↑ Si llegamos aquí, TODO se confirmó
    }
    catch (Exception)
    {
        // 7. Si algo falla, cancelar TODO
        await unitOfWork.CancelarTransaccion();
        // ↑ Revierte todos los cambios
        throw;
    }
}
```

**Flujo con Transacción:**
```
IniciarTransaccion() → BEGIN TRANSACTION (PostgreSQL)
    ↓
Operaciones múltiples (preparadas)
    ↓
GuardarCambios() → Ejecuta SQL (dentro de la transacción)
    ↓
ConfirmarTransaccion() → COMMIT (confirma todo)
    O
CancelarTransaccion() → ROLLBACK (cancela todo)
```

---

## Registro de Dependencias

### Cómo se Configuran

**Archivo**: `PsicoAgenda.Persistence/DependencyInjection.cs`

```csharp
public static void AddPersistence(this IServiceCollection services)
{
    // 1. Registra UnitOfWork como Scoped
    services.AddScoped<IUnitOfWork, UnitOfWorkManager>();
    // ↑ Una instancia por petición HTTP
    
    // 2. Registra Repository genérico como Scoped
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    // ↑ Esto significa: "Para cualquier IRepository<TEntity>, usa Repository<TEntity>"
    //   Cuando se pide IRepository<Paciente>, se crea Repository<Paciente>
    //   Cuando se pide IRepository<Cita>, se crea Repository<Cita>
    //   etc.
}
```

### Inyección en UnitOfWork

**Archivo**: `PsicoAgenda.Persistence/UnitOfWork/UnitOfWorkManager.cs`

```csharp
public class UnitOfWorkManager : IUnitOfWork
{
    // El constructor recibe todos los repositorios inyectados
    public UnitOfWorkManager(
        AppDbContext context,
        IRepository<Cita> citas,              // ← Inyectado automáticamente
        IRepository<Cuestionario> cuestionarios, // ← Inyectado automáticamente
        IRepository<Paciente> pacientes,      // ← Inyectado automáticamente
        // ... más repositorios
    )
    {
        Citas = citas;
        Pacientes = pacientes;
        // ...
    }
}
```

**¿Cómo funciona esto?**
1. Al crear `UnitOfWorkManager`, el sistema ve que necesita `IRepository<Paciente>`
2. Busca en el registro y encuentra que debe usar `Repository<Paciente>`
3. Crea una instancia de `Repository<Paciente>` con el `AppDbContext`
4. La inyecta en el constructor de `UnitOfWorkManager`
5. Lo mismo para todos los demás repositorios

---

## Diferencias Clave: Repository vs UnitOfWork

| Aspecto | Repository | UnitOfWork |
|---------|-----------|------------|
| **Propósito** | Operaciones CRUD de UNA entidad | Coordinación de MÚLTIPLES entidades |
| **Alcance** | Específico (`Repository<Paciente>`) | General (todos los repositorios) |
| **Guardado** | NO guarda, solo prepara | Controla cuándo guardar |
| **Transacciones** | No maneja | Sí maneja |
| **Uso** | `repository.Crear(...)` | `unitOfWork.Pacientes.Crear(...)` |

### Analogía Final

Imagina que estás organizando una fiesta:

- **Repository**: Es como un ayudante especializado
  - Un ayudante para comida
  - Un ayudante para bebidas
  - Cada uno prepara su parte

- **UnitOfWork**: Es como el organizador principal
  - Coordina a todos los ayudantes
  - Decide cuándo traer TODO junto
  - Si algo falla, puede cancelar toda la fiesta

---

## Buenas Prácticas

### ✅ Hacer

1. **Siempre usar UnitOfWork en Services**
   ```csharp
   // ✅ BIEN
   await unitOfWork.Pacientes.Crear(paciente);
   await unitOfWork.GuardarCambios();
   ```

2. **Una sola llamada a GuardarCambios() por operación**
   ```csharp
   // ✅ BIEN
   await unitOfWork.Pacientes.Crear(p1);
   await unitOfWork.Citas.Crear(c1);
   await unitOfWork.GuardarCambios(); // Una vez al final
   ```

3. **Usar transacciones para operaciones complejas**
   ```csharp
   // ✅ BIEN (para operaciones críticas)
   await unitOfWork.IniciarTransaccion();
   // ... operaciones ...
   await unitOfWork.ConfirmarTransaccion();
   ```

### ❌ No Hacer

1. **NO crear repositorios directamente**
   ```csharp
   // ❌ MAL
   var repo = new Repository<Paciente>(context);
   ```

2. **NO olvidar GuardarCambios()**
   ```csharp
   // ❌ MAL - Los datos NO se guardan
   await unitOfWork.Pacientes.Crear(paciente);
   // Falta: await unitOfWork.GuardarCambios();
   ```

3. **NO usar múltiples GuardarCambios() innecesariamente**
   ```csharp
   // ❌ MAL - Ineficiente
   await unitOfWork.Pacientes.Crear(p1);
   await unitOfWork.GuardarCambios();
   await unitOfWork.Citas.Crear(c1);
   await unitOfWork.GuardarCambios(); // Mejor guardar todo junto
   ```

---

## Preguntas Frecuentes

### ¿Por qué Repository NO guarda automáticamente?

Para permitir hacer múltiples operaciones y guardarlas todas juntas. Si guardara automáticamente, cada operación sería una transacción separada.

### ¿Puedo usar Repository sin UnitOfWork?

Técnicamente sí, pero NO es recomendado. Pierdes la coordinación y el control de transacciones.

### ¿Cuándo usar transacciones explícitas?

Cuando tienes operaciones que DEBEN ser "todo o nada". Por defecto, `GuardarCambios()` ya es una transacción implícita.

### ¿Qué pasa si no llamo GuardarCambios()?

Los cambios NO se guardan en la base de datos. Quedan solo en memoria y se pierden al terminar la petición.

### ¿Puedo tener múltiples UnitOfWork en una petición?

No es recomendado. UnitOfWork está configurado como Scoped, así que hay una instancia por petición HTTP.

---

## Resumen

1. **Repository**: Operaciones CRUD específicas por entidad. Solo prepara, NO guarda.

2. **UnitOfWork**: Coordinador que agrupa repositorios y controla cuándo guardar.

3. **Relación**: Service → UnitOfWork → Repository → DbContext → PostgreSQL

4. **Flujo típico**: 
   - Preparar operaciones (Repository)
   - Guardar todo junto (UnitOfWork.GuardarCambios())

5. **Transacciones**: UnitOfWork maneja transacciones para operaciones "todo o nada".

---

## Próximos Pasos

- [Flujo de Ejecución](FLUJO_EJECUCION.md) - Ver cómo se usan en una petición real
- [Inyección de Dependencias](INYECCION_DEPENDENCIAS.md) - Cómo se registran
- [Agregar Funcionalidad](AGREGAR_FUNCIONALIDAD.md) - Cómo usarlos al agregar features
- [Referencias del Proyecto](REFERENCIAS_PROYECTO.md) - Más sobre patrones de diseño

