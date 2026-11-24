using System.ComponentModel.DataAnnotations;

namespace Contrato;

public sealed class TutorCrearDto
{
    [Required, StringLength(150, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [Required, StringLength(120, MinimumLength = 2)]
    public string Especialidad { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(254)]
    public string Correo { get; set; } = string.Empty;
}
