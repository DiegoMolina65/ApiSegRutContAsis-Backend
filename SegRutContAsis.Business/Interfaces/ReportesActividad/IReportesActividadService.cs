using SegRutContAsis.Business.DTO.Request.ReporteActividadRequestDTO;
using SegRutContAsis.Business.DTO.Response.ReporteActividadResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.ReportesActividad
{
    public interface IReportesActividadService
    {
        // Reporte diario de asistencias
        Task<ReporteActividadResponseDTO> GenerarReporteDiarioAsistenciasAsync(ReporteActividadRequestDTO request);

        // Reporte semanal o mensual de asistencias
        Task<ReporteActividadResponseDTO> GenerarReporteAsistenciaPeriodoAsync(ReporteActividadRequestDTO request);

        // Informe de control de campo (asistencia + visitas + ubicación)
        Task<ReporteActividadResponseDTO> GenerarInformeControlCampoAsync(ReporteActividadRequestDTO request);

        // Reporte de visitas agrupadas por zona
        Task<ReporteActividadResponseDTO> GenerarReporteVisitasPorZonaAsync(ReporteActividadRequestDTO request);

        // Reporte de visitas realizadas por vendedor / fecha
        Task<ReporteActividadResponseDTO> GenerarReporteVisitasRealizadasAsync(ReporteActividadRequestDTO request);

        // Exportar cualquier reporte en PDF
        Task<byte[]> ExportarReportePDFAsync(ReporteActividadResponseDTO datosReporte);
    }
}
