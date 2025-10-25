using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class UsuarioRol
    {
        public int usrID { get; set; }
        public Usuario Usuario { get; set; }

        public int rolId { get; set; }
        public Rol Rol { get; set; }
    }
}
