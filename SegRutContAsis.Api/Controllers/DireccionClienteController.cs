using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.DireccionCliente;
using SegRutContAsis.Business.Interfaces.Authentication;
using SegRutContAsis.Business.Interfaces.DireccionCliente;
using SegRutContAsis.Business.Services;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DireccionClienteController : ControllerBase
    {
        private readonly IDireccionClienteService _direccionClienteService;
        private readonly IUsuarioService _usuarioService;

        public DireccionClienteController(IDireccionClienteService direccionClienteService, IUsuarioService usuarioService)
        {
            _direccionClienteService = direccionClienteService;
            _usuarioService = usuarioService;
        }

        [HttpPost("crearDireccion")]
        public async Task<IActionResult> CrearDireccion([FromBody] DireccionClienteRequestDTO dto)
        {
            try
            {
                var result = await _direccionClienteService.CrearDireccion(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("actualizarDireccion/{id}")]
        public async Task<IActionResult> ActualizarDireccion(int id, [FromBody] DireccionClienteRequestDTO dto)
        {
            try
            {
                var result = await _direccionClienteService.ActualizarDireccion(id, dto);
                if (result == null)
                    return NotFound(new { mensaje = "Dirección no encontrada" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("desactivarDireccion/{id}")]
        public async Task<IActionResult> DesactivarDireccion(int id)
        {
            var result = await _direccionClienteService.DesactivarDireccion(id);
            if (!result)
                return NotFound(new { mensaje = "Dirección no encontrada o ya desactivada" });

            return Ok(new { mensaje = "Dirección desactivada correctamente" });
        }

        [HttpGet("obtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var result = await _direccionClienteService.ObtenerPorId(id);
            if (result == null)
                return NotFound(new { mensaje = "Dirección no encontrada" });

            return Ok(result);
        }

        [HttpGet("obtenerPorCliente/{clId}")]
        public async Task<IActionResult> ObtenerPorCliente(int clId)
        {
            var result = await _direccionClienteService.ObtenerPorCliente(clId);
            return Ok(result);
        }

        [HttpGet("obtenerTodas")]
        public async Task<IActionResult> ObtenerTodas()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Token inválido");

            var usuarioActual = await _usuarioService.ObtenerUsuarioId(userId); 

            var resultado = await _direccionClienteService.ObtenerTodas(usuarioActual);
            return Ok(resultado);
        }

    }
}
