using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.Visita;
using SegRutContAsis.Business.Interfaces.Authentication;
using SegRutContAsis.Business.Interfaces.Visita;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VisitaController : ControllerBase
    {
        private readonly IVisitaService _visitaService;
        private readonly IUsuarioService _usuarioService;   

        public VisitaController(IVisitaService visitaService, IUsuarioService usuarioService)
        {
            _visitaService = visitaService;
            _usuarioService = usuarioService;
        }

        [HttpPost("crearVisita")]
        public async Task<IActionResult> CrearVisita([FromBody] VisitaRequestDTO dto)
        {
            try
            {
                var visita = await _visitaService.CrearVisita(dto);
                return Ok(visita);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        [HttpGet("obtenerTodasVisitas")]
        public async Task<IActionResult> ObtenerTodasVisitas()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Token inválido");

            var usuarioActual = await _usuarioService.ObtenerUsuarioId(userId);
            var visitas = await _visitaService.ObtenerTodasVisitas(usuarioActual);
            return Ok(visitas);
        }

        [HttpGet("obtenerVisitaId/{id}")]   
        public async Task<IActionResult> ObtenerVisitaId(int id)
        {
            try
            {
                var visita = await _visitaService.ObtenerVisitaId(id);
                return Ok(visita);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpPut("actualizarVisita/{id}")]
        public async Task<IActionResult> ActualizarVisita(int id, [FromBody] VisitaRequestDTO dto)
        {
            try
            {
                var visita = await _visitaService.ActualizarVisita(id, dto);
                return Ok(visita);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("deshabilitarVisita/{id}")]
        public async Task<IActionResult> DeshabilitarVisita(int id)
        {
            var result = await _visitaService.DeshabilitarVisita(id);
            if (!result) return NotFound(new { mensaje = "Visita no encontrada" });
            return Ok(new { mensaje = "Visita deshabilitada correctamente" });
        }

        [HttpGet("obtenerVisitasPorRuta/{rutaId}")]
        public async Task<IActionResult> ObtenerVisitasPorRuta(int rutaId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Token inválido");

            var usuarioActual = await _usuarioService.ObtenerUsuarioId(userId);
            var visitas = await _visitaService.ObtenerVisitasPorRuta(rutaId, usuarioActual);
            return Ok(visitas);
        }

        [HttpGet("obtenerVisitasPorDireccionCliente/{clienteId}")]
        public async Task<IActionResult> ObtenerVisitasPorDireccionCliente(int clienteId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Token inválido");

            var usuarioActual = await _usuarioService.ObtenerUsuarioId(userId);
            var visitas = await _visitaService.ObtenerVisitasPorDireccionCliente(clienteId, usuarioActual);
            return Ok(visitas);
        }

        [HttpGet("obtenerVisitasPorVendedor/{venId}")]
        public async Task<IActionResult> ObtenerVisitasPorVendedor(int venId)
        {
            var visitas = await _visitaService.ObtenerVisitasPorVendedor(venId);
            return Ok(visitas);
        }
    }
}
