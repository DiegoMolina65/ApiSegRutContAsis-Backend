using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.AsignacionClienteVendedor;
using SegRutContAsis.Business.DTO.Response.AsignacionClienteVendedor;
using SegRutContAsis.Business.Interfaces.AsignacionClienteVendedor;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services
{
    public class AsignacionClienteVendedorService : IAsignacionClienteVendedorService
    {
        private readonly SegRutContAsisContext _context;

        public AsignacionClienteVendedorService(SegRutContAsisContext context)
        {
            _context = context;
        }

        public async Task<AsignacionClienteVendedorResponseDTO> CrearAsignacion(AsignacionClienteVendedorRequestDTO dto)
        {
            var asignacion = new AsignacionClienteVendedor
            {
                SupId = dto.SupId,
                VenId = dto.VenId,
                ClId = dto.ClId,
                FechaCreacion = DateTime.Now,
                EstadoDel = true
            };

            _context.AsignacionClienteVendedor.Add(asignacion);
            await _context.SaveChangesAsync();

            var result = await _context.AsignacionClienteVendedor
                .Include(a => a.Supervisor)
                .ThenInclude(s => s.Usuario)
                .Include(a => a.Vendedor)
                .ThenInclude(v => v.Usuario)
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(a => a.Id == asignacion.Id);

            return new AsignacionClienteVendedorResponseDTO
            {
                Id = result.Id,
                SupId = result.SupId,
                SupervisorNombre = result.Supervisor?.Usuario?.NombreCompleto,
                VenId = result.VenId,
                VendedorNombre = result.Vendedor?.Usuario?.NombreCompleto,
                ClId = result.ClId,
                ClienteNombre = result.Cliente?.NombreCompleto,
                FechaCreacion = result.FechaCreacion,
                EstadoDel = result.EstadoDel
            };
        }

        public async Task<AsignacionClienteVendedorResponseDTO> ActualizarAsignacion(int id, AsignacionClienteVendedorRequestDTO dto)
        {
            var asignacion = await _context.AsignacionClienteVendedor
                .FirstOrDefaultAsync(a => a.Id == id && a.EstadoDel);

            if (asignacion == null)
                return null;

            asignacion.SupId = dto.SupId;
            asignacion.VenId = dto.VenId;
            asignacion.ClId = dto.ClId;

            _context.AsignacionClienteVendedor.Update(asignacion);
            await _context.SaveChangesAsync();

            var result = await _context.AsignacionClienteVendedor
                .Include(a => a.Supervisor)
                .ThenInclude(s => s.Usuario)
                .Include(a => a.Vendedor)
                .ThenInclude(v => v.Usuario)
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(a => a.Id == id);

            return new AsignacionClienteVendedorResponseDTO
            {
                Id = result.Id,
                SupId = result.SupId,
                SupervisorNombre = result.Supervisor?.Usuario?.NombreCompleto,
                VenId = result.VenId,
                VendedorNombre = result.Vendedor?.Usuario?.NombreCompleto,
                ClId = result.ClId,
                ClienteNombre = result.Cliente?.NombreCompleto,
                FechaCreacion = result.FechaCreacion,
                EstadoDel = result.EstadoDel
            };
        }

        public async Task<bool> DesactivarAsignacion(int id)
        {
            var asignacion = await _context.AsignacionClienteVendedor
                .FirstOrDefaultAsync(a => a.Id == id && a.EstadoDel);

            if (asignacion == null)
                return false;

            asignacion.EstadoDel = false;
            _context.AsignacionClienteVendedor.Update(asignacion);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<AsignacionClienteVendedorResponseDTO> ObtenerAsignacionPorId(int id)
        {
            var result = await _context.AsignacionClienteVendedor
                .Include(a => a.Supervisor)
                .ThenInclude(s => s.Usuario)
                .Include(a => a.Vendedor)
                .ThenInclude(v => v.Usuario)
                .Include(a => a.Cliente)
                .FirstOrDefaultAsync(a => a.Id == id && a.EstadoDel);

            if (result == null) return null;

            return new AsignacionClienteVendedorResponseDTO
            {
                Id = result.Id,
                SupId = result.SupId,
                SupervisorNombre = result.Supervisor?.Usuario?.NombreCompleto,
                VenId = result.VenId,
                VendedorNombre = result.Vendedor?.Usuario?.NombreCompleto,
                ClId = result.ClId,
                ClienteNombre = result.Cliente?.NombreCompleto,
                FechaCreacion = result.FechaCreacion,
                EstadoDel = result.EstadoDel
            };
        }

        public async Task<List<AsignacionClienteVendedorResponseDTO>> ObtenerAsignacionesPorVendedor(int venId)
        {
            var asignaciones = await _context.AsignacionClienteVendedor
                .Include(a => a.Supervisor)
                .ThenInclude(s => s.Usuario)
                .Include(a => a.Cliente)
                .Where(a => a.VenId == venId && a.EstadoDel)
                .ToListAsync();

            return asignaciones.Select(result => new AsignacionClienteVendedorResponseDTO
            {
                Id = result.Id,
                SupId = result.SupId,
                SupervisorNombre = result.Supervisor?.Usuario?.NombreCompleto,
                VenId = result.VenId,
                ClId = result.ClId,
                ClienteNombre = result.Cliente?.NombreCompleto,
                FechaCreacion = result.FechaCreacion,
                EstadoDel = result.EstadoDel
            }).ToList();
        }

        public async Task<List<AsignacionClienteVendedorResponseDTO>> ObtenerTodasAsignaciones()
        {
            var asignaciones = await _context.AsignacionClienteVendedor
                .Include(a => a.Supervisor)
                .ThenInclude(s => s.Usuario)
                .Include(a => a.Vendedor)
                .ThenInclude(v => v.Usuario)
                .Include(a => a.Cliente)
                .Where(a => a.EstadoDel)
                .ToListAsync();

            return asignaciones.Select(result => new AsignacionClienteVendedorResponseDTO
            {
                Id = result.Id,
                SupId = result.SupId,
                SupervisorNombre = result.Supervisor?.Usuario?.NombreCompleto,
                VenId = result.VenId,
                VendedorNombre = result.Vendedor?.Usuario?.NombreCompleto,
                ClId = result.ClId,
                ClienteNombre = result.Cliente?.NombreCompleto,
                FechaCreacion = result.FechaCreacion,
                EstadoDel = result.EstadoDel
            }).ToList();
        }
    }
}
