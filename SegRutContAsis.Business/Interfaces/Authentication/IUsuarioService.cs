using SegRutContAsis.Business.DTO.Request.Usuario;
using SegRutContAsis.Business.DTO.Response.Usuario;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.Authentication
{
    public interface IUsuarioService
    {
        Task<UsuarioReponseDTO> LoginUsuario(UsuarioRequestDTO request);
        Task<Usuario> RegistrarUsuario(UsuarioRegistroRequestDTO dto);
        Task<bool> LogoutUsuario();
        Task<List<UsuarioReponseDTO>> ObtenerUsuarios();
        Task<UsuarioReponseDTO> ObtenerUsuarioId(int id);
        Task<Usuario> ActualizarUsuario(int id, UsuarioRegistroRequestDTO dto);
        Task<bool> DeshabilitarUsuario(int id);
    }
}
