using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Ruta
{
    public class RutaResponseDTO
    {
        public int Id { get; set; }
        public int VenId { get; set; }
        public int? SupId { get; set; } = 0;
        public string Nombre { get; set; } = "";
        public string? Comentario { get; set; } = null!;
    }
}
