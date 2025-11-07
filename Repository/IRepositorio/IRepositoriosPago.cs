using System.Text;
using ApiInmobiliaria.Models;

namespace ApiInmobiliaria.Repository.IRepositorio
{
    public interface IRepositorioPago
    {
        // Define aquí los métodos específicos para el repositorio de Propietarios 
        Pago ObtenerPorId(int id);
        List<Pago> ObtenerTodosPorContrato(int idContrato);
        
    }
}