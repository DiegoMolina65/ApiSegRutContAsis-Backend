using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class SeguimientoVendedor
    {
        public int segId { get; set; }
        public DateTime segFechaCreacion { get; set; } = DateTime.Now;
        public int venId { get; set; }
        public decimal segLatitud { get; set; } 
        public decimal segLongitud { get; set; } 

        // Relaciones
        public Vendedor Vendedor { get; set; } = null!;

    }
}
