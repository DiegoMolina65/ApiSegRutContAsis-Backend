using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.ReporteActividadRequestDTO;
using SegRutContAsis.Business.DTO.Response.ReporteActividadResponseDTO;
using SegRutContAsis.Business.Interfaces.ReportesActividad;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using System.IO;

namespace SegRutContAsis.Business.Services.Reporte
{
    public class ReporteActividadService : IReportesActividadService
    {
        private readonly SegRutContAsisContext _context;

        public ReporteActividadService(SegRutContAsisContext context)
        {
            _context = context;
        }

        // Reporte diario de asistencias
        public async Task<ReporteActividadResponseDTO> GenerarReporteDiarioAsistenciasAsync(ReporteActividadRequestDTO request)
        {
            var fecha = request.FechaInicio ?? DateTime.Today;

            var asistencias = await _context.Reportes
                .Include(r => r.Vendedor)
                .Where(r => r.repTipoActividad == "Asistencia"
                            && r.repFecha.Date == fecha.Date
                            && r.repEstadoDel == true)
                .ToListAsync();

            var response = new ReporteActividadResponseDTO
            {
                TipoReporte = "Asistencia Diaria",
                Titulo = request.TituloReporte ?? "Reporte Diario de Asistencias",
                Subtitulo = $"Fecha: {fecha:dd/MM/yyyy}",
                GeneradoPor = request.NombreUsuarioGenera ?? "Sistema",
                FechaGeneracion = DateTime.Now,
                Detalles = asistencias.Select(a => new DetalleActividadDTO
                {
                    Fecha = a.repFecha,
                    TipoActividad = a.repTipoActividad,
                    Estado = "Presente",
                    Latitud = a.repLatitud,
                    Longitud = a.repLongitud,
                }).ToList(),
                TotalAsistencias = asistencias.Count
            };

            return response;
        }

        // Reporte semanal/mensual de asistencia
        public async Task<ReporteActividadResponseDTO> GenerarReporteAsistenciaPeriodoAsync(ReporteActividadRequestDTO request)
        {
            var inicio = request.FechaInicio ?? DateTime.Today.AddDays(-7);
            var fin = request.FechaFin ?? DateTime.Today;

            var asistencias = await _context.Reportes
                .Include(r => r.Vendedor)
                .Where(r => r.repTipoActividad == "Asistencia"
                            && r.repFecha >= inicio
                            && r.repFecha <= fin
                            && r.repEstadoDel == true)
                .OrderBy(r => r.repFecha)
                .ToListAsync();

            var response = new ReporteActividadResponseDTO
            {
                TipoReporte = "Asistencia Periodo",
                Titulo = request.TituloReporte ?? "Reporte Semanal/Mensual de Asistencia",
                Subtitulo = $"Desde {inicio:dd/MM/yyyy} hasta {fin:dd/MM/yyyy}",
                GeneradoPor = request.NombreUsuarioGenera ?? "Sistema",
                FechaGeneracion = DateTime.Now,
                Detalles = asistencias.Select(a => new DetalleActividadDTO
                {
                    Fecha = a.repFecha,
                    TipoActividad = a.repTipoActividad,
                    Estado = "Presente",
                }).ToList(),
                TotalAsistencias = asistencias.Count
            };

            return response;
        }

        // Informe de control de campo (asistencia + visitas + ubicación)
        public async Task<ReporteActividadResponseDTO> GenerarInformeControlCampoAsync(ReporteActividadRequestDTO request)
        {
            var inicio = request.FechaInicio ?? DateTime.Today;
            var fin = request.FechaFin ?? DateTime.Today;

            var actividades = await _context.Reportes
                .Where(r => (r.repTipoActividad == "Asistencia" || r.repTipoActividad == "Visita")
                            && r.repFecha >= inicio && r.repFecha <= fin
                            && r.repEstadoDel == true)
                .Include(r => r.Vendedor)
                .Include(r => r.Cliente)
                .ToListAsync();

            var response = new ReporteActividadResponseDTO
            {
                TipoReporte = "Control de Campo",
                Titulo = request.TituloReporte ?? "Informe de Control de Campo",
                Subtitulo = $"Del {inicio:dd/MM/yyyy} al {fin:dd/MM/yyyy}",
                GeneradoPor = request.NombreUsuarioGenera ?? "Sistema",
                FechaGeneracion = DateTime.Now,
                Detalles = actividades.Select(a => new DetalleActividadDTO
                {
                    Fecha = a.repFecha,
                    TipoActividad = a.repTipoActividad,
                    Cliente = a.Cliente?.clNombreCompleto,
                    Latitud = a.repLatitud,
                    Longitud = a.repLongitud,
                    Estado = a.repTipoActividad == "Asistencia" ? "Presente" : "Visita Realizada"
                }).ToList(),
                TotalAsistencias = actividades.Count(x => x.repTipoActividad == "Asistencia"),
                TotalVisitas = actividades.Count(x => x.repTipoActividad == "Visita")
            };

            return response;
        }

        // Reporte de visitas por zona
        public async Task<ReporteActividadResponseDTO> GenerarReporteVisitasPorZonaAsync(ReporteActividadRequestDTO request)
        {
            var inicio = request.FechaInicio ?? DateTime.Today.AddDays(-7);
            var fin = request.FechaFin ?? DateTime.Today;

            var visitas = await _context.Reportes
                .Include(r => r.Zona)
                .Include(r => r.Cliente)
                .Where(r => r.repTipoActividad == "Visita"
                            && r.repFecha >= inicio && r.repFecha <= fin
                            && (request.ZonaId == null || r.zonId == request.ZonaId)
                            && r.repEstadoDel == true)
                .ToListAsync();

            var response = new ReporteActividadResponseDTO
            {
                TipoReporte = "Visitas por Zona",
                Titulo = request.TituloReporte ?? "Reporte de Visitas por Zona",
                Subtitulo = $"Del {inicio:dd/MM/yyyy} al {fin:dd/MM/yyyy}",
                GeneradoPor = request.NombreUsuarioGenera ?? "Sistema",
                FechaGeneracion = DateTime.Now,
                Detalles = visitas.Select(v => new DetalleActividadDTO
                {
                    Fecha = v.repFecha,
                    TipoActividad = v.repTipoActividad,
                    Cliente = v.Cliente?.clNombreCompleto,
                    Estado = "Realizada"
                }).ToList(),
                TotalVisitas = visitas.Count
            };

            return response;
        }

        // Reporte de visitas realizadas
        public async Task<ReporteActividadResponseDTO> GenerarReporteVisitasRealizadasAsync(ReporteActividadRequestDTO request)
        {
            var inicio = request.FechaInicio ?? DateTime.Today;
            var fin = request.FechaFin ?? DateTime.Today;

            var visitas = await _context.Reportes
                .Include(r => r.Vendedor)
                .Include(r => r.Cliente)
                .Where(r => r.repTipoActividad == "Visita"
                            && r.repFecha >= inicio && r.repFecha <= fin
                            && (request.VendedorId == null || r.venId == request.VendedorId)
                            && r.repEstadoDel == true)
                .ToListAsync();

            var response = new ReporteActividadResponseDTO
            {
                TipoReporte = "Visitas Realizadas",
                Titulo = request.TituloReporte ?? "Reporte de Visitas Realizadas",
                Subtitulo = $"Del {inicio:dd/MM/yyyy} al {fin:dd/MM/yyyy}",
                GeneradoPor = request.NombreUsuarioGenera ?? "Sistema",
                FechaGeneracion = DateTime.Now,
                Detalles = visitas.Select(v => new DetalleActividadDTO
                {
                    Fecha = v.repFecha,
                    TipoActividad = v.repTipoActividad,
                    Cliente = v.Cliente?.clNombreCompleto,
                    Estado = "Realizada"
                }).ToList(),
                TotalVisitas = visitas.Count
            };

            return response;
        }

        // Exportar cualquier reporte en PDF
        public async Task<byte[]> ExportarReportePDFAsync(ReporteActividadResponseDTO datosReporte)
        {
            return await Task.Run(() =>
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.Size(PageSizes.A4);
                        page.DefaultTextStyle(x => x.FontSize(10));
                        page.PageColor(Colors.White);

                        // Encabezado
                        page.Header()
                            .Text(datosReporte.Titulo)
                            .SemiBold().FontSize(16).FontColor(Colors.Blue.Darken2)
                            .AlignCenter();

                        // Subtítulo
                        if (!string.IsNullOrWhiteSpace(datosReporte.Subtitulo))
                        {
                            page.Content()
                                .Text(datosReporte.Subtitulo)
                                .FontSize(11)
                                .FontColor(Colors.Grey.Darken1)
                                .AlignCenter();
                        }

                        // Espaciado
                        page.Content().PaddingVertical(10);

                        // Tabla de detalles
                        page.Content().Table(table =>
                        {
                            // Cabecera
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Fecha");
                                header.Cell().Element(CellStyle).Text("Actividad");
                                header.Cell().Element(CellStyle).Text("Cliente / Dirección");
                                header.Cell().Element(CellStyle).Text("Latitud");
                                header.Cell().Element(CellStyle).Text("Longitud");

                                static IContainer CellStyle(IContainer container) =>
                                    container.PaddingVertical(5).Background(Colors.Grey.Lighten3)
                                             .BorderBottom(1).BorderColor(Colors.Grey.Medium)
                                             .AlignCenter();
                            });

                            // Filas
                            foreach (var d in datosReporte.Detalles)
                            {
                                table.Cell().Element(CellStyle).Text(d.Fecha.ToString("dd/MM/yyyy"));
                                table.Cell().Element(CellStyle).Text(d.TipoActividad);
                                table.Cell().Element(CellStyle).Text($"{d.Cliente ?? "-"} / {d.Direccion ?? "-"}");
                                table.Cell().Element(CellStyle).Text(d.Latitud?.ToString("F5") ?? "-");
                                table.Cell().Element(CellStyle).Text(d.Longitud?.ToString("F5") ?? "-");

                                static IContainer CellStyle(IContainer container) =>
                                    container.PaddingVertical(3).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten1);
                            }
                        });

                        // Espaciado y totales
                        page.Content().PaddingTop(15);

                        page.Content().Column(col =>
                        {
                            if (datosReporte.TotalAsistencias != null)
                                col.Item().Text($"Total de Asistencias: {datosReporte.TotalAsistencias}").Bold();
                            if (datosReporte.TotalVisitas != null)
                                col.Item().Text($"Total de Visitas: {datosReporte.TotalVisitas}").Bold();
                            if (datosReporte.TotalHorasTrabajadas != null)
                                col.Item().Text($"Total Horas: {datosReporte.TotalHorasTrabajadas}").Bold();

                            col.Item().PaddingTop(5).Text($"Generado por: {datosReporte.GeneradoPor} - {datosReporte.FechaGeneracion:dd/MM/yyyy HH:mm}");
                        });
                    });
                });

                // Generar PDF en memoria
                using var stream = new MemoryStream();
                document.GeneratePdf(stream);
                return stream.ToArray();
            });
        }
    }
}
