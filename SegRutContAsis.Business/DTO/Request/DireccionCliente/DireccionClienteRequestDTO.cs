using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.DireccionCliente
{
    public class DireccionClienteRequestDTO
    {
        public int ClId { get; set; }
        public int? ZonId { get; set; }
        public string? NombreSucursal { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }
}
