using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.Asistencia;
using SegRutContAsis.Business.DTO.Response.Asistencia;
using SegRutContAsis.Business.DTO.Response.Usuario;
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


        // Registrar Entrada
        public async Task<AsistenciaResponseDTO> RegistrarEntrada(AsistenciaRequestDTO dto)
        {
            var hoy = DateTime.Now.Date;
            var existe = await _context.Asistencia
                .FirstOrDefaultAsync(a => a.venId == dto.venId && a.asiHoraEntrada.HasValue && a.asiHoraEntrada.Value.Date == hoy);

            if (existe != null)
                throw new Exception("El vendedor ya marcó entrada hoy.");

            var asistencia = new Asistencia
            {
                venId = dto.venId,
                asiHoraEntrada = DateTime.Now,
                asiLatitud = dto.asiLatitud,
                asiLongitud = dto.asiLongitud
            };

            _context.Asistencia.Add(asistencia);
            await _context.SaveChangesAsync();

            return new AsistenciaResponseDTO
            {
                asiId = asistencia.asiId,
                venId = asistencia.venId,
                asiHoraEntrada = asistencia.asiHoraEntrada,
                asiLatitud = asistencia.asiLatitud,
                asiLongitud = asistencia.asiLongitud
            };
        }


        // Registrar Salida
        public async Task<AsistenciaResponseDTO> RegistrarSalida(int venId)
        {
            var hoy = DateTime.Now.Date;
            var asistencia = await _context.Asistencia
                .FirstOrDefaultAsync(a => a.venId == venId && a.asiHoraEntrada.HasValue && a.asiHoraEntrada.Value.Date == hoy);

            if (asistencia == null)
                throw new Exception("El vendedor no marcó entrada hoy.");

            if (asistencia.asiHoraSalida != null)
                throw new Exception("El vendedor ya marcó salida hoy.");

            asistencia.asiHoraSalida = DateTime.Now;
            _context.Asistencia.Update(asistencia);
            await _context.SaveChangesAsync();

            return new AsistenciaResponseDTO
            {
                asiId = asistencia.asiId,
                venId = asistencia.venId,
                asiHoraEntrada = asistencia.asiHoraEntrada,
                asiHoraSalida = asistencia.asiHoraSalida,
                asiLatitud = asistencia.asiLatitud,
                asiLongitud = asistencia.asiLongitud
            };
        }

        public async Task<List<AsistenciaResponseDTO>> ObtenerAsistencias(UsuarioReponseDTO usuarioActual)
        {
            //  ADMINISTRADOR 
            if (usuarioActual.EsAdministrador)
            {
                return await _context.Asistencia
                    .Include(a => a.Vendedor).ThenInclude(v => v.Usuario)
                    .Select(a => new AsistenciaResponseDTO
                    {
                        asiId = a.asiId,
                        venId = a.venId,
                        asiHoraEntrada = a.asiHoraEntrada,
                        asiHoraSalida = a.asiHoraSalida,
                        asiLatitud = a.asiLatitud,
                        asiLongitud = a.asiLongitud,
                        nombreVendedor = a.Vendedor.Usuario.usrNombreCompleto
                    })
                    .ToListAsync();
            }


            //  SUPERVISOR 
            if (usuarioActual.EsSupervisor)
            {
                // Obtener supervisor
                var supervisor = await _context.Supervisor
                    .FirstOrDefaultAsync(s => s.usrId == usuarioActual.usrId && s.supEstadoDel);

                if (supervisor == null)
                    return new List<AsistenciaResponseDTO>();

                int supId = supervisor.supId;

                // Vendedores asignados
                var vendedoresIds = await _context.AsignacionSupervisorVendedor
                    .Where(a => a.supId == supId && a.asvEstadoDel)
                    .Select(a => a.venId)
                    .ToListAsync();

                if (!vendedoresIds.Any())
                    return new List<AsistenciaResponseDTO>();

                return await _context.Asistencia
                    .Include(a => a.Vendedor).ThenInclude(v => v.Usuario)
                    .Where(a => vendedoresIds.Contains(a.venId))
                    .Select(a => new AsistenciaResponseDTO
                    {
                        asiId = a.asiId,
                        venId = a.venId,
                        asiHoraEntrada = a.asiHoraEntrada,
                        asiHoraSalida = a.asiHoraSalida,
                        asiLatitud = a.asiLatitud,
                        asiLongitud = a.asiLongitud,
                        nombreVendedor = a.Vendedor.Usuario.usrNombreCompleto
                    })
                    .ToListAsync();
            }


            //  VENDEDOR 
            if (usuarioActual.EsVendedor)
            {
                var vendedor = await _context.Vendedor
                    .FirstOrDefaultAsync(v => v.usrId == usuarioActual.usrId && v.venEstadoDel);

                if (vendedor == null)
                    return new List<AsistenciaResponseDTO>();

                int venId = vendedor.venId;

                return await _context.Asistencia
                    .Include(a => a.Vendedor).ThenInclude(v => v.Usuario)
                    .Where(a => a.venId == venId)
                    .Select(a => new AsistenciaResponseDTO
                    {
                        asiId = a.asiId,
                        venId = a.venId,
                        asiHoraEntrada = a.asiHoraEntrada,
                        asiHoraSalida = a.asiHoraSalida,
                        asiLatitud = a.asiLatitud,
                        asiLongitud = a.asiLongitud,
                        nombreVendedor = a.Vendedor.Usuario.usrNombreCompleto
                    })
                    .ToListAsync();
            }

            return new List<AsistenciaResponseDTO>();
        }



    }
}
