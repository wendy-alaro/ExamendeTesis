using System.Data;
using Contrato;
using Microsoft.EntityFrameworkCore;
using WebApplication_Ejemplo_1.Data;
using WebApplication_Ejemplo_1.Interfaces;
using WebApplication_Ejemplo_1.Models;

namespace WebApplication_Ejemplo_1.Services;

public sealed class TesisService(BaseDeDatos context, IWebHostEnvironment environment) : ITesisService
{
    public async Task<List<TesisDto>> ObtenerTodasAsync()
    {
        var tesis = await context.Tesis.AsNoTracking()
            .Include(item => item.Tutor)
            .OrderBy(item => item.Titulo)
            .ToListAsync();
        return tesis.Select(Mapear).ToList();
    }

    public async Task<TesisDto?> ObtenerPorIdAsync(Guid id)
    {
        var tesis = await context.Tesis.AsNoTracking()
            .Include(item => item.Tutor)
            .SingleOrDefaultAsync(item => item.Id == id);
        return tesis is null ? null : Mapear(tesis);
    }

    public async Task<TesisDto> CrearAsync(TesisCrearDto dto)
    {
        var titulo = dto.Titulo.Trim();
        var estudiante = dto.Estudiante.Trim();
        var imagen = string.IsNullOrWhiteSpace(dto.Imagen) ? null : dto.Imagen.Trim();

        if (imagen is not null && !ImagenValida(imagen))
        {
            throw new ReglaNegocioException("La imagen seleccionada no existe en la carpeta de imágenes del servidor.");
        }

        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var tutor = await context.Tutores.SingleOrDefaultAsync(item => item.Id == dto.IdTutor);
        if (tutor is null)
        {
            throw new KeyNotFoundException("No se encontró el tutor seleccionado.");
        }

        if (await context.Tesis.AnyAsync(item => item.Titulo == titulo))
        {
            throw new ReglaNegocioException("Ya existe una tesis con ese título.");
        }

        var cantidadTesis = await context.Tesis.CountAsync(item => item.IdTutor == dto.IdTutor);
        if (cantidadTesis >= 3)
        {
            throw new ReglaNegocioException("El tutor seleccionado ya tiene el máximo de 3 tesis asignadas.");
        }

        var nuevaTesis = new Tesis
        {
            Titulo = titulo,
            Estudiante = estudiante,
            Estado = "En desarrollo",
            Imagen = imagen,
            IdTutor = dto.IdTutor,
            Tutor = tutor
        };

        context.Tesis.Add(nuevaTesis);
        try
        {
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception) when (SqlServerUniqueConstraint.IsViolation(exception))
        {
            throw new ReglaNegocioException("Ya existe una tesis con ese título.");
        }

        return Mapear(nuevaTesis);
    }

    public async Task<TesisDto?> CambiarEstadoAsync(Guid id, string nuevoEstado)
    {
        var tesis = await context.Tesis
            .Include(item => item.Tutor)
            .SingleOrDefaultAsync(item => item.Id == id);
        if (tesis is null)
        {
            return null;
        }

        if (tesis.Estado == "Finalizada")
        {
            throw new ReglaNegocioException("La tesis ya está finalizada y no admite más cambios de estado.");
        }

        var siguienteEstado = tesis.Estado switch
        {
            "En desarrollo" => "En revisión",
            "En revisión" => "Finalizada",
            _ => null
        };
        if (siguienteEstado is null || nuevoEstado != siguienteEstado)
        {
            throw new ReglaNegocioException("La tesis solo puede avanzar de En desarrollo a En revisión y luego a Finalizada.");
        }

        tesis.Estado = siguienteEstado;
        await context.SaveChangesAsync();
        return Mapear(tesis);
    }

    private bool ImagenValida(string ruta)
    {
        const string prefijo = "/imagenes/";
        if (!ruta.StartsWith(prefijo, StringComparison.Ordinal))
            return false;

        var nombre = Uri.UnescapeDataString(ruta[prefijo.Length..]);
        if (string.IsNullOrWhiteSpace(nombre)
            || Path.GetFileName(nombre) != nombre
            || !new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" }
                .Contains(Path.GetExtension(nombre), StringComparer.OrdinalIgnoreCase))
            return false;

        var carpeta = Path.Combine(environment.WebRootPath ?? "wwwroot", "imagenes");
        var rutaCompleta = Path.GetFullPath(Path.Combine(carpeta, nombre));
        return rutaCompleta.StartsWith(Path.GetFullPath(carpeta) + Path.DirectorySeparatorChar,
                   StringComparison.OrdinalIgnoreCase)
               && File.Exists(rutaCompleta);
    }

    private static TesisDto Mapear(Tesis tesis)
    {
        return new TesisDto
        {
            Id = tesis.Id,
            Titulo = tesis.Titulo,
            Estudiante = tesis.Estudiante,
            Estado = tesis.Estado,
            Imagen = tesis.Imagen,
            IdTutor = tesis.IdTutor,
            Tutor = new TutorDto
            {
                Id = tesis.Tutor.Id,
                Nombre = tesis.Tutor.Nombre,
                Especialidad = tesis.Tutor.Especialidad,
                Correo = tesis.Tutor.Correo
            }
        };
    }
}
