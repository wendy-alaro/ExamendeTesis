using System.ComponentModel.DataAnnotations;

namespace Contrato;

public sealed class EstadoTesisDto
{
    [Required, StringLength(50)]
    public string Estado { get; set; } = string.Empty;
}
