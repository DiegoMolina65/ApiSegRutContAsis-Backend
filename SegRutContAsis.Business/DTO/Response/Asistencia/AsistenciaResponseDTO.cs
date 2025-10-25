using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Asistencia
{
    public class AsistenciaResponseDTO
    {
        public int Id { get; set; }
        public int VenId { get; set; }
        public DateTime? HoraEntrada { get; set; }
        public DateTime? HoraSalida { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }
}
