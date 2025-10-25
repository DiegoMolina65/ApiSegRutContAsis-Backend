using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.Ruta
{
    public class RutaRequestDTO
    {
        public int VenId { get; set; }
        public int? SupId { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public string? Comentario { get; set; } = null!;
    }
}
