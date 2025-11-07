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

        if (id_token != inmueble.id_propietario.ToString())
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

        if (id_token != original.id_propietario.ToString())
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

    // POST: /api/Inmuebles/cargar
    [HttpPost("Crear")]
        public IActionResult Create([FromForm] string inmueble,[FromForm] IFormFile? imagen)
        {
            try
            {
                var inmuebleData = System.Text.Json.JsonSerializer.Deserialize<Inmueble>(inmueble);//conviere la cadena json a objeto inmueble
            var idPropClaim = User.FindFirst(ClaimTypes.NameIdentifier);//busca el claim con el id del propietario
            Console.WriteLine("JSON recibido: " + inmueble);
            if (inmuebleData == null)
                return BadRequest("Datos de inmueble inválidos.");
            if(inmuebleData.id_tipo <= 0)
                return BadRequest("El tipo de inmueble es obligatorio.");        
                if (idPropClaim == null)
                    return Unauthorized("Token inválido o no proporcionado");
                int idPropietario = int.Parse(idPropClaim.Value);//convierte el id del propietario a entero
                inmuebleData.id_propietario = idPropietario;//asigna el id del propietario al inmueble

            
                if (imagen != null && imagen.Length > 0)//si se envio una imagen
                {

                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");//ruta fisica para guardar la imagen
                    if (!Directory.Exists(uploadsFolder))//si no existe la carpeta uploads, la crea
                        Directory.CreateDirectory(uploadsFolder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);//genera un nombre unico para la imagen
                    string filePath = Path.Combine(uploadsFolder, fileName);//ruta completa del archivo

                    FileInfo fileInfo = new FileInfo(filePath);//informacion del archivo
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))//guarda la imagen en el servidor
                    {
                        imagen.CopyTo(fileStream);
                    }
                    var baseUrl = $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";//obtiene la url base del servidor

                    inmuebleData.ImagenUrl = $"{baseUrl}/uploads/{fileName}";
                    inmuebleData.ImagenUrl = filePath;
                }
                else
                {
                    inmuebleData.ImagenUrl = "https://placehold.co/600x400";
                }

                _inmuebleRepositorio.Create(inmuebleData);
                
                return Ok("Inmueble agregado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
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
