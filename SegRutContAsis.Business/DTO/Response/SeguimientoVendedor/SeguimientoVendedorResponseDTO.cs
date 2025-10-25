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
        public DateTime FechaCreacion { get; set; }
        public decimal Latitud {  get; set; }
        public decimal Longitud { get; set; }
    }
}
