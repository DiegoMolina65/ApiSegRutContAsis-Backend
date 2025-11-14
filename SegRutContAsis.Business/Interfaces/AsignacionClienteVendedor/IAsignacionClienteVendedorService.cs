using SegRutContAsis.Business.DTO.Request.AsignacionClienteVendedor;
using SegRutContAsis.Business.DTO.Response.AsignacionClienteVendedor;
using SegRutContAsis.Business.DTO.Response.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.AsignacionClienteVendedor
{
    public interface IAsignacionClienteVendedorService
    {
        Task<AsignacionClienteVendedorResponseDTO> CrearAsignacion(AsignacionClienteVendedorRequestDTO dto);
        Task<AsignacionClienteVendedorResponseDTO> ActualizarAsignacion(int id, AsignacionClienteVendedorRequestDTO dto);
        Task<bool> DesactivarAsignacion(int id);
        Task<AsignacionClienteVendedorResponseDTO> ObtenerAsignacionPorId(int id);
        Task<List<AsignacionClienteVendedorResponseDTO>> ObtenerAsignacionesPorVendedor(int venId);
        Task<List<AsignacionClienteVendedorResponseDTO>> ObtenerTodasAsignaciones(UsuarioReponseDTO usuarioActual);
    }
}
