namespace Contrato;

public sealed class TutorDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public int TesisAsignadas { get; set; }
}
