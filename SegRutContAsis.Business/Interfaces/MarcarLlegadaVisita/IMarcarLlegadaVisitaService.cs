using SegRutContAsis.Business.DTO.Request.MarcarLlegadaVisita;
using SegRutContAsis.Business.DTO.Response.MarcarLlegadaVisita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.MarcarLlegadaVisita
{
    public interface IMarcarLlegadaVisitaService
    {
        Task<MarcarLlegadaVisitaResponseDTO> crearMarcarLlegadaVisita(MarcarLlegadaVisitaRequestDTO dto);
        Task<List<MarcarLlegadaVisitaResponseDTO>> obtenerTodasMarcacionesLlegadaVisita();
        Task<MarcarLlegadaVisitaResponseDTO> obtenerPorIdMarcacionesLlegadaVisita(int id);
        Task<bool> desactivarMarcacionLlegadaVisita(int id);
        Task<MarcarLlegadaVisitaResponseDTO> actualizarMarcarLlegadaVisita(MarcarLlegadaVisitaRequestDTO dto, int id);
        Task<List<MarcarLlegadaVisitaResponseDTO>> obtenerMarcacionesPorVisita(int visId);

    }
}
