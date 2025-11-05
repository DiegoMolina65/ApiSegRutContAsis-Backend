using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.AsignacionSupervisorVendedor;
using SegRutContAsis.Business.Interfaces.AsignacionSupervisorVendedor;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services
{
    public class AsignacionSupervisorVendedorService : IAsignacionSupervisorVendedorService
    {
        private readonly SegRutContAsisContext _context;

        public AsignacionSupervisorVendedorService(SegRutContAsisContext context)
        {
            _context = context;
        }

        // Crear nueva asignación
        public async Task<AsignacionSupervisorVendedorResponseDTO> CrearAsignacionSupervisorVendedor(AsignacionSupervisorVendedorRequestDTO request)
        {
            try
            {
                var supervisor = await _context.Supervisor.FindAsync(request.supId);
                var vendedor = await _context.Vendedor.FindAsync(request.venId);

                if (supervisor == null)
                    throw new Exception("El supervisor especificado no existe.");

                if (vendedor == null)
                    throw new Exception("El vendedor especificado no existe.");

                var existeAsignacion = await _context.AsignacionSupervisorVendedor
                    .AnyAsync(a => a.supId == request.supId && a.venId == request.venId && a.asvEstadoDel);

                if (existeAsignacion)
                    throw new Exception("Ya existe una asignación activa entre este supervisor y vendedor.");

                var asignacion = new AsignacionSupervisorVendedor
                {
                    supId = request.supId,
                    venId = request.venId,
                    asvFechaCreacion = DateTime.Now,
                    asvEstadoDel = true
                };

                _context.AsignacionSupervisorVendedor.Add(asignacion);
                await _context.SaveChangesAsync();

                return new AsignacionSupervisorVendedorResponseDTO
                {
                    asvId = asignacion.asvId,
                    supId = asignacion.supId,
                    venId = asignacion.venId,
                    asvFechaCreacion = asignacion.asvFechaCreacion,
                    asvEstadoDel = asignacion.asvEstadoDel,                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear la asignación: {ex.Message}");
            }
        }

        // Actualizar una asignación existente
        public async Task<AsignacionSupervisorVendedorResponseDTO> CrearAsignacionSupervisorVendedor(AsignacionSupervisorVendedorRequestDTO request, int id)
        {
            try
            {
                var asignacion = await _context.AsignacionSupervisorVendedor.FindAsync(id);

                if (asignacion == null)
                    throw new Exception("No se encontró la asignación a actualizar.");

                asignacion.supId = request.supId;
                asignacion.venId = request.venId;
                asignacion.asvFechaCreacion = DateTime.Now;

                _context.AsignacionSupervisorVendedor.Update(asignacion);
                await _context.SaveChangesAsync();

                return new AsignacionSupervisorVendedorResponseDTO
                {
                    asvId = asignacion.asvId,
                    supId = asignacion.supId,
                    venId = asignacion.venId,
                    asvFechaCreacion = asignacion.asvFechaCreacion,
                    asvEstadoDel = asignacion.asvEstadoDel,
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar la asignación: {ex.Message}");
            }
        }

        // Obtener asignación por ID
        public async Task<AsignacionSupervisorVendedorResponseDTO> ObtenerAsignacionSupervisorVendedorId(int id)
        {
            try
            {
                var asignacion = await _context.AsignacionSupervisorVendedor
                    .Include(a => a.Supervisor)
                    .Include(a => a.Vendedor)
                    .FirstOrDefaultAsync(a => a.asvId == id);

                if (asignacion == null)
                    throw new Exception("No se encontró la asignación solicitada.");

                return new AsignacionSupervisorVendedorResponseDTO
                {
                    asvId = asignacion.asvId,
                    supId = asignacion.supId,
                    NombreSupervisor = asignacion.Supervisor?.Usuario?.usrNombreCompleto!,
                    venId = asignacion.venId,
                    NombreVendedor = asignacion.Vendedor?.Usuario?.usrNombreCompleto!,
                    asvFechaCreacion = asignacion.asvFechaCreacion,
                    asvEstadoDel = asignacion.asvEstadoDel,
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la asignación: {ex.Message}");
            }
        }

        // Obtener todas las asignaciones
        public async Task<List<AsignacionSupervisorVendedorResponseDTO>> ObtenerAsignacionSupervisorVendedorTodas()
        {
            try
            {
                var asignaciones = await _context.AsignacionSupervisorVendedor
                    .Include(a => a.Supervisor)
                    .Include(a => a.Vendedor)
                    .OrderByDescending(a => a.asvFechaCreacion)
                    .ToListAsync();

                if (!asignaciones.Any())
                    throw new Exception("No existen asignaciones registradas.");

                return asignaciones.Select(a => new AsignacionSupervisorVendedorResponseDTO
                {
                    asvId = a.asvId,
                    supId = a.supId,
                    NombreSupervisor = a.Supervisor?.Usuario?.usrNombreCompleto!,
                    venId = a.venId,
                    NombreVendedor = a.Vendedor?.Usuario?.usrNombreCompleto!,
                    asvFechaCreacion = a.asvFechaCreacion,
                    asvEstadoDel = a.asvEstadoDel,
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar las asignaciones: {ex.Message}");
            }
        }

        // Desactivar asignación (cambia Estado a false)
        public async Task<bool> DesactivarAsignacionSupervisorVendedor(int id)
        {
            try
            {
                var asignacion = await _context.AsignacionSupervisorVendedor.FindAsync(id);

                if (asignacion == null)
                    throw new Exception("No se encontró la asignación a desactivar.");

                asignacion.asvEstadoDel = false;

                _context.AsignacionSupervisorVendedor.Update(asignacion);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al desactivar la asignación: {ex.Message}");
            }
        }

        // Obtener vendedores asignados a un supervisor
        public async Task<List<AsignacionSupervisorVendedorResponseDTO>> ObtenerVendedoresPorSupervisor(int supervisorId)
        {
            try
            {
                var lista = await _context.AsignacionSupervisorVendedor
                    .Include(x => x.Vendedor)
                    .Where(x => x.supId == supervisorId && x.asvEstadoDel)
                    .ToListAsync();

                return lista.Select(x => new AsignacionSupervisorVendedorResponseDTO
                {
                    asvId = x.asvId,
                    supId = x.supId,
                    venId = x.venId,
                    NombreVendedor = x.Vendedor?.Usuario?.usrNombreCompleto!,
                    asvFechaCreacion = x.asvFechaCreacion
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los vendedores del supervisor: {ex.Message}");
            }
        }

        // Obtener supervisores asignados a un vendedor
        public async Task<List<AsignacionSupervisorVendedorResponseDTO>> ObtenerSupervisoresPorVendedor(int vendedorId)
        {
            try
            {
                var lista = await _context.AsignacionSupervisorVendedor
                    .Include(x => x.Supervisor)
                    .Where(x => x.venId == vendedorId && x.asvEstadoDel)
                    .ToListAsync();

                return lista.Select(x => new AsignacionSupervisorVendedorResponseDTO
                {
                    asvId = x.asvId,
                    supId = x.supId,
                    venId = x.venId,
                    NombreSupervisor = x.Supervisor?.Usuario?.usrNombreCompleto!,
                    asvFechaCreacion = x.asvFechaCreacion   
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los supervisores del vendedor: {ex.Message}");
            }
        }
    }
}
