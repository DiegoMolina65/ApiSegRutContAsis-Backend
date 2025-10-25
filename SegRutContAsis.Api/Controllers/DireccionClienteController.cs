using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.DireccionCliente;
using SegRutContAsis.Business.Interfaces.DireccionCliente;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DireccionClienteController : ControllerBase
    {
        private readonly IDireccionClienteService _service;

        public DireccionClienteController(IDireccionClienteService service)
        {
            _service = service;
        }

        [HttpPost("crearDireccion")]
        public async Task<IActionResult> CrearDireccion([FromBody] DireccionClienteRequestDTO dto)
        {
            try
            {
                var result = await _service.CrearDireccion(dto);
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
                var result = await _service.ActualizarDireccion(id, dto);
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
            var result = await _service.DesactivarDireccion(id);
            if (!result)
                return NotFound(new { mensaje = "Dirección no encontrada o ya desactivada" });

            return Ok(new { mensaje = "Dirección desactivada correctamente" });
        }

        [HttpGet("obtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var result = await _service.ObtenerPorId(id);
            if (result == null)
                return NotFound(new { mensaje = "Dirección no encontrada" });

            return Ok(result);
        }

        [HttpGet("obtenerPorCliente/{clId}")]
        public async Task<IActionResult> ObtenerPorCliente(int clId)
        {
            var result = await _service.ObtenerPorCliente(clId);
            return Ok(result);
        }

        [HttpGet("obtenerTodas")]
        public async Task<IActionResult> ObtenerTodas()
        {
            var result = await _service.ObtenerTodas();
            return Ok(result);
        }
    }
}
