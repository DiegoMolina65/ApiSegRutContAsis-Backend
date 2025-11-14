using SegRutContAsis.Business.DTO.Request.Visita;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Helpers;

namespace Tests.Services
{
    public class VisitaServiceTests
    {
        private readonly SegRutContAsisContext _context;
        private readonly VisitaService _service;

        public VisitaServiceTests()
        {
            _context = DbContextFactory.CreateInMemoryContext(Guid.NewGuid().ToString());
            _service = new VisitaService(_context);
        }

        [Fact]
        public async Task CrearVisita_DeberiaCrearCorrectamente()
        {
            // Arrange
            var usuario = new Usuario
            {
                usrId = 1,
                usrNombreCompleto = "Juan Perez",
                usrCorreo = "juan@example.com",
                usrTelefono = "789456123",
                usrNitEmpleado = "123456",
                usrCarnetIdentidad = "12345678",
                usrUsuarioLog = "juanlog",
                usrContrasenaLog = "123456",
            };

            var vendedor = new Vendedor { venId = 1, Usuario = usuario };
            var supervisor = new Supervisor
            {
                supId = 1,
                Usuario = new Usuario
                {
                    usrId = 2,
                    usrNombreCompleto = "Supervisor 1",
                    usrCorreo = "sup@example.com",
                    usrTelefono = "111111",
                    usrNitEmpleado = "111111",
                    usrCarnetIdentidad = "222222",
                    usrUsuarioLog = "sup",
                    usrContrasenaLog = "123"
                }
            };

            var ruta = new Ruta
            {
                rutId = 1,
                rutNombre = "Ruta 1",
                rutEstadoDel = true,
                Vendedor = vendedor,
                Supervisor = supervisor
            };

            var cliente = new Cliente
            {
                clId = 1,
                clNombreCompleto = "Cliente 1",
                clCarnetIdentidad = "1234",
                clNitCliente = "5678",
                clTelefono = "7890"
            };

            var direccion = new DireccionCliente
            {
                dirClId = 1,
                Cliente = cliente,
                dirClNombreSucursal = "Sucursal Central",
                dirClDireccion = "Calle Falsa 123",
                dirClLatitud = 0,
                dirClLongitud = 0
            };

            _context.Usuario.Add(usuario);
            _context.Vendedor.Add(vendedor);
            _context.Supervisor.Add(supervisor);
            _context.Ruta.Add(ruta);
            _context.Cliente.Add(cliente);
            _context.DireccionCliente.Add(direccion);
            await _context.SaveChangesAsync();

            var dto = new VisitaRequestDTO
            {
                rutId = ruta.rutId,
                dirClId = direccion.dirClId,
                visComentario = "Primera visita"
            };

            // Act
            var result = await _service.CrearVisita(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.rutId, result.rutId);
            Assert.Equal(cliente.clNombreCompleto, result.NombreCliente);
        }

        [Fact]
        public async Task ActualizarVisita_DeberiaActualizarDatos()
        {
            // Arrange: Crear todos los datos relacionados
            var usuarioVendedor = new Usuario
            {
                usrId = 1,
                usrNombreCompleto = "Juan Perez",
                usrCorreo = "juan@example.com",
                usrTelefono = "789456123",
                usrNitEmpleado = "123456",
                usrCarnetIdentidad = "12345678",
                usrUsuarioLog = "juanlog",
                usrContrasenaLog = "123456",
            };
            var vendedor = new Vendedor { venId = 1, Usuario = usuarioVendedor };

            var usuarioSupervisor = new Usuario
            {
                usrId = 2,
                usrNombreCompleto = "Supervisor 1",
                usrCorreo = "sup@example.com",
                usrTelefono = "111111",
                usrNitEmpleado = "111111",
                usrCarnetIdentidad = "222222",
                usrUsuarioLog = "sup",
                usrContrasenaLog = "123"
            };
            var supervisor = new Supervisor { supId = 1, Usuario = usuarioSupervisor };

            var ruta = new Ruta
            {
                rutId = 1,
                rutNombre = "Ruta 1",
                rutEstadoDel = true,
                Vendedor = vendedor,
                Supervisor = supervisor
            };

            var cliente = new Cliente
            {
                clId = 1,
                clNombreCompleto = "Cliente 1",
                clCarnetIdentidad = "1234",
                clNitCliente = "5678",
                clTelefono = "7890"
            };

            var direccion = new DireccionCliente
            {
                dirClId = 1,
                Cliente = cliente,
                dirClNombreSucursal = "Sucursal Central",
                dirClDireccion = "Calle Falsa 123",
                dirClLatitud = 0,
                dirClLongitud = 0
            };

            _context.Usuario.AddRange(usuarioVendedor, usuarioSupervisor);
            _context.Vendedor.Add(vendedor);
            _context.Supervisor.Add(supervisor);
            _context.Ruta.Add(ruta);
            _context.Cliente.Add(cliente);
            _context.DireccionCliente.Add(direccion);
            await _context.SaveChangesAsync();

            // Crear visita asociada correctamente
            var visita = new Visita
            {
                rutId = ruta.rutId,
                dirClId = direccion.dirClId,
                visComentario = "Comentario inicial",
                visEstadoDel = true // Muy importante
            };
            _context.Visita.Add(visita);
            await _context.SaveChangesAsync();

            var dtoUpdate = new VisitaRequestDTO
            {
                rutId = ruta.rutId,
                dirClId = direccion.dirClId,
                visComentario = "Comentario actualizado"
            };

            // Act
            var result = await _service.ActualizarVisita(visita.visId, dtoUpdate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dtoUpdate.visComentario, result.visComentario);
        }



    }
}
