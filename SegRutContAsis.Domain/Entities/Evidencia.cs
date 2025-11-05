using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Evidencia
    {
        public int eviId { get; set; }
        public DateTime eviFechaCreacion { get; set; } = DateTime.Now;
        public int visId { get; set; }
        public string? eviTipo { get; set; } = "";
        public string? eviObservaciones { get; set; } = "";

        // Relaciones
        public Visita Visita { get; set; } = null!;
    }
}
