using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.MarcarLlegadaVisita;
using SegRutContAsis.Business.DTO.Response.MarcarLlegadaVisita;
using SegRutContAsis.Business.Interfaces.MarcarLlegadaVisita;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services
{
    public class MarcarLlegadaVisitaService : IMarcarLlegadaVisitaService
    {
        private readonly SegRutContAsisContext _context;

        public MarcarLlegadaVisitaService(SegRutContAsisContext context)
        {
            _context = context;
        }

        // Crear Marcar Llegada Visita
        public async Task<MarcarLlegadaVisitaResponseDTO> crearMarcarLlegadaVisita(MarcarLlegadaVisitaRequestDTO dto)
        {
            var entity = new MarcarLlegadaVisita
            {
                visId = dto.visId,
                mlvHora = dto.mlvHora,
                mlvLatitud = dto.mlvLatitud,
                mlvLongitud = dto.mlvLongitud,
                mlvEstadoDel = true,
                mlvFechaCreacion = DateTime.Now
            };

            _context.MarcarLlegadaVisita.Add(entity);
            await _context.SaveChangesAsync();

            return new MarcarLlegadaVisitaResponseDTO
            {
                mlvId = entity.mlvId,
                visId = entity.visId,
                mlvHora = entity.mlvHora,
                mlvLatitud = entity.mlvLatitud,
                mlvLongitud = entity.mlvLongitud,
                mlvEstadoDel = entity.mlvEstadoDel,
                mlvFechaCreacion = entity.mlvFechaCreacion
            };
        }

        // Obtener todas Marcar Llegada Visita
        public async Task<List<MarcarLlegadaVisitaResponseDTO>> obtenerTodasMarcacionesLlegadaVisita()
        {
            var entities = await _context.MarcarLlegadaVisita
                .Where(m => m.mlvEstadoDel)
                .Include(m => m.Visita)
                    .ThenInclude(v => v.Ruta)
                        .ThenInclude(r => r.Vendedor)
                            .ThenInclude(vend => vend.Usuario)
                .ToListAsync();

            var resultado = entities.Select(m => new MarcarLlegadaVisitaResponseDTO
            {
                mlvId = m.mlvId,
                visId = m.visId,
                mlvHora = m.mlvHora,
                mlvLatitud = m.mlvLatitud,
                mlvLongitud = m.mlvLongitud,
                mlvEstadoDel = m.mlvEstadoDel,
                mlvFechaCreacion = m.mlvFechaCreacion,
            }).ToList();

            return resultado;
        }

        // Obtener por Id Marcar Llegada Visita
        public async Task<MarcarLlegadaVisitaResponseDTO> obtenerPorIdMarcacionesLlegadaVisita(int id)
        {
            var entity = await _context.MarcarLlegadaVisita.FirstOrDefaultAsync(m => m.mlvId == id && m.mlvEstadoDel); 

            return new MarcarLlegadaVisitaResponseDTO
            {
                mlvId = entity.mlvId,
                visId = entity.visId,
                mlvHora = entity.mlvHora,
                mlvLatitud = entity.mlvLatitud,
                mlvLongitud = entity.mlvLongitud,
                mlvEstadoDel = entity.mlvEstadoDel,
                mlvFechaCreacion = entity.mlvFechaCreacion
            };
        }

        // Desactivar Marcar Llegada Visita
        public async Task<bool> desactivarMarcacionLlegadaVisita(int id)
        {
            var entity = await _context.MarcarLlegadaVisita.FirstOrDefaultAsync(m => m.mlvId == id && m.mlvEstadoDel);
            if (entity == null) return false;

            entity.mlvEstadoDel = false;
            await _context.SaveChangesAsync();
            return true;
        }

        // Actualizar Marcar Llegada Visita
        public async Task<MarcarLlegadaVisitaResponseDTO> actualizarMarcarLlegadaVisita(MarcarLlegadaVisitaRequestDTO dto, int id)
        {
            var entity = await _context.MarcarLlegadaVisita.FirstOrDefaultAsync(m => m.mlvId == id && m.mlvEstadoDel);
            if (entity == null) return null;

            entity.visId = dto.visId;
            entity.mlvHora = dto.mlvHora;
            entity.mlvLatitud = dto.mlvLatitud;
            entity.mlvLongitud = dto.mlvLongitud;

            await _context.SaveChangesAsync();

            return new MarcarLlegadaVisitaResponseDTO
            {
                mlvId = entity.mlvId,
                visId = entity.visId,
                mlvHora = entity.mlvHora,
                mlvLatitud = entity.mlvLatitud,
                mlvLongitud = entity.mlvLongitud,
                mlvEstadoDel = entity.mlvEstadoDel,
                mlvFechaCreacion = entity.mlvFechaCreacion
            };
        }

        // Obtener Marcaciones por visita
        public async Task<List<MarcarLlegadaVisitaResponseDTO>> obtenerMarcacionesPorVisita(int visId)
        {
            var marcacionesEntities = await _context.MarcarLlegadaVisita
                .Where(m => m.visId == visId && m.mlvEstadoDel)
                .Include(m => m.Visita)
                    .ThenInclude(v => v.Ruta)
                        .ThenInclude(r => r.Vendedor)
                            .ThenInclude(vend => vend.Usuario)
                .Include(m => m.Visita)
                    .ThenInclude(v => v.DireccionCliente)
                        .ThenInclude(dc => dc.Cliente)
                .ToListAsync();

            var marcaciones = marcacionesEntities.Select(m => new MarcarLlegadaVisitaResponseDTO
            {
                mlvId = m.mlvId,
                visId = m.visId,
                mlvHora = m.mlvHora,
                mlvLatitud = m.mlvLatitud,
                mlvLongitud = m.mlvLongitud,
                mlvEstadoDel = m.mlvEstadoDel,
                mlvFechaCreacion = m.mlvFechaCreacion,
                NombreVendedor = m.Visita?.Ruta?.Vendedor?.Usuario?.usrNombreCompleto,
                NombreSucursalCliente = m.Visita?.DireccionCliente?.dirClNombreSucursal,
                SucursalLatitud = m.Visita?.DireccionCliente?.dirClLatitud,
                SucursalLongitud = m.Visita?.DireccionCliente?.dirClLongitud,
                NombreCliente = m.Visita?.DireccionCliente?.Cliente?.clNombreCompleto,
                UsuarioLogVendedor = m.Visita?.Ruta?.Vendedor?.Usuario?.usrUsuarioLog,
                TelefonoVendedor = m.Visita?.Ruta?.Vendedor?.Usuario?.usrTelefono
            }).ToList();

            return marcaciones;
        }



    }
}
