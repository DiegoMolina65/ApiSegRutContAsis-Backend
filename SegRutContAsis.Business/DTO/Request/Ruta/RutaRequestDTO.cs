using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.Ruta
{
    public class RutaRequestDTO
    {
        public int venId { get; set; }
        public int? supId { get; set; } = 0;
        public string rutNombre { get; set; } = "";
        public string? rutComentario { get; set; } = null!;
        public DateTime rutFechaEjecucion { get; set; }
    }
}
