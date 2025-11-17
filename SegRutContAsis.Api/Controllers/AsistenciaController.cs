using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.Asistencia;
using SegRutContAsis.Business.Interfaces.Asistencia;
using SegRutContAsis.Business.Interfaces.Authentication;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AsistenciaController : ControllerBase
    {
        private readonly IAsistenciaService _asistenciaService;
        private readonly IUsuarioService _usuarioService;

        public AsistenciaController(IAsistenciaService asistenciaService, IUsuarioService usuarioService)
        {
            _asistenciaService = asistenciaService;
            _usuarioService = usuarioService;
        }

        [HttpPost("entradaDia")]
        public async Task<IActionResult> RegistrarEntrada([FromBody] AsistenciaRequestDTO dto)
        {
            try
            {
                var result = await _asistenciaService.RegistrarEntrada(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("salidaDia/{venId}")]
        public async Task<IActionResult> RegistrarSalida(int venId)
        {
            try
            {
                var result = await _asistenciaService.RegistrarSalida(venId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("obtenerAsistencia")]
        public async Task<IActionResult> ObtenerAsistencias()
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (!int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("Token inválido");

                var usuarioActual = await _usuarioService.ObtenerUsuarioId(userId);
                var result = await _asistenciaService.ObtenerAsistencias(usuarioActual);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }

}

