using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.Ruta;
using SegRutContAsis.Business.Interfaces.Ruta;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RutaController : ControllerBase
    {
        private readonly IRutaService _rutaService;

        public RutaController(IRutaService rutaService)
        {
            _rutaService = rutaService;
        }

        [HttpPost("crearRuta")]
        public async Task<IActionResult> CrearRuta([FromBody] RutaRequestDTO dto)
        {
            var ruta = await _rutaService.CrearRuta(dto);
            return Ok(ruta);
        }

        [HttpGet("obtenerRutas")]
        public async Task<IActionResult> ObtenerRutas()
        {
            var rutas = await _rutaService.ObtenerRutas();
            return Ok(rutas);

        }

        [HttpGet("obtenerRutaId/{id}")]
        public async Task<IActionResult> ObtenerRutaId(int id)
        {

            var ruta = await _rutaService.ObtenerRutaId(id);
            return Ok(ruta);
        }

        [HttpPut("actualizarRuta/{id}")]
        public async Task<IActionResult> ActualizarRuta(int id, [FromBody] RutaRequestDTO dto)
        {
            var ruta = await _rutaService.ActualizarRuta(id, dto);
            return Ok(ruta);
        }

        [HttpPut("desactivarRuta/{id}")]
        public async Task<IActionResult> DesactivarRuta(int id)
        {
            await _rutaService.DeshabilitarRuta(id);
            return Ok(new { mensaje = "Ruta desactivada correctamente" });
        }

        [HttpGet("listarRutasVendedor/{venId}")]
        public async Task<IActionResult> ObtenerRutasPorVendedor(int venId)
        {

            var rutas = await _rutaService.ObtenerRutasPorVendedor(venId);
            return Ok(rutas);
        }

        [HttpGet("listarRutasSupervisor/{supId}")]
        public async Task<IActionResult> ObtenerRutasPorSupervisor(int supId)
        {

            var rutas = await _rutaService.ObtenerRutasPorSupervisor(supId);
            return Ok(rutas);
        }
    }
}
