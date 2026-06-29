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


        // Nombre del test:
        // Metodo: GetFlujoActualPorCodigo -> GetFlujoActualPorCodigo
        // Escenario: Flujo con el código especificado existe y está activo -> FlujoExisteYEstaActivo
        // Resultado esperado: Devuelve el flujo actual -> DevuelveFlujoActual
        // Nombre completo del test:
        // GetFlujoActualPorCodigo_FlujoExisteYEstaActivo_DevuelveFlujoActual
        // El SUT es el método GetFlujoActualPorCodigo de la clase FlujoService.

        public Flujo? GetFlujoActualPorCodigo(string codigo, bool debeEstarActivo = true)
        {
            return context.Flujo
                .Where(x => x.Codigo == codigo)
                //.Where(x => x.Codigo != codigo)
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
