using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.MarcarLlegadaVisita
{
    public class MarcarLlegadaVisitaRequestDTO
    {
        public int visId { get; set; }
        public TimeSpan? mlvHora { get; set; }
        public decimal mlvLatitud { get; set; }
        public decimal mlvLongitud { get; set; }
    }
}
