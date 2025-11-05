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

        // Obtener todos Seguimiento vendedor
        public async Task<List<SeguimientoVendedorResponseDTO>> ObtenerTodosSeguimientosVendedores()
        {
            var lista = await _context.SeguimientoVendedor
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

