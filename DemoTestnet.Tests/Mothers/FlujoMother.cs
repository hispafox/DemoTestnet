using DemoTestnet.Models;
using DemoTestnet.Tests.Builders;

namespace DemoTestnet.Tests.Mothers
{
    /// <summary>
    /// Object Mother para <see cref="Flujo"/>: los arquetipos que se repiten en la suite, uno por cada
    /// regla de dominio. Por dentro usa el <see cref="FlujoBuilder"/> con valores por defecto — la comodidad
    /// del Mother para lo habitual y el Builder por debajo para construirlo.
    /// </summary>
    public static class FlujoMother
    {
        /// <summary>Activo y no histórico → <c>EsVigente()</c> es true.</summary>
        public static Flujo Vigente() => new FlujoBuilder().Vigente().Build();

        /// <summary>En borrador y no histórico → no activo, pero vigente si no se exige activo.</summary>
        public static Flujo Borrador() => new FlujoBuilder().EnBorrador().Build();

        /// <summary>Con fecha de paso a histórico → <c>EsHistorico</c> es true.</summary>
        public static Flujo Historico() => new FlujoBuilder().Activo().Historico().Build();
    }
}
