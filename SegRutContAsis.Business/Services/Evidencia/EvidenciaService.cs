using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EvidenciaService(SegRutContAsisContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Crear evidencia
        public async Task<EvidenciaResponseDTO> CrearEvidencia(EvidenciaRequestDTO requestDTO)
        {
            var existe = await _context.Evidencia
                .Include(e => e.Visita)
                .AnyAsync(e => e.visId == requestDTO.VisitaId &&
                               e.eviTipo == requestDTO.eviTipo &&
                               e.Visita.Ruta.rutEstadoDel);

            if (existe)
                throw new Exception("Ya existe una evidencia del mismo tipo para esta visita");

            var evidencia = new Evidencia
            {
                visId = requestDTO.VisitaId,
                eviTipo = requestDTO.eviTipo,
                eviObservaciones = requestDTO.eviObservaciones
            };

            if (requestDTO.Archivo != null && requestDTO.Archivo.Length > 0)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Evidencias");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var extension = Path.GetExtension(requestDTO.Archivo.FileName);
                var nombreArchivo = $"evi_{Guid.NewGuid()}{extension}";

                var rutaFisica = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await requestDTO.Archivo.CopyToAsync(stream);
                }
                evidencia.EviArchivoPath = $"Uploads/Evidencias/{nombreArchivo}";
            }
            _context.Evidencia.Add(evidencia);
            await _context.SaveChangesAsync();
            return new EvidenciaResponseDTO
            {
                eviId = evidencia.eviId,
                eviFechaCreacion = evidencia.eviFechaCreacion,
                VisitaId = evidencia.visId,
                eviTipo = evidencia.eviTipo,
                eviObservaciones = evidencia.eviObservaciones,
                eviArchivoUrl = evidencia.EviArchivoPath
            };
        }


        // Obtener evidencia
        public async Task<List<EvidenciaResponseDTO>> ObtenerEvidencia()
        {
            string baseUrl = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            return await _context.Evidencia
                .Include(e => e.Visita)
                .Where(e => e.Visita.Ruta.rutEstadoDel)
                .Select(e => new EvidenciaResponseDTO
                {
                    eviId = e.eviId,
                    eviFechaCreacion = e.eviFechaCreacion,
                    VisitaId = e.visId,
                    eviTipo = e.eviTipo,
                    eviObservaciones = e.eviObservaciones,
                    eviArchivoUrl = string.IsNullOrEmpty(e.EviArchivoPath) ? null : $"{baseUrl}/Uploads/{Path.GetFileName(e.EviArchivoPath)}"
                })
                .ToListAsync();
        }



        // Obtener evidencia por ID
        public async Task<EvidenciaResponseDTO> ObtenerEvidenciaId(int id)
        {
            var e = await _context.Evidencia
                .Include(ev => ev.Visita)
                .Where(ev => ev.eviId == id && ev.Visita.Ruta.rutEstadoDel)
                .FirstOrDefaultAsync();

            if (e == null) throw new Exception("Evidencia no encontrada");

            return new EvidenciaResponseDTO
            {
                eviId = e.eviId,
                eviFechaCreacion = e.eviFechaCreacion,
                VisitaId = e.visId,
                eviTipo = e.eviTipo,
                eviObservaciones = e.eviObservaciones
            };
        }

        // Actualizar evidencia
        public async Task<EvidenciaResponseDTO> ActualizarEvidencia(int id, EvidenciaRequestDTO requestDTO)
        {
            var e = await _context.Evidencia.Include(ev => ev.Visita).FirstOrDefaultAsync(ev => ev.eviId == id);

            if (e == null || !e.Visita.Ruta.rutEstadoDel)
                throw new Exception("Evidencia no encontrada");

            var existe = await _context.Evidencia
                .Include(ev => ev.Visita)
                .AnyAsync(ev => ev.visId == requestDTO.VisitaId &&
                                ev.eviTipo == requestDTO.eviTipo &&
                                ev.eviId != id &&
                                ev.Visita.Ruta.rutEstadoDel);

            if (existe)
                throw new Exception("Ya existe una evidencia del mismo tipo para esta visita");

            e.visId = requestDTO.VisitaId;
            e.eviTipo = requestDTO.eviTipo;
            e.eviObservaciones = requestDTO.eviObservaciones;

            await _context.SaveChangesAsync();

            return new EvidenciaResponseDTO
            {
                eviId = e.eviId,
                eviFechaCreacion = e.eviFechaCreacion,
                VisitaId = e.visId,
                eviTipo = e.eviTipo,
                eviObservaciones = e.eviObservaciones
            };
        }

        // Filtros de evidencia
        public async Task<List<EvidenciaResponseDTO>> ObtenerEvidenciaPorVisita(int visitaId)
        {
            return await _context.Evidencia
                .Where(e => e.visId == visitaId && e.Visita.Ruta.rutEstadoDel)
                .Select(e => new EvidenciaResponseDTO
                {
                    eviId = e.eviId,
                    eviFechaCreacion = e.eviFechaCreacion,
                    VisitaId = e.visId,
                    eviTipo = e.eviTipo,
                    eviObservaciones = e.eviObservaciones
                }).ToListAsync();
        }

        public async Task<List<EvidenciaResponseDTO>> ObtenerEvidenciaPorVendedor(int venId)
        {
            return await _context.Evidencia
                .Include(e => e.Visita)
                .ThenInclude(v => v.Ruta)
                .Where(e => e.Visita.Ruta.VendedorId == venId && e.Visita.Ruta.rutEstadoDel)
                .Select(e => new EvidenciaResponseDTO
                {
                    eviId = e.eviId,
                    eviFechaCreacion = e.eviFechaCreacion,
                    VisitaId = e.visId,
                    eviTipo = e.eviTipo,
                    eviObservaciones = e.eviObservaciones
                }).ToListAsync();
        }

        public async Task<List<EvidenciaResponseDTO>> ObtenerEvidenciaPorTipo(string tipo)
        {
            return await _context.Evidencia
                .Include(e => e.Visita)
                .Where(e => e.eviTipo == tipo && e.Visita.Ruta.rutEstadoDel)
                .Select(e => new EvidenciaResponseDTO
                {
                    eviId = e.eviId,
                    eviFechaCreacion = e.eviFechaCreacion,
                    VisitaId = e.visId,
                    eviTipo = e.eviTipo,
                    eviObservaciones = e.eviObservaciones
                }).ToListAsync();
        }
    }

}
