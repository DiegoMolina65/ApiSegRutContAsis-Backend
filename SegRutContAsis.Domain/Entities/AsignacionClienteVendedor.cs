using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class AsignacionClienteVendedor
    {
        public int Id { get; set; }
        public int SupId { get; set; }
        public int VenId { get; set; }
        public int ClId { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public bool EstadoDel { get; set; } = true;

        // Relaciones
        public Supervisor Supervisor { get; set; }
        public Vendedor Vendedor { get; set; }
        public Cliente Cliente { get; set; }

    }
}
