using Microsoft.AspNetCore.Mvc;
using Tp_inmobiliaria.Models;
using ApiInmobiliaria.Repository.IRepositorio;
using ApiInmobiliaria.Data;
using ApiInmobiliaria.Models;

namespace ApiInmobiliaria.Repository
{
    public class ContratoRespositorio : IRepositorioContratos
    {
        private readonly ConexionBD _context;
        public ContratoRespositorio(ConexionBD context)
        {
            _context = context;
        }

        public Contrato? ObtenerPorId(int id)
        {
            return _context.contratos.FirstOrDefault(c => c.id == id);
        }

        public List<Contrato> ObtenerTodosPorInmueble(int idInmueble)
        {
            var contratos = _context.contratos.Where(c => c.id_inmueble == idInmueble).ToList();
            return contratos;
        }
    }
}