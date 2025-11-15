using SegRutContAsis.Business.DTO.Request.Asistencia;
using SegRutContAsis.Business.DTO.Response.Asistencia;
using SegRutContAsis.Business.DTO.Response.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.Asistencia
{
    public interface IAsistenciaService
    {
        Task<AsistenciaResponseDTO> RegistrarEntrada(AsistenciaRequestDTO dto);
        Task<AsistenciaResponseDTO> RegistrarSalida(int venId);
        Task<List<AsistenciaResponseDTO>> ObtenerAsistencias();
    }
}
