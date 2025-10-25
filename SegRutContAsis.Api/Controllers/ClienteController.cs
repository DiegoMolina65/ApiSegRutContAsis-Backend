using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.Cliente;
using SegRutContAsis.Business.Interfaces.Cliente;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet("obtenerClientes")]
        public async Task<IActionResult> ObtenerClientes()
        {
            var clientes = await _clienteService.ObtenerClientes();
            return Ok(clientes);
        }

        [HttpGet("obtenerClienteId/{id}")]
        public async Task<IActionResult> ObtenerClientePorId(int id)
        {
            var cliente = await _clienteService.ObtenerClientePorId(id);
            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado." });

            return Ok(cliente);
        }

        [HttpPost("crearCliente")]
        public async Task<IActionResult> CrearCliente([FromBody] ClienteRequestDTO request)
        {
            var cliente = await _clienteService.CrearCliente(request);
            return Ok(cliente);
        }

        [HttpPut("actualizarCliente/{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] ClienteRequestDTO request)
        {
            var cliente = await _clienteService.ActualizarCliente(id, request);
            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado." });

            return Ok(cliente);
        }

        [HttpPut("deshabilitarCliente/{id}")]
        public async Task<IActionResult> DeshabilitarCliente(int id)
        {
            var resultado = await _clienteService.DeshabilitarCliente(id);
            if (!resultado)
                return NotFound(new { mensaje = "Cliente no encontrado o ya deshabilitado." });

            return Ok(new { mensaje = "Cliente deshabilitado correctamente." });
        }
    }
}
