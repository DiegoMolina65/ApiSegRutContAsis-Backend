using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.AsignacionClienteVendedor;
using SegRutContAsis.Business.DTO.Response.AsignacionClienteVendedor;
using SegRutContAsis.Business.DTO.Response.Usuario;
using SegRutContAsis.Business.Interfaces.AsignacionClienteVendedor;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services
{
    public class AsignacionClienteVendedorService : IAsignacionClienteVendedorService
    {
        private readonly SegRutContAsisContext _context;

        public AsignacionClienteVendedorService(SegRutContAsisContext context)
        {
            _context = context;
        }

        // Crear Asignacion Cliente-Vendedor
        public async Task<AsignacionClienteVendedorResponseDTO> CrearAsignacion(AsignacionClienteVendedorRequestDTO dto)
        {
            var asignacion = new AsignacionClienteVendedor
            {
                supId = dto.supId,
                venId = dto.venId,
                clId = dto.clId,
                asgFechaCreacion = DateTime.Now,
                asgEstadoDel = true
            };

            _context.AsignacionClienteVendedor.Add(asignacion);
            await _context.SaveChangesAsync();

            var result = await _context.AsignacionClienteVendedor
                .Include(a => a.Supervisor)
                .ThenInclude(s => s.Usuario)
                .Include(a => a.Vendedor)
                .ThenInclude(v => v.Usuario)
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(a => a.asgId == asignacion.asgId);

            return new AsignacionClienteVendedorResponseDTO
            {
                asgId = result.asgId,
                supId = result.supId,
                SupervisorNombre = result.Supervisor?.Usuario?.usrNombreCompleto!,
                venId = result.venId,
                VendedorNombre = result.Vendedor?.Usuario?.usrNombreCompleto!,
                clId = result.clId,
                ClienteNombre = result.Cliente?.clNombreCompleto!,
                asgFechaCreacion = result.asgFechaCreacion,
                asgEstadoDel = result.asgEstadoDel
            };
        }

        // Actualizar Asignacion Cliente-Vendedor
        public async Task<AsignacionClienteVendedorResponseDTO> ActualizarAsignacion(int id, AsignacionClienteVendedorRequestDTO dto)
        {
            var asignacion = await _context.AsignacionClienteVendedor
                .FirstOrDefaultAsync(a => a.asgId == id && a.asgEstadoDel);

            if (asignacion == null)
                return null;

            asignacion.supId = dto.supId;
            asignacion.venId = dto.venId;
            asignacion.clId = dto.clId;

            _context.AsignacionClienteVendedor.Update(asignacion);
            await _context.SaveChangesAsync();

            var result = await _context.AsignacionClienteVendedor
                .Include(a => a.Supervisor)
                .ThenInclude(s => s.Usuario)
                .Include(a => a.Vendedor)
                .ThenInclude(v => v.Usuario)
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(a => a.asgId == id);

            return new AsignacionClienteVendedorResponseDTO
            {
                asgId = result.asgId,
                supId = result.supId,
                SupervisorNombre = result.Supervisor?.Usuario?.usrNombreCompleto!,
                venId = result.venId,
                VendedorNombre = result.Vendedor?.Usuario?.usrNombreCompleto!,
                clId = result.clId,
                ClienteNombre = result.Cliente?.clNombreCompleto!,
                asgFechaCreacion = result.asgFechaCreacion,
                asgEstadoDel = result.asgEstadoDel
            };
        }

        // Desactivar Asignacion Cliente-Vendedor
        public async Task<bool> DesactivarAsignacion(int id)
        {
            var asignacion = await _context.AsignacionClienteVendedor
                .FirstOrDefaultAsync(a => a.asgId == id && a.asgEstadoDel);

            if (asignacion == null)
                return false;

            asignacion.asgEstadoDel = false;
            _context.AsignacionClienteVendedor.Update(asignacion);
            await _context.SaveChangesAsync();

            return true;
        }

        // Obtener por ID Asignacion Cliente-Vendedor
        public async Task<AsignacionClienteVendedorResponseDTO> ObtenerAsignacionPorId(int id)
        {
            var result = await _context.AsignacionClienteVendedor
                .Include(a => a.Supervisor)
                .ThenInclude(s => s.Usuario)
                .Include(a => a.Vendedor)
                .ThenInclude(v => v.Usuario)
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(a => a.asgId == id && a.asgEstadoDel);

            if (result == null) return null;

            return new AsignacionClienteVendedorResponseDTO
            {
                asgId = result.asgId,
                supId = result.supId,
                SupervisorNombre = result.Supervisor?.Usuario?.usrNombreCompleto!,
                venId = result.venId,
                VendedorNombre = result.Vendedor?.Usuario?.usrNombreCompleto!,
                clId = result.clId,
                ClienteNombre = result.Cliente?.clNombreCompleto!,
                asgFechaCreacion = result.asgFechaCreacion,
                asgEstadoDel = result.asgEstadoDel
            };
        }

        // Obtener Asignaciones por vendedor Cliente-Vendedor
        public async Task<List<AsignacionClienteVendedorResponseDTO>> ObtenerAsignacionesPorVendedor(int venId)
        {
            var asignaciones = await _context.AsignacionClienteVendedor
                .Include(a => a.Supervisor)
                .ThenInclude(s => s.Usuario)
                .Include(a => a.Cliente)
                .Where(a => a.venId == venId && a.asgEstadoDel)
                .ToListAsync();

            return asignaciones.Select(result => new AsignacionClienteVendedorResponseDTO
            {
                asgId = result.asgId,
                supId = result.supId,
                SupervisorNombre = result.Supervisor?.Usuario?.usrNombreCompleto!,
                venId = result.venId,
                clId = result.clId,
                ClienteNombre = result.Cliente?.clNombreCompleto!,
                asgFechaCreacion = result.asgFechaCreacion,
                asgEstadoDel = result.asgEstadoDel
            }).ToList();
        }

        // Obtener todas Asignacion Cliente-Vendedor
        public async Task<List<AsignacionClienteVendedorResponseDTO>> ObtenerTodasAsignaciones(UsuarioReponseDTO usuarioActual)
        {
            // ADMINISTRADOR: Ver todas las asignaciones
            if (usuarioActual.EsAdministrador)
            {
                var asignaciones = await _context.AsignacionClienteVendedor
                    .Include(a => a.Supervisor).ThenInclude(s => s.Usuario)
                    .Include(a => a.Vendedor).ThenInclude(v => v.Usuario)
                    .Include(a => a.Cliente)
                    .Where(a => a.asgEstadoDel)
                    .ToListAsync();

                return asignaciones.Select(a => new AsignacionClienteVendedorResponseDTO
                {
                    asgId = a.asgId,
                    supId = a.supId,
                    SupervisorNombre = a.Supervisor?.Usuario?.usrNombreCompleto!,
                    venId = a.venId,
                    VendedorNombre = a.Vendedor?.Usuario?.usrNombreCompleto!,
                    clId = a.clId,
                    ClienteNombre = a.Cliente?.clNombreCompleto!,
                    asgFechaCreacion = a.asgFechaCreacion,
                    asgEstadoDel = a.asgEstadoDel
                }).ToList();
            }

            // SUPERVISOR: Ver solo asignaciones de sus vendedores
            if (usuarioActual.EsSupervisor)
            {
                var supervisor = await _context.Supervisor
                    .FirstOrDefaultAsync(s => s.usrId == usuarioActual.usrId && s.supEstadoDel);

                if (supervisor == null)
                    return new List<AsignacionClienteVendedorResponseDTO>();

                int supId = supervisor.supId;

                var asignaciones = await _context.AsignacionClienteVendedor
                    .Include(a => a.Supervisor).ThenInclude(s => s.Usuario)
                    .Include(a => a.Vendedor).ThenInclude(v => v.Usuario)
                    .Include(a => a.Cliente)
                    .Where(a => a.asgEstadoDel && a.supId == supId)
                    .ToListAsync();

                return asignaciones.Select(a => new AsignacionClienteVendedorResponseDTO
                {
                    asgId = a.asgId,
                    supId = a.supId,
                    SupervisorNombre = a.Supervisor?.Usuario?.usrNombreCompleto!,
                    venId = a.venId,
                    VendedorNombre = a.Vendedor?.Usuario?.usrNombreCompleto!,
                    clId = a.clId,
                    ClienteNombre = a.Cliente?.clNombreCompleto!,
                    asgFechaCreacion = a.asgFechaCreacion,
                    asgEstadoDel = a.asgEstadoDel
                }).ToList();
            }

            return new List<AsignacionClienteVendedorResponseDTO>();
        }

    }
}
