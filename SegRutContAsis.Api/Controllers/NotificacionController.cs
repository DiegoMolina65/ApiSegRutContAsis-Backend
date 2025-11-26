using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.Interfaces.Authentication;
using SegRutContAsis.Business.Interfaces.Notificacion;
using SegRutContAsis.Business.Interfaces.Visita;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;
        private readonly IUsuarioService _usuarioService;

        public NotificacionController(INotificacionService notificacionService, IUsuarioService usuarioService)
        {
            _notificacionService = notificacionService;
            _usuarioService = usuarioService;
        }

        [HttpGet("obtenerNotificaciones")]
        public async Task<IActionResult> obtenerNotificaciones()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Token inválido");

            var usuarioActual = await _usuarioService.ObtenerUsuarioId(userId);
            var visitas = await _notificacionService.obtenerNotificaciones(usuarioActual);
            return Ok(visitas);
        }
    }
}
