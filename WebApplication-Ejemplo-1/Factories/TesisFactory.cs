using Microsoft.EntityFrameworkCore;
using WebApplication_Ejemplo_1.Data;
using WebApplication_Ejemplo_1.Models;

namespace WebApplication_Ejemplo_1.Factories;

public static class TesisFactory
{
    public static async Task SeedAsync(BaseDeDatos context)
    {
        var tutoresSemilla = new[]
        {
            new Tutor { Nombre = "Tutor de ejemplo 1", Especialidad = "Ingeniería de software", Correo = "tutor1@ejemplo.local" },
            new Tutor { Nombre = "Tutor de ejemplo 2", Especialidad = "Educación", Correo = "tutor2@ejemplo.local" },
            new Tutor { Nombre = "Tutor de ejemplo 3", Especialidad = "Ciencia de datos", Correo = "tutor3@ejemplo.local" },
            new Tutor { Nombre = "Tutor de ejemplo 4", Especialidad = "Sistemas de información", Correo = "tutor4@ejemplo.local" },
            new Tutor { Nombre = "Tutor de ejemplo 5", Especialidad = "Inteligencia artificial", Correo = "tutor5@ejemplo.local" },
            new Tutor { Nombre = "Tutor de ejemplo 6", Especialidad = "Ciberseguridad", Correo = "tutor6@ejemplo.local" },
            new Tutor { Nombre = "Tutor de ejemplo 7", Especialidad = "Bases de datos", Correo = "tutor7@ejemplo.local" },
            new Tutor { Nombre = "Tutor de ejemplo 8", Especialidad = "Redes", Correo = "tutor8@ejemplo.local" },
            new Tutor { Nombre = "Tutor de ejemplo 9", Especialidad = "Desarrollo web", Correo = "tutor9@ejemplo.local" },
            new Tutor { Nombre = "Tutor de ejemplo 10", Especialidad = "Investigación educativa", Correo = "tutor10@ejemplo.local" }
        };

        var tutores = new List<Tutor>(tutoresSemilla.Length);
        foreach (var tutorSemilla in tutoresSemilla)
        {
            var tutorExistente = await context.Tutores
                .SingleOrDefaultAsync(tutor => tutor.Correo == tutorSemilla.Correo);
            tutores.Add(tutorExistente ?? tutorSemilla);
            if (tutorExistente is null)
            {
                await context.Tutores.AddAsync(tutorSemilla);
            }
        }

        await context.SaveChangesAsync();

        var datosTesisSemilla = new (string Titulo, string Estudiante, string Estado, int IndiceTutor)[]
        {
            ("Sistema web para la gestión de bibliotecas académicas", "Estudiante de ejemplo 1", "En desarrollo", 0),
            ("Plataforma digital para el seguimiento de tesis", "Estudiante de ejemplo 2", "En revisión", 1),
            ("Análisis de datos aplicado a la educación superior", "Estudiante de ejemplo 3", "En desarrollo", 2),
            ("Aplicación para la gestión de tutorías universitarias", "Estudiante de ejemplo 4", "En desarrollo", 3),
            ("Sistema de alertas para el rendimiento académico", "Estudiante de ejemplo 5", "En revisión", 4),
            ("Repositorio institucional de proyectos de investigación", "Estudiante de ejemplo 6", "Finalizada", 5),
            ("Modelo de aprendizaje automático para datos educativos", "Estudiante de ejemplo 7", "En desarrollo", 6),
            ("Portal de servicios para estudiantes universitarios", "Estudiante de ejemplo 8", "En revisión", 7),
            ("Herramienta de apoyo para la planificación curricular", "Estudiante de ejemplo 9", "En desarrollo", 8),
            ("Sistema de evaluación de proyectos académicos", "Estudiante de ejemplo 10", "Finalizada", 9)
        };

        foreach (var tesisSemilla in datosTesisSemilla)
        {
            if (await context.Tesis.AnyAsync(tesis => tesis.Titulo == tesisSemilla.Titulo))
            {
                continue;
            }

            await context.Tesis.AddAsync(CrearTesis(
                tesisSemilla.Titulo,
                tesisSemilla.Estudiante,
                tesisSemilla.Estado,
                tutores[tesisSemilla.IndiceTutor]));
        }

        await context.SaveChangesAsync();
    }

    private static Tesis CrearTesis(string titulo, string estudiante, string estado, Tutor tutor)
    {
        return new Tesis
        {
            Titulo = titulo,
            Estudiante = estudiante,
            Estado = estado,
            IdTutor = tutor.Id
        };
    }
}
