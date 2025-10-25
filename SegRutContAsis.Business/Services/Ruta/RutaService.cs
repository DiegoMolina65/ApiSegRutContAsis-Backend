using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.Ruta;
using SegRutContAsis.Business.DTO.Response.Ruta;
using SegRutContAsis.Business.Interfaces.Ruta;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        // =========================
        // Crear Ruta
        // =========================
        public async Task<RutaResponseDTO> CrearRuta(RutaRequestDTO dto)
        {
            var ruta = new Ruta
            {
                VendedorId = dto.VenId,
                SupervisorId = dto.SupId,
                Nombre = dto.Nombre,
                Comentario = dto.Comentario,
                EstadoDel = true
            };

            _context.Ruta.Add(ruta);
            await _context.SaveChangesAsync();

            return new RutaResponseDTO
            {
                Id = ruta.Id,
                VenId = ruta.VendedorId,
                SupId = ruta.SupervisorId,
                Nombre = ruta.Nombre,
                Comentario = ruta.Comentario
            };
        }

        // =========================
        // Obtener Rutas
        // =========================
        public async Task<List<RutaResponseDTO>> ObtenerRutas()
        {
            return await _context.Ruta
                .Where(r => r.EstadoDel)
                .Select(r => new RutaResponseDTO
                {
                    Id = r.Id,
                    VenId = r.VendedorId,
                    SupId = r.SupervisorId,
                    Nombre = r.Nombre,
                    Comentario = r.Comentario
                })
                .ToListAsync();
        }

        // =========================
        // Obtener Ruta por Id
        // =========================
        public async Task<RutaResponseDTO> ObtenerRutaId(int id)
        {
            var rutaDTO = await _context.Ruta
                .Where(r => r.EstadoDel && r.Id == id)
                .Select(r => new RutaResponseDTO
                {
                    Id = r.Id,
                    VenId = r.VendedorId,
                    SupId = r.SupervisorId,
                    Nombre = r.Nombre,
                    Comentario = r.Comentario
                })
                .FirstOrDefaultAsync();

            if (rutaDTO == null)
                throw new Exception("Ruta no encontrada");

            return rutaDTO;
        }

        // =========================
        // Actualizar Ruta
        // =========================
        public async Task<RutaResponseDTO> ActualizarRuta(int id, RutaRequestDTO dto)
        {
            var ruta = await _context.Ruta.FindAsync(id);
            if (ruta == null) throw new Exception("Ruta no encontrada");

            ruta.VendedorId = dto.VenId;
            ruta.SupervisorId = dto.SupId;
            ruta.Nombre = dto.Nombre;
            ruta.Comentario = dto.Comentario;

            await _context.SaveChangesAsync();

            return new RutaResponseDTO
            {
                Id = ruta.Id,
                VenId = ruta.VendedorId,
                SupId = ruta.SupervisorId,
                Nombre = ruta.Nombre,
                Comentario = ruta.Comentario
            };
        }

        // =========================
        // Deshabilitar Ruta
        // =========================
        public async Task<bool> DeshabilitarRuta(int id)
        {
            var ruta = await _context.Ruta.FindAsync(id);
            if (ruta == null) throw new Exception("Ruta no encontrada");

            ruta.EstadoDel = false;
            await _context.SaveChangesAsync();
            return true;
        }

        // =========================
        // Obtener Rutas por Vendedor
        // =========================
        public async Task<List<RutaResponseDTO>> ObtenerRutasPorVendedor(int venId)
        {
            return await _context.Ruta
                .Where(r => r.EstadoDel && r.VendedorId == venId)

                .Select(r => new RutaResponseDTO
                {
                    Id = r.Id,
                    VenId = r.VendedorId,
                    SupId = r.SupervisorId,
                    Nombre = r.Nombre,
                    Comentario = r.Comentario
                })
                .ToListAsync();
        }

        // =========================
        // Obtener Rutas por Supervisor
        // =========================
        public async Task<List<RutaResponseDTO>> ObtenerRutasPorSupervisor(int supId)
        {
            return await _context.Ruta
                .Where(r => r.EstadoDel && r.SupervisorId == supId)

                .Select(r => new RutaResponseDTO
                {
                    Id = r.Id,
                    VenId = r.VendedorId,
                    SupId = r.SupervisorId,
                    Nombre = r.Nombre,
                    Comentario = r.Comentario
                })
                .ToListAsync();
        }
    }
}