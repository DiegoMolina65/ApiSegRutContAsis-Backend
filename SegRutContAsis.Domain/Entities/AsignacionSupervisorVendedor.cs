using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class AsignacionSupervisorVendedor
    {
        public int asvId { get; set; }
        public int supId { get; set; }
        public int venId { get; set; }
        public DateTime asvFechaCreacion { get; set; } = DateTime.Now;
        public bool asvEstadoDel { get; set; }  = true;

        // Relaciones
        public Supervisor? Supervisor { get; set; }
        public Vendedor? Vendedor { get; set; }  
    }
}
