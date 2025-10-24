using AdquisicionesAPI.Models.DTOs;

namespace AdquisicionesAPI.Services.Interfaces;

public interface IPedidoService
{
    Task<PedidoListResponse> GetAllAsync(
        int startRowIndex,
        int maximumRows,
        string? where,
        string? orderBy);

    Task<PedidoDto?> GetByIdAsync(int id);

    Task<PedidoDto> CreateAsync(PedidoDto pedidoDto);

    Task<PedidoDto?> UpdateAsync(int id, PedidoDto pedidoDto);

    Task<bool> DeleteAsync(int id);
}
