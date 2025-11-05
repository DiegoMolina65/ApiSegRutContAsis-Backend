using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.Usuario
{
    public class UsuarioRegistroRequestDTO
    {
        public string usrNombreCompleto { get; set; }
        public string usrCorreo { get; set; }
        public string usrTelefono { get; set; }
        public string usrNitEmpleado { get; set; }
        public string usrCarnetIdentidad { get; set; }
        public string usrUsuarioLog { get; set; }
        public string usrContrasenaLog { get; set; }
        public List<string> Roles { get; set; }
    }
}
