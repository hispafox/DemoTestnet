using System.Linq.Expressions;

namespace DemoTestnet.Models
{
    public class Flujo
    {
        public int Id { get; set; }

        public string Codigo { get; set; } = string.Empty;
        //public string Codigo { get; set; } = "---";

        public string Estado { get; set; } = string.Empty;

        public DateTime? Fecha_Historico { get; set; }

        // --- Reglas de dominio ---

        /// <summary>Un flujo es histórico cuando tiene fecha de paso a histórico.</summary>
        public bool EsHistorico => Fecha_Historico.HasValue;

        /// <summary>Un flujo está activo cuando su estado es el estado activo de aprobaciones.</summary>
        public bool EstaActivo => Estado == Constants.Strings.Aprobaciones.Flujo_Estado_Activo;

        /// <summary>
        /// Un flujo es vigente cuando no es histórico y, si se exige, además está activo.
        /// </summary>
        public bool EsVigente(bool debeEstarActivo = true)
            => !EsHistorico && (!debeEstarActivo || EstaActivo);

        /// <summary>
        /// Misma regla que <see cref="EsVigente"/> pero como expresión traducible a SQL por EF Core.
        /// Es la única fuente de verdad para las consultas en base de datos.
        /// </summary>
        public static Expression<Func<Flujo, bool>> SpecVigente(bool debeEstarActivo = true)
            => x => !x.Fecha_Historico.HasValue &&
                    (!debeEstarActivo || x.Estado == Constants.Strings.Aprobaciones.Flujo_Estado_Activo);
    }
}
