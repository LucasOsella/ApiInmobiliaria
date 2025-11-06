
namespace ApiInmobiliaria.Models;

public class CambiarPasswordRequest
{
    public string NewPassword { get; set; }
    public string OldPassword { get; set; }
}