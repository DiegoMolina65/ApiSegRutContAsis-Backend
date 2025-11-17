using SegRutContAsis.Business.DTO.Request.SeguimientoVendedor;
using SegRutContAsis.Business.DTO.Response.SeguimientoVendedor;
using SegRutContAsis.Business.DTO.Response.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.SeguimientoVendedor
{
    public interface ISeguimientoVendedorService
    {
        Task<SeguimientoVendedorResponseDTO> CrearSeguimientoVendedor(SeguimientoVendedorRequestDTO dto, UsuarioReponseDTO usuarioActual);
        Task<List<SeguimientoVendedorResponseDTO>> ObtenerTodosSeguimientosVendedores(UsuarioReponseDTO usuarioActual);
        Task<List<SeguimientoVendedorResponseDTO>> ObtenerSeguimientosDeUnVendedor(int venId);

    }
}
