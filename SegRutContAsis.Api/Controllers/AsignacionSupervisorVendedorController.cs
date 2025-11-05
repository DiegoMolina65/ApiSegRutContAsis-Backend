using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.AsignacionSupervisorVendedor;
using SegRutContAsis.Business.Interfaces.AsignacionSupervisorVendedor;
using System;
using System.Threading.Tasks;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AsignacionSupervisorVendedorController : ControllerBase
    {
        private readonly IAsignacionSupervisorVendedorService _service;

        public AsignacionSupervisorVendedorController(IAsignacionSupervisorVendedorService service)
        {
            _service = service;
        }

        // Crear nueva asignación
        [HttpPost("crearAsignacion")]
        public async Task<IActionResult> CrearAsignacion([FromBody] AsignacionSupervisorVendedorRequestDTO dto)
        {
            try
            {
                var result = await _service.CrearAsignacionSupervisorVendedor(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // Actualizar asignación existente
        [HttpPut("actualizarAsignacion/{id}")]
        public async Task<IActionResult> ActualizarAsignacion(int id, [FromBody] AsignacionSupervisorVendedorRequestDTO dto)
        {
            try
            {
                var result = await _service.CrearAsignacionSupervisorVendedor(dto, id);
                if (result == null)
                    return NotFound(new { mensaje = "Asignación no encontrada" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        //  Desactivar asignación
        [HttpPut("desactivarAsignacion/{id}")]
        public async Task<IActionResult> DesactivarAsignacion(int id)
        {
            try
            {
                var result = await _service.DesactivarAsignacionSupervisorVendedor(id);
                if (!result)
                    return NotFound(new { mensaje = "Asignación no encontrada o ya desactivada" });

                return Ok(new { mensaje = "Asignación desactivada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // Obtener una asignación por ID
        [HttpGet("obtenerAsignacionPorId/{id}")]
        public async Task<IActionResult> ObtenerAsignacionPorId(int id)
        {
            try
            {
                var result = await _service.ObtenerAsignacionSupervisorVendedorId(id);
                if (result == null)
                    return NotFound(new { mensaje = "Asignación no encontrada" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // Obtener todas las asignaciones
        [HttpGet("obtenerTodasAsignaciones")]
        public async Task<IActionResult> ObtenerTodasAsignaciones()
        {
            try
            {
                var result = await _service.ObtenerAsignacionSupervisorVendedorTodas();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // Vendedores por supervisor
        [HttpGet("obtenerVendedoresPorSupervisor/{supervisorId}")]
        public async Task<IActionResult> ObtenerVendedoresPorSupervisor(int supervisorId)
        {
            try
            {
                var result = await _service.ObtenerVendedoresPorSupervisor(supervisorId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // Supervisores por vendedor
        [HttpGet("obtenerSupervisoresPorVendedor/{vendedorId}")]
        public async Task<IActionResult> ObtenerSupervisoresPorVendedor(int vendedorId)
        {
            try
            {
                var result = await _service.ObtenerSupervisoresPorVendedor(vendedorId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
