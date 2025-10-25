using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.Asistencia;
using SegRutContAsis.Business.DTO.Response.Asistencia;
using SegRutContAsis.Business.Interfaces.Asistencia;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly SegRutContAsisContext _context;

        public AsistenciaService(SegRutContAsisContext context)
        {
            _context = context;
        }


        // =========================
        // RegistrarEntrada
        // =========================
        public async Task<AsistenciaResponseDTO> RegistrarEntrada(AsistenciaRequestDTO dto)
        {
            var hoy = DateTime.Now.Date;
            var existe = await _context.Asistencia
                .FirstOrDefaultAsync(a => a.VenId == dto.VenId && a.HoraEntrada.HasValue && a.HoraEntrada.Value.Date == hoy);

            if (existe != null)
                throw new Exception("El vendedor ya marcó entrada hoy.");

            var asistencia = new Asistencia
            {
                VenId = dto.VenId,
                HoraEntrada = DateTime.Now,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud
            };

            _context.Asistencia.Add(asistencia);
            await _context.SaveChangesAsync();

            return new AsistenciaResponseDTO
            {
                Id = asistencia.Id,
                VenId = asistencia.VenId,
                HoraEntrada = asistencia.HoraEntrada,
                Latitud = asistencia.Latitud,
                Longitud = asistencia.Longitud
            };
        }


        // =========================
        // Registrar Salida
        // =========================
        public async Task<AsistenciaResponseDTO> RegistrarSalida(int venId)
        {
            var hoy = DateTime.Now.Date;
            var asistencia = await _context.Asistencia
                .FirstOrDefaultAsync(a => a.VenId == venId && a.HoraEntrada.HasValue && a.HoraEntrada.Value.Date == hoy);

            if (asistencia == null)
                throw new Exception("El vendedor no marcó entrada hoy.");

            if (asistencia.HoraSalida != null)
                throw new Exception("El vendedor ya marcó salida hoy.");

            asistencia.HoraSalida = DateTime.Now;
            _context.Asistencia.Update(asistencia);
            await _context.SaveChangesAsync();

            return new AsistenciaResponseDTO
            {
                Id = asistencia.Id,
                VenId = asistencia.VenId,
                HoraEntrada = asistencia.HoraEntrada,
                HoraSalida = asistencia.HoraSalida,
                Latitud = asistencia.Latitud,
                Longitud = asistencia.Longitud
            };
        }
    }
}
