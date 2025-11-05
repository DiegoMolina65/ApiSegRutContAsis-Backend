using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SegRutContAsis.Business.DTO.Request.Usuario;
using SegRutContAsis.Business.DTO.Response.Usuario;
using SegRutContAsis.Business.Services;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Services
{
    public class UsuarioServiceTests
    {
        private readonly SegRutContAsisContext _context;
        private readonly UsuarioService _service;

        public UsuarioServiceTests()
        {
            // Configuración InMemory
            var options = new DbContextOptionsBuilder<SegRutContAsisContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SegRutContAsisContext(options);

            // Configuración de JWT mínima para los tests
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "clave_secreta_para_tests_1234567890"},
                {"Jwt:Issuer", "Tests"},
                {"Jwt:Audience", "Tests"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _service = new UsuarioService(_context, configuration);

            // Seed inicial de roles y usuarios
            SeedData().Wait();
        }

        private async Task SeedData()
        {
            // Roles
            var adminRol = new Rol { rolId = 1, rolNombre = "ADMINISTRADOR", rolDescripcion = "Administrador" };
            var supervisorRol = new Rol { rolId = 2, rolNombre = "SUPERVISOR", rolDescripcion = "Supervisor" };
            var vendedorRol = new Rol { rolId = 3, rolNombre = "VENDEDOR", rolDescripcion = "Vendedor" };

            _context.Rol.AddRange(adminRol, supervisorRol, vendedorRol);
            await _context.SaveChangesAsync();

            // Usuario con contraseña plana
            var usuario = new Usuario
            {
                usrId = 1,
                usrNombreCompleto = "Juan Perez",
                usrCorreo = "juan@example.com",
                usrTelefono = "789456123",
                usrNitEmpleado = "123456",
                usrCarnetIdentidad = "12345678",
                usrUsuarioLog = "juanlog",
                usrContrasenaLog = "123456", // contraseña plana
                usrEstadoDel = true,
                usrFechaCreacion = DateTime.Now
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            // Asociar roles
            _context.UsuarioRol.Add(new UsuarioRol { usrID = usuario.usrId, rolId = adminRol.rolId });
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task RegistrarUsuario_DeberiaCrearUsuarioConRoles()
        {
            var dto = new UsuarioRegistroRequestDTO
            {
                usrNombreCompleto = "Maria Lopez",
                usrCorreo = "maria@example.com",
                usrTelefono = "654987321",
                usrNitEmpleado = "654321",
                usrCarnetIdentidad = "87654321",
                usrUsuarioLog = "marialog",
                usrContrasenaLog = "123456",
                Roles = new List<string> { "VENDEDOR" }
            };

            var result = await _service.RegistrarUsuario(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.usrNombreCompleto, result.usrNombreCompleto);
            Assert.True(result.usrId > 0);
        }

        /*
        [Fact]
        public async Task LoginUsuario_DeberiaRetornarToken()
        {
            var dto = new UsuarioRequestDTO
            {
                UsuarioLog = "juanlog",
                ContrasenaLog = "123456" // coincide con la contraseña plana
            };

            var result = await _service.LoginUsuario(dto);
            Assert.NotNull(result.Token);
            Assert.Contains("ADMINISTRADOR", result.Roles);
        }
        */


        [Fact]
        public async Task ObtenerUsuarios_DeberiaRetornarLista()
        {
            var result = await _service.ObtenerUsuarios();
            Assert.NotEmpty(result);
            Assert.Contains(result, u => u.usrUsuarioLog == "juanlog");
        }

        [Fact]
        public async Task ActualizarUsuario_DeberiaCambiarDatosYRoles()
        {
            var dto = new UsuarioRegistroRequestDTO
            {
                usrNombreCompleto = "Juan Actualizado",
                usrCorreo = "juanup@example.com",
                usrTelefono = "111222333",
                usrNitEmpleado = "999888",
                usrCarnetIdentidad = "12345678",
                usrUsuarioLog = "juanlog",
                usrContrasenaLog = "123456",
                Roles = new List<string> { "SUPERVISOR" }
            };

            var usuario = await _service.ActualizarUsuario(1, dto);

            Assert.Equal("Juan Actualizado", usuario.usrNombreCompleto);
            var roles = _context.UsuarioRol.Where(ur => ur.usrID == usuario.usrId).Select(ur => ur.Rol.rolNombre).ToList();
            Assert.Contains("SUPERVISOR", roles);
        }

        [Fact]
        public async Task DeshabilitarUsuario_DeberiaMarcarEstadoDelComoFalso()
        {
            var result = await _service.DeshabilitarUsuario(1);
            Assert.True(result);

            var usuario = await _context.Usuario.FindAsync(1);
            Assert.False(usuario.usrEstadoDel);
        }
    }
}
