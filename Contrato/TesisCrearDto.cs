using System.ComponentModel.DataAnnotations;

namespace Contrato;

public sealed class TesisCrearDto
{
    [Required, StringLength(250, MinimumLength = 3)]
    public string Titulo { get; set; } = string.Empty;

    [Required, StringLength(150, MinimumLength = 2)]
    public string Estudiante { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Imagen { get; set; }

    [Required]
    public Guid IdTutor { get; set; }
}
