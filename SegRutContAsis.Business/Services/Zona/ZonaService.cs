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

        // =========================
        // Obtener Zonas
        // =========================
        public async Task<List<ZonaResponseDTO>> ObtenerZonas()
        {
            return await _context.Zona
                .Where(z => z.EstadoDel)
                .Select(z => new ZonaResponseDTO
                {
                    IdZona = z.Id,
                    Nombre = z.Nombre,
                    Descripcion = z.Descripcion
                })
                .ToListAsync();
        }

        // =========================
        // Crear Zona
        // =========================
        public async Task<ZonaResponseDTO> CrearZona(ZonaRequestDTO request)
        {
            var existe = await _context.Zona.AnyAsync(z => z.Nombre == request.Nombre);
            if (existe)
                throw new Exception("Ya existe una zona con ese nombre.");

            var zona = new Zona
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                EstadoDel = true,
                FechaCreacion = DateTime.Now
            };

            _context.Zona.Add(zona);
            await _context.SaveChangesAsync();

            return new ZonaResponseDTO
            {
                IdZona = zona.Id,
                Nombre = zona.Nombre,
                Descripcion = zona.Descripcion
            };
        }

        // =========================
        // ACTUALIZAR ZONA
        // =========================
        public async Task<ZonaResponseDTO> ActualizarZona(int id, ZonaRequestDTO request)
        {
            var zona = await _context.Zona.FirstOrDefaultAsync(z => z.Id == id && z.EstadoDel);
            if (zona == null)
                throw new Exception("Zona no encontrada o desactivada.");

            var existe = await _context.Zona
                .AnyAsync(z => z.Nombre == request.Nombre && z.Id != id);
            if (existe)
                throw new Exception("Ya existe otra zona con ese nombre.");

            zona.Nombre = request.Nombre;
            zona.Descripcion = request.Descripcion;

            _context.Zona.Update(zona);
            await _context.SaveChangesAsync();

            return new ZonaResponseDTO
            {
                IdZona = zona.Id,
                Nombre = zona.Nombre,
                Descripcion = zona.Descripcion
            };
        }

        // =========================
        // DESHABILITAR ZONA
        // =========================
        public async Task<bool> DeshabilitarZona(int id)
        {
            var zona = await _context.Zona.FirstOrDefaultAsync(z => z.Id == id && z.EstadoDel);
            if (zona == null)
                throw new Exception("Zona no encontrada o ya deshabilitada.");

            zona.EstadoDel = false;

            _context.Zona.Update(zona);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
