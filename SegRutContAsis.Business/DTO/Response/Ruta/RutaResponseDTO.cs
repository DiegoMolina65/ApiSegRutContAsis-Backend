using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Ruta
{
    public class RutaResponseDTO
    {
        public int rutId { get; set; }
        public int venId { get; set; }
        public int? supId { get; set; } = 0;
        public string rutNombre { get; set; } = "";
        public string? rutComentario { get; set; } = null!;
        public DateTime rutFechaEjecucion { get; set; }

        // Datos adicionales
        public string? NombreVendedor { get; set; }
    }
}


