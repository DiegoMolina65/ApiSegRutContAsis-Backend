using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.Cliente
{
    public class ClienteRequestDTO
    {
        public string NombreCompleto { get; set; } = null!;
        public string CarnetIdentidad { get; set; } = null!;
        public string NitCliente { get; set; } = null!;
        public string? TipoCliente { get; set; }
        public string Telefono { get; set; } = null!;
    }
}
