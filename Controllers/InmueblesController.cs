using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ApiInmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ApiInmobiliaria.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ApiInmobiliaria.Repository.IRepositorio;


namespace ApiInmobiliaria.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InmueblesController : ControllerBase
{
    private readonly ConexionBD _context;
    private readonly IRepositorioInmueble _inmuebleRepositorio;
    private readonly IRepositorioPropietario _propietarioRepositorio;

    public InmueblesController(ConexionBD context, IRepositorioInmueble inmuebleRepositorio, IRepositorioPropietario propietarioRepositorio)
    {
        _context = context;
        _inmuebleRepositorio = inmuebleRepositorio;
        _propietarioRepositorio = propietarioRepositorio;

    }
    [HttpGet]
    public IActionResult GetInmueble()
    {
        var id_token = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        Propietario propietario = _propietarioRepositorio.obtenerPorEmail(email);

        if (id_token != propietario.Id.ToString())
        {
            return Unauthorized("Propietario no autorizado para ver estos inmuebles.");
        }

        var inmuebles = _inmuebleRepositorio.ObtenerTodosPorPropietario(Convert.ToInt32(id_token));
        return Ok(inmuebles);
    }

    [HttpGet("ObtenerInmueblePorId/{id}")]
    public IActionResult ObtenerInmueble(int id)
    {
        var id_token = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var inmueble = _inmuebleRepositorio.ObtenerPorId(id);

        if (id_token != inmueble.Id_Propietario.ToString())
        {
            return Unauthorized("Propietario no autorizado para ver este inmueble.");
        }

        return Ok(inmueble);
    }

    [HttpPut("Actualizar/{id}")]
    public IActionResult Actualizar(int id,[FromBody] string Estado)
    {

        var id_token = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var original = _inmuebleRepositorio.ObtenerPorId(id);

        if (id_token != original.Id_Propietario.ToString())
        {
            return Unauthorized("Propietario no autorizado para actualizar este inmueble.");
        }
        if (Estado != "DISPONIBLE" && Estado != "SUSPENDIDO" && Estado != "OCUPADO")
        {
            return BadRequest("El Estado debe ser 'DISPONIBLE', 'SUSPENDIDO' o 'OCUPADO'");
        }

        original.Estado = Estado;

        _inmuebleRepositorio.Actualizar(original);
        return Ok("Inmueble agregado exitosamente.");
    }

    [HttpPost("Crear")]
    public IActionResult Crear([FromBody] Inmueble inmueble, IFormFile imagen)
    {
        var id_token = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id_token != inmueble.Id_Propietario.ToString())
        {
            return Unauthorized("Propietario no autorizado para crear este inmueble.");
        }

        _inmuebleRepositorio.Create(inmueble);
        return Ok("Inmueble agregado exitosamente.");
    }

    

    //endpoint para hashear las contraseñas existentes que estaban en texto plano, no usar en producción
    /*[HttpPost("hashPasswords")]
    public IActionResult HashExistingPasswords()
    {
        var propietarios = _context.propietarios.ToList();

        foreach (var p in propietarios)
        {
            // Solo si la contraseña aún no está hasheada
            if (!p.Password.StartsWith("$2b$")) // así empiezan los hashes de BCrypt
            {
                p.Password = BCrypt.Net.BCrypt.HashPassword(p.Password);
            }
        }

        _context.SaveChanges();

        return Ok("Contraseñas actualizadas correctamente.");
    }
    */

}
