using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ApiInmobiliaria.Models;
using ApiInmobiliaria.Data;
using ApiInmobiliaria.Repository.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace ApiInmobiliaria.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IRepositorioPago _pagoRepositorio;
        private readonly ConexionBD _context;
        public PagoController(IRepositorioPago pagoRepositorio, ConexionBD context)
        {
            _pagoRepositorio = pagoRepositorio;
            _context = context;
        }

        [HttpGet("ObtenerPagosPorContrato/{idContrato}")]
        public IActionResult GetPagosPorContrato(int idContrato)
        {
            var pagos = _pagoRepositorio.ObtenerTodosPorContrato(idContrato);
            return Ok(pagos);
        }
    }
}