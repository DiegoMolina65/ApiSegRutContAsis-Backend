using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.AsignacionClienteVendedor
{
    public class AsignacionClienteVendedorResponseDTO
    {
        public int asgId { get; set; }
        public int supId { get; set; }
        public string SupervisorNombre { get; set; }
        public int venId { get; set; }
        public string VendedorNombre { get; set; }
        public int clId { get; set; }
        public string ClienteNombre { get; set; }
        public DateTime asgFechaCreacion { get; set; }
        public bool asgEstadoDel { get; set; }
    }
}
