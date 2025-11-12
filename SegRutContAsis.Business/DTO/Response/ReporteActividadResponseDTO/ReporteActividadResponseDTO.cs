using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.ReporteActividadResponseDTO
{
    public class ReporteActividadResponseDTO
    {
        // Datos generales del documento
        public string Titulo { get; set; } = string.Empty;
        public string Subtitulo { get; set; } = string.Empty;
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
        public string GeneradoPor { get; set; } = string.Empty;

        // Tipo de reporte generado
        public string TipoReporte { get; set; } = string.Empty;

        // Información resumida o de cabecera
        public string? NombreSupervisor { get; set; }
        public string? NombreVendedor { get; set; }
        public string? NombreZona { get; set; }
        public string? NombreRuta { get; set; }

        // Detalles de los registros (la tabla del PDF)
        public List<DetalleActividadDTO> Detalles { get; set; } = new();

        // Totales o resumen final
        public int? TotalAsistencias { get; set; }
        public int? TotalVisitas { get; set; }
        public TimeSpan? TotalHorasTrabajadas { get; set; }
    }

    // Subobjeto con los datos tabulares que se imprimen en el PDF
    public class DetalleActividadDTO
    {
        public DateTime Fecha { get; set; }
        public string TipoActividad { get; set; } = string.Empty;  

        // Asistencia
        public TimeSpan? HoraEntrada { get; set; }
        public TimeSpan? HoraSalida { get; set; }
        public string? DuracionJornada { get; set; }

        // Visitas
        public string? Cliente { get; set; }
        public string? Direccion { get; set; }
        public string? ResultadoVisita { get; set; }

        // Ubicación (solo si aplica)
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }

        // Estado general
        public string? Estado { get; set; }   
    }
}
