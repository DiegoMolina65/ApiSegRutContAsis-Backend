﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.Usuario;
using SegRutContAsis.Business.Interfaces.Authentication;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService) 
        { 
            _usuarioService = usuarioService;
        }

        [HttpPost("logoutUsuario")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var resultado = await _usuarioService.LogoutUsuario();
                return Ok(new { mensaje = "Sesión cerrada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("loginUsuario")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UsuarioRequestDTO request)
        {
            var response = await _usuarioService.LoginUsuario(request);
            return Ok(response);
        }

        [HttpPost("registrarUsuario")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroRequestDTO dto)
        {
            try
            {
                var usuario = await _usuarioService.RegistrarUsuario(dto);
                return Ok(new { usuario.Id, usuario.NombreCompleto, usuario.UsuarioLog });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("obtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.ObtenerUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("obtenerUsuarioId/{id}")]
        public async Task<IActionResult> ObtenerUsuarioId(int id)
        {
            try
            {
                var usuario = await _usuarioService.ObtenerUsuarioId(id);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpPut("actualizarUsuario/{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] UsuarioRegistroRequestDTO dto)
        {
            try
            {
                var usuarioActualizado = await _usuarioService.ActualizarUsuario(id, dto);
                return Ok(new
                {
                    usuarioActualizado.Id,
                    usuarioActualizado.NombreCompleto,
                    usuarioActualizado.UsuarioLog
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("deshabilitarUsuario/{id}")]
        public async Task<IActionResult> DeshabilitarUsuario(int id)
        {
            try
            {
                var resultado = await _usuarioService.DeshabilitarUsuario(id);
                if (!resultado)
                    return NotFound(new { mensaje = "Usuario no encontrado" });

                return Ok(new { mensaje = "Usuario deshabilitado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
