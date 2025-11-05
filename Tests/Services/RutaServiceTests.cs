using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.Ruta;
using SegRutContAsis.Business.Services;
using SegRutContAsis.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Services
{
    public class RutaServiceTests : IDisposable
    {
        private readonly SegRutContAsisContext _context;
        private readonly RutaService _service;

        public RutaServiceTests()
        {
            var options = new DbContextOptionsBuilder<SegRutContAsisContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            _context = new SegRutContAsisContext(options);

            // Llenar la BD de prueba
            SeedData();

            _service = new RutaService(_context);
        }

        private void SeedData()
        {
            // Crear usuarios completos con todos los campos requeridos
            var usuario1 = new Usuario
            {
                usrNombreCompleto = "Juan Perez",
                usrCarnetIdentidad = "123456",
                usrContrasenaLog = "sistema123",
                usrCorreo = "juan@correo.com",
                usrNitEmpleado = "NIT001",
                usrTelefono = "555-1234",
                usrUsuarioLog = "sistema"
            };

            var usuario2 = new Usuario
            {
                usrNombreCompleto = "Maria Gomez",
                usrCarnetIdentidad = "654321",
                usrContrasenaLog = "pass456",
                usrCorreo = "maria@correo.com",
                usrNitEmpleado = "NIT002",
                usrTelefono = "555-5678",
                usrUsuarioLog = "mariag"
            };

            _context.Usuario.AddRange(usuario1, usuario2);
            _context.SaveChanges();

            // Crear vendedores asociados a usuarios
            var vendedor1 = new Vendedor { usrId = usuario1.usrId };
            var vendedor2 = new Vendedor { usrId = usuario2.usrId };

            _context.Vendedor.AddRange(vendedor1, vendedor2);
            _context.SaveChanges();

            // Crear rutas
            var ruta1 = new Ruta
            {
                VendedorId = vendedor1.venId,
                SupervisorId = 1,
                rutNombre = "Ruta Norte",
                rutComentario = "Comentario 1",
                rutEstadoDel = true
            };

            var ruta2 = new Ruta
            {
                VendedorId = vendedor2.venId,
                SupervisorId = 2,
                rutNombre = "Ruta Sur",
                rutComentario = "Comentario 2",
                rutEstadoDel = true
            };

            _context.Ruta.AddRange(ruta1, ruta2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task ObtenerRutas_DeberiaRetornarListaConRutas()
        {
            var resultado = await _service.ObtenerRutas();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
        }

        [Fact]
        public async Task ObtenerRutasPorVendedor_DeberiaFiltrarCorrectamente()
        {
            var vendedor = _context.Vendedor.First();
            var resultado = await _service.ObtenerRutasPorVendedor(vendedor.venId);

            Assert.All(resultado, r => Assert.Equal(vendedor.venId, r.venId));
        }

        [Fact]
        public async Task ObtenerRutasPorSupervisor_DeberiaFiltrarCorrectamente()
        {
            var resultado = await _service.ObtenerRutasPorSupervisor(1);

            Assert.Single(resultado);
            Assert.Equal(1, resultado.First().supId);
        }

        [Fact]
        public async Task ObtenerRutaId_DeberiaRetornarRutaCorrecta()
        {
            var ruta = _context.Ruta.First();
            var resultado = await _service.ObtenerRutaId(ruta.rutId);

            Assert.NotNull(resultado);
            Assert.Equal(ruta.rutNombre, resultado.rutNombre);
        }

        [Fact]
        public async Task CrearRuta_DeberiaRetornarRutaCreada()
        {
            var vendedor = _context.Vendedor.First();

            var dto = new RutaRequestDTO
            {
                venId = vendedor.venId,
                supId = 3,
                rutNombre = "Ruta Nueva",
                rutComentario = "Comentario Nuevo"
            };

            var resultado = await _service.CrearRuta(dto);

            Assert.NotNull(resultado);
            Assert.Equal(dto.rutNombre, resultado.rutNombre);
            Assert.Equal(vendedor.venId, resultado.venId);
        }

        [Fact]
        public async Task ActualizarRuta_DeberiaCambiarDatos()
        {
            var ruta = _context.Ruta.First();

            var dto = new RutaRequestDTO
            {
                venId = ruta.VendedorId,
                supId = ruta.SupervisorId,
                rutNombre = "Ruta Actualizada",
                rutComentario = "Comentario Actualizado"
            };

            var resultado = await _service.ActualizarRuta(ruta.rutId, dto);

            Assert.NotNull(resultado);
            Assert.Equal("Ruta Actualizada", resultado.rutNombre);
            Assert.Equal("Comentario Actualizado", resultado.rutComentario);
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
