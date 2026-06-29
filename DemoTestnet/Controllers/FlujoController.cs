using DemoTestnet.Models;
using DemoTestnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoTestnet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlujoController : ControllerBase
    {
        private readonly FlujoService flujoService;

        public FlujoController(FlujoService flujoService)
        {
            this.flujoService = flujoService;
        }

        [HttpGet("{codigo}/actual")]
        public ActionResult<Flujo> GetActualPorCodigo(string codigo, [FromQuery] bool debeEstarActivo = true)
        {
            var flujo = flujoService.GetFlujoActualPorCodigo(codigo, debeEstarActivo);
            return flujo is null ? NotFound() : Ok(flujo);
        }

        [HttpGet("{codigo}")]
        public ActionResult<IEnumerable<Flujo>> GetPorCodigo(string codigo)
        {
            return Ok(flujoService.GetFlujosPorCodigo(codigo));
        }
    }
}
