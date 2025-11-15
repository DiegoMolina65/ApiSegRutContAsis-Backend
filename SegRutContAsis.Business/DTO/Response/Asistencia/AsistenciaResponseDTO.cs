using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Asistencia
{
    public class AsistenciaResponseDTO
    {
        public int asiId { get; set; }
        public int venId { get; set; }
        public DateTime? asiHoraEntrada { get; set; }
        public DateTime? asiHoraSalida { get; set; }
        public decimal asiLatitud { get; set; }
        public decimal asiLongitud { get; set; }

        // Datos adicionales
        public string? nombreVendedor { get; set; }
    }
}
