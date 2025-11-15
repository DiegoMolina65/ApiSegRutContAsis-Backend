using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.SeguimientoVendedor;
using SegRutContAsis.Business.DTO.Response.SeguimientoVendedor;
using SegRutContAsis.Business.DTO.Response.Usuario;
using SegRutContAsis.Business.Interfaces.SeguimientoVendedor;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services
{
    public class SeguimientoVendedorService : ISeguimientoVendedorService
    {
        public readonly SegRutContAsisContext _context;
        public SeguimientoVendedorService(SegRutContAsisContext context)
        {
            _context = context;
        }

        // Crear Seguimiento vendedor
        public async Task<SeguimientoVendedorResponseDTO> CrearSeguimientoVendedor(SeguimientoVendedorRequestDTO dto)
        {
            var vendedor = await _context.Vendedor.FindAsync(dto.venId);
            if (vendedor == null)
                throw new Exception("Vendedor no encontrado");

            var nuevo = new SeguimientoVendedor
            {
                venId = dto.venId,
                segLatitud = dto.segLatitud,
                segLongitud = dto.segLongitud,
                segFechaCreacion = DateTime.UtcNow
            };

            _context.SeguimientoVendedor.Add(nuevo);
            await _context.SaveChangesAsync();

            return new SeguimientoVendedorResponseDTO
            {
                segId = nuevo.segId,
                venId = nuevo.venId,
                segFechaCreacion = nuevo.segFechaCreacion,
                segLatitud = nuevo.segLatitud,
                segLongitud = nuevo.segLongitud
            };
        }

        // Obtener todos Seguimiento vendedor SOLO DEL DÍA ACTUAL
        public async Task<List<SeguimientoVendedorResponseDTO>> ObtenerTodosSeguimientosVendedores(UsuarioReponseDTO usuarioActual)
        {
            var hoy = DateTime.Today;
            var inicioDia = hoy;
            var finDia = hoy.AddDays(1);

            // ADMINISTRADOR: puede ver todos los seguimientos del día
            if (usuarioActual.EsAdministrador)
            {
                var lista = await _context.SeguimientoVendedor
                    .Include(s => s.Vendedor)
                        .ThenInclude(v => v.Usuario)
                    .Where(s =>
                        s.Vendedor.venEstadoDel &&
                        s.segFechaCreacion >= inicioDia &&
                        s.segFechaCreacion < finDia
                    )
                    .OrderByDescending(s => s.segFechaCreacion)
                    .ToListAsync();

                return lista.Select(s => new SeguimientoVendedorResponseDTO
                {
                    segId = s.segId,
                    venId = s.venId,
                    segFechaCreacion = s.segFechaCreacion,
                    segLatitud = s.segLatitud,
                    segLongitud = s.segLongitud,
                    VendedorNombre = s.Vendedor.Usuario.usrNombreCompleto
                }).ToList();
            }

            // SUPERVISOR: ver solo seguimientos de sus vendedores del día
            if (usuarioActual.EsSupervisor)
            {
                var supervisor = await _context.Supervisor
                    .FirstOrDefaultAsync(s => s.usrId == usuarioActual.usrId && s.supEstadoDel);

                if (supervisor == null)
                    return new List<SeguimientoVendedorResponseDTO>();

                int supId = supervisor.supId;

                var vendedoresIds = await _context.AsignacionSupervisorVendedor
                    .Where(a => a.supId == supId && a.asvEstadoDel)
                    .Select(a => a.venId)
                    .ToListAsync();

                var lista = await _context.SeguimientoVendedor
                    .Include(s => s.Vendedor)
                        .ThenInclude(v => v.Usuario)
                    .Where(s =>
                        vendedoresIds.Contains(s.venId) &&
                        s.Vendedor.venEstadoDel &&
                        s.segFechaCreacion >= inicioDia &&
                        s.segFechaCreacion < finDia
                    )
                    .OrderByDescending(s => s.segFechaCreacion)
                    .ToListAsync();

                return lista.Select(s => new SeguimientoVendedorResponseDTO
                {
                    segId = s.segId,
                    venId = s.venId,
                    segFechaCreacion = s.segFechaCreacion,
                    segLatitud = s.segLatitud,
                    segLongitud = s.segLongitud,
                    VendedorNombre = s.Vendedor.Usuario.usrNombreCompleto
                }).ToList();
            }

            // VENDEDOR: solo su propio seguimiento del día
            var listaVendedor = await _context.SeguimientoVendedor
                .Include(s => s.Vendedor)
                    .ThenInclude(v => v.Usuario)
                .Where(s =>
                    s.venId == usuarioActual.VendedorId &&
                    s.segFechaCreacion >= inicioDia &&
                    s.segFechaCreacion < finDia
                )
                .OrderByDescending(s => s.segFechaCreacion)
                .ToListAsync();

            return listaVendedor.Select(s => new SeguimientoVendedorResponseDTO
            {
                segId = s.segId,
                venId = s.venId,
                segFechaCreacion = s.segFechaCreacion,
                segLatitud = s.segLatitud,
                segLongitud = s.segLongitud,
                VendedorNombre = s.Vendedor.Usuario.usrNombreCompleto
            }).ToList();
        }



        // Obtener seguimiento de un vendedor
        public async Task<List<SeguimientoVendedorResponseDTO>> ObtenerSeguimientosDeUnVendedor(int venId)
        {
            var lista = await _context.SeguimientoVendedor
                .Where(s => s.venId == venId)
                .OrderByDescending(s => s.segFechaCreacion)
                .ToListAsync();

            return lista.Select(s => new SeguimientoVendedorResponseDTO
            {
                segId = s.segId,
                venId = s.venId,
                segFechaCreacion = s.segFechaCreacion,
                segLatitud = s.segLatitud,
                segLongitud = s.segLongitud
            }).ToList();
        }
    }
}

