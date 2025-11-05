using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class MarcarLlegadaVisita
    {
        public int mlvId { get; set; } 
        public int visId { get; set; }
        public TimeSpan? mlvHora { get; set; }
        public decimal mlvLatitud { get; set; }
        public decimal mlvLongitud { get; set; }
        public bool mlvEstadoDel { get; set; } = true;
        public DateTime mlvFechaCreacion { get; set; } = DateTime.Now;

        public Visita? Visita { get; set; }
    }
}
