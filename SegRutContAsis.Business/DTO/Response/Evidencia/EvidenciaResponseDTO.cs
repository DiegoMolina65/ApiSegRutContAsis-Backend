using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Evidencia
{
    public class EvidenciaResponseDTO
    {
        public int eviId { get; set; }
        public DateTime eviFechaCreacion { get; set; }
        public int VisitaId { get; set; }
        public string? eviTipo { get; set; }
        public string? eviObservaciones { get; set; }
    }
}
