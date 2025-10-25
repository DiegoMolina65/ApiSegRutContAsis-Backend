using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Evidencia
{
    public class EvidenciaResponseDTO
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int VisitaId { get; set; }
        public string? Tipo { get; set; }
        public string? Observaciones { get; set; }
    }
}
