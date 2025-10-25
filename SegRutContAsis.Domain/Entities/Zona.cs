using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Zona
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EstadoDel { get; set; } = true;
    }
}
