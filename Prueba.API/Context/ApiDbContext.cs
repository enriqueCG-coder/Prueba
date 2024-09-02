using Microsoft.EntityFrameworkCore;
using Prueba.API.Models;

namespace Prueba.API.Context
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        // DbSet para cada entidad
        public DbSet<Titular> Titulares { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<TarjetaCredito> TarjetaCred { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Transacciones> Transacciones { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransaccionCargo>().HasNoKey(); 
        }
    }
}
