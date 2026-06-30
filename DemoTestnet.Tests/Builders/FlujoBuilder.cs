using DemoTestnet;
using DemoTestnet.Models;

namespace DemoTestnet.Tests.Builders
{
    /// <summary>
    /// Test Data Builder para <see cref="Flujo"/>: monta un flujo a medida encadenando decisiones.
    /// Es TRANSPARENTE: parte de un Flujo con los valores por defecto del modelo y solo cambia lo que
    /// el test le pide. Lo que no se especifica conserva el default del modelo, de modo que esos
    /// defaults se pueden comprobar a través del propio builder. Patrón de Nat Pryce.
    /// </summary>
    public class FlujoBuilder
    {
        private const string EstadoActivo = Constants.Strings.Aprobaciones.Flujo_Estado_Activo;

        private readonly Flujo _flujo = new();

        public FlujoBuilder ConId(int id)            { _flujo.Id = id; return this; }
        public FlujoBuilder ConCodigo(string codigo) { _flujo.Codigo = codigo; return this; }
        public FlujoBuilder ConEstado(string estado) { _flujo.Estado = estado; return this; }
        public FlujoBuilder Activo()                 { _flujo.Estado = EstadoActivo; return this; }
        public FlujoBuilder EnBorrador()             { _flujo.Estado = "Borrador"; return this; }

        /// <summary>Marca el flujo como histórico. Sin fecha usa una por defecto.</summary>
        public FlujoBuilder Historico(DateTime? fecha = null)
        {
            _flujo.Fecha_Historico = fecha ?? new DateTime(2020, 1, 1);
            return this;
        }

        /// <summary>Estado vigente explícito: activo y no histórico.</summary>
        public FlujoBuilder Vigente()
        {
            _flujo.Estado = EstadoActivo;
            _flujo.Fecha_Historico = null;
            return this;
        }

        public Flujo Build() => _flujo;
    }
}
