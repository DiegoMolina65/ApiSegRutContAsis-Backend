using SegRutContAsis.Business.DTO.Request.AsignacionClienteVendedor;
using SegRutContAsis.Business.DTO.Request.AsignacionSupervisorVendedor;
using SegRutContAsis.Business.DTO.Response.AsignacionClienteVendedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Interfaces.AsignacionSupervisorVendedor
{
    public interface IAsignacionSupervisorVendedorService
    {
        Task<AsignacionSupervisorVendedorResponseDTO> CrearAsignacionSupervisorVendedor(AsignacionSupervisorVendedorRequestDTO request);
        Task<AsignacionSupervisorVendedorResponseDTO> ObtenerAsignacionSupervisorVendedorId(int id);
        Task<List<AsignacionSupervisorVendedorResponseDTO>> ObtenerAsignacionSupervisorVendedorTodas();
        Task<AsignacionSupervisorVendedorResponseDTO> CrearAsignacionSupervisorVendedor(AsignacionSupervisorVendedorRequestDTO request, int id);
        Task<bool> DesactivarAsignacionSupervisorVendedor(int id);

        Task<List<AsignacionSupervisorVendedorResponseDTO>> ObtenerVendedoresPorSupervisor(int supervisorId);
        Task<List<AsignacionSupervisorVendedorResponseDTO>> ObtenerSupervisoresPorVendedor(int vendedorId);
    }
}
