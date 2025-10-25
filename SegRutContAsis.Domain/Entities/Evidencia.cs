using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Evidencia
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public int visId { get; set; }
        public string? Tipo { get; set; } = "";
        public string? Observaciones { get; set; } = "";

        // Relaciones
        public Visita Visita { get; set; } = null!;
    }
}
