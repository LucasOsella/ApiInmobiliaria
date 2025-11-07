using Microsoft.AspNetCore.Mvc;
using Tp_inmobiliaria.Models;
using System.Text;
using ApiInmobiliaria.Repository.IRepositorio;
using ApiInmobiliaria.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ApiInmobiliaria.Models;


namespace ApiInmobiliaria.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContratoController : ControllerBase
    {
        private readonly ConexionBD _context;
        private readonly IRepositorioContratos _contratoRepositorio;
        private readonly IConfiguration _configuration;
        private readonly IRepositorioPropietario repositorioPropietario;
        private readonly IRepositorioInmueble repositorioInmueble;

        public ContratoController(ConexionBD context, IRepositorioContratos contratoRepositorio, IConfiguration configuration, IRepositorioPropietario repositorioPropietario, IRepositorioInmueble repositorioInmueble)
        {
            _context = context;
            _contratoRepositorio = contratoRepositorio;
            _configuration = configuration;
            repositorioPropietario = repositorioPropietario;
            repositorioInmueble = repositorioInmueble;
        }

        [HttpGet("ObtenerContrato/{id}")]
        public IActionResult GetContrato(int id)
        {
            var contratos = _contratoRepositorio.ObtenerTodosPorInmueble(id);
            return Ok(contratos);
        }

        [HttpGet("ObtenerContratoPorId/{id}")]
        public IActionResult ObtenerContratoPorId(int id)
        {
            var contrato = _contratoRepositorio.ObtenerPorId(id);
            return Ok(contrato);
        }

    }
}