using SegRutContAsis.Business.DTO.Request.Visita;
using SegRutContAsis.Business.DTO.Response.Visita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.Visita
{
    public interface IVisitaService
    {
        Task<VisitaResponseDTO> CrearVisita(VisitaRequestDTO dto);
        Task<VisitaResponseDTO> ActualizarVisita(int id, VisitaRequestDTO dto);
        Task<bool> DeshabilitarVisita(int id);
        Task<VisitaResponseDTO> ObtenerVisitaId(int id);
        Task<List<VisitaResponseDTO>> ObtenerTodasVisitas();
        Task<List<VisitaResponseDTO>> ObtenerVisitasPorRuta(int rutaId);
        Task<List<VisitaResponseDTO>> ObtenerVisitasPorDireccionCliente(int clienteId);
        Task<List<VisitaResponseDTO>> ObtenerVisitasPorVendedor(int venId);
    }
}
