using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Asistencia
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public int VenId { get; set; }
        public DateTime? HoraEntrada { get; set; }
        public DateTime? HoraSalida { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public bool EstadoDel { get; set; } = true;

        // Relaciones
        public Vendedor Vendedor { get; set; } = null!;
    }
}
