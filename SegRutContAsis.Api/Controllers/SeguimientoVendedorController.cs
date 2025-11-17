using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.SeguimientoVendedor;
using SegRutContAsis.Business.Interfaces.Authentication;
using SegRutContAsis.Business.Interfaces.MarcarLlegadaVisita;
using SegRutContAsis.Business.Interfaces.SeguimientoVendedor;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SeguimientoVendedorController : ControllerBase
    {
        private readonly ISeguimientoVendedorService _seguimientoVendedorService;
        private readonly IUsuarioService _usuarioService;
        public SeguimientoVendedorController(ISeguimientoVendedorService seguimientoVendedorService, IUsuarioService usuarioService)
        {
            _seguimientoVendedorService = seguimientoVendedorService;
            _usuarioService = usuarioService;

        }

        [HttpPost("crearSeguimientoVendedor")]
        public async Task<IActionResult> CrearSeguimientoVendedor([FromBody] SeguimientoVendedorRequestDTO dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (!int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("Token inválido");

                var usuarioActual = await _usuarioService.ObtenerUsuarioId(userId);
                var result = await _seguimientoVendedorService.CrearSeguimientoVendedor(dto, usuarioActual);

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
                var userIdClaim = User.FindFirst("id")?.Value;
                if (!int.TryParse(userIdClaim, out var userId))
                    return Unauthorized("Token inválido");

                var usuarioActual = await _usuarioService.ObtenerUsuarioId(userId);
                var result = await _seguimientoVendedorService.ObtenerTodosSeguimientosVendedores(usuarioActual);
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
