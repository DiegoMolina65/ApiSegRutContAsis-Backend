using SegRutContAsis.Business.DTO.Request.Ruta;
using SegRutContAsis.Business.DTO.Response.Ruta;
using SegRutContAsis.Business.DTO.Response.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.Ruta
{
    public interface IRutaService
    {
        Task<RutaResponseDTO> CrearRuta(RutaRequestDTO dto);
        Task<List<RutaResponseDTO>> ObtenerRutas(UsuarioReponseDTO usuarioActual);
        Task<RutaResponseDTO> ObtenerRutaId(int id);
        Task<RutaResponseDTO> ActualizarRuta(int id, RutaRequestDTO dto);
        Task<bool> DeshabilitarRuta(int id);
        Task<List<RutaResponseDTO>> ObtenerRutasPorVendedor(int venId);
        Task<List<RutaResponseDTO>> ObtenerRutasPorSupervisor(int supId);
    }
}
