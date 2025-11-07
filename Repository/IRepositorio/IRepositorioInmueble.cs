using System;
using ApiInmobiliaria.Models;

namespace ApiInmobiliaria.Repository.IRepositorio
{
    public interface IRepositorioInmueble
    {
        // Define aquí los métodos específicos para el repositorio de Inmuebles 
        Inmueble ObtenerPorId(int id);
        List<Inmueble> ObtenerTodosPorPropietario(int id);
        void Actualizar(Inmueble inmueble);
        void Create (Inmueble inmueble);
    }
}