using DemoTestnet.Data;
using DemoTestnet.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoTestnet.Services
{
    public class FlujoService
    {
        private readonly AppDbContext context;

        public FlujoService(AppDbContext context)
        {
            this.context = context;
        }

        public Flujo? GetFlujoActualPorCodigo(string codigo, bool debeEstarActivo = true)
        {
            return context.Flujo
                .Where(x => x.Codigo == codigo)
                .Where(Flujo.SpecVigente(debeEstarActivo))
                .AsNoTracking()
                .FirstOrDefault();
        }

        public IEnumerable<Flujo> GetFlujosPorCodigo(string codigo)
        {
            return context.Flujo
                .Where(x => x.Codigo == codigo)
                .AsNoTracking()
                .AsEnumerable();
        }
    }
}
