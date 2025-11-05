using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.DireccionCliente
{
    public class DireccionClienteRequestDTO
    {
        public int clId { get; set; }
        public int? zonId { get; set; }
        public string? dirClNombreSucursal { get; set; }
        public string dirClDireccion { get; set; } = string.Empty;
        public decimal dirClLatitud { get; set; }
        public decimal dirClLongitud { get; set; }
    }
}
