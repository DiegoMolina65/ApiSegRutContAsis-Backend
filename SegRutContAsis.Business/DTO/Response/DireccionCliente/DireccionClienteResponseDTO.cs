using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.DireccionCliente
{
    public class DireccionClienteResponseDTO
    {
        public int dirClId { get; set; }
        public int clId { get; set; }
        public string? NombreCliente { get; set; }
        public int? zonId { get; set; }
        public string? NombreZona { get; set; }
        public string? dirClNombreSucursal { get; set; }
        public string dirClDireccion { get; set; } = string.Empty;
        public decimal dirClLatitud { get; set; }
        public decimal dirClLongitud { get; set; }
        public DateTime dirClFechaCreacion { get; set; }
        public bool dirClEstadoDel { get; set; }

    }
}
