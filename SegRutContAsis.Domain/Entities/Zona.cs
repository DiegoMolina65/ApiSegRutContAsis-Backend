using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Zona
    {
        public int zonId { get; set; }
        public DateTime zonFechaCreacion { get; set; } = DateTime.Now;
        public string zonNombre { get; set; }
        public string zonDescripcion { get; set; }
        public bool zonEstadoDel { get; set; } = true;
    }
}
