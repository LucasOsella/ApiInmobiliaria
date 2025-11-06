using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tp_inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiInmobiliaria.Models;

[Table("propietario")]
public class Propietario
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "El DNI es obligatorio")]
    [Range(1000000, 99999999, ErrorMessage = "El DNI debe tener entre 7 y 8 dígitos")]
    public int Dni { get; set; }
    [Required(ErrorMessage = "El Nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El Nombre debe tener menos de 100 caracteres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El Nombre solo puede contener letras y espacios")]
    public string Nombre { get; set; } = "";
    [Required(ErrorMessage = "El Apellido es obligatorio")]
    [StringLength(100, ErrorMessage = "El Apellido debe tener menos de 100 caracteres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El Apellido solo puede contener letras y espacios")]
    public string Apellido { get; set; } = "";

    [Required(ErrorMessage = "El Teléfono es obligatorio")]
    [Phone(ErrorMessage = "Debe ingresar un número válido")]
    public string Telefono { get; set; } = "";

    [Required(ErrorMessage = "El Email es obligatorio")]
    [EmailAddress(ErrorMessage = "Debe ingresar un Email valido")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "La Direccion es obligatoria")]
    [StringLength(100, ErrorMessage = "La Direccion debe tener menos de 100 caracteres")]
    public string Direccion { get; set; } = "";
    [Required(ErrorMessage = "La contraseña es requerida")]
    public string? Password { get; set; }

}   

