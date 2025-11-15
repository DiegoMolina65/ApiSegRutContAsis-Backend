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
            var fechaInicioDelDia = fecha.Date; 
            var fechaFinDelDia = fecha.Date.AddDays(1); 

            var query = _context.Reportes
                .Include(r => r.Vendedor).ThenInclude(v => v.Usuario) 
                .Include(r => r.Supervisor).ThenInclude(s => s.Usuario) 
                .Include(r => r.Asistencia) 
                .Where(r =>
                        r.repTipoActividad == "Asistencia"
                        && r.repEstadoDel == true
                        && r.repFechaCreacion >= fechaInicioDelDia
                        && r.repFechaCreacion < fechaFinDelDia);

            if (request.VendedorId.HasValue && request.VendedorId.Value > 0)
            {
                query = query.Where(r => r.venId == request.VendedorId.Value);
            }
            else if (request.SupervisorId.HasValue && request.SupervisorId.Value > 0)
            {
                query = query.Where(r => r.supId == request.SupervisorId.Value);
            }

            var asistencias = await query.ToListAsync();

            var nombreVendedor = asistencias.FirstOrDefault(a => a.venId == request.VendedorId)?.Vendedor?.Usuario?.usrNombreCompleto;
            var nombreSupervisor = asistencias.FirstOrDefault(a => a.supId == request.SupervisorId)?.Supervisor?.Usuario?.usrNombreCompleto;

            var response = new ReporteActividadResponseDTO
            {
                TipoReporte = "Asistencia Diaria",
                Titulo = request.TituloReporte ?? "Reporte Diario de Asistencias",
                Subtitulo = $"Fecha: {fecha:dd/MM/yyyy}",
                GeneradoPor = request.NombreUsuarioGenera ?? "Sistema",
                FechaGeneracion = DateTime.Now,

                NombreSupervisor = nombreSupervisor,
                NombreVendedor = nombreVendedor,

                Detalles = asistencias.Select(a => new DetalleActividadDTO
                {
                    Fecha = a.repFecha,
                    TipoActividad = a.repTipoActividad,
                    Estado = "Presente",
                    Latitud = a.repLatitud,
                    Longitud = a.repLongitud,
                    HoraEntrada = a.Asistencia?.asiHoraEntrada?.TimeOfDay,
                    HoraSalida = a.Asistencia?.asiHoraSalida?.TimeOfDay,
                    Cliente = a.Cliente?.clNombreCompleto,
                }).ToList(),
                TotalAsistencias = asistencias.Count
            };

            return response;
        }

        public async Task<ReporteActividadResponseDTO> GenerarReporteAsistenciaPeriodoAsync(ReporteActividadRequestDTO request)
        {
            var inicio = request.FechaInicio ?? DateTime.Today.AddDays(-7);
            var fin = request.FechaFin ?? DateTime.Today;

            var query = _context.Reportes
                .Include(r => r.Vendedor).ThenInclude(v => v.Usuario)
                .Include(r => r.Supervisor).ThenInclude(s => s.Usuario)
                .Include(r => r.Asistencia)
                .Where(r => r.repTipoActividad == "Asistencia"
                            && r.repEstadoDel == true
                            && r.repFecha >= inicio && r.repFecha <= fin);

            if (request.VendedorId.HasValue && request.VendedorId.Value > 0)
            {
                query = query.Where(r => r.venId == request.VendedorId.Value);
            }
            else if (request.SupervisorId.HasValue && request.SupervisorId.Value > 0)
            {
                query = query.Where(r => r.supId == request.SupervisorId.Value);
            }

            var asistencias = await query.OrderBy(r => r.repFecha).ToListAsync();

            var nombreVendedor = asistencias.FirstOrDefault(a => a.venId == request.VendedorId)?.Vendedor?.Usuario?.usrNombreCompleto;
            var nombreSupervisor = asistencias.FirstOrDefault(a => a.supId == request.SupervisorId)?.Supervisor?.Usuario?.usrNombreCompleto;

            var response = new ReporteActividadResponseDTO
            {
                TipoReporte = "Asistencia Periodo",
                Titulo = request.TituloReporte ?? "Reporte Semanal/Mensual de Asistencia",
                Subtitulo = $"Desde {inicio:dd/MM/yyyy} hasta {fin:dd/MM/yyyy}",
                GeneradoPor = request.NombreUsuarioGenera ?? "Sistema",
                FechaGeneracion = DateTime.Now,
                NombreSupervisor = nombreSupervisor,
                NombreVendedor = nombreVendedor,
                Detalles = asistencias.Select(a => new DetalleActividadDTO
                {
                    Fecha = a.repFecha,
                    TipoActividad = a.repTipoActividad,
                    Estado = "Presente",
                    HoraEntrada = a.Asistencia?.asiHoraEntrada?.TimeOfDay,
                    HoraSalida = a.Asistencia?.asiHoraSalida?.TimeOfDay,
                    Latitud = a.repLatitud,
                    Longitud = a.repLongitud,
                    Cliente = a.Cliente?.clNombreCompleto,
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

            var query = _context.Reportes
                .Include(r => r.Vendedor).ThenInclude(v => v.Usuario)
                .Include(r => r.Supervisor).ThenInclude(s => s.Usuario)
                .Include(r => r.Cliente)
                .Include(r => r.Asistencia)
                .Where(r => (r.repTipoActividad == "Asistencia" || r.repTipoActividad == "Visita")
                            && r.repEstadoDel == true
                            && r.repFecha >= inicio && r.repFecha <= fin);

            if (request.VendedorId.HasValue && request.VendedorId.Value > 0)
                query = query.Where(r => r.venId == request.VendedorId.Value);
            if (request.SupervisorId.HasValue && request.SupervisorId.Value > 0)
                query = query.Where(r => r.supId == request.SupervisorId.Value);


            var actividades = await query.ToListAsync();

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
                    Estado = a.repTipoActividad == "Asistencia" ? "Presente" : "Visita Realizada",
                    HoraEntrada = a.Asistencia?.asiHoraEntrada?.TimeOfDay,
                    HoraSalida = a.Asistencia?.asiHoraSalida?.TimeOfDay,
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

            var query = _context.Reportes
                .Include(r => r.Zona)
                .Include(r => r.Cliente)
                .Where(r => r.repTipoActividad == "Visita"
                            && r.repEstadoDel == true
                            && r.repFecha >= inicio && r.repFecha <= fin);

            if (request.ZonaId.HasValue)
                query = query.Where(r => r.zonId == request.ZonaId.Value);
            if (request.VendedorId.HasValue)
                query = query.Where(r => r.venId == request.VendedorId.Value);
            if (request.SupervisorId.HasValue)
                query = query.Where(r => r.supId == request.SupervisorId.Value);

            var visitas = await query.ToListAsync();

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
                    Zona = v.Zona?.zonNombre!,
                    Latitud = v.repLatitud,
                    Longitud = v.repLongitud,
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

            var query = _context.Reportes
                .Include(r => r.Vendedor).ThenInclude(v => v.Usuario)
                .Include(r => r.Cliente)
                .Where(r => r.repTipoActividad == "Visita"
                            && r.repEstadoDel == true
                            && r.repFecha >= inicio && r.repFecha <= fin);

            if (request.VendedorId.HasValue)
                query = query.Where(r => r.venId == request.VendedorId.Value);
            if (request.SupervisorId.HasValue)
                query = query.Where(r => r.supId == request.SupervisorId.Value);

            var visitas = await query.ToListAsync();

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
                    Latitud = v.repLatitud,
                    Longitud = v.repLongitud,
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

                        page.Header()
                            .Text(datosReporte.Titulo)
                            .SemiBold().FontSize(16).FontColor(Colors.Blue.Darken2)
                            .AlignCenter();

                        page.Content().Column(column =>
                        {
                            if (!string.IsNullOrWhiteSpace(datosReporte.Subtitulo))
                            {
                                column.Item().Text(datosReporte.Subtitulo)
                                    .FontSize(11)
                                    .FontColor(Colors.Grey.Darken1)
                                    .AlignCenter();
                            }

                            column.Item().PaddingVertical(10);

                            column.Item().Table(table =>
                            {
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

                            column.Item().PaddingTop(15).Column(col =>
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
                });

                using var stream = new MemoryStream();
                document.GeneratePdf(stream);
                return stream.ToArray();
            });
        }
    }
}
