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
        private readonly IRepositorioPropietario _repositorioPropietario;
        private readonly IRepositorioInmueble _repositorioInmueble;

        public ContratoController(ConexionBD context, IRepositorioContratos contratoRepositorio, IConfiguration configuration, IRepositorioPropietario repositorioPropietario, IRepositorioInmueble repositorioInmueble)
        {
            _context = context;
            _contratoRepositorio = contratoRepositorio;
            _configuration = configuration;
            _repositorioPropietario = repositorioPropietario;
            _repositorioInmueble = repositorioInmueble;
        }

        [HttpGet("ObtenerContrato/{id}")]
        public IActionResult GetContrato(int id)
        {
            var id_token = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var inmueble = _repositorioInmueble.ObtenerPorId(id);

            if (id_token != inmueble.id_propietario.ToString())
            {
                return Unauthorized("Propietario no autorizado para ver este contrato.");
            }

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