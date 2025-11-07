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
        public int Id { get; set; }
        public string Direccion { get; set; } = "";
        public int Ambientes { get; set; }

        public string Coordenadas { get; set; } = "";

        public decimal Precio { get; set; }
        public string Estado { get; set; } = "";

        public int Id_Propietario { get; set; }
        public int Id_Tipo { get; set; }
       // public string ImagenUrl { get; set; } = "";
    }
}