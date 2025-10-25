using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Visita
    {
        public int Id { get; set; }
        public DateTime VisFechaCreacion { get; set; } = DateTime.Now;
        public int RutId { get; set; }
        public int DirClId { get; set; }
        public DateTime? VisFecha { get; set; }
        public TimeSpan? VisHora { get; set; }
        public int? VisSemanaDelMes { get; set; }
        public decimal VisLatitud { get; set; }
        public decimal VisLongitud { get; set; }
        public bool VisEstadoDel { get; set; } = true;
        public string? VisComentario { get; set; }

        // Relaciones
        public Ruta Ruta { get; set; } = null!;
        public ICollection<Evidencia> Evidencias { get; set; } = new List<Evidencia>();
        public DireccionCliente DireccionCliente { get; set; } = null!;

    }
}
