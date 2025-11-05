using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Cliente
{
    public class ClienteResponseDTO
    {
        public int clId { get; set; }
        public string clNombreCompleto { get; set; } = null!;
        public string clCarnetIdentidad { get; set; } = null!;
        public string clNitCliente { get; set; } = null!;
        public string? clTipoCliente { get; set; }
        public string clTelefono { get; set; } = null!;
    }
}
