using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Usuario
{
    public class UsuarioReponseDTO
    {
        public int usrId { get; set; }
        public string Token { get; set; }
        public string usrNombreCompleto { get; set; }
        public string usrCorreo { get; set; }
        public string usrTelefono { get; set; }
        public string usrNitEmpleado { get; set; }
        public string usrCarnetIdentidad { get; set; }
        public string usrUsuarioLog { get; set; }
        public string usrContrasenaLog { get; set; }
        public bool usrEstadoDel { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public int? VendedorId { get; set; }
        public int? SupervisorId { get; set; }

        // Propiedades de conveniencia para el frontend / lógica
        public bool EsAdministrador => Roles.Contains("ADMINISTRADOR");
        public bool EsSupervisor => Roles.Contains("SUPERVISOR");
        public bool EsVendedor => Roles.Contains("VENDEDOR");

    }
}


