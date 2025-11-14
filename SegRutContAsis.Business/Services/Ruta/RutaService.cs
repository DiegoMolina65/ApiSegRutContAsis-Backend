using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.Ruta;
using SegRutContAsis.Business.DTO.Response.Ruta;
using SegRutContAsis.Business.DTO.Response.Usuario;
using SegRutContAsis.Business.Interfaces.Ruta;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services
{
    public class RutaService : IRutaService
    {
        private readonly SegRutContAsisContext _context;

        public RutaService(SegRutContAsisContext context)
        {
            _context = context;
        }

        // Crear ruta
        public async Task<RutaResponseDTO> CrearRuta(RutaRequestDTO dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto), "Los datos de la ruta no pueden ser nulos.");

                if (dto.rutFechaEjecucion == null)
                    throw new Exception("Debe especificar la fecha de ejecución de la ruta.");

                // Validar duplicado antes de insertar
                bool existeRuta = await _context.Ruta.AnyAsync(r =>
                    r.VendedorId == dto.venId &&
                    r.rutFechaEjecucion == dto.rutFechaEjecucion);

                if (existeRuta)
                    throw new Exception("Ya existe una ruta registrada para este vendedor en la misma fecha de ejecución.");

                var ruta = new Ruta
                {
                    VendedorId = dto.venId,
                    SupervisorId = dto.supId,
                    rutNombre = dto.rutNombre,
                    rutComentario = dto.rutComentario,
                    rutFechaEjecucion = dto.rutFechaEjecucion,
                    rutEstadoDel = true
                };

                _context.Ruta.Add(ruta);
                await _context.SaveChangesAsync();

                return new RutaResponseDTO
                {
                    rutId = ruta.rutId,
                    venId = ruta.VendedorId,
                    supId = ruta.SupervisorId,
                    rutNombre = ruta.rutNombre,
                    rutComentario = ruta.rutComentario,
                    rutFechaEjecucion = ruta.rutFechaEjecucion,
                };
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null && dbEx.InnerException.Message.Contains("UQ_Ruta_Ven_Fecha"))
                    throw new Exception("Ya existe una ruta con el mismo vendedor y fecha de ejecución.");

                throw new Exception($"Error al guardar la ruta: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear la ruta: {ex.Message}");
            }
        }

        // Obtener rutas
        public async Task<List<RutaResponseDTO>> ObtenerRutas(UsuarioReponseDTO usuarioActual)
        {
            try
            {
                // ADMINISTRADOR: Ve todas las rutas
                if (usuarioActual.EsAdministrador)
                {
                    return await _context.Ruta
                        .Where(r => r.rutEstadoDel)
                        .Include(r => r.Vendedor).ThenInclude(v => v.Usuario)
                        .Include(r => r.Supervisor).ThenInclude(s => s.Usuario)
                        .Select(r => new RutaResponseDTO
                        {
                            rutId = r.rutId,
                            venId = r.VendedorId,
                            supId = r.SupervisorId,
                            rutNombre = r.rutNombre,
                            rutComentario = r.rutComentario,
                            rutFechaEjecucion = r.rutFechaEjecucion,
                            NombreVendedor = r.Vendedor.Usuario.usrNombreCompleto,
                            NombreSupervisor = r.Supervisor.Usuario.usrNombreCompleto
                        })
                        .ToListAsync();
                }

                // SUPERVISOR: Solo rutas de sus vendedores asignados
                if (usuarioActual.EsSupervisor)
                {
                    var supervisor = await _context.Supervisor
                        .FirstOrDefaultAsync(s => s.usrId == usuarioActual.usrId && s.supEstadoDel);

                    if (supervisor == null)
                        return new List<RutaResponseDTO>();

                    int supId = supervisor.supId;

                    var vendedoresIds = await _context.AsignacionSupervisorVendedor
                        .Where(a => a.supId == supId && a.asvEstadoDel)
                        .Select(a => a.venId)
                        .ToListAsync();

                    if (!vendedoresIds.Any())
                        return new List<RutaResponseDTO>();

                    return await _context.Ruta
                        .Where(r => vendedoresIds.Contains(r.VendedorId) && r.rutEstadoDel)
                        .Include(r => r.Vendedor).ThenInclude(v => v.Usuario)
                        .Include(r => r.Supervisor).ThenInclude(s => s.Usuario)
                        .Select(r => new RutaResponseDTO
                        {
                            rutId = r.rutId,
                            venId = r.VendedorId,
                            supId = r.SupervisorId,
                            rutNombre = r.rutNombre,
                            rutComentario = r.rutComentario,
                            rutFechaEjecucion = r.rutFechaEjecucion,
                            NombreVendedor = r.Vendedor.Usuario.usrNombreCompleto,
                            NombreSupervisor = r.Supervisor.Usuario.usrNombreCompleto
                        })
                        .ToListAsync();
                }

                return new List<RutaResponseDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las rutas: {ex.Message}");
            }
        }


        // Obtener rutas por ID
        public async Task<RutaResponseDTO> ObtenerRutaId(int id)
        {
            try
            {
                var ruta = await _context.Ruta
                    .Include(r => r.Vendedor).ThenInclude(v => v.Usuario)
                    .Include(r => r.Supervisor).ThenInclude(s => s.Usuario)
                    .Where(r => r.rutEstadoDel && r.rutId == id)
                    .Select(r => new RutaResponseDTO
                    {
                        rutId = r.rutId,
                        venId = r.VendedorId,
                        supId = r.SupervisorId,
                        rutNombre = r.rutNombre,
                        rutComentario = r.rutComentario,
                        rutFechaEjecucion = r.rutFechaEjecucion,
                        NombreVendedor = r.Vendedor.Usuario.usrNombreCompleto,
                        NombreSupervisor = r.Supervisor.Usuario.usrNombreCompleto
                    })
                    .FirstOrDefaultAsync();

                if (ruta == null)
                    throw new Exception("Ruta no encontrada o está deshabilitada.");

                return ruta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la ruta con ID {id}: {ex.Message}");
            }
        }

        // Actualizar ruta
        public async Task<RutaResponseDTO> ActualizarRuta(int id, RutaRequestDTO dto)
        {
            try
            {
                var ruta = await _context.Ruta.FindAsync(id);
                if (ruta == null || !ruta.rutEstadoDel)
                    throw new Exception("Ruta no encontrada o deshabilitada.");

                if (dto.rutFechaEjecucion == null)
                    throw new Exception("Debe especificar la nueva fecha de ejecución.");

                ruta.VendedorId = dto.venId;
                ruta.SupervisorId = dto.supId;
                ruta.rutNombre = dto.rutNombre;
                ruta.rutComentario = dto.rutComentario;
                ruta.rutFechaEjecucion = dto.rutFechaEjecucion;

                await _context.SaveChangesAsync();

                return new RutaResponseDTO
                {
                    rutId = ruta.rutId,
                    venId = ruta.VendedorId,
                    supId = ruta.SupervisorId,
                    rutNombre = ruta.rutNombre,
                    rutComentario = ruta.rutComentario,
                    rutFechaEjecucion = ruta.rutFechaEjecucion,
                };
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null && dbEx.InnerException.Message.Contains("UQ_Ruta_Ven_Fecha"))
                    throw new Exception("No se puede actualizar: existe otra ruta con el mismo vendedor y fecha.");

                throw new Exception($"Error al actualizar la ruta: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar la ruta con ID {id}: {ex.Message}");
            }
        }

        // Deshabilitar Ruta
        public async Task<bool> DeshabilitarRuta(int id)
        {
            try
            {
                var ruta = await _context.Ruta.FindAsync(id);
                if (ruta == null)
                    throw new Exception("Ruta no encontrada.");

                if (!ruta.rutEstadoDel)
                    throw new Exception("La ruta ya está deshabilitada.");

                ruta.rutEstadoDel = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al deshabilitar la ruta con ID {id}: {ex.Message}");
            }
        }

        // Rutas por vendedor
        public async Task<List<RutaResponseDTO>> ObtenerRutasPorVendedor(int venId)
        {
            try
            {
                var rutas = await _context.Ruta
                    .Include(r => r.Vendedor).ThenInclude(v => v.Usuario)
                    .Include(r => r.Supervisor).ThenInclude(s => s.Usuario)
                    .Where(r => r.rutEstadoDel && r.VendedorId == venId)
                    .Select(r => new RutaResponseDTO
                    {
                        rutId = r.rutId,
                        venId = r.VendedorId,
                        supId = r.SupervisorId,
                        rutNombre = r.rutNombre,
                        rutComentario = r.rutComentario,
                        rutFechaEjecucion = r.rutFechaEjecucion,
                        NombreVendedor = r.Vendedor.Usuario.usrNombreCompleto,
                        NombreSupervisor = r.Supervisor.Usuario.usrNombreCompleto
                    })
                    .ToListAsync();

                if (!rutas.Any())
                    throw new Exception($"No se encontraron rutas activas para el vendedor con ID {venId}.");

                return rutas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener rutas del vendedor {venId}: {ex.Message}");
            }
        }

        // Rutas por supervisor
        public async Task<List<RutaResponseDTO>> ObtenerRutasPorSupervisor(int supId)
        {
            try
            {
                var rutas = await _context.Ruta
                    .Include(r => r.Vendedor).ThenInclude(v => v.Usuario)
                    .Include(r => r.Supervisor).ThenInclude(s => s.Usuario)
                    .Where(r => r.rutEstadoDel && r.SupervisorId == supId)
                    .Select(r => new RutaResponseDTO
                    {
                        rutId = r.rutId,
                        venId = r.VendedorId,
                        supId = r.SupervisorId,
                        rutNombre = r.rutNombre,
                        rutComentario = r.rutComentario,
                        rutFechaEjecucion = r.rutFechaEjecucion,
                        NombreVendedor = r.Vendedor.Usuario.usrNombreCompleto,
                        NombreSupervisor = r.Supervisor.Usuario.usrNombreCompleto
                    })
                    .ToListAsync();

                if (!rutas.Any())
                    throw new Exception($"No se encontraron rutas activas para el supervisor con ID {supId}.");

                return rutas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener rutas del supervisor {supId}: {ex.Message}");
            }
        }
    }
}
