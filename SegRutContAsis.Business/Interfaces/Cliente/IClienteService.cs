using SegRutContAsis.Business.DTO.Request.Cliente;
using SegRutContAsis.Business.DTO.Response.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.Cliente
{
    public interface IClienteService
    {
        Task<List<ClienteResponseDTO>> ObtenerClientes();
        Task<ClienteResponseDTO?> ObtenerClientePorId(int id);
        Task<ClienteResponseDTO> CrearCliente(ClienteRequestDTO request);
        Task<ClienteResponseDTO?> ActualizarCliente(int id, ClienteRequestDTO request);
        Task<bool> DeshabilitarCliente(int id); 
    }
}
