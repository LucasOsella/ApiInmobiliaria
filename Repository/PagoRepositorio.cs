using Microsoft.AspNetCore.Mvc;
using Tp_inmobiliaria.Models;
using ApiInmobiliaria.Repository.IRepositorio;
using ApiInmobiliaria.Data;
using ApiInmobiliaria.Models;

namespace ApiInmobiliaria.Repository
{
    public class PagoRepositorio : IRepositorioPago
    {
        private readonly ConexionBD _context;
        public PagoRepositorio(ConexionBD context)
        {
            _context = context;
        }

        public Pago? ObtenerPorId(int id)
        {
            return _context.pagos.FirstOrDefault(p => p.id == id);
        }

        public List<Pago> ObtenerTodosPorContrato(int idContrato)
        {
            List<Pago> pagos = _context.pagos.Where(p => p.id_contrato == idContrato).ToList();
            return pagos;
        }
    }
}