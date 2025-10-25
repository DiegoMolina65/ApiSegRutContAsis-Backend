using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.Asistencia
{
    public class AsistenciaRequestDTO
    {
        public int VenId { get; set; }
        public string? Coordenadas { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }
}
