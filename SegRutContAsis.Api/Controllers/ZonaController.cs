using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.Zona;
using SegRutContAsis.Business.Interfaces.Zona;


namespace SegRutContAsis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ZonaController : ControllerBase
    {
        private readonly IZonaService _zonaService;

        public ZonaController(IZonaService zonaService)
        {
            _zonaService = zonaService;
        }

        [HttpGet("obtenerZona")]
        public async Task<IActionResult> ObtenerZonas()
        {
            var zonas = await _zonaService.ObtenerZonas();
            return Ok(zonas);
        }

        [HttpPost("crearZona")]
        public async Task<IActionResult> CrearZona([FromBody] ZonaRequestDTO request)
        {
            var zona = await _zonaService.CrearZona(request);
            return Ok(zona);
        }

        [HttpPut("actualizarZona/{id}")]
        public async Task<IActionResult> ActualizarZona(int id, [FromBody] ZonaRequestDTO request)
        {
            try
            {
                var zonaActualizada = await _zonaService.ActualizarZona(id, request);
                return Ok(new { mensaje = "Zona actualizada correctamente.", zona = zonaActualizada });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("deshabilitarZona/{id}")]
        public async Task<IActionResult> DeshabilitarZona(int id)
        {
            try
            {
                var result = await _zonaService.DeshabilitarZona(id);
                return Ok(new { mensaje = "Zona deshabilitada correctamente.", success = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
