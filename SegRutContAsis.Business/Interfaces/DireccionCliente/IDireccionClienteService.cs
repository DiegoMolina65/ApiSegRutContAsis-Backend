using SegRutContAsis.Business.DTO.Request.DireccionCliente;
using SegRutContAsis.Business.DTO.Response.DireccionCliente;
using SegRutContAsis.Business.DTO.Response.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.DireccionCliente
{
    public interface IDireccionClienteService
    {
        Task<DireccionClienteResponseDTO> CrearDireccion(DireccionClienteRequestDTO dto);
        Task<DireccionClienteResponseDTO?> ActualizarDireccion(int id, DireccionClienteRequestDTO dto);
        Task<bool> DesactivarDireccion(int id);
        Task<DireccionClienteResponseDTO?> ObtenerPorId(int id);
        Task<List<DireccionClienteResponseDTO>> ObtenerPorCliente(int clId);
        Task<List<DireccionClienteResponseDTO>> ObtenerTodas(UsuarioReponseDTO usuarioActual);

    }
}
