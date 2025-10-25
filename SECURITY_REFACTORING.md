# Refactorización de Seguridad - Filtros de Pedidos

## Problema de Seguridad Identificado

La implementación original enviaba cláusulas SQL `WHERE` construidas en el frontend directamente al backend, lo cual representa un **riesgo crítico de inyección SQL**.

### Código Inseguro (Antes)

**Frontend:**
```javascript
// ❌ INSEGURO: Construyendo cláusulas SQL en el frontend
function buildWhereClause() {
  conditions.push(`year=${filters.value.year}`)
  conditions.push(`folio LIKE '%${filters.value.folio}%'`)
  conditions.push(`pedido.id_proveedor=${filters.value.supplier}`)
  return conditions.join(' AND ')
}
```

**Backend:**
```csharp
// ❌ INSEGURO: Parsing de strings SQL del frontend
public async Task<PedidoListResponse> GetAllAsync(
    int startRowIndex,
    int maximumRows,
    string? where,  // ❌ String SQL del frontend
    string? orderBy)
```

## Solución Implementada

### 1. DTO de Filtros Tipado (`PedidoFilterDto.cs`)

Creamos un DTO fuertemente tipado para los filtros:

```csharp
public class PedidoFilterDto
{
    public int? Year { get; set; }
    public string? Folio { get; set; }
    public int? IdProveedor { get; set; }
    public int? IdEstadoPedido { get; set; }
    public int? IdEstadoSurtido { get; set; }
    public int? IdTipoPedido { get; set; }
    public DateOnly? FechaDesde { get; set; }
    public DateOnly? FechaHasta { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}
```

### 2. Controlador con Parámetros Individuales

El controlador ahora recibe parámetros individuales y construye el DTO de forma segura:

```csharp
[HttpGet("ListaSelAll")]
public async Task<ActionResult<PedidoListResponse>> ListaSelAll(
    [FromQuery] int startRowIndex = 1,
    [FromQuery] int maximumRows = 10,
    [FromQuery] int? year = null,
    [FromQuery] string? folio = null,
    [FromQuery] int? idProveedor = null,
    [FromQuery] int? idEstadoSurtido = null,
    // ... más parámetros
)
{
    var filters = new PedidoFilterDto
    {
        Year = year,
        Folio = folio,
        IdProveedor = idProveedor,
        IdEstadoSurtido = idEstadoSurtido,
        // ... etc
    };
    
    var result = await _pedidoService.GetAllAsync(startRowIndex, maximumRows, filters);
    return Ok(result);
}
```

### 3. Servicio con LINQ Seguro

El servicio usa expresiones LINQ tipadas en lugar de parsing de strings:

```csharp
private IQueryable<Pedido> ApplyFilters(IQueryable<Pedido> query, PedidoFilterDto filters)
{
    // ✅ SEGURO: Expresiones LINQ tipadas
    if (filters.Year.HasValue)
    {
        query = query.Where(p => p.FechaPedido.Year == filters.Year.Value);
    }

    if (!string.IsNullOrWhiteSpace(filters.Folio))
    {
        query = query.Where(p => p.Folio != null && p.Folio.Contains(filters.Folio));
    }

    if (filters.IdProveedor.HasValue)
    {
        query = query.Where(p => p.IdProveedor == filters.IdProveedor.Value);
    }
    
    // ... más filtros
    
    return query;
}
```

### 4. Frontend con Parámetros Discretos

El frontend ahora envía parámetros individuales en lugar de cláusulas SQL:

```javascript
// ✅ SEGURO: Parámetros individuales
async function fetchPedidos(params = {}) {
  const statusMap = {
    'COMPLETO': 1,
    'PENDIENTE': 2,
    'PARCIAL': 3
  }

  const queryParams = {
    startRowIndex: params.startRowIndex || 1,
    maximumRows: params.maximumRows || 10,
    year: filters.value.year || null,
    folio: filters.value.folio || null,
    idProveedor: filters.value.supplier ? parseInt(filters.value.supplier) : null,
    idEstadoSurtido: filters.value.status ? statusMap[filters.value.status.toUpperCase()] : null,
    fechaDesde: filters.value.dateFrom || null,
    fechaHasta: filters.value.dateTo || null,
    sortBy: params.sortBy || 'fecha_pedido',
    sortDirection: params.sortDirection || 'DESC',
  }

  const response = await pedidoService.getAll(queryParams)
  // ...
}
```

## Beneficios de la Refactorización

### 🔒 Seguridad
- ✅ **Eliminación del riesgo de inyección SQL**: No se construyen strings SQL en el frontend
- ✅ **Validación de tipos**: Los parámetros son tipados y validados por ASP.NET Core
- ✅ **Expresiones LINQ seguras**: Entity Framework genera SQL parametrizado
- ✅ **Sin parsing de strings SQL**: Eliminación de errores de parsing

### 🛠️ Mantenibilidad
- ✅ **Código más limpio y legible**: Parámetros explícitos en lugar de strings concatenados
- ✅ **Fácil de extender**: Agregar nuevos filtros es simple y directo
- ✅ **Mejor documentación**: Los parámetros están documentados en el Swagger/OpenAPI
- ✅ **Intellisense completo**: Los IDEs pueden proporcionar autocompletado

### 🧪 Testabilidad
- ✅ **Pruebas unitarias más fáciles**: Los DTOs son fáciles de instanciar
- ✅ **Mock más simple**: Los parámetros tipados facilitan los mocks
- ✅ **Sin dependencias de SQL**: Las pruebas no necesitan parsear strings SQL

### 🎯 Funcionalidad
- ✅ **Todos los filtros funcionando correctamente**:
  - Filtro por año
  - Filtro por folio (búsqueda parcial)
  - Filtro por proveedor
  - Filtro por estado de surtido
  - Filtro por rango de fechas
  - Ordenamiento personalizable

## Archivos Modificados

### Backend
1. ✅ `Models/DTOs/PedidoFilterDto.cs` - **NUEVO**
2. ✅ `Services/Interfaces/IPedidoService.cs` - Refactorizado
3. ✅ `Services/Implementation/PedidoService.cs` - Refactorizado
4. ✅ `Controllers/PedidoController.cs` - Refactorizado

### Frontend
1. ✅ `services/pedidoService.js` - Refactorizado
2. ✅ `stores/pedidos.js` - Refactorizado
3. ✅ `views/PedidosView.vue` - Actualizado (valores de proveedores)

## Pruebas

Para probar la funcionalidad:

1. Accede a http://localhost:8080/
2. Inicia sesión con:
   - Email: `admin@caasim.gob.mx`
   - Password: `admin123`
3. Ve a "Consulta de Pedidos"
4. Prueba cada filtro:
   - **Año**: Selecciona 2024
   - **Folio**: Escribe "PED-2024"
   - **Proveedor**: Selecciona cualquier proveedor
   - **Estado**: Selecciona Completo/Pendiente/Parcial
   - **Fechas**: Selecciona un rango

Todos los filtros funcionarán correctamente y de forma segura.

## API Endpoint

```
GET /api/pedido/ListaSelAll?startRowIndex=1&maximumRows=10&year=2024&idEstadoSurtido=1
```

**Parámetros disponibles:**
- `startRowIndex` (int): Índice de inicio (base 1)
- `maximumRows` (int): Número máximo de registros
- `year` (int?): Filtrar por año
- `folio` (string?): Filtrar por folio (búsqueda parcial)
- `idProveedor` (int?): Filtrar por ID de proveedor
- `idEstadoPedido` (int?): Filtrar por ID de estado de pedido
- `idEstadoSurtido` (int?): Filtrar por ID de estado de surtido (1=Completo, 2=Pendiente, 3=Parcial)
- `idTipoPedido` (int?): Filtrar por ID de tipo de pedido
- `fechaDesde` (string?): Filtrar desde fecha (YYYY-MM-DD)
- `fechaHasta` (string?): Filtrar hasta fecha (YYYY-MM-DD)
- `sortBy` (string?): Campo de ordenamiento
- `sortDirection` (string?): Dirección de ordenamiento (ASC/DESC)

## Conclusión

Esta refactorización elimina completamente el riesgo de inyección SQL y mejora significativamente la calidad, mantenibilidad y seguridad del código. El sistema ahora sigue las mejores prácticas de seguridad de la industria.

