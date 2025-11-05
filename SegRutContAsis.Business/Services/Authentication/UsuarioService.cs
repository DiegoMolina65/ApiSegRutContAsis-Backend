using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SegRutContAsis.Business.DTO.Request.Usuario;
using SegRutContAsis.Business.DTO.Response.Usuario;
using SegRutContAsis.Business.Interfaces.Authentication;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly SegRutContAsisContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioService(SegRutContAsisContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // MÉTODO LOGOUT USUARIO
        public async Task<bool> LogoutUsuario()
        { 
            return await Task.FromResult(true);
        }

        // LOGIN
        public async Task<UsuarioReponseDTO> LoginUsuario(UsuarioRequestDTO request)
        {
            var usuario = await _context.Usuario
                 .Include(u => u.UsuarioRoles)
                     .ThenInclude(ur => ur.Rol)
                 .FirstOrDefaultAsync(u => u.usrUsuarioLog == request.UsuarioLog);

            if (!usuario.usrEstadoDel)
                throw new Exception("El usuario está desactivado. Contacte con el administrador.");

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            if (!VerifyPassword(request.ContrasenaLog, usuario.usrContrasenaLog))
                throw new Exception("Contraseña incorrecta");

            var roles = usuario.UsuarioRoles.Select(r => r.Rol.rolNombre).ToList();
            var token = GenerateJwtToken(usuario, roles);

            return new UsuarioReponseDTO
            {
                usrId = usuario.usrId,
                Token = token,
                usrNombreCompleto = usuario.usrNombreCompleto,
                Roles = roles
            };
        }

        // REGISTRAR USUARIO (MULTI-ROL)
        public async Task<Usuario> RegistrarUsuario(UsuarioRegistroRequestDTO dto)
        {
            var existeUsuario = await _context.Usuario
                .AnyAsync(u => u.usrUsuarioLog == dto.usrUsuarioLog || u.usrCorreo == dto.usrCorreo);
            if (existeUsuario)
                throw new Exception("Usuario o correo ya existe.");

            var usuario = new Usuario
            {
                usrNombreCompleto = dto.usrNombreCompleto,
                usrCorreo = dto.usrCorreo,
                usrTelefono = dto.usrTelefono,
                usrNitEmpleado = dto.usrNitEmpleado,
                usrCarnetIdentidad = dto.usrCarnetIdentidad,
                usrUsuarioLog = dto.usrUsuarioLog,
                usrContrasenaLog = HashPassword(dto.usrContrasenaLog),
                usrEstadoDel = true,
                usrFechaCreacion = DateTime.Now
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            foreach (var rolNombre in dto.Roles)
            {
                var rol = await _context.Rol.FirstOrDefaultAsync(r => r.rolNombre.ToUpper() == rolNombre.ToUpper());
                if (rol == null) continue;

                _context.UsuarioRol.Add(new UsuarioRol
                {
                    usrID = usuario.usrId,
                    rolId = rol.rolId
                });

                switch (rolNombre.ToUpper())
                {
                    case "ADMINISTRADOR":
                        _context.Administrador.Add(new Administrador
                        {
                            usrId = usuario.usrId,
                            admFechaCreacion = DateTime.Now,
                            admEstadoDel = true
                        });
                        break;

                    case "SUPERVISOR":
                        _context.Supervisor.Add(new Supervisor
                        {
                            usrId = usuario.usrId,
                            supFechaCreacion = DateTime.Now,
                            supEstadoDel = true
                        });
                        break;

                    case "VENDEDOR":
                        _context.Vendedor.Add(new Vendedor
                        {
                            usrId = usuario.usrId,
                            venFechaCreacion = DateTime.Now,
                            venEstadoDel = true
                        });
                        break;
                }
            }

            await _context.SaveChangesAsync();
            return usuario;
        }

        // OBTENER TODOS LOS USUARIOS
        public async Task<List<UsuarioReponseDTO>> ObtenerUsuarios()
        {
            return await _context.Usuario
                .Where(u => u.usrEstadoDel == true)
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .Select(u => new UsuarioReponseDTO
                {
                    usrId = u.usrId,
                    usrNombreCompleto = u.usrNombreCompleto,
                    usrCorreo = u.usrCorreo,
                    usrCarnetIdentidad = u.usrCarnetIdentidad,
                    usrNitEmpleado = u.usrNitEmpleado,
                    usrTelefono = u.usrTelefono,
                    usrUsuarioLog = u.usrUsuarioLog,
                    usrContrasenaLog = u.usrContrasenaLog,
                    usrEstadoDel = u.usrEstadoDel,
                    Roles = u.UsuarioRoles.Select(r => r.Rol.rolNombre).ToList(),
                    Token = null 
                })
                .ToListAsync();
        }

        // OBTENER USUARIO POR ID
        public async Task<UsuarioReponseDTO> ObtenerUsuarioId(int id)
        {
            var usuario = await _context.Usuario
                .Where(u => u.usrEstadoDel == true)
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.usrId == id);

            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            return new UsuarioReponseDTO
            {
                usrId = usuario.usrId,
                usrNombreCompleto = usuario.usrNombreCompleto,
                usrCorreo = usuario.usrCorreo,
                usrCarnetIdentidad = usuario.usrCarnetIdentidad,
                usrNitEmpleado = usuario.usrNitEmpleado,
                usrTelefono = usuario.usrTelefono,
                usrUsuarioLog = usuario.usrUsuarioLog,
                usrContrasenaLog = usuario.usrContrasenaLog,
                usrEstadoDel = usuario.usrEstadoDel,
                Roles = usuario.UsuarioRoles.Select(r => r.Rol.rolNombre).ToList()
            };
        }

        // ACTUALIZAR USUARIO
        public async Task<Usuario> ActualizarUsuario(int id, UsuarioRegistroRequestDTO dto)
        {
            var usuario = await _context.Usuario
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol) 
                .FirstOrDefaultAsync(u => u.usrId == id);

            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            usuario.usrNombreCompleto = dto.usrNombreCompleto;
            usuario.usrCorreo = dto.usrCorreo;
            usuario.usrTelefono = dto.usrTelefono;
            usuario.usrNitEmpleado = dto.usrNitEmpleado;
            usuario.usrCarnetIdentidad = dto.usrCarnetIdentidad;
            usuario.usrUsuarioLog = dto.usrUsuarioLog;

            if (!string.IsNullOrEmpty(dto.usrContrasenaLog))
                usuario.usrContrasenaLog = HashPassword(dto.usrContrasenaLog);

            var rolesActuales = usuario.UsuarioRoles
                .Where(ur => ur.Rol != null)
                .Select(ur => ur.Rol.rolNombre.ToUpper())
                .ToList();

            var rolesNuevos = dto.Roles.Select(r => r.ToUpper()).ToList();

            var rolesAEliminar = usuario.UsuarioRoles
                .Where(ur => ur.Rol != null && !rolesNuevos.Contains(ur.Rol.rolNombre.ToUpper()))
                .ToList();

            foreach (var ur in rolesAEliminar)
            {
                _context.UsuarioRol.Remove(ur);

                switch (ur.Rol.rolNombre.ToUpper())
                {
                    case "ADMINISTRADOR":
                        var admin = await _context.Administrador.FirstOrDefaultAsync(a => a.usrId == id);
                        if (admin != null) _context.Administrador.Remove(admin);
                        break;
                    case "SUPERVISOR":
                        var sup = await _context.Supervisor.FirstOrDefaultAsync(s => s.usrId == id);
                        if (sup != null) _context.Supervisor.Remove(sup);
                        break;
                    case "VENDEDOR":
                        var ven = await _context.Vendedor.FirstOrDefaultAsync(v => v.usrId == id);
                        if (ven != null) _context.Vendedor.Remove(ven);
                        break;
                }
            }
            var rolesParaAgregar = rolesNuevos.Except(rolesActuales).ToList();
            foreach (var rolNombre in rolesParaAgregar)
            {
                var rol = await _context.Rol.FirstOrDefaultAsync(r => r.rolNombre.ToUpper() == rolNombre);
                if (rol == null) continue;

                _context.UsuarioRol.Add(new UsuarioRol
                {
                    usrID = usuario.usrId,
                    rolId = rol.rolId
                });

                switch (rolNombre)
                {
                    case "ADMINISTRADOR":
                        _context.Administrador.Add(new Administrador
                        {
                            usrId = usuario.usrId,
                            admFechaCreacion = DateTime.Now,
                            admEstadoDel = true
                        });
                        break;
                    case "SUPERVISOR":
                        _context.Supervisor.Add(new Supervisor
                        {
                            usrId = usuario.usrId,
                            supFechaCreacion = DateTime.Now,
                            supEstadoDel = true
                        });
                        break;
                    case "VENDEDOR":
                        _context.Vendedor.Add(new Vendedor
                        {
                            usrId = usuario.usrId,
                            venFechaCreacion = DateTime.Now,
                            venEstadoDel = true
                        });
                        break;
                }
            }

            await _context.SaveChangesAsync();
            return usuario;
        }

        // DESHABILITAR USUARIO
        public async Task<bool> DeshabilitarUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null) return false;

            usuario.usrEstadoDel = false;
            await _context.SaveChangesAsync();
            return true;
        }

        // OBTENER TODOS LOS VENDEDORES ACTIVOS
        public async Task<List<UsuarioReponseDTO>> ObtenerVendedores()
        {
            return await _context.Vendedor
                .Include(v => v.Usuario)
                    .ThenInclude(u => u.UsuarioRoles)
                        .ThenInclude(ur => ur.Rol)
                .Where(v => v.venEstadoDel)
                .Select(v => new UsuarioReponseDTO
                {
                    usrId = v.Usuario.usrId,
                    usrNombreCompleto = v.Usuario.usrNombreCompleto,
                    usrCorreo = v.Usuario.usrCorreo,
                    Roles = v.Usuario.UsuarioRoles.Select(ur => ur.Rol.rolNombre).ToList(),
                    VendedorId = v.venId
                })
                .ToListAsync();
        }

        // OBTENER TODOS LOS SUPERVISORES ACTIVOS
        public async Task<List<UsuarioReponseDTO>> ObtenerSupervisores()
        {
            return await _context.Supervisor
                .Include(s => s.Usuario)
                    .ThenInclude(u => u.UsuarioRoles)
                        .ThenInclude(ur => ur.Rol)
                .Where(s => s.supEstadoDel)
                .Select(s => new UsuarioReponseDTO
                {
                    usrId = s.Usuario.usrId,
                    usrNombreCompleto = s.Usuario.usrNombreCompleto,
                    usrCorreo = s.Usuario.usrCorreo,
                    Roles = s.Usuario.UsuarioRoles.Select(ur => ur.Rol.rolNombre).ToList(),
                    SupervisorId = s.supId
                })
                .ToListAsync();
        }

        // HASH DE CONTRASEÑA
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }

        // JWT
        public string GenerateJwtToken(Usuario usuario, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.usrUsuarioLog),
                new Claim("id", usuario.usrId.ToString())
            };

            foreach (var rol in roles)
                claims.Add(new Claim(ClaimTypes.Role, rol));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
