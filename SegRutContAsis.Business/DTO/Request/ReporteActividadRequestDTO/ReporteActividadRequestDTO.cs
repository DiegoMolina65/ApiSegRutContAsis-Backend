using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.ReporteActividadRequestDTO
{
    public class ReporteActividadRequestDTO
    {
        // Tipo de reporte a generar
        // "asistencia-diaria", "asistencia-semanal", "control-campo", "visitas-zona", "visitas-realizadas"
        public string TipoReporte { get; set; } = string.Empty;

        // Filtros principales
        public int? SupervisorId { get; set; }
        public int? VendedorId { get; set; }
        public int? ZonaId { get; set; }
        public int? RutaId { get; set; }

        // Rango de fechas o fecha única
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        // Opciones del reporte
        public bool IncluirUbicacion { get; set; } = false;  
        public string? NombreUsuarioGenera { get; set; }    

        // Personalización del PDF
        public string? TituloReporte { get; set; }         
        public string? Subtitulo { get; set; }           
        public string? NombreArchivo { get; set; }     
    }
}
