using DemoTestnet;
using DemoTestnet.Data;
using DemoTestnet.Models;
using DemoTestnet.Services;
using Microsoft.EntityFrameworkCore;

namespace DemoTestnet.Tests
{
    public class FlujoServiceTests
    {
        private static AppDbContext CreateContext(params Flujo[] flujos)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Flujo.AddRange(flujos);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetFlujoActualPorCodigo_CodigoConFlujoActivoVigente_DevuelveEseFlujo()
        {
            // Arrange
            using var context = CreateContext(
                new Flujo { Codigo = "A", Estado = Constants.Strings.Aprobaciones.Flujo_Estado_Activo },
                new Flujo { Codigo = "B", Estado = Constants.Strings.Aprobaciones.Flujo_Estado_Activo });
            var service = new FlujoService(context);

            // Act
            var result = service.GetFlujoActualPorCodigo("A");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("A", result!.Codigo);
        }

        [Fact]
        public void GetFlujoActualPorCodigo_FlujoHistorico_DevuelveNull()
        {
            // Arrange
            using var context = CreateContext(
                new Flujo
                {
                    Codigo = "A",
                    Estado = Constants.Strings.Aprobaciones.Flujo_Estado_Activo,
                    Fecha_Historico = new DateTime(2020, 1, 1)
                });
            var service = new FlujoService(context);

            // Act
            var result = service.GetFlujoActualPorCodigo("A");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetFlujoActualPorCodigo_FlujoNoActivo_DevuelveNullSalvoQueNoSeExijaActivo()
        {
            // Arrange
            using var context = CreateContext(
                new Flujo { Codigo = "A", Estado = "Borrador" });
            var service = new FlujoService(context);

            // Act
            var conFiltroActivo = service.GetFlujoActualPorCodigo("A");
            var sinFiltroActivo = service.GetFlujoActualPorCodigo("A", debeEstarActivo: false);

            // Assert
            Assert.Null(conFiltroActivo);
            Assert.NotNull(sinFiltroActivo);
        }

        [Fact]
        public void GetFlujosPorCodigo_VariosFlujos_DevuelveSoloLosDelCodigo()
        {
            // Arrange
            using var context = CreateContext(
                new Flujo { Codigo = "A", Estado = Constants.Strings.Aprobaciones.Flujo_Estado_Activo },
                new Flujo { Codigo = "A", Estado = "Borrador", Fecha_Historico = new DateTime(2020, 1, 1) },
                new Flujo { Codigo = "B", Estado = "Borrador" });
            var service = new FlujoService(context);

            // Act
            var result = service.GetFlujosPorCodigo("A").ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, f => Assert.Equal("A", f.Codigo));
        }
    }
}
