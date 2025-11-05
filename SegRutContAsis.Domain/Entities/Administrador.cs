using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Administrador
    {
        public int admId { get; set; }
        public DateTime admFechaCreacion { get; set; } = DateTime.Now;
        public int usrId { get; set; }
        public bool admEstadoDel { get; set; } = true;

        // Relaciones
        public Usuario Usuario { get; set; }

    }
}
