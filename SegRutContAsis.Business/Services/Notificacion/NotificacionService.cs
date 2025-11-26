using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Response.Notificacion;
using SegRutContAsis.Business.DTO.Response.Usuario;
using SegRutContAsis.Business.Interfaces.Notificacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services.Notificacion
{
    public class NotificacionService : INotificacionService
    {
        private readonly SegRutContAsisContext _context;
        public NotificacionService(SegRutContAsisContext context)
        {
            _context = context;
        }

        public async Task<List<NotificacionResponseDTO>> obtenerNotificaciones(UsuarioReponseDTO usuarioActual)
        {
            // ADMIN: todas las notificaciones
            if (usuarioActual.EsAdministrador)
            {
                return await _context.Notificacion
                    .Where(n => n.notEstadoDel)
                    .Include(n => n.Vendedor).ThenInclude(v => v.Usuario)
                    .Include(n => n.Supervisor).ThenInclude(s => s.Usuario)
                    .Select(n => new NotificacionResponseDTO
                    {
                        notId = n.notId,
                        notFechaCreacion = n.notFechaCreacion,
                        notTitulo = n.notTitulo,
                        notMensaje = n.notMensaje,
                        notTipo = n.notTipo,
                        entidadId = n.entidadId,
                        venId = n.venId,
                        supId = n.supId,
                        nombreVendedor = n.Vendedor != null && n.Vendedor.Usuario != null ? n.Vendedor.Usuario.usrNombreCompleto : null,
                        nombreSupervisor = n.Supervisor != null && n.Supervisor.Usuario != null ? n.Supervisor.Usuario.usrNombreCompleto : null
                    })
                    .OrderByDescending(n => n.notFechaCreacion)
                    .ToListAsync();
            }

            // SUPERVISOR: solo notificaciones de sus vendedores
            if (usuarioActual.EsSupervisor)
            {
                var supervisor = await _context.Supervisor
                    .FirstOrDefaultAsync(s => s.usrId == usuarioActual.usrId && s.supEstadoDel);

                if (supervisor == null)
                    return new List<NotificacionResponseDTO>();

                int supId = supervisor.supId;

                var vendedoresIds = await _context.AsignacionSupervisorVendedor
                    .Where(a => a.supId == supId && a.asvEstadoDel)
                    .Select(a => a.venId)
                    .ToListAsync();

                if (!vendedoresIds.Any())
                    return new List<NotificacionResponseDTO>();

                return await _context.Notificacion
                    .Where(n => n.notEstadoDel && n.venId != null && vendedoresIds.Contains(n.venId.Value))
                    .Include(n => n.Vendedor).ThenInclude(v => v.Usuario)
                    .Include(n => n.Supervisor).ThenInclude(s => s.Usuario)
                    .Select(n => new NotificacionResponseDTO
                    {
                        notId = n.notId,
                        notFechaCreacion = n.notFechaCreacion,
                        notTitulo = n.notTitulo,
                        notMensaje = n.notMensaje,
                        notTipo = n.notTipo,
                        entidadId = n.entidadId,
                        venId = n.venId,
                        supId = n.supId,
                        nombreVendedor = n.Vendedor != null && n.Vendedor.Usuario != null ? n.Vendedor.Usuario.usrNombreCompleto : null,
                        nombreSupervisor = n.Supervisor != null && n.Supervisor.Usuario != null ? n.Supervisor.Usuario.usrNombreCompleto : null
                    })
                    .OrderByDescending(n => n.notFechaCreacion)
                    .ToListAsync();
            }

            // VENDEDOR: solo sus propias notificaciones
            if (usuarioActual.EsVendedor)
            {
                var vendedor = await _context.Vendedor
                    .FirstOrDefaultAsync(v => v.usrId == usuarioActual.usrId && v.venEstadoDel);

                if (vendedor == null)
                    return new List<NotificacionResponseDTO>();

                int venId = vendedor.venId;

                return await _context.Notificacion
                    .Where(n => n.notEstadoDel && n.venId == venId)
                    .Include(n => n.Vendedor).ThenInclude(v => v.Usuario)
                    .Include(n => n.Supervisor).ThenInclude(s => s.Usuario)
                    .Select(n => new NotificacionResponseDTO
                    {
                        notId = n.notId,
                        notFechaCreacion = n.notFechaCreacion,
                        notTitulo = n.notTitulo,
                        notMensaje = n.notMensaje,
                        notTipo = n.notTipo,
                        entidadId = n.entidadId,
                        venId = n.venId,
                        supId = n.supId,
                        nombreVendedor = n.Vendedor != null && n.Vendedor.Usuario != null ? n.Vendedor.Usuario.usrNombreCompleto : null,
                        nombreSupervisor = n.Supervisor != null && n.Supervisor.Usuario != null ? n.Supervisor.Usuario.usrNombreCompleto : null
                    })
                    .OrderByDescending(n => n.notFechaCreacion)
                    .ToListAsync();
            }


            return new List<NotificacionResponseDTO>();
        }

    }
}
