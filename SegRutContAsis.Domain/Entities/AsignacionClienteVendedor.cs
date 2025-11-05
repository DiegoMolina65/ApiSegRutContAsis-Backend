using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class AsignacionClienteVendedor
    {
        public int asgId { get; set; }
        public int supId { get; set; }
        public int venId { get; set; }
        public int clId { get; set; }
        public DateTime asgFechaCreacion { get; set; } = DateTime.Now;
        public bool asgEstadoDel { get; set; } = true;

        // Relaciones
        public Supervisor Supervisor { get; set; }
        public Vendedor Vendedor { get; set; }
        public Cliente Cliente { get; set; }

    }
}
