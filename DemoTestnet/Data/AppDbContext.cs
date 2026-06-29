using DemoTestnet.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoTestnet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Flujo> Flujo => Set<Flujo>();
    }
}
