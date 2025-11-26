using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Notificacion
    {
        public int notId { get; set; }
        public string notTitulo { get; set; }
        public string notMensaje { get; set; }
        public string notTipo { get; set; }
        public int? entidadId { get; set; }
        public int? venId { get; set; }
        public int? supId { get; set; }
        public DateTime notFechaCreacion {get; set;} = DateTime.Now;
        public bool notEstadoDel { get; set; } = true;

        // Relaciones
        public Vendedor? Vendedor { get; set; }
        public Supervisor? Supervisor { get; set; }

    }
}
