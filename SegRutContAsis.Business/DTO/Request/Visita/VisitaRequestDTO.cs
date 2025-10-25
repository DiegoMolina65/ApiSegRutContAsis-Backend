using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.Visita
{
    public class VisitaRequestDTO
    {
        public int RutId { get; set; }
        public int DirClId { get; set; }
        public DateTime? VisFecha { get; set; }
        public TimeSpan? VisHora { get; set; }
        public int? VisSemanaDelMes { get; set; }
        public decimal VisLatitud { get; set; }
        public decimal VisLongitud { get; set; }
        public string? VisComentario { get; set; }
    }

}
