using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Visita
    {
        public int visId { get; set; }
        public DateTime visFechaCreacion { get; set; } = DateTime.Now;
        public int rutId { get; set; }
        public int dirClId { get; set; }
        public DateTime? visFecha { get; set; }
        public int? visSemanaDelMes { get; set; }
        public bool visEstadoDel { get; set; } = true;
        public string? visComentario { get; set; }

        // Relaciones
        public Ruta Ruta { get; set; } = null!;
        public ICollection<Evidencia> Evidencias { get; set; } = new List<Evidencia>();
        public DireccionCliente DireccionCliente { get; set; } = null!;
        public ICollection<MarcarLlegadaVisita> MarcarLlegadaVisitas { get; set; }


    }
}
