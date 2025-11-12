using Microsoft.AspNetCore.Mvc;
using SegRutContAsis.Business.DTO.Request.ReporteActividadRequestDTO;
using SegRutContAsis.Business.Interfaces.ReportesActividad;
using System;
using System.Threading.Tasks;

namespace SegRutContAsis.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteActividadController : ControllerBase
    {
        private readonly IReportesActividadService _reporteService;

        public ReporteActividadController(IReportesActividadService reporteService)
        {
            _reporteService = reporteService;
        }

        /// <summary>
        /// Genera el reporte diario de asistencias en PDF
        /// </summary>
        [HttpPost("asistencia/diaria/pdf")]
        public async Task<IActionResult> GenerarReporteDiarioAsistenciasPDF([FromBody] ReporteActividadRequestDTO request)
        {
            var reporte = await _reporteService.GenerarReporteDiarioAsistenciasAsync(request);
            var pdf = await _reporteService.ExportarReportePDFAsync(reporte);

            return File(pdf, "application/pdf", $"Reporte_AsistenciaDiaria_{reporte.FechaGeneracion:yyyyMMddHHmm}.pdf");
        }

        /// <summary>
        /// Genera el reporte semanal o mensual de asistencias en PDF
        /// </summary>
        [HttpPost("asistencia/periodo/pdf")]
        public async Task<IActionResult> GenerarReporteAsistenciaPeriodoPDF([FromBody] ReporteActividadRequestDTO request)
        {
            var reporte = await _reporteService.GenerarReporteAsistenciaPeriodoAsync(request);
            var pdf = await _reporteService.ExportarReportePDFAsync(reporte);

            return File(pdf, "application/pdf", $"Reporte_AsistenciaPeriodo_{reporte.FechaGeneracion:yyyyMMddHHmm}.pdf");
        }

        /// <summary>
        /// Genera el informe de control de campo (asistencia + visitas + ubicación)
        /// </summary>
        [HttpPost("control-campo/pdf")]
        public async Task<IActionResult> GenerarInformeControlCampoPDF([FromBody] ReporteActividadRequestDTO request)
        {
            var reporte = await _reporteService.GenerarInformeControlCampoAsync(request);
            var pdf = await _reporteService.ExportarReportePDFAsync(reporte);

            return File(pdf, "application/pdf", $"Informe_ControlCampo_{reporte.FechaGeneracion:yyyyMMddHHmm}.pdf");
        }

        /// <summary>
        /// Genera el reporte de visitas por zona
        /// </summary>
        [HttpPost("visitas/por-zona/pdf")]
        public async Task<IActionResult> GenerarReporteVisitasPorZonaPDF([FromBody] ReporteActividadRequestDTO request)
        {
            var reporte = await _reporteService.GenerarReporteVisitasPorZonaAsync(request);
            var pdf = await _reporteService.ExportarReportePDFAsync(reporte);

            return File(pdf, "application/pdf", $"Reporte_VisitasZona_{reporte.FechaGeneracion:yyyyMMddHHmm}.pdf");
        }

        /// <summary>
        /// Genera el reporte de visitas realizadas
        /// </summary>
        [HttpPost("visitas/realizadas/pdf")]
        public async Task<IActionResult> GenerarReporteVisitasRealizadasPDF([FromBody] ReporteActividadRequestDTO request)
        {
            var reporte = await _reporteService.GenerarReporteVisitasRealizadasAsync(request);
            var pdf = await _reporteService.ExportarReportePDFAsync(reporte);

            return File(pdf, "application/pdf", $"Reporte_VisitasRealizadas_{reporte.FechaGeneracion:yyyyMMddHHmm}.pdf");
        }
    }
}
