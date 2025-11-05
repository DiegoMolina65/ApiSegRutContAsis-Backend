using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.Zona;
using SegRutContAsis.Business.DTO.Response.Zona;
using SegRutContAsis.Business.Interfaces.Zona;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services
{
    public class ZonaService : IZonaService
    {
        private readonly SegRutContAsisContext _context;

        public ZonaService(SegRutContAsisContext context)
        {
            _context = context;
        }

        // Obtener Zonas
        public async Task<List<ZonaResponseDTO>> ObtenerZonas()
        {
            return await _context.Zona
                .Where(z => z.zonEstadoDel)
                .Select(z => new ZonaResponseDTO
                {
                    zonId = z.zonId,
                    zonNombre = z.zonNombre,
                    zonDescripcion = z.zonDescripcion
                })
                .ToListAsync();
        }

        // Crear Zona
        public async Task<ZonaResponseDTO> CrearZona(ZonaRequestDTO request)
        {
            var existe = await _context.Zona.AnyAsync(z => z.zonNombre == request.zonNombre);
            if (existe)
                throw new Exception("Ya existe una zona con ese nombre.");

            var zona = new Zona
            {
                zonNombre = request.zonNombre,
                zonDescripcion = request.zonDescripcion,
                zonEstadoDel = true,
                zonFechaCreacion = DateTime.Now
            };

            _context.Zona.Add(zona);
            await _context.SaveChangesAsync();

            return new ZonaResponseDTO
            {
                zonId = zona.zonId,
                zonNombre = zona.zonNombre,
                zonDescripcion = zona.zonDescripcion
            };
        }

        // ACTUALIZAR ZONA
        public async Task<ZonaResponseDTO> ActualizarZona(int id, ZonaRequestDTO request)
        {
            var zona = await _context.Zona.FirstOrDefaultAsync(z => z.zonId == id && z.zonEstadoDel);
            if (zona == null)
                throw new Exception("Zona no encontrada o desactivada.");

            var existe = await _context.Zona
                .AnyAsync(z => z.zonNombre == request.zonNombre && z.zonId != id);
            if (existe)
                throw new Exception("Ya existe otra zona con ese nombre.");

            zona.zonNombre = request.zonNombre;
            zona.zonDescripcion = request.zonDescripcion;

            _context.Zona.Update(zona);
            await _context.SaveChangesAsync();

            return new ZonaResponseDTO
            {
                zonId = zona.zonId,
                zonNombre = zona.zonNombre,
                zonDescripcion = zona.zonDescripcion
            };
        }

        // DESHABILITAR ZONA
        public async Task<bool> DeshabilitarZona(int id)
        {
            var zona = await _context.Zona.FirstOrDefaultAsync(z => z.zonId == id && z.zonEstadoDel);
            if (zona == null)
                throw new Exception("Zona no encontrada o ya deshabilitada.");

            zona.zonEstadoDel = false;

            _context.Zona.Update(zona);
            await _context.SaveChangesAsync();
            return true;
        }

        // OBTENER ZONA POR ID
        public async Task<ZonaResponseDTO> ObtenerZonaPorId(int id)
        {
            var zona = await _context.Zona
                .Where(z => z.zonEstadoDel && z.zonId == id)
                .Select(z => new ZonaResponseDTO
                {
                    zonId = z.zonId,
                    zonNombre = z.zonNombre,
                    zonDescripcion = z.zonDescripcion
                })
                .FirstOrDefaultAsync();

            if (zona == null)
                throw new Exception("Zona no encontrada o deshabilitada.");

            return zona;
        }
    }
}
