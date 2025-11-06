using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using ApiInmobiliaria.Models;

namespace ApiInmobiliaria.Data
{
    public class ConexionBD : DbContext
    {
        public ConexionBD(DbContextOptions<ConexionBD> options)
            : base(options)
        {
        }
        private readonly string _connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria-lucasosella;SslMode=none;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        // Tus tablas (DbSets)
        public DbSet<Inmueble> inmueble { get; set; }
        public DbSet<Inquilinos> inquilinos { get; set; }
        public DbSet<Contratos> contratos { get; set; }
        public DbSet<Propietario> propietarios { get; set; }
        public DbSet<Pago> pagos { get; set; }
    }
}


