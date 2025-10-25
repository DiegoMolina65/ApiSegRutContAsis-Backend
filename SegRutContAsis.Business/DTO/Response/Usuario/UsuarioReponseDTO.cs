using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Usuario
{
    public class UsuarioReponseDTO
    {
        public int IdUsuario { get; set; }
        public string Token { get; set; }
        public string Usuario { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
