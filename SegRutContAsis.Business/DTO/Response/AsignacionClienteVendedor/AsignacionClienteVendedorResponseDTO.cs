using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.AsignacionClienteVendedor
{
    public class AsignacionClienteVendedorResponseDTO
    {
        public int Id { get; set; }
        public int SupId { get; set; }
        public string SupervisorNombre { get; set; }
        public int VenId { get; set; }
        public string VendedorNombre { get; set; }
        public int ClId { get; set; }
        public string ClienteNombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EstadoDel { get; set; }
    }
}
