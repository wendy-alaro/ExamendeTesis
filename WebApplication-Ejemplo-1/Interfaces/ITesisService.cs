using Contrato;

namespace WebApplication_Ejemplo_1.Interfaces;

public interface ITesisService
{
    Task<List<TesisDto>> ObtenerTodasAsync();
    Task<TesisDto?> ObtenerPorIdAsync(Guid id);
    Task<TesisDto> CrearAsync(TesisCrearDto dto);
    Task<TesisDto?> CambiarEstadoAsync(Guid id, string nuevoEstado);
}
