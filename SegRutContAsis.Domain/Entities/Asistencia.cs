using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Asistencia
    {
        public int asiId { get; set; }
        public DateTime asiFechaCreacion { get; set; } = DateTime.Now;
        public int venId { get; set; }
        public DateTime? asiHoraEntrada { get; set; }
        public DateTime? asiHoraSalida { get; set; }
        public decimal asiLatitud { get; set; }
        public decimal asiLongitud { get; set; }
        public bool asiEstadoDel { get; set; } = true;

        // Relaciones
        public Vendedor Vendedor { get; set; } = null!;
    }
}
