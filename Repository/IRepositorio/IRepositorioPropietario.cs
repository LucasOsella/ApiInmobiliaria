using System;
using ApiInmobiliaria.Models;

namespace ApiInmobiliaria.Repository.IRepositorio
{
    public interface IRepositorioPropietario
    {
        // Define aquí los métodos específicos para el repositorio de Propietario
        Propietario obtenerPorEmail(string email);
        Propietario ObtenerPorId(int id);
        void Actualizar(Propietario propietario);
        Task<bool> changerPassword(string email, string newPassword, string oldPassword);
    }
}