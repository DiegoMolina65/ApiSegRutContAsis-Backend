using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.DireccionCliente
{
    public class DireccionClienteResponseDTO
    {
        public int Id { get; set; }
        public int ClId { get; set; }
        public int? ZonId { get; set; }
        public string? NombreSucursal { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EstadoDel { get; set; }

        //  Mostrar datos relacionados
        public string? NombreCliente { get; set; }
        public string? NombreZona { get; set; }
    }
}
