using Contrato;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication_Ejemplo_1.Interfaces;
using WebApplication_Ejemplo_1.Services;

namespace WebApplication_Ejemplo_1.Controllers;

[Route("api/tutores")]
[ApiController]
[Authorize]
public sealed class TutoresController(ITutorService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TutorDto>>> ObtenerTodos()
    {
        return Ok(await service.ObtenerTodosAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TutorDto>> ObtenerPorId(Guid id)
    {
        var tutor = await service.ObtenerPorIdAsync(id);
        return tutor is null ? NotFound() : Ok(tutor);
    }

    [HttpPost]
    public async Task<ActionResult<TutorDto>> Crear([FromBody] TutorCrearDto dto)
    {
        try
        {
            var tutor = await service.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = tutor.Id }, tutor);
        }
        catch (ReglaNegocioException exception)
        {
            return Conflict(new { mensaje = exception.Message });
        }
    }
}
