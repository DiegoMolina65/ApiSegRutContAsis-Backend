using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.Asistencia;
using SegRutContAsis.Business.Interfaces.Asistencia;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AsistenciaController : ControllerBase
    {
        private readonly IAsistenciaService _asistenciaService;

        public AsistenciaController(IAsistenciaService asistenciaService)
        {
            _asistenciaService = asistenciaService;
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
                var result = await _asistenciaService.ObtenerAsistencias();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }

}

