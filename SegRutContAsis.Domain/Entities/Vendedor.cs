using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Vendedor
    {
        public int venId { get; set; }
        public DateTime venFechaCreacion { get; set; } = DateTime.Now;
        public int usrId { get; set; }
        public bool venEstadoDel { get; set; } = true;

        // Relaciones
        public Usuario Usuario { get; set; }
        public ICollection<Ruta> Rutas { get; set; } = new List<Ruta>();
        public ICollection<AsignacionClienteVendedor> AsignacionesClienteVendedor { get; set; } = new List<AsignacionClienteVendedor>();
        public ICollection<AsignacionSupervisorVendedor> AsignacionesSupervisorVendedor { get; set; } = new List<AsignacionSupervisorVendedor>();

    }

}
