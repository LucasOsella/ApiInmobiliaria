using ApiInmobiliaria.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiInmobiliaria.Models;
using System.Security.Claims;


namespace ApiInmobiliaria.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PropietarioController : ControllerBase
{

    private readonly ConexionBD _context;
    private readonly IConfiguration _configuration;
    public PropietarioController(ConexionBD context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetPropietarios()
    {
        var id_Propietario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id_Propietario == null)
        {
            return Unauthorized("Propietario no identificado");
        }
        var propietarios = _context.propietarios.Where(p => p.Id.ToString() == id_Propietario).ToList();
        return Ok(propietarios);
    }

    [HttpPost("EditarPropietario")]
    public IActionResult EditarPropietario([FromBody] Propietario propietario)
    {
        var id_Propietario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id_Propietario == null || propietario.Id.ToString() != id_Propietario)
        {
            return Unauthorized("No autorizado para editar este propietario");
        }

        var existingPropietario = _context.propietarios.Find(propietario.Id);
        if (existingPropietario == null)
        {
            return NotFound("Propietario no encontrado");
        }

        // Actualizar los campos
        if (!string.IsNullOrWhiteSpace(propietario.Nombre))
        {
            existingPropietario.Nombre = propietario.Nombre;
        }
        if (!string.IsNullOrWhiteSpace(propietario.Apellido))
        {
            existingPropietario.Apellido = propietario.Apellido;
        }
        if (!string.IsNullOrWhiteSpace(propietario.Email))
        {
            existingPropietario.Email = propietario.Email;
        }
        if (!string.IsNullOrWhiteSpace(propietario.Direccion))
        {
            existingPropietario.Direccion = propietario.Direccion;
        }
        if (!string.IsNullOrWhiteSpace(propietario.Telefono))
        {
            existingPropietario.Telefono = propietario.Telefono;
        }
        
        _context.SaveChanges();
        return Ok("Propietario actualizado correctamente.");
    }

    [HttpPost("CambiarPassword")]
    public IActionResult CambiarPassword([FromBody] CambiarPasswordRequest model)
    {
        if (string.IsNullOrWhiteSpace(model.NewPassword))
        {
            return BadRequest("La nueva contraseña es obligatoria.");
        }
        if (model.NewPassword.Equals(model.OldPassword))
        {
            return BadRequest("Las contraseñas nos pueden ser iguales.");
        }
        var id_Propietario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id_Propietario == null)
        {
            return Unauthorized("Propietario no identificado");
        }

        var propietario = _context.propietarios.Find(id_Propietario);
        if (propietario == null)
        {
            return NotFound("Propietario no encontrado");
        }
        if (BCrypt.Net.BCrypt.Verify(model.NewPassword, propietario.Password))//si la nueva contraseña es la misma que la antigua
        {
            return BadRequest("No puede ingresar la misma contraseña");
        }
        propietario.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        _context.SaveChanges();
        return Ok("Contraseñas cambiadas correctamente.");
    }

}