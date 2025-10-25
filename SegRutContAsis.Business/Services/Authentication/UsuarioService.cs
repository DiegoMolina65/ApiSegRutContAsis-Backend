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

        // =========================
        // MÉTODO LOGOUT USUARIO
        // =========================
        public async Task<bool> LogoutUsuario()
        { 
            return await Task.FromResult(true);
        }

        // =========================
        // LOGIN
        // =========================
        public async Task<UsuarioReponseDTO> LoginUsuario(UsuarioRequestDTO request)
        {
            var usuario = await _context.Usuario
                 .Include(u => u.UsuarioRoles)
                     .ThenInclude(ur => ur.Rol)
                 .FirstOrDefaultAsync(u => u.UsuarioLog == request.UsuarioLog);

            if (!usuario.EstadoDel)
                throw new Exception("El usuario está desactivado. Contacte con el administrador.");

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            if (!VerifyPassword(request.ContrasenaLog, usuario.ContrasenaLog))
                throw new Exception("Contraseña incorrecta");

            var roles = usuario.UsuarioRoles.Select(r => r.Rol.Nombre).ToList();
            var token = GenerateJwtToken(usuario, roles);

            return new UsuarioReponseDTO
            {
                Token = token,
                Usuario = usuario.NombreCompleto,
                Roles = roles
            };
        }

        // =========================
        // REGISTRAR USUARIO (MULTI-ROL)
        // =========================
        public async Task<Usuario> RegistrarUsuario(UsuarioRegistroRequestDTO dto)
        {
            var existeUsuario = await _context.Usuario
                .AnyAsync(u => u.UsuarioLog == dto.UsuarioLog || u.Correo == dto.Correo);
            if (existeUsuario)
                throw new Exception("Usuario o correo ya existe.");

            var usuario = new Usuario
            {
                NombreCompleto = dto.NombreCompleto,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                NitEmpleado = dto.NitEmpleado,
                CarnetIdentidad = dto.CarnetIdentidad,
                UsuarioLog = dto.UsuarioLog,
                ContrasenaLog = HashPassword(dto.ContrasenaLog),
                EstadoDel = true,
                FechaCreacion = DateTime.Now
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            foreach (var rolNombre in dto.Roles)
            {
                var rol = await _context.Rol.FirstOrDefaultAsync(r => r.Nombre.ToUpper() == rolNombre.ToUpper());
                if (rol == null) continue;

                _context.UsuarioRol.Add(new UsuarioRol
                {
                    usrID = usuario.Id,
                    rolId = rol.Id
                });

                switch (rolNombre.ToUpper())
                {
                    case "ADMINISTRADOR":
                        _context.Administrador.Add(new Administrador
                        {
                            UsuarioId = usuario.Id,
                            FechaCreacion = DateTime.Now,
                            EstadoDel = true
                        });
                        break;

                    case "SUPERVISOR":
                        _context.Supervisor.Add(new Supervisor
                        {
                            UsuarioId = usuario.Id,
                            FechaCreacion = DateTime.Now,
                            EstadoDel = true
                        });
                        break;

                    case "VENDEDOR":
                        _context.Vendedor.Add(new Vendedor
                        {
                            UsuarioId = usuario.Id,
                            FechaCreacion = DateTime.Now,
                            EstadoDel = true
                        });
                        break;
                }
            }

            await _context.SaveChangesAsync();
            return usuario;
        }

        // =========================
        // OBTENER TODOS LOS USUARIOS
        // =========================
        public async Task<List<UsuarioReponseDTO>> ObtenerUsuarios()
        {
            return await _context.Usuario
                .Where(u => u.EstadoDel == true)
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .Select(u => new UsuarioReponseDTO
                {
                    IdUsuario = u.Id,
                    Usuario = u.NombreCompleto,
                    Roles = u.UsuarioRoles.Select(r => r.Rol.Nombre).ToList(),
                    Token = null 
                })
                .ToListAsync();
        }

        // =========================
        // OBTENER USUARIO POR ID
        // =========================
        public async Task<UsuarioReponseDTO> ObtenerUsuarioId(int id)
        {
            var usuario = await _context.Usuario
                .Where(u => u.EstadoDel == true)
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            return new UsuarioReponseDTO
            {
                IdUsuario = usuario.Id,
                Usuario = usuario.NombreCompleto,
                Roles = usuario.UsuarioRoles.Select(r => r.Rol.Nombre).ToList()
            };
        }

        // =========================
        // ACTUALIZAR USUARIO
        // =========================
        public async Task<Usuario> ActualizarUsuario(int id, UsuarioRegistroRequestDTO dto)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            usuario.NombreCompleto = dto.NombreCompleto;
            usuario.Correo = dto.Correo;
            usuario.Telefono = dto.Telefono;
            usuario.NitEmpleado = dto.NitEmpleado;
            usuario.CarnetIdentidad = dto.CarnetIdentidad;
            usuario.UsuarioLog = dto.UsuarioLog;

            if (!string.IsNullOrEmpty(dto.ContrasenaLog))
                usuario.ContrasenaLog = HashPassword(dto.ContrasenaLog);

            await _context.SaveChangesAsync();
            return usuario;
        }

        // =========================
        // DESHABILITAR USUARIO
        // =========================
        public async Task<bool> DeshabilitarUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null) return false;

            usuario.EstadoDel = false;
            await _context.SaveChangesAsync();
            return true;
        }


        // =========================
        // HASH DE CONTRASEÑA
        // =========================
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

        // =========================
        // JWT
        // =========================
        public string GenerateJwtToken(Usuario usuario, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.UsuarioLog),
                new Claim("id", usuario.Id.ToString())
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
