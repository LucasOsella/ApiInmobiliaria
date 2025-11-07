using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tp_inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;

namespace Tp_inmobiliaria.Models
{
    public enum TipoUsuario
{
    Admin = 1,
    Empleado = 2
}
    public class Usuario
    {   
        [Key]
        public int Id { get; set; }
        public string? Nombre_Usuario { get; set; }
        public string? Apellido_Usuario { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? Id_TipoUsuario { get; set; }
        public string? RolUsuario { get; set; }
        public int Activo { get; set; }
        public string? foto { get; set; }
    }
}
