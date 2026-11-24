using Contrato;
using Microsoft.EntityFrameworkCore;
using WebApplication_Ejemplo_1.Data;
using WebApplication_Ejemplo_1.Interfaces;
using WebApplication_Ejemplo_1.Models;

namespace WebApplication_Ejemplo_1.Services;

public sealed class TutorService(BaseDeDatos context) : ITutorService
{
    public async Task<List<TutorDto>> ObtenerTodosAsync()
    {
        return await context.Tutores.AsNoTracking()
            .OrderBy(tutor => tutor.Nombre)
            .Select(tutor => new TutorDto
            {
                Id = tutor.Id,
                Nombre = tutor.Nombre,
                Especialidad = tutor.Especialidad,
                Correo = tutor.Correo,
                TesisAsignadas = tutor.Tesis.Count
            })
            .ToListAsync();
    }

    public async Task<TutorDto?> ObtenerPorIdAsync(Guid id)
    {
        return await context.Tutores.AsNoTracking()
            .Where(tutor => tutor.Id == id)
            .Select(tutor => new TutorDto
            {
                Id = tutor.Id,
                Nombre = tutor.Nombre,
                Especialidad = tutor.Especialidad,
                Correo = tutor.Correo,
                TesisAsignadas = tutor.Tesis.Count
            })
            .SingleOrDefaultAsync();
    }

    public async Task<TutorDto> CrearAsync(TutorCrearDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();
        var tutor = new Tutor
        {
            Nombre = dto.Nombre.Trim(),
            Especialidad = dto.Especialidad.Trim(),
            Correo = correo
        };

        await using var transaction = await context.Database.BeginTransactionAsync(
            System.Data.IsolationLevel.Serializable);

        if (await context.Tutores.AnyAsync(item => item.Correo == correo))
        {
            throw new ReglaNegocioException("Ya existe un tutor registrado con ese correo.");
        }

        context.Tutores.Add(tutor);
        try
        {
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception) when (SqlServerUniqueConstraint.IsViolation(exception))
        {
            throw new ReglaNegocioException("Ya existe un tutor registrado con ese correo.");
        }

        return new TutorDto
        {
            Id = tutor.Id,
            Nombre = tutor.Nombre,
            Especialidad = tutor.Especialidad,
            Correo = tutor.Correo,
            TesisAsignadas = 0
        };
    }
}
