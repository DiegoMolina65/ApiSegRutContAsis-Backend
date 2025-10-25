using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.Evidencia;
using SegRutContAsis.Business.DTO.Response.Evidencia;
using SegRutContAsis.Business.Interfaces.Evidencia;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services
{
    public class EvidenciaService : IEvidenciaService
    {
        private readonly SegRutContAsisContext _context;

        public EvidenciaService(SegRutContAsisContext context)
        {
            _context = context;
        }

        // =========================
        // Crear evidencia
        // =========================
        public async Task<EvidenciaResponseDTO> CrearEvidencia(EvidenciaRequestDTO requestDTO)
        {
            var existe = await _context.Evidencia
                .Include(e => e.Visita)
                .AnyAsync(e => e.visId == requestDTO.VisitaId &&
                               e.Tipo == requestDTO.Tipo &&
                               e.Visita.Ruta.EstadoDel);

            if (existe)
                throw new Exception("Ya existe una evidencia del mismo tipo para esta visita");

            var evidencia = new Evidencia
            {
                visId = requestDTO.VisitaId,
                Tipo = requestDTO.Tipo,
                Observaciones = requestDTO.Observaciones
            };

            _context.Evidencia.Add(evidencia);
            await _context.SaveChangesAsync();

            return new EvidenciaResponseDTO
            {
                Id = evidencia.Id,
                FechaCreacion = evidencia.FechaCreacion,
                VisitaId = evidencia.visId,
                Tipo = evidencia.Tipo,
                Observaciones = evidencia.Observaciones
            };
        }


        // =========================
        // Obtener evidencia
        // =========================
        public async Task<List<EvidenciaResponseDTO>> ObtenerEvidencia()
        {
            return await _context.Evidencia
                .Include(e => e.Visita)
                .Where(e => e.Visita.Ruta.EstadoDel)
                .Select(e => new EvidenciaResponseDTO
                {
                    Id = e.Id,
                    FechaCreacion = e.FechaCreacion,
                    VisitaId = e.visId,
                    Tipo = e.Tipo,
                    Observaciones = e.Observaciones
                }).ToListAsync();
        }


        // =========================
        // Obtener evidencia por ID
        // =========================
        public async Task<EvidenciaResponseDTO> ObtenerEvidenciaId(int id)
        {
            var e = await _context.Evidencia
                .Include(ev => ev.Visita)
                .Where(ev => ev.Id == id && ev.Visita.Ruta.EstadoDel)
                .FirstOrDefaultAsync();

            if (e == null) throw new Exception("Evidencia no encontrada");

            return new EvidenciaResponseDTO
            {
                Id = e.Id,
                FechaCreacion = e.FechaCreacion,
                VisitaId = e.visId,
                Tipo = e.Tipo,
                Observaciones = e.Observaciones
            };
        }


        // =========================
        // Actualizar evidencia
        // =========================
        public async Task<EvidenciaResponseDTO> ActualizarEvidencia(int id, EvidenciaRequestDTO requestDTO)
        {
            var e = await _context.Evidencia.Include(ev => ev.Visita).FirstOrDefaultAsync(ev => ev.Id == id);

            if (e == null || !e.Visita.Ruta.EstadoDel)
                throw new Exception("Evidencia no encontrada");

            var existe = await _context.Evidencia
                .Include(ev => ev.Visita)
                .AnyAsync(ev => ev.visId == requestDTO.VisitaId &&
                                ev.Tipo == requestDTO.Tipo &&
                                ev.Id != id &&
                                ev.Visita.Ruta.EstadoDel);

            if (existe)
                throw new Exception("Ya existe una evidencia del mismo tipo para esta visita");

            e.visId = requestDTO.VisitaId;
            e.Tipo = requestDTO.Tipo;
            e.Observaciones = requestDTO.Observaciones;

            await _context.SaveChangesAsync();

            return new EvidenciaResponseDTO
            {
                Id = e.Id,
                FechaCreacion = e.FechaCreacion,
                VisitaId = e.visId,
                Tipo = e.Tipo,
                Observaciones = e.Observaciones
            };
        }

        // =========================
        // Filtros de evidencia
        // =========================
        public async Task<List<EvidenciaResponseDTO>> ObtenerEvidenciaPorVisita(int visitaId)
        {
            return await _context.Evidencia
                .Where(e => e.visId == visitaId && e.Visita.Ruta.EstadoDel)
                .Select(e => new EvidenciaResponseDTO
                {
                    Id = e.Id,
                    FechaCreacion = e.FechaCreacion,
                    VisitaId = e.visId,
                    Tipo = e.Tipo,
                    Observaciones = e.Observaciones
                }).ToListAsync();
        }

        public async Task<List<EvidenciaResponseDTO>> ObtenerEvidenciaPorVendedor(int venId)
        {
            return await _context.Evidencia
                .Include(e => e.Visita)
                .ThenInclude(v => v.Ruta)
                .Where(e => e.Visita.Ruta.VendedorId == venId && e.Visita.Ruta.EstadoDel)
                .Select(e => new EvidenciaResponseDTO
                {
                    Id = e.Id,
                    FechaCreacion = e.FechaCreacion,
                    VisitaId = e.visId,
                    Tipo = e.Tipo,
                    Observaciones = e.Observaciones
                }).ToListAsync();
        }

        public async Task<List<EvidenciaResponseDTO>> ObtenerEvidenciaPorTipo(string tipo)
        {
            return await _context.Evidencia
                .Include(e => e.Visita)
                .Where(e => e.Tipo == tipo && e.Visita.Ruta.EstadoDel)
                .Select(e => new EvidenciaResponseDTO
                {
                    Id = e.Id,
                    FechaCreacion = e.FechaCreacion,
                    VisitaId = e.visId,
                    Tipo = e.Tipo,
                    Observaciones = e.Observaciones
                }).ToListAsync();
        }
    }

}
