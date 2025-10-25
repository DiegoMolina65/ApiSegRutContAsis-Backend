using SegRutContAsis.Business.DTO.Request.Zona;
using SegRutContAsis.Business.DTO.Response.Zona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.Zona
{
    public interface IZonaService
    {
        Task<List<ZonaResponseDTO>> ObtenerZonas();
        Task<ZonaResponseDTO> CrearZona(ZonaRequestDTO request);
        Task<ZonaResponseDTO> ActualizarZona(int id, ZonaRequestDTO request);
        Task<bool> DeshabilitarZona(int id);
    }
}
