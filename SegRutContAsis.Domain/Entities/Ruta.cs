using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Ruta
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";
        public string? Comentario { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public bool EstadoDel { get; set; } = true;

        // Relaciones
        public int VendedorId { get; set; }
        public Vendedor Vendedor { get; set; } = null!;

        public int? SupervisorId { get; set; }
        public Supervisor? Supervisor { get; set; }
    }
}
