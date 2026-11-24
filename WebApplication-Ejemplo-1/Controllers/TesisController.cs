using Contrato;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication_Ejemplo_1.Interfaces;
using WebApplication_Ejemplo_1.Services;

namespace WebApplication_Ejemplo_1.Controllers;

[Route("api/tesis")]
[ApiController]
[Authorize]
public sealed class TesisController(ITesisService service, IWebHostEnvironment environment) : ControllerBase
{
    private static readonly HashSet<string> ExtensionesPermitidas = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif"
    };

    [HttpGet]
    public async Task<ActionResult<List<TesisDto>>> ObtenerTodas()
    {
        return Ok(await service.ObtenerTodasAsync());
    }

    [HttpGet("imagenes")]
    public ActionResult<List<string>> ObtenerImagenes()
    {
        var carpeta = Path.Combine(environment.WebRootPath ?? "wwwroot", "imagenes");
        Directory.CreateDirectory(carpeta);

        var imagenes = Directory.EnumerateFiles(carpeta)
            .Where(ruta => ExtensionesPermitidas.Contains(Path.GetExtension(ruta)))
            .Select(ruta => $"/imagenes/{Uri.EscapeDataString(Path.GetFileName(ruta))}")
            .OrderBy(ruta => ruta)
            .ToList();

        return Ok(imagenes);
    }

    [HttpPost("subir-imagen")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<IActionResult> SubirImagen(IFormFile? archivo)
    {
        if (archivo is null || archivo.Length == 0)
        {
            return BadRequest(new { mensaje = "No se seleccionó ningún archivo." });
        }

        if (!ExtensionesPermitidas.Contains(Path.GetExtension(archivo.FileName)))
        {
            return BadRequest(new { mensaje = "Formato no permitido. Usa JPG, PNG, WEBP o GIF." });
        }

        var carpeta = Path.Combine(environment.WebRootPath ?? "wwwroot", "imagenes");
        Directory.CreateDirectory(carpeta);
        var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
        var nombre = $"{Guid.NewGuid():N}{extension}";
        var rutaCompleta = Path.Combine(carpeta, nombre);

        await using (var stream = System.IO.File.Create(rutaCompleta))
        {
            await archivo.CopyToAsync(stream);
        }

        return Ok(new { url = $"/imagenes/{Uri.EscapeDataString(nombre)}" });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TesisDto>> ObtenerPorId(Guid id)
    {
        var tesis = await service.ObtenerPorIdAsync(id);
        return tesis is null ? NotFound() : Ok(tesis);
    }

    [HttpPost]
    public async Task<ActionResult<TesisDto>> Crear([FromBody] TesisCrearDto dto)
    {
        try
        {
            var tesis = await service.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = tesis.Id }, tesis);
        }
        catch (ReglaNegocioException exception)
        {
            return Conflict(new { mensaje = exception.Message });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { mensaje = exception.Message });
        }
    }

    [HttpPatch("{id:guid}/estado")]
    public async Task<ActionResult<TesisDto>> CambiarEstado(Guid id, [FromBody] EstadoTesisDto dto)
    {
        try
        {
            var tesis = await service.CambiarEstadoAsync(id, dto.Estado);
            return tesis is null ? NotFound() : Ok(tesis);
        }
        catch (ReglaNegocioException exception)
        {
            return Conflict(new { mensaje = exception.Message });
        }
    }
}
