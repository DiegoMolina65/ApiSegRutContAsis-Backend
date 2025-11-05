using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.Visita
{
    public class VisitaRequestDTO
    {
        public int rutId { get; set; }
        public int dirClId { get; set; }
        public DateTime? visFecha { get; set; }
        public string? visComentario { get; set; }
    }

}
