using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.Cliente;
using SegRutContAsis.Business.Services;
using SegRutContAsis.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Services
{
    public class ClienteServiceTests : IDisposable
    {
        private readonly SegRutContAsisContext _context;
        private readonly ClienteService _service;

        public ClienteServiceTests()
        {
            var options = new DbContextOptionsBuilder<SegRutContAsisContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            _context = new SegRutContAsisContext(options);

            // Inicializar datos de prueba
            SeedData();

            _service = new ClienteService(_context);
        }

        private void SeedData()
        {
            var cliente1 = new Cliente
            {
                clNombreCompleto = "Juan Perez",
                clCarnetIdentidad = "123456",
                clNitCliente = "NIT001",
                clTipoCliente = "Minorista",
                clTelefono = "555-1111",
                clEstadoDel = true
            };

            var cliente2 = new Cliente
            {
                clNombreCompleto = "Maria Gomez",
                clCarnetIdentidad = "654321",
                clNitCliente = "NIT002",
                clTipoCliente = "Mayorista",
                clTelefono = "555-2222",
                clEstadoDel = true
            };

            _context.Cliente.AddRange(cliente1, cliente2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task ObtenerClientes_DeberiaRetornarTodosLosClientesActivos()
        {
            var resultado = await _service.ObtenerClientes();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, c => Assert.True(!string.IsNullOrEmpty(c.clNombreCompleto)));
        }

        [Fact]
        public async Task ObtenerClientePorId_DeberiaRetornarClienteCorrecto()
        {
            var cliente = _context.Cliente.First();
            var resultado = await _service.ObtenerClientePorId(cliente.clId);

            Assert.NotNull(resultado);
            Assert.Equal(cliente.clNombreCompleto, resultado.clNombreCompleto);
            Assert.Equal(cliente.clCarnetIdentidad, resultado.clCarnetIdentidad);
        }

        [Fact]
        public async Task ObtenerClientePorId_NoExistente_DeberiaRetornarNull()
        {
            var resultado = await _service.ObtenerClientePorId(9999);
            Assert.Null(resultado);
        }

        [Fact]
        public async Task CrearCliente_DeberiaAgregarNuevoCliente()
        {
            var dto = new ClienteRequestDTO
            {
                clNombreCompleto = "Carlos Lopez",
                clCarnetIdentidad = "789012",
                clNitCliente = "NIT003",
                clTipoCliente = "Minorista",
                clTelefono = "555-3333"
            };

            var resultado = await _service.CrearCliente(dto);

            Assert.NotNull(resultado);
            Assert.Equal(dto.clNombreCompleto, resultado.clNombreCompleto);
            Assert.True(resultado.clId > 0);

            // Verificar que se agregó en el contexto
            var clienteEnDb = await _context.Cliente.FindAsync(resultado.clId);
            Assert.NotNull(clienteEnDb);
            Assert.Equal(dto.clNombreCompleto, clienteEnDb.clNombreCompleto);
        }

        [Fact]
        public async Task CrearCliente_CarnetDuplicado_DeberiaLanzarExcepcion()
        {
            var dto = new ClienteRequestDTO
            {
                clNombreCompleto = "Duplicado",
                clCarnetIdentidad = "123456", // mismo que cliente1
                clNitCliente = "NIT004",
                clTipoCliente = "Mayorista",
                clTelefono = "555-4444"
            };

            await Assert.ThrowsAsync<Exception>(() => _service.CrearCliente(dto));
        }

        [Fact]
        public async Task ActualizarCliente_DeberiaCambiarDatos()
        {
            var cliente = _context.Cliente.First();

            var dto = new ClienteRequestDTO
            {
                clNombreCompleto = "Juan Actualizado",
                clCarnetIdentidad = "1234567",
                clNitCliente = "NIT001-Act",
                clTipoCliente = "Mayorista",
                clTelefono = "555-9999"
            };

            var resultado = await _service.ActualizarCliente(cliente.clId, dto);

            Assert.NotNull(resultado);
            Assert.Equal("Juan Actualizado", resultado.clNombreCompleto);
            Assert.Equal("555-9999", resultado.clTelefono);
        }

        [Fact]
        public async Task ActualizarCliente_NoExistente_DeberiaRetornarNull()
        {
            var dto = new ClienteRequestDTO
            {
                clNombreCompleto = "No Existe",
                clCarnetIdentidad = "0000",
                clNitCliente = "NIT000",
                clTipoCliente = "Minorista",
                clTelefono = "555-0000"
            };

            var resultado = await _service.ActualizarCliente(9999, dto);
            Assert.Null(resultado);
        }

        [Fact]
        public async Task DeshabilitarCliente_DeberiaMarcarEstadoDelComoFalso()
        {
            var cliente = _context.Cliente.First();

            var resultado = await _service.DeshabilitarCliente(cliente.clId);
            Assert.True(resultado);

            var clienteActualizado = await _context.Cliente.FindAsync(cliente.clId);
            Assert.False(clienteActualizado.clEstadoDel);
        }

        [Fact]
        public async Task DeshabilitarCliente_NoExistente_DeberiaRetornarFalse()
        {
            var resultado = await _service.DeshabilitarCliente(9999);
            Assert.False(resultado);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
