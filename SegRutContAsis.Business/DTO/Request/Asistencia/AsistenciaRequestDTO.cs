using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.Asistencia
{
    public class AsistenciaRequestDTO
    {
        public int venId { get; set; }
        public decimal asiLatitud { get; set; }
        public decimal asiLongitud { get; set; }
    }
}
