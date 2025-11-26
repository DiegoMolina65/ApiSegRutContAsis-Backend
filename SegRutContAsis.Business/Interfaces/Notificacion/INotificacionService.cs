using SegRutContAsis.Business.DTO.Response.Notificacion;
using SegRutContAsis.Business.DTO.Response.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.Notificacion
{
    public interface INotificacionService
    {
        Task<List<NotificacionResponseDTO>> obtenerNotificaciones(UsuarioReponseDTO usuarioActual);
    }
}
