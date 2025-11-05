using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Supervisor
    {
        public int supId { get; set; }
        public DateTime supFechaCreacion { get; set; } = DateTime.Now;
        public int usrId { get; set; }
        public bool supEstadoDel { get; set; } = true;

        // Relaciones
        public Usuario Usuario { get; set; }
        public ICollection<Ruta> Rutas { get; set; } = new List<Ruta>();
        public ICollection<AsignacionClienteVendedor> AsignacionesClienteVendedor { get; set; } = new List<AsignacionClienteVendedor>();
        public ICollection<AsignacionSupervisorVendedor> AsignacionesSupervisorVendedor { get; set; } = new List<AsignacionSupervisorVendedor>();
    }

}
