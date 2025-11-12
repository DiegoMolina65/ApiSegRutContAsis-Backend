using System;

namespace SegRutContAsis.Domain.Entities
{
    public class Reportes
    {
        public int repId { get; set; }

        public int venId { get; set; }
        public int? supId { get; set; }
        public int? clId { get; set; }
        public int? visId { get; set; }
        public int? asiId { get; set; }
        public int? eviId { get; set; }
        public int? rutId { get; set; }
        public int? zonId { get; set; }

        // Datos principales
        public DateTime repFechaCreacion { get; set; } = DateTime.Now;
        public DateTime repFecha { get; set; }
        public string repTipoActividad { get; set; } = string.Empty; // Asistencia, Visita, etc.
        public string? repDescripcion { get; set; }
        public decimal? repLatitud { get; set; }
        public decimal? repLongitud { get; set; }
        public bool repEstadoDel { get; set; } = true;

        // Relaciones 
        public Vendedor? Vendedor { get; set; }
        public Supervisor? Supervisor { get; set; }
        public Cliente? Cliente { get; set; }
        public Visita? Visita { get; set; }
        public Asistencia? Asistencia { get; set; }
        public Evidencia? Evidencia { get; set; }
        public Ruta? Ruta { get; set; }
        public Zona? Zona { get; set; }
    }
}
