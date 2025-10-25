using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.SeguimientoVendedor;
using SegRutContAsis.Business.DTO.Response.SeguimientoVendedor;
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

        public async Task<SeguimientoVendedorResponseDTO> CrearSeguimientoVendedor(SeguimientoVendedorRequestDTO dto)
        {
            var vendedor = await _context.Vendedor.FindAsync(dto.venId);
            if (vendedor == null)
                throw new Exception("Vendedor no encontrado");

            var nuevo = new SeguimientoVendedor
            {
                venId = dto.venId,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                FechaCreacion = DateTime.UtcNow
            };

            _context.SeguimientoVendedor.Add(nuevo);
            await _context.SaveChangesAsync();

            return new SeguimientoVendedorResponseDTO
            {
                segId = nuevo.Id,
                venId = nuevo.venId,
                FechaCreacion = nuevo.FechaCreacion,
                Latitud = nuevo.Latitud,
                Longitud = nuevo.Longitud
            };
        }

        public async Task<List<SeguimientoVendedorResponseDTO>> ObtenerTodosSeguimientosVendedores()
        {
            var lista = await _context.SeguimientoVendedor
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();

            return lista.Select(s => new SeguimientoVendedorResponseDTO
            {
                segId = s.Id,
                venId = s.venId,
                FechaCreacion = s.FechaCreacion,
                Latitud = s.Latitud,
                Longitud = s.Longitud
            }).ToList();
        }

        public async Task<List<SeguimientoVendedorResponseDTO>> ObtenerSeguimientosDeUnVendedor(int venId)
        {
            var lista = await _context.SeguimientoVendedor
                .Where(s => s.venId == venId)
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();

            return lista.Select(s => new SeguimientoVendedorResponseDTO
            {
                segId = s.Id,
                venId = s.venId,
                FechaCreacion = s.FechaCreacion,
                Latitud = s.Latitud,
                Longitud = s.Longitud
            }).ToList();
        }
    }
}

