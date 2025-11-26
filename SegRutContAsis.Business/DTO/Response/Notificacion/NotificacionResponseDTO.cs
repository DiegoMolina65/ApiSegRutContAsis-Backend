using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Notificacion
{
    public class NotificacionResponseDTO
    {
        public int notId { get; set; }
        public string notTitulo { get; set; }
        public string notMensaje { get; set; }
        public string notTipo { get; set; }
        public int? entidadId { get; set; }
        public int? venId { get; set; }
        public int? supId { get; set; }
        public bool notEstadoDel { get; set; }
        public DateTime notFechaCreacion { get; set; }

        // Relaciones
        public string nombreVendedor { get; set; }
        public string nombreSupervisor { get; set; }
    }
}
