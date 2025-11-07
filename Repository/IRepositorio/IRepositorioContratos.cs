using System;
using ApiInmobiliaria.Models;

namespace ApiInmobiliaria.Repository.IRepositorio
{
    public interface IRepositorioContratos
    {
        // Define aquí los métodos específicos para el repositorio de Contratos 
        Contrato ObtenerPorId(int id);
        List<Contrato> ObtenerTodosPorInmueble(int idInmueble);
    }
}