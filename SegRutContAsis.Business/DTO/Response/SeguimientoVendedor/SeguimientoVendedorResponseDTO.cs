using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.SeguimientoVendedor
{
    public class SeguimientoVendedorResponseDTO
    {
        public int segId { get; set; }
        public int venId { get; set; }
        public DateTime segFechaCreacion { get; set; }
        public decimal segLatitud {  get; set; }
        public decimal segLongitud { get; set; }
        public string? VendedorNombre { get; set; }
    }
}
