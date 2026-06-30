using DemoTestnet.Models;
using DemoTestnet.Tests.Builders;
using DemoTestnet.Tests.Mothers;

namespace DemoTestnet.Tests.Domain
{
    /// <summary>
    /// Los MISMOS tests que <see cref="FlujoTests"/> (mismos nombres, mismo Act y mismo Assert), pero con
    /// el Arrange montado por el <see cref="FlujoBuilder"/> en vez de a mano. Puestos lado a lado se ve el
    /// contraste: misma cobertura, el Arrange queda en una línea que se lee como una frase.
    /// </summary>
    public class ConstruccionDatosTests
    {
        [Fact]
        public void EsHistorico_ConFechaHistorico_DevuelveTrue()
        {
            // Arrange
            var flujo = new FlujoBuilder().Historico().Build();

            // Act
            var result = flujo.EsHistorico;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EsHistorico_SinFechaHistorico_DevuelveFalse()
        {
            // Arrange
            var flujo = new FlujoBuilder().Build();

            // Act
            var result = flujo.EsHistorico;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EstaActivo_EstadoActivo_DevuelveTrue()
        {
            // Arrange
            var flujo = new FlujoBuilder().Activo().Build();

            // Act
            var result = flujo.EstaActivo;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EstaActivo_EstadoDistinto_DevuelveFalse()
        {
            // Arrange
            var flujo = new FlujoBuilder().EnBorrador().Build();

            // Act
            var result = flujo.EstaActivo;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EstaActivo_EstadoEnMinusculas_DevuelveFalse()
        {
            // Arrange
            var flujo = new FlujoBuilder().ConEstado("activo").Build();

            // Act
            var result = flujo.EstaActivo;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EsVigente_NoHistoricoYActivo_DevuelveTrue()
        {
            // Arrange
            var flujo = new FlujoBuilder().Vigente().Build();

            // Act
            var result = flujo.EsVigente();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EsVigente_NoHistoricoYNoActivo_DevuelveFalse()
        {
            // Arrange
            var flujo = new FlujoBuilder().EnBorrador().Build();

            // Act
            var result = flujo.EsVigente();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EsVigente_HistoricoYActivo_DevuelveFalse()
        {
            // Arrange
            var flujo = new FlujoBuilder().Activo().Historico().Build();

            // Act
            var result = flujo.EsVigente();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EsVigente_DebeEstarActivoFalseYNoHistorico_DevuelveTrue()
        {
            // Arrange
            var flujo = new FlujoBuilder().EnBorrador().Build();

            // Act
            var result = flujo.EsVigente(debeEstarActivo: false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EsVigente_DebeEstarActivoFalseYHistorico_DevuelveFalse()
        {
            // Arrange
            var flujo = new FlujoBuilder().Activo().Historico().Build();

            // Act
            var result = flujo.EsVigente(debeEstarActivo: false);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(false, "Activo", true)]
        [InlineData(false, "Borrador", true)]
        [InlineData(true, "Activo", true)]
        [InlineData(true, "Borrador", true)]
        [InlineData(false, "Activo", false)]
        [InlineData(false, "Borrador", false)]
        [InlineData(true, "Activo", false)]
        [InlineData(true, "Borrador", false)]
        public void SpecVigente_ParaCualquierCombinacion_CoincideConEsVigente(
            bool esHistorico, string estado, bool debeEstarActivo)
        {
            // Arrange
            var builder = new FlujoBuilder().ConEstado(estado);
            if (esHistorico) builder.Historico();
            var flujo = builder.Build();
            var spec = Flujo.SpecVigente(debeEstarActivo).Compile();

            // Act
            var porSpec = spec(flujo);
            var porMetodo = flujo.EsVigente(debeEstarActivo);

            // Assert
            Assert.Equal(porMetodo, porSpec);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Tests AMPLIADOS con builder
        // Documentación viva del FlujoBuilder: un test por método, cada uno explicando
        // qué hace. Cubren TODOS los métodos del builder. Al final, tres tests del
        // FlujoMother (arquetipos fijos montados sobre el propio builder).
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Build_SinConfigurar_ConservaLosDefaultsDelModelo()
        {
            // El builder es transparente: sin decirle nada, Build() devuelve un Flujo con los
            // valores por defecto del MODELO. No inventa ninguno; solo cambia lo que se le pide.
            var flujo = new FlujoBuilder().Build();

            Assert.Equal(0, flujo.Id);
            Assert.Equal(string.Empty, flujo.Codigo);
            Assert.Equal(string.Empty, flujo.Estado);
            Assert.Null(flujo.Fecha_Historico);
        }

        [Fact]
        public void Codigo_NoEspecificado_ConservaElDefaultDelModelo()
        {
            // Demo del valor por defecto de Codigo A TRAVÉS DEL BUILDER: como no se llama a
            // ConCodigo(), el builder no lo toca y Build() conserva el default del modelo.
            var flujo = new FlujoBuilder().Build();

            Assert.Equal(string.Empty, flujo.Codigo);
        }

        [Fact]
        public void Estado_NoEspecificado_ConservaElDefaultDelModelo()
        {
            // Igual con Estado: sin Activo()/EnBorrador()/ConEstado(), el builder no lo impone.
            var flujo = new FlujoBuilder().Build();

            Assert.Equal(string.Empty, flujo.Estado);
        }

        [Fact]
        public void ConId_FijaElIdentificador()
        {
            // ConId(n): sobreescribe el Id por defecto.
            var flujo = new FlujoBuilder().ConId(42).Build();

            Assert.Equal(42, flujo.Id);
        }

        [Fact]
        public void ConCodigo_FijaElCodigo()
        {
            // ConCodigo(s): sobreescribe el código por defecto.
            var flujo = new FlujoBuilder().ConCodigo("APROB-2024").Build();

            Assert.Equal("APROB-2024", flujo.Codigo);
        }

        [Fact]
        public void ConEstado_FijaUnEstadoArbitrario()
        {
            // ConEstado(s): escotilla de escape para cualquier estado sin atajo propio.
            // "Revision" no es el estado activo → EstaActivo es false.
            var flujo = new FlujoBuilder().ConEstado("Revision").Build();

            Assert.Equal("Revision", flujo.Estado);
            Assert.False(flujo.EstaActivo);
        }

        [Fact]
        public void Activo_PoneElEstadoActivo()
        {
            // Activo(): atajo al estado activo de aprobaciones. Partimos de borrador para
            // que se vea que Activo() realmente cambia el estado.
            var flujo = new FlujoBuilder().EnBorrador().Activo().Build();

            Assert.True(flujo.EstaActivo);
        }

        [Fact]
        public void EnBorrador_PoneUnEstadoNoActivo()
        {
            // EnBorrador(): atajo a un estado distinto del activo ("Borrador").
            var flujo = new FlujoBuilder().EnBorrador().Build();

            Assert.False(flujo.EstaActivo);
        }

        [Fact]
        public void Historico_SinFecha_MarcaHistoricoConFechaPorDefecto()
        {
            // Historico(): marca el flujo como histórico; sin argumento usa una fecha por defecto.
            var flujo = new FlujoBuilder().Historico().Build();

            Assert.True(flujo.EsHistorico);
        }

        [Fact]
        public void Historico_ConFecha_UsaEsaFecha()
        {
            // Historico(fecha): marca histórico con una fecha concreta.
            var fecha = new DateTime(2019, 5, 4);
            var flujo = new FlujoBuilder().Historico(fecha).Build();

            Assert.True(flujo.EsHistorico);
            Assert.Equal(fecha, flujo.Fecha_Historico);
        }

        [Fact]
        public void Vigente_DejaElFlujoActivoYNoHistorico()
        {
            // Vigente(): atajo que combina activo + no histórico. Partimos de histórico
            // para que se vea que Vigente() lo "resetea" a vigente.
            var flujo = new FlujoBuilder().Historico().Vigente().Build();

            Assert.True(flujo.EstaActivo);
            Assert.False(flujo.EsHistorico);
            Assert.True(flujo.EsVigente());
        }

        [Fact]
        public void DecisionesEncadenadas_MontanElFlujoCompleto()
        {
            // El builder es fluido: varias decisiones se encadenan en una sola expresión.
            var flujo = new FlujoBuilder()
                .ConId(7)
                .ConCodigo("APROB-2024")
                .EnBorrador()
                .Build();

            Assert.Equal(7, flujo.Id);
            Assert.Equal("APROB-2024", flujo.Codigo);
            Assert.False(flujo.EstaActivo);
            Assert.True(flujo.EsVigente(debeEstarActivo: false));
        }

        [Fact]
        public void Mother_Vigente_DaUnFlujoVigente()
        {
            // Arrange — el Mother da el arquetipo; al test solo le importa la regla
            var flujo = FlujoMother.Vigente();

            // Assert
            Assert.True(flujo.EsVigente());
        }

        [Fact]
        public void Mother_Borrador_NoEstaActivoPeroEsVigenteSinExigirActivo()
        {
            // Arrange
            var flujo = FlujoMother.Borrador();

            // Assert
            Assert.False(flujo.EstaActivo);
            Assert.True(flujo.EsVigente(debeEstarActivo: false));
        }

        [Fact]
        public void Mother_Historico_EsHistorico()
        {
            // Arrange
            var flujo = FlujoMother.Historico();

            // Assert
            Assert.True(flujo.EsHistorico);
            Assert.False(flujo.EsVigente());
        }
    }
}
