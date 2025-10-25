using SegRutContAsis.Business.DTO.Request.SeguimientoVendedor;
using SegRutContAsis.Business.DTO.Response.SeguimientoVendedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.SeguimientoVendedor
{
    public interface ISeguimientoVendedorService
    {
        Task<SeguimientoVendedorResponseDTO> CrearSeguimientoVendedor(SeguimientoVendedorRequestDTO dto);
        Task<List<SeguimientoVendedorResponseDTO>> ObtenerTodosSeguimientosVendedores();
        Task<List<SeguimientoVendedorResponseDTO>> ObtenerSeguimientosDeUnVendedor(int venId);

    }
}
