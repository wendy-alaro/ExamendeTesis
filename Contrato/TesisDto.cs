namespace Contrato;

public sealed class TesisDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Estudiante { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string? Imagen { get; set; }
    public Guid IdTutor { get; set; }
    public TutorDto? Tutor { get; set; }
}
