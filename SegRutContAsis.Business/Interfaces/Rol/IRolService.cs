using SegRutContAsis.Business.DTO.Response.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.Rol
{
    public interface IRolService
    {
        Task<List<RolResponseDTO>> ObtenerRoles();
    }
}
