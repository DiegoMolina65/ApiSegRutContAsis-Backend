using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Cliente
{
    public class ClienteResponseDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string CarnetIdentidad { get; set; } = null!;
        public string NitCliente { get; set; } = null!;
        public string? TipoCliente { get; set; }
        public string Telefono { get; set; } = null!;
    }
}
