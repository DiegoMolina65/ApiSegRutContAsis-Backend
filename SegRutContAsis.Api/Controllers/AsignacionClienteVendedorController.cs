using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.AsignacionClienteVendedor;
using SegRutContAsis.Business.Interfaces.AsignacionClienteVendedor;
using SegRutContAsis.Business.Interfaces.Authentication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AsignacionClienteVendedorController : ControllerBase
    {
        private readonly IAsignacionClienteVendedorService _asignacionClienteVendedorService;
        private readonly IUsuarioService _usuarioService;

        public AsignacionClienteVendedorController(IAsignacionClienteVendedorService asignacionClienteVendedorService, IUsuarioService usuarioService)
        {
            _asignacionClienteVendedorService = asignacionClienteVendedorService;
            _usuarioService = usuarioService;
        }

        [HttpPost("crearAsignacion")]
        public async Task<IActionResult> CrearAsignacion([FromBody] AsignacionClienteVendedorRequestDTO dto)
        {
            try
            {
                var result = await _asignacionClienteVendedorService.CrearAsignacion(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("actualizarAsignacion/{id}")]
        public async Task<IActionResult> ActualizarAsignacion(int id, [FromBody] AsignacionClienteVendedorRequestDTO dto)
        {
            try
            {
                var result = await _asignacionClienteVendedorService.ActualizarAsignacion(id, dto);
                if (result == null)
                    return NotFound(new { mensaje = "Asignación no encontrada" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("desactivarAsignacion/{id}")]
        public async Task<IActionResult> DesactivarAsignacion(int id)
        {
            try
            {
                var result = await _asignacionClienteVendedorService.DesactivarAsignacion(id);
                if (!result)
                    return NotFound(new { mensaje = "Asignación no encontrada o ya desactivada" });

                return Ok(new { mensaje = "Asignación desactivada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("obtenerAsignacionPorId/{id}")]
        public async Task<IActionResult> ObtenerAsignacionPorId(int id)
        {
            var result = await _asignacionClienteVendedorService.ObtenerAsignacionPorId(id);
            if (result == null)
                return NotFound(new { mensaje = "Asignación no encontrada" });

            return Ok(result);
        }

        [HttpGet("obtenerAsignacionesPorVendedor/{venId}")]
        public async Task<IActionResult> ObtenerAsignacionesPorVendedor(int venId)
        {
            var result = await _asignacionClienteVendedorService.ObtenerAsignacionesPorVendedor(venId);
            return Ok(result);
        }

        [HttpGet("obtenerTodasAsignaciones")]
        public async Task<IActionResult> ObtenerTodas()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Token inválido");

            var usuarioActual = await _usuarioService.ObtenerUsuarioId(userId);
            var result = await _asignacionClienteVendedorService.ObtenerTodasAsignaciones(usuarioActual);
            return Ok(result);
        }
    }
}
