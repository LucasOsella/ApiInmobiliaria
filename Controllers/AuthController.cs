using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ApiInmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ApiInmobiliaria.Data;
using System.Security.Claims;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;




namespace ApiInmobiliaria.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ConexionBD _context;

    public AuthController(ConexionBD context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

[HttpPost]
[Route("login")]
public IActionResult Login([FromBody] LoginRequest model)
{
    if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
    {
        return BadRequest("El email y la contraseña son obligatorios.");
    }

    var propietario = _context.propietarios.FirstOrDefault(u => u.Email == model.Email);
    if (propietario == null)
    {
        return Unauthorized("Usuario no encontrado");
    }

    if (!BCrypt.Net.BCrypt.Verify(model.Password, propietario.Password))
    {
        return Unauthorized("Credenciales inválidas");
    }

    var token = GenerateToken(propietario);
    return Ok(new
    {
        token,
        propietario.Email,
        propietario.Nombre,
        propietario.Id
    });
}


    private string GenerateToken(Propietario user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

