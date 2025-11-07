using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tp_inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiInmobiliaria.Models
{
    [Table("contrato")]
    public class Contrato
    {
        [Key]
        public int id { get; set; } // este es PK, no suele necesitar validación

        public int id_inquilino { get; set; }

        public int id_inmueble { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public decimal monto_mensual { get; set; }

        public int? id_usuario_creador { get; set; }

        public DateTime? fecha_rescision { get; set; }   
        public decimal? multa { get; set; }              
        public bool multa_pagada { get; set; } = false;

        // este puede ser null hasta que alguien finalice el contrato
        public int? id_usuario_finalizador { get; set; }
    }
}