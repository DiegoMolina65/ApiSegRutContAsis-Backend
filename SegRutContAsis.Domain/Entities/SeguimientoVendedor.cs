using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class SeguimientoVendedor
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public int venId { get; set; }
        public decimal Latitud { get; set; } 
        public decimal Longitud { get; set; } 

        // Relaciones
        public Vendedor Vendedor { get; set; } = null!;

    }
}
