using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.MarcarLlegadaVisita;
using SegRutContAsis.Business.DTO.Response.MarcarLlegadaVisita;
using SegRutContAsis.Business.Interfaces.MarcarLlegadaVisita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SegRutContAsis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MarcarLlegadaVisitaController : ControllerBase
    {
        private readonly IMarcarLlegadaVisitaService _service;

        public MarcarLlegadaVisitaController(IMarcarLlegadaVisitaService service)
        {
            _service = service;
        }

        [HttpPost("crearMarcarLlegadaVisita")]
        public async Task<IActionResult> crearMarcarLlegadaVisita([FromBody] MarcarLlegadaVisitaRequestDTO dto)
        {
            try
            {
                var resultado = await _service.crearMarcarLlegadaVisita(dto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("obtenerTodasMarcacionesLlegadaVisita")]
        public async Task<IActionResult> obtenerTodasMarcacionesLlegadaVisita()
        {
            try
            {
                var resultado = await _service.obtenerTodasMarcacionesLlegadaVisita();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("obtenerPorIdMarcacionesLlegadaVisita/{id}")]
        public async Task<IActionResult> obtenerPorIdMarcacionesLlegadaVisita(int id)
        {
            try
            {
                var resultado = await _service.obtenerPorIdMarcacionesLlegadaVisita(id);
                if (resultado == null)
                    return NotFound(new { mensaje = "Marcación no encontrada" });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("actualizarMarcarLlegadaVisita/{id}")]
        public async Task<IActionResult> actualizarMarcarLlegadaVisita(int id, [FromBody] MarcarLlegadaVisitaRequestDTO dto)
        {
            try
            {
                var resultado = await _service.actualizarMarcarLlegadaVisita(dto, id);
                if (resultado == null)
                    return NotFound(new { mensaje = "Marcación no encontrada" });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("desactivarMarcacionLlegadaVisita/{id}")]
        public async Task<IActionResult> desactivarMarcacionLlegadaVisita(int id)
        {
            try
            {
                var resultado = await _service.desactivarMarcacionLlegadaVisita(id);
                if (!resultado)
                    return NotFound(new { mensaje = "Marcación no encontrada" });

                return Ok(new { mensaje = "Marcación desactivada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("obtenerMarcacionesPorVisita/{visId}")]
        public async Task<IActionResult> obtenerMarcacionesPorVisita(int visId)
        {
            try
            {
                var resultado = await _service.obtenerMarcacionesPorVisita(visId);
                if (resultado == null || resultado.Count == 0)
                    return NotFound(new { mensaje = "No se encontraron marcaciones para esta visita" });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

    }
}
