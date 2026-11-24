namespace WebApplication_Ejemplo_1.Models;

public sealed class Tutor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nombre { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public ICollection<Tesis> Tesis { get; set; } = new List<Tesis>();
}
