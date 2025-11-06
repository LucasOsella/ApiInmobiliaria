using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ApiInmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ApiInmobiliaria.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace ApiInmobiliaria.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InmueblesController : ControllerBase
{
    private readonly ConexionBD _context;

    public InmueblesController(ConexionBD context)
    {
        _context = context;
    }
    [HttpGet]
    public IActionResult GetInmueble()
    {
        var id_Propietario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id_Propietario == null)
        {
            return Unauthorized("Propietario no identificado");
        }

        var inmuebles = _context.inmueble.Where(i => i.Id_Propietario.ToString() == id_Propietario).ToList();
        return Ok(inmuebles);
    }

    [HttpPost("AgregarInmueble")]
    public IActionResult AgregarInmueble([FromBody] Inmueble inmueble)
    {
        inmueble.Id_Propietario = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        _context.inmueble.Add(inmueble);
        _context.SaveChanges();
        return Ok("Inmueble agregado correctamente.");
    }

    [HttpPut("EditarEstadoInmueble/{id}")]
        public IActionResult EditarEstadoInmueble(int id, [FromBody] Inmueble inmueble)
        {
            var existingInmueble = _context.inmueble.Find(id);
            if (existingInmueble == null)
            {
                return NotFound("Inmueble no encontrado");
            }

        // Actualizar solo el estado del inmueble
            if (!string.IsNullOrWhiteSpace(inmueble.Estado))
            {
            existingInmueble.Estado = inmueble.Estado;
            }
            

            _context.SaveChanges();
            return Ok("Estado del inmueble actualizado correctamente.");
        }


    

    //endpoint para hashear las contraseñas existentes que estaban en texto plano, no usar en producción
    [Authorize(Roles = "Admin")]
    [HttpPost("hashPasswords")]
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

}
