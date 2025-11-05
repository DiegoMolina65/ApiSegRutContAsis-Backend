using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class DireccionCliente
    {
        public int dirClId { get; set; }
        public DateTime dirClFechaCreacion { get; set; }
        public int clId { get; set; }
        public int? zonId { get; set; }
        public string? dirClNombreSucursal { get; set; }
        public string dirClDireccion { get; set; } = string.Empty;
        public decimal dirClLatitud { get; set; }
        public decimal dirClLongitud { get; set; }
        public bool dirClEstadoDel { get; set; } = true;

        // Relaciones
        public virtual Cliente? Cliente { get; set; } 
        public virtual Zona? Zona { get; set; }
        public virtual ICollection<Visita> Visitas { get; set; } = new List<Visita>();  
    }
}
