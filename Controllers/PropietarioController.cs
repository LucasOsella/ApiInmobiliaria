using ApiInmobiliaria.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiInmobiliaria.Models;
using System.Security.Claims;
using ApiInmobiliaria.Repository.IRepositorio;


namespace ApiInmobiliaria.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PropietarioController : ControllerBase
{
    private readonly IRepositorioPropietario _repositorio;
    private readonly ConexionBD _context;
    private readonly IConfiguration _configuration;

    public PropietarioController(ConexionBD context, IConfiguration configuration, IRepositorioPropietario repositorio)
    {
        _repositorio = repositorio;
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetPropietarios()
    {
        var id_Propietario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var propietario = _repositorio.ObtenerPorId(Convert.ToInt32(id_Propietario));
        return Ok(propietario);
    }

    [Authorize]
    [HttpPut("Actualizar")]
    public IActionResult Actualizar([FromBody] Propietario propietario)
    {
        // Obtiene el ID del propietario autenticado
        var id_token = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id_token != propietario.Id.ToString())
        {
            return Unauthorized("Propietario no autorizado para actualizar estos datos.");
        }
    
        // Obtiene el email del usuario autenticado (desde el token)
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        // Busca los datos originales en la base de datos
        var original = _repositorio.obtenerPorEmail(email);

        // Si no lo encuentra, devuelve 404
        if (original == null)
            return NotFound();

        // Mantiene el ID original (para no crear un nuevo propietario)
        original.Dni = propietario.Dni;
        original.Nombre = propietario.Nombre;
        original.Apellido = propietario.Apellido;
        original.Telefono = propietario.Telefono;
        original.Email = propietario.Email;

        // Llama al método del repositorio para actualizar los datos
        _repositorio.Actualizar(original);

        // Devuelve el objeto actualizado
        return Ok(original);
    }

    [HttpPost("CambiarPassword")]
    public IActionResult CambiarPassword([FromForm] string newPassword, [FromForm] string oldPassword)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var id_token = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var propietario = _repositorio.obtenerPorEmail(email);
        if (id_token != propietario.Id.ToString())
        {
            return Unauthorized("Propietario no autorizado para cambiar esta contraseña.");
        }

        if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(oldPassword))
        {
            return BadRequest("La nueva contraseña y la contraseña antigua son obligatorias.");
        }

        var result = _repositorio.changerPassword(email, newPassword, oldPassword).Result;

        if (!result)
        {
            return BadRequest("No se pudo cambiar la contraseña. Verifique la contraseña antigua.");
        }

        return Ok("Contraseña cambiada exitosamente.");
    }

}