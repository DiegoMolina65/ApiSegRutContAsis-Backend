using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.Evidencia;
using SegRutContAsis.Business.Interfaces.Evidencia;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EvidenciaController : ControllerBase
    {
        private readonly IEvidenciaService _service;

        public EvidenciaController(IEvidenciaService service)
        {
            _service = service;
        }

        [HttpPost("crearEvidencia")]
        public async Task<IActionResult> CrearEvidencia([FromBody] EvidenciaRequestDTO dto)
        {
            var e = await _service.CrearEvidencia(dto);
            return Ok(e);
        }

        [HttpGet("obtenerEvidencia")]
        public async Task<IActionResult> ObtenerEvidencia()
        {
            var list = await _service.ObtenerEvidencia();
            return Ok(list);
        }

        [HttpGet("obtenerEvidenciaId/{id}")]
        public async Task<IActionResult> ObtenerEvidenciaId(int id)
        {
            var e = await _service.ObtenerEvidenciaId(id);
            return Ok(e);
        }

        [HttpPut("actualizarEvidencia/{id}")]
        public async Task<IActionResult> ActualizarEvidencia(int id, [FromBody] EvidenciaRequestDTO dto)
        {
            var e = await _service.ActualizarEvidencia(id, dto);
            return Ok(e);
        }

        [HttpGet("obtenerEvidenciaPorVisita/{visitaId}")]
        public async Task<IActionResult> ObtenerEvidenciaPorVisita(int visitaId)
        {
            var list = await _service.ObtenerEvidenciaPorVisita(visitaId);
            return Ok(list);
        }

        [HttpGet("obtenerEvidenciaPorVendedor/{venId}")]
        public async Task<IActionResult> ObtenerEvidenciaPorVendedor(int venId)
        {
            var list = await _service.ObtenerEvidenciaPorVendedor(venId);
            return Ok(list);
        }

        [HttpGet("obtenerEvidenciaPorTipo/{tipo}")]
        public async Task<IActionResult> ObtenerEvidenciaPorTipo(string tipo)
        {
            var list = await _service.ObtenerEvidenciaPorTipo(tipo);
            return Ok(list);
        }
    }
}
