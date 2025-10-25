
using SegRutContAsis.Business.DTO.Request.Evidencia;
using SegRutContAsis.Business.DTO.Response.Evidencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.Evidencia
{
    public interface IEvidenciaService
    {
        Task<EvidenciaResponseDTO> CrearEvidencia(EvidenciaRequestDTO requestDTO);
        Task<List<EvidenciaResponseDTO>> ObtenerEvidencia();
        Task<EvidenciaResponseDTO> ObtenerEvidenciaId(int id);
        Task<EvidenciaResponseDTO> ActualizarEvidencia(int id, EvidenciaRequestDTO requestDTO);
        Task<List<EvidenciaResponseDTO>> ObtenerEvidenciaPorVisita(int visId);
        Task<List<EvidenciaResponseDTO>> ObtenerEvidenciaPorVendedor(int vendedorId);
        Task<List<EvidenciaResponseDTO>> ObtenerEvidenciaPorTipo(string tipo);
    }
}
