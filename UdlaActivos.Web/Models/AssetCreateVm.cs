using System.ComponentModel.DataAnnotations;

namespace UdlaActivos.Web.Models;

public class AssetCreateVm
{
    [Display(Name = "Tipo de Activo")]
    [Required(ErrorMessage = "Seleccione un tipo.")]
    public string Type { get; set; } = "";

    [Display(Name = "Estado")]
    [Required(ErrorMessage = "Seleccione un estado.")]
    public string Status { get; set; } = "Disponible";

    [Display(Name = "Marca")]
    [Required(ErrorMessage = "Ingrese la marca.")]
    [StringLength(60)]
    public string Brand { get; set; } = "";

    [Display(Name = "Modelo")]
    [Required(ErrorMessage = "Ingrese el modelo.")]
    [StringLength(80)]
    public string Model { get; set; } = "";

    [Display(Name = "Número de Serie")]
    [Required(ErrorMessage = "Ingrese el número de serie.")]
    [StringLength(80)]
    public string Serial { get; set; } = "";

    [Display(Name = "Código UDLA")]
    [Required(ErrorMessage = "Ingrese el código UDLA.")]
    [StringLength(40)]
    public string UdlaCode { get; set; } = "";

    [Display(Name = "Sede / Ubicación")]
    [Required(ErrorMessage = "Seleccione una sede.")]
    public string Site { get; set; } = "";

    [Display(Name = "Descripción")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Display(Name = "Observaciones")]
    [DataType(DataType.MultilineText)]
    public string? Notes { get; set; }

    public DateTime? PurchaseDate { get; set; }
}
