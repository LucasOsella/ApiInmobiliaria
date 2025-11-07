using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ApiInmobiliaria.Models;
using ApiInmobiliaria.Data;
using ApiInmobiliaria.Repository.IRepositorio;

namespace ApiInmobiliaria.Repository
{   
    public class InmuebleRepositorio : IRepositorioInmueble
    {
        private readonly ConexionBD _context;
        public InmuebleRepositorio(ConexionBD context)
        {
            _context = context;
        }
        // Implementa aquí los métodos específicos para el repositorio de Inmueble
        public void Actualizar(Inmueble inmueble)
        {
            /* _context.inmueble.Update(inmueble);
            _context.SaveChanges();
             */
            var original = _context.inmueble.FirstOrDefault(i => i.Id == inmueble.Id);
            if (original != null)
            {
                original.Estado = inmueble.Estado;
                _context.SaveChanges();
            }

            
        }

        public Inmueble? ObtenerPorId(int id)
        {
            return _context.inmueble.FirstOrDefault(i => i.Id == id);

        }

        public List<Inmueble> ObtenerTodosPorPropietario(int id)
        {
            return _context.inmueble.Where(i => i.Id_Propietario == id).ToList();
        }
        public void Create(Inmueble inmueble)
        {
            _context.inmueble.Add(inmueble);
            _context.SaveChanges();
        }
    }
}