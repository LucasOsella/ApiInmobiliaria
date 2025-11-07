using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tp_inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiInmobiliaria.Models
{
    [Table("inmueble")]
    public class Inmueble
    {   
        [Key]
        public int id { get; set; }
        public string direccion { get; set; } = "";
        public int ambientes { get; set; }

        public string coordenadas { get; set; } = "";

        public decimal precio { get; set; }
        public string Estado { get; set; } = "";

        public int id_propietario { get; set; }
        public int id_tipo { get; set; }
        public string? ImagenUrl { get; set; } = "";
    }
}