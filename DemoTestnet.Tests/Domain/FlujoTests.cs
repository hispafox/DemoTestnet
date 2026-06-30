using DemoTestnet;
using DemoTestnet.Models;

namespace DemoTestnet.Tests.Domain
{
    public class FlujoTests
    {
        private const string Activo = Constants.Strings.Aprobaciones.Flujo_Estado_Activo;

        [Fact]
        public void Codigo_FlujoNuevo_TieneValorPorDefecto()
        {
            // El valor por defecto vive en el MODELO, no en el builder. El FlujoBuilder pone
            // su propio Codigo ("FLUJO-TEST"), así que no sirve para comprobar este default:
            // hay que instanciar el modelo directo.
            // Arrange / Act
            var flujo = new Flujo();

            // Assert
            Assert.Equal(string.Empty, flujo.Codigo);
        }

        [Fact]
        public void Estado_FlujoNuevo_TieneValorPorDefecto()
        {
            // Mismo punto que Codigo: el default vive en el modelo, no en el builder.
            // Arrange / Act
            var flujo = new Flujo();

            // Assert
            Assert.Equal(string.Empty, flujo.Estado);
        }

        [Fact]
        public void EsHistorico_ConFechaHistorico_DevuelveTrue()
        {
            // Arrange
            var flujo = new Flujo { Fecha_Historico = new DateTime(2020, 1, 1) };

            // Act
            var result = flujo.EsHistorico;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EsHistorico_SinFechaHistorico_DevuelveFalse()
        {
            // Arrange
            var flujo = new Flujo { Fecha_Historico = null };

            // Act
            var result = flujo.EsHistorico;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EstaActivo_EstadoActivo_DevuelveTrue()
        {
            // Arrange
            var flujo = new Flujo { Estado = Activo };

            // Act
            var result = flujo.EstaActivo;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EstaActivo_EstadoDistinto_DevuelveFalse()
        {
            // Arrange
            var flujo = new Flujo { Estado = "Borrador" };

            // Act
            var result = flujo.EstaActivo;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EstaActivo_EstadoEnMinusculas_DevuelveFalse()
        {
            // Arrange
            var flujo = new Flujo { Estado = "activo" };

            // Act
            var result = flujo.EstaActivo;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EsVigente_NoHistoricoYActivo_DevuelveTrue()
        {
            // Arrange
            var flujo = new Flujo { Estado = Activo, Fecha_Historico = null };

            // Act
            var result = flujo.EsVigente();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EsVigente_NoHistoricoYNoActivo_DevuelveFalse()
        {
            // Arrange
            var flujo = new Flujo { Estado = "Borrador", Fecha_Historico = null };

            // Act
            var result = flujo.EsVigente();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EsVigente_HistoricoYActivo_DevuelveFalse()
        {
            // Arrange
            var flujo = new Flujo { Estado = Activo, Fecha_Historico = new DateTime(2020, 1, 1) };

            // Act
            var result = flujo.EsVigente();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EsVigente_DebeEstarActivoFalseYNoHistorico_DevuelveTrue()
        {
            // Arrange
            var flujo = new Flujo { Estado = "Borrador", Fecha_Historico = null };

            // Act
            var result = flujo.EsVigente(debeEstarActivo: false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EsVigente_DebeEstarActivoFalseYHistorico_DevuelveFalse()
        {
            // Arrange
            var flujo = new Flujo { Estado = Activo, Fecha_Historico = new DateTime(2020, 1, 1) };

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
            var flujo = new Flujo
            {
                Estado = estado,
                Fecha_Historico = esHistorico ? new DateTime(2020, 1, 1) : null
            };
            var spec = Flujo.SpecVigente(debeEstarActivo).Compile();

            // Act
            var porSpec = spec(flujo);
            var porMetodo = flujo.EsVigente(debeEstarActivo);

            // Assert
            Assert.Equal(porMetodo, porSpec);
        }
    }
}
