using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.SeguimientoVendedor
{
    public class SeguimientoVendedorRequestDTO
    {
        public int venId { get; set; }
        public decimal segLatitud { get; set; }
        public decimal segLongitud { get; set; }
    }
}
