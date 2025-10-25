using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.SeguimientoVendedor;
using SegRutContAsis.Business.Interfaces.SeguimientoVendedor;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SeguimientoVendedorController : ControllerBase
    {
        private readonly ISeguimientoVendedorService _seguimientoVendedorService;
        public SeguimientoVendedorController(ISeguimientoVendedorService seguimientoVendedorService)
        {
            _seguimientoVendedorService = seguimientoVendedorService;

        }

        [HttpPost("crearSeguimientoVendedor")]
        public async Task<IActionResult> CrearSeguimientoVendedor([FromBody] SeguimientoVendedorRequestDTO dto)
        {
            try
            {
                var result = await _seguimientoVendedorService.CrearSeguimientoVendedor(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("obtenerTodosSeguimientosVendedores")]
        public async Task<IActionResult> ObtenerTodosSeguimientosVendedores()
        {
            try
            {
                var result = await _seguimientoVendedorService.ObtenerTodosSeguimientosVendedores();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("obtenerSeguimientosDeUnVendedor/{venId}")]
        public async Task<IActionResult> ObtenerSeguimientosDeUnVendedor(int venId)
        {
            try
            {
                var result = await _seguimientoVendedorService.ObtenerSeguimientosDeUnVendedor(venId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

    }
}
