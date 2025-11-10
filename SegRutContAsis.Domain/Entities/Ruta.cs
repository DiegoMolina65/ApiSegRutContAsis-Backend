using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Ruta
    {
        public int rutId { get; set; }
        public int VendedorId { get; set; }
        public int? SupervisorId { get; set; }
        public string rutNombre { get; set; } = "";
        public string? rutComentario { get; set; }
        public DateTime rutFechaEjecucion { get; set; }
        public DateTime rutFechaCreacion { get; set; } = DateTime.Now;
        public bool rutEstadoDel { get; set; } = true;

        // Relaciones
        public Vendedor Vendedor { get; set; } = null!;
        public Supervisor? Supervisor { get; set; }
    }
}
