using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ApiInmobiliaria.Models;

namespace ApiInmobiliaria.Repository.IRepositorio
{
    public interface IRespository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
    }

    public interface IInmuebleRepository : IRespository<Inmueble>
    {
        Task<IEnumerable<Inmueble>> GetByPropietarioId(int propietarioId);
    }

    public interface IPropietarioRepository : IRespository<Propietario>
    {
        Task<Propietario> GetByEmail(string email);
    } 
    
}