namespace WebApplication_Ejemplo_1.Models;

public sealed class Tesis
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Titulo { get; set; } = string.Empty;
    public string Estudiante { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string? Imagen { get; set; }
    public Guid IdTutor { get; set; }
    public Tutor Tutor { get; set; } = null!;
}
