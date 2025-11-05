using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Zona
{
    public class ZonaResponseDTO
    {
        public int zonId { get; set; }
        public string zonNombre { get; set; } = string.Empty;
        public string zonDescripcion { get; set; } = string.Empty;
        public DateTime zonFechaCreacion { get; set; } = DateTime.Now;
        public bool zonEstadoDel { get; set; } = true;
    }
}
