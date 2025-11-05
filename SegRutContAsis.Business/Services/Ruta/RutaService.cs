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

        // Crear Ruta
        public async Task<RutaResponseDTO> CrearRuta(RutaRequestDTO dto)
        {
            var ruta = new Ruta
            {
                VendedorId = dto.venId,
                SupervisorId = dto.supId,
                rutNombre = dto.rutNombre,
                rutComentario = dto.rutComentario,
                rutEstadoDel = true
            };

            _context.Ruta.Add(ruta);
            await _context.SaveChangesAsync();

            return new RutaResponseDTO
            {
                rutId = ruta.rutId,
                venId = ruta.VendedorId,
                supId = ruta.SupervisorId,
                rutNombre = ruta.rutNombre,
                rutComentario = ruta.rutComentario
            };
        }

        // Obtener Rutas
        public async Task<List<RutaResponseDTO>> ObtenerRutas()
        {
            return await _context.Ruta
                .Where(r => r.rutEstadoDel)
                .Include(r => r.Vendedor)
                .Select(r => new RutaResponseDTO
                {
                    rutId = r.rutId,
                    venId = r.VendedorId,
                    supId = r.SupervisorId,
                    rutNombre = r.rutNombre,
                    rutComentario = r.rutComentario,
                    NombreVendedor = r.Vendedor != null ? r.Vendedor.Usuario.usrNombreCompleto : null
                })
                .ToListAsync();
        }

        // Obtener Ruta por Id
        public async Task<RutaResponseDTO> ObtenerRutaId(int id)
        {
            var rutaDTO = await _context.Ruta
                .Where(r => r.rutEstadoDel && r.rutId == id)
                .Select(r => new RutaResponseDTO
                {
                    rutId = r.rutId,
                    venId = r.VendedorId,
                    supId = r.SupervisorId,
                    rutNombre = r.rutNombre,
                    rutComentario = r.rutComentario,
                    NombreVendedor = r.Vendedor != null ? r.Vendedor.Usuario.usrNombreCompleto : null
                })
                .FirstOrDefaultAsync();

            if (rutaDTO == null)
                throw new Exception("Ruta no encontrada");

            return rutaDTO;
        }

        // Actualizar Ruta
        public async Task<RutaResponseDTO> ActualizarRuta(int id, RutaRequestDTO dto)
        {
            var ruta = await _context.Ruta.FindAsync(id);
            if (ruta == null) throw new Exception("Ruta no encontrada");

            ruta.VendedorId = dto.venId;
            ruta.SupervisorId = dto.supId;
            ruta.rutNombre = dto.rutNombre;
            ruta.rutComentario = dto.rutComentario;

            await _context.SaveChangesAsync();

            return new RutaResponseDTO
            {
                rutId = ruta.rutId,
                venId = ruta.VendedorId,
                supId = ruta.SupervisorId,
                rutNombre = ruta.rutNombre,
                rutComentario = ruta.rutComentario
            };
        }

        // Deshabilitar Ruta
        public async Task<bool> DeshabilitarRuta(int id)
        {
            var ruta = await _context.Ruta.FindAsync(id);
            if (ruta == null) throw new Exception("Ruta no encontrada");

            ruta.rutEstadoDel = false;
            await _context.SaveChangesAsync();
            return true;
        }

        // Obtener Rutas por Vendedor
        public async Task<List<RutaResponseDTO>> ObtenerRutasPorVendedor(int venId)
        {
            return await _context.Ruta
                .Where(r => r.rutEstadoDel && r.VendedorId == venId)

                .Select(r => new RutaResponseDTO
                {
                    rutId = r.rutId,
                    venId = r.VendedorId,
                    supId = r.SupervisorId,
                    rutNombre = r.rutNombre,
                    rutComentario = r.rutComentario,
                    NombreVendedor = r.Vendedor != null ? r.Vendedor.Usuario.usrNombreCompleto : null
                })
                .ToListAsync();
        }

        // Obtener Rutas por Supervisor
        public async Task<List<RutaResponseDTO>> ObtenerRutasPorSupervisor(int supId)
        {
            return await _context.Ruta
                .Where(r => r.rutEstadoDel && r.SupervisorId == supId)

                .Select(r => new RutaResponseDTO
                {
                    rutId = r.rutId,
                    venId = r.VendedorId,
                    supId = r.SupervisorId,
                    rutNombre = r.rutNombre,
                    rutComentario = r.rutComentario,
                    NombreVendedor = r.Vendedor != null ? r.Vendedor.Usuario.usrNombreCompleto : null
                })
                .ToListAsync();
        }
    }
}