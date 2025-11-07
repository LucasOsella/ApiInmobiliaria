using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ApiInmobiliaria.Models;
using ApiInmobiliaria.Data;
using ApiInmobiliaria.Repository.IRepositorio;
using Microsoft.VisualBasic;
using NuGet.Protocol.Core.Types;

namespace ApiInmobiliaria.Repository
{
    public class PropietarioRepositorio : IRepositorioPropietario
    {
        private readonly ConexionBD _context;
        public PropietarioRepositorio(ConexionBD context)
        {
            _context = context;
        }

        public void Actualizar(Propietario propietario)
        {
                _context.propietarios.Update(propietario);
                _context.SaveChanges();
        
        }

        public Task<bool> changerPassword(string email, string newPassword, string oldPassword)
        {
            var propietario = _context.propietarios.FirstOrDefault(p => p.Email == email);

            if (propietario == null)
            {
                return Task.FromResult(false);
            }   
            if (!BCrypt.Net.BCrypt.Verify(oldPassword, propietario.Password)) //verifica que la contraseña antigua sea correcta
            {
                return Task.FromResult(false);
            }
            else
            {
                var Hashpassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                propietario.Password = Hashpassword;
                _context.propietarios.Update(propietario);
                _context.SaveChanges();
                return Task.FromResult(true);
            }
        }

        public Propietario? obtenerPorEmail(string email)
        {
            return _context.propietarios.FirstOrDefault(p => p.Email == email);
        }

        public Propietario? ObtenerPorId(int id)
        {
            return _context.propietarios.FirstOrDefault(p => p.Id == id);
        }

    }
}