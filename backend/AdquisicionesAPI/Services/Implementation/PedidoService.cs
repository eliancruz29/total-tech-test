using Microsoft.EntityFrameworkCore;
using AdquisicionesAPI.Data;
using AdquisicionesAPI.Models.DTOs;
using AdquisicionesAPI.Models.Entities;
using AdquisicionesAPI.Services.Interfaces;

namespace AdquisicionesAPI.Services.Implementation;

public class PedidoService : IPedidoService
{
    private readonly AdquisicionesDbContext _context;
    private readonly ILogger<PedidoService> _logger;

    public PedidoService(AdquisicionesDbContext context, ILogger<PedidoService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PedidoListResponse> GetAllAsync(
        int startRowIndex,
        int maximumRows,
        string? where,
        string? orderBy)
    {
        try
        {
            var query = _context.Pedidos
                .Include(p => p.Proveedor)
                .Include(p => p.TipoPedido)
                .Include(p => p.EstadoPedido)
                .Include(p => p.EstadoSurtido)
                .AsQueryable();

            // Apply WHERE clause if provided
            if (!string.IsNullOrEmpty(where))
            {
                query = ApplyWhereClause(query, where);
            }

            // Get total count before pagination
            var totalRecords = await query.CountAsync();

            // Apply ORDER BY clause
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = ApplyOrderByClause(query, orderBy);
            }
            else
            {
                query = query.OrderByDescending(p => p.FechaPedido);
            }

            // Apply pagination
            var skip = Math.Max(0, startRowIndex - 1);
            query = query.Skip(skip).Take(maximumRows);

            var pedidos = await query.ToListAsync();

            var pedidosDto = pedidos.Select(p => new PedidoListDto
            {
                IdPedido = p.IdPedido,
                Folio = p.Folio,
                FechaPedido = p.FechaPedido.ToString("yyyy-MM-dd"),
                TipoPedido = p.TipoPedido?.Descripcion,
                Iniciales = p.Iniciales,
                ProveedorRazonSocial = p.Proveedor?.RazonSocial ?? "",
                ProveedorRfc = p.Proveedor?.Rfc ?? "",
                MontoTotal = p.MontoTotal,
                EstadoPedido = p.EstadoPedido?.Descripcion ?? "",
                EstadoSurtido = p.EstadoSurtido?.Descripcion,
                Observaciones = p.Observaciones
            }).ToList();

            return new PedidoListResponse
            {
                Data = pedidosDto,
                TotalRecords = totalRecords,
                PageNumber = (skip / maximumRows) + 1,
                PageSize = maximumRows
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pedidos list");
            throw;
        }
    }

    public async Task<PedidoDto?> GetByIdAsync(int id)
    {
        try
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Proveedor)
                .Include(p => p.TipoDocumentoPedido)
                .Include(p => p.TipoPedido)
                .Include(p => p.EstadoPedido)
                .Include(p => p.EstadoSurtido)
                .Include(p => p.ProcedimientoAdquisicion)
                .Include(p => p.PedidoDetalles)
                    .ThenInclude(d => d.Insumo)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
                return null;

            return MapToDto(pedido);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pedido by ID: {Id}", id);
            throw;
        }
    }

    public async Task<PedidoDto> CreateAsync(PedidoDto pedidoDto)
    {
        try
        {
            var pedido = MapToEntity(pedidoDto);
            pedido.FechaRegistro = DateOnly.FromDateTime(DateTime.Now);
            pedido.HoraRegistro = TimeOnly.FromDateTime(DateTime.Now);

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(pedido.IdPedido) ?? pedidoDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating pedido");
            throw;
        }
    }

    public async Task<PedidoDto?> UpdateAsync(int id, PedidoDto pedidoDto)
    {
        try
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return null;

            UpdateEntityFromDto(pedido, pedidoDto);
            pedido.FechaModifica = DateOnly.FromDateTime(DateTime.Now);
            pedido.HoraModifica = TimeOnly.FromDateTime(DateTime.Now);

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating pedido ID: {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return false;

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting pedido ID: {Id}", id);
            throw;
        }
    }

    #region Private Helper Methods

    private IQueryable<Pedido> ApplyWhereClause(IQueryable<Pedido> query, string where)
    {
        // Enhanced WHERE clause parser that handles multiple conditions and data types
        // Supports: field=value, field='value', multiple conditions with AND/OR

        if (string.IsNullOrWhiteSpace(where))
            return query;

        try
        {
            // Split by AND/OR operators (simple implementation)
            var conditions = where.Split(new[] { " AND ", " and " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var condition in conditions)
            {
                var trimmedCondition = condition.Trim();

                // Parse field=value or field='value'
                if (trimmedCondition.Contains("="))
                {
                    var parts = trimmedCondition.Split('=');
                    if (parts.Length != 2) continue;

                    var field = parts[0].Trim().ToLower();
                    var value = parts[1].Trim().Trim('\'', '"'); // Remove quotes if present

                    // Handle different field types
                    if (field.Contains("id_estado_pedido"))
                    {
                        if (int.TryParse(value, out int intValue))
                        {
                            query = query.Where(p => p.IdEstadoPedido == intValue);
                        }
                    }
                    else if (field.Contains("id_proveedor"))
                    {
                        if (int.TryParse(value, out int intValue))
                        {
                            query = query.Where(p => p.IdProveedor == intValue);
                        }
                    }
                    else if (field.Contains("id_tipo_pedido"))
                    {
                        if (int.TryParse(value, out int intValue))
                        {
                            query = query.Where(p => p.IdTipoPedido == intValue);
                        }
                    }
                    else if (field.Contains("id_estado_surtido"))
                    {
                        if (int.TryParse(value, out int intValue))
                        {
                            query = query.Where(p => p.IdEstadoSurtido == intValue);
                        }
                    }
                    else if (field.Contains("folio"))
                    {
                        query = query.Where(p => p.Folio != null && p.Folio.Contains(value));
                    }
                    else if (field.Contains("numero_contrato"))
                    {
                        query = query.Where(p => p.NumeroContrato != null && p.NumeroContrato.Contains(value));
                    }
                    else if (field.Contains("observaciones"))
                    {
                        query = query.Where(p => p.Observaciones != null && p.Observaciones.Contains(value));
                    }
                }
                // Handle LIKE operator
                else if (trimmedCondition.ToUpper().Contains(" LIKE "))
                {
                    var parts = trimmedCondition.Split(new[] { " LIKE ", " like " }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        var field = parts[0].Trim().ToLower();
                        var value = parts[1].Trim().Trim('\'', '"', '%');

                        if (field.Contains("folio"))
                        {
                            query = query.Where(p => p.Folio != null && p.Folio.Contains(value));
                        }
                        else if (field.Contains("numero_contrato"))
                        {
                            query = query.Where(p => p.NumeroContrato != null && p.NumeroContrato.Contains(value));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error parsing WHERE clause: {Where}. Returning unfiltered results.", where);
            // Return query without filtering if parsing fails
        }

        return query;
    }

    private IQueryable<Pedido> ApplyOrderByClause(IQueryable<Pedido> query, string orderBy)
    {
        // Simple ORDER BY parser
        // Example: "pedido.id_pedido ASC" or "pedido.fecha_pedido DESC"
        var parts = orderBy.Split(' ');
        var field = parts[0].ToLower();
        var direction = parts.Length > 1 ? parts[1].ToUpper() : "ASC";

        query = field switch
        {
            var f when f.Contains("id_pedido") => direction == "ASC"
                ? query.OrderBy(p => p.IdPedido)
                : query.OrderByDescending(p => p.IdPedido),
            var f when f.Contains("fecha_pedido") => direction == "ASC"
                ? query.OrderBy(p => p.FechaPedido)
                : query.OrderByDescending(p => p.FechaPedido),
            var f when f.Contains("folio") => direction == "ASC"
                ? query.OrderBy(p => p.Folio)
                : query.OrderByDescending(p => p.Folio),
            _ => query.OrderByDescending(p => p.FechaPedido)
        };

        return query;
    }

    private PedidoDto MapToDto(Pedido pedido)
    {
        return new PedidoDto
        {
            IdPedido = pedido.IdPedido,
            Folio = pedido.Folio,
            ConsecutivoCompleto = pedido.ConsecutivoCompleto,
            IdTipoDocumentoPedido = pedido.IdTipoDocumentoPedido,
            IdTipoPedido = pedido.IdTipoPedido,
            IdProveedor = pedido.IdProveedor,
            IdProcedimientoAdquisicion = pedido.IdProcedimientoAdquisicion,
            FechaPedido = pedido.FechaPedido.ToString("yyyy-MM-dd"),
            NumeroContrato = pedido.NumeroContrato,
            DestinatarioFactura = pedido.DestinatarioFactura,
            DireccionEntrega = pedido.DireccionEntrega,
            FechaEntrega = pedido.FechaEntrega?.ToString("yyyy-MM-dd"),
            TiempoEntrega = pedido.TiempoEntrega,
            PersonaElaboro = pedido.PersonaElaboro,
            PersonaAutorizo = pedido.PersonaAutorizo,
            Iniciales = pedido.Iniciales,
            Subtotal = pedido.Subtotal,
            TotalIva = pedido.TotalIva,
            TotalRetenciones = pedido.TotalRetenciones,
            MontoTotal = pedido.MontoTotal,
            IdEstadoPedido = pedido.IdEstadoPedido,
            IdEstadoSurtido = pedido.IdEstadoSurtido,
            Observaciones = pedido.Observaciones,
            FechaRegistro = pedido.FechaRegistro.ToString("yyyy-MM-dd"),
            HoraRegistro = pedido.HoraRegistro.ToString("HH:mm:ss"),
            IdUsuarioRegistro = pedido.IdUsuarioRegistro,
            FechaModifica = pedido.FechaModifica?.ToString("yyyy-MM-dd"),
            HoraModifica = pedido.HoraModifica?.ToString("HH:mm:ss"),
            IdUsuarioModifica = pedido.IdUsuarioModifica,
            FechaAprueba = pedido.FechaAprueba?.ToString("yyyy-MM-dd"),
            HoraAprueba = pedido.HoraAprueba?.ToString("HH:mm:ss"),
            IdUsuarioAprueba = pedido.IdUsuarioAprueba,
            IdArchivoFirma = pedido.IdArchivoFirma,
            TipoDocumentoPedidoDescripcion = pedido.TipoDocumentoPedido?.Descripcion,
            TipoPedidoDescripcion = pedido.TipoPedido?.Descripcion,
            ProveedorRazonSocial = pedido.Proveedor?.RazonSocial,
            ProveedorRfc = pedido.Proveedor?.Rfc,
            EstadoPedidoDescripcion = pedido.EstadoPedido?.Descripcion,
            EstadoSurtidoDescripcion = pedido.EstadoSurtido?.Descripcion,
            Detalles = pedido.PedidoDetalles.Select(d => new PedidoDetalleDto
            {
                IdPedidoDetalle = d.IdPedidoDetalle,
                IdPedido = d.IdPedido,
                IdRequisicionDetalle = d.IdRequisicionDetalle,
                IdRequisicion = d.IdRequisicion,
                ClavePresupuestal = d.ClavePresupuestal,
                NombrePartida = d.NombrePartida,
                IdInsumo = d.IdInsumo,
                Descripcion = d.Descripcion,
                NumeroPartida = d.NumeroPartida,
                Anio = d.Anio,
                Cantidad = d.Cantidad,
                CantidadSurtida = d.CantidadSurtida,
                Unidad = d.Unidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal,
                Iva = d.Iva,
                Retenciones = d.Retenciones,
                Total = d.Total
            }).ToList()
        };
    }

    private Pedido MapToEntity(PedidoDto dto)
    {
        return new Pedido
        {
            Folio = dto.Folio,
            ConsecutivoCompleto = dto.ConsecutivoCompleto,
            IdTipoDocumentoPedido = dto.IdTipoDocumentoPedido,
            IdTipoPedido = dto.IdTipoPedido,
            IdProveedor = dto.IdProveedor,
            IdProcedimientoAdquisicion = dto.IdProcedimientoAdquisicion,
            FechaPedido = DateOnly.Parse(dto.FechaPedido),
            NumeroContrato = dto.NumeroContrato,
            DestinatarioFactura = dto.DestinatarioFactura,
            DireccionEntrega = dto.DireccionEntrega,
            FechaEntrega = string.IsNullOrEmpty(dto.FechaEntrega) ? null : DateOnly.Parse(dto.FechaEntrega),
            TiempoEntrega = dto.TiempoEntrega,
            PersonaElaboro = dto.PersonaElaboro,
            PersonaAutorizo = dto.PersonaAutorizo,
            Iniciales = dto.Iniciales,
            Subtotal = dto.Subtotal,
            TotalIva = dto.TotalIva,
            TotalRetenciones = dto.TotalRetenciones,
            MontoTotal = dto.MontoTotal,
            IdEstadoPedido = dto.IdEstadoPedido,
            IdEstadoSurtido = dto.IdEstadoSurtido,
            Observaciones = dto.Observaciones,
            FechaRegistro = DateOnly.Parse(dto.FechaRegistro),
            HoraRegistro = TimeOnly.Parse(dto.HoraRegistro),
            IdUsuarioRegistro = dto.IdUsuarioRegistro,
            FechaModifica = string.IsNullOrEmpty(dto.FechaModifica) ? null : DateOnly.Parse(dto.FechaModifica),
            HoraModifica = string.IsNullOrEmpty(dto.HoraModifica) ? null : TimeOnly.Parse(dto.HoraModifica),
            IdUsuarioModifica = dto.IdUsuarioModifica,
            FechaAprueba = string.IsNullOrEmpty(dto.FechaAprueba) ? null : DateOnly.Parse(dto.FechaAprueba),
            HoraAprueba = string.IsNullOrEmpty(dto.HoraAprueba) ? null : TimeOnly.Parse(dto.HoraAprueba),
            IdUsuarioAprueba = dto.IdUsuarioAprueba,
            IdArchivoFirma = dto.IdArchivoFirma
        };
    }

    private void UpdateEntityFromDto(Pedido pedido, PedidoDto dto)
    {
        pedido.Folio = dto.Folio;
        pedido.ConsecutivoCompleto = dto.ConsecutivoCompleto;
        pedido.IdTipoDocumentoPedido = dto.IdTipoDocumentoPedido;
        pedido.IdTipoPedido = dto.IdTipoPedido;
        pedido.IdProveedor = dto.IdProveedor;
        pedido.IdProcedimientoAdquisicion = dto.IdProcedimientoAdquisicion;
        pedido.FechaPedido = DateOnly.Parse(dto.FechaPedido);
        pedido.NumeroContrato = dto.NumeroContrato;
        pedido.DestinatarioFactura = dto.DestinatarioFactura;
        pedido.DireccionEntrega = dto.DireccionEntrega;
        pedido.FechaEntrega = string.IsNullOrEmpty(dto.FechaEntrega) ? null : DateOnly.Parse(dto.FechaEntrega);
        pedido.TiempoEntrega = dto.TiempoEntrega;
        pedido.PersonaElaboro = dto.PersonaElaboro;
        pedido.PersonaAutorizo = dto.PersonaAutorizo;
        pedido.Iniciales = dto.Iniciales;
        pedido.Subtotal = dto.Subtotal;
        pedido.TotalIva = dto.TotalIva;
        pedido.TotalRetenciones = dto.TotalRetenciones;
        pedido.MontoTotal = dto.MontoTotal;
        pedido.IdEstadoPedido = dto.IdEstadoPedido;
        pedido.IdEstadoSurtido = dto.IdEstadoSurtido;
        pedido.Observaciones = dto.Observaciones;
        pedido.IdUsuarioModifica = dto.IdUsuarioModifica;
        pedido.FechaAprueba = string.IsNullOrEmpty(dto.FechaAprueba) ? null : DateOnly.Parse(dto.FechaAprueba);
        pedido.HoraAprueba = string.IsNullOrEmpty(dto.HoraAprueba) ? null : TimeOnly.Parse(dto.HoraAprueba);
        pedido.IdUsuarioAprueba = dto.IdUsuarioAprueba;
        pedido.IdArchivoFirma = dto.IdArchivoFirma;
    }

    #endregion
}
