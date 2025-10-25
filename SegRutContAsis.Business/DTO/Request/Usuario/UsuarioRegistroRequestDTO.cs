using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.Usuario
{
    public class UsuarioRegistroRequestDTO
    {
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string NitEmpleado { get; set; }
        public string CarnetIdentidad  { get; set; }
        public string UsuarioLog { get; set; }
        public string ContrasenaLog { get; set; }
        public List<string> Roles { get; set; }
    }
}
