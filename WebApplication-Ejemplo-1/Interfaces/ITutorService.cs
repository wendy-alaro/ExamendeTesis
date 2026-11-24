using Contrato;

namespace WebApplication_Ejemplo_1.Interfaces;

public interface ITutorService
{
    Task<List<TutorDto>> ObtenerTodosAsync();
    Task<TutorDto?> ObtenerPorIdAsync(Guid id);
    Task<TutorDto> CrearAsync(TutorCrearDto dto);
}
