using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.DireccionCliente;
using SegRutContAsis.Business.DTO.Response.DireccionCliente;
using SegRutContAsis.Business.Interfaces.DireccionCliente;
using SegRutContAsis.Domain.Entities;

namespace SegRutContAsis.Business.Services
{
    public class DireccionClienteService : IDireccionClienteService
    {
        private readonly SegRutContAsisContext _context;

        public DireccionClienteService(SegRutContAsisContext context)
        {
            _context = context;
        }

        public async Task<DireccionClienteResponseDTO> CrearDireccion(DireccionClienteRequestDTO dto)
        {
            var direccion = new DireccionCliente
            {
                ClId = dto.ClId,
                ZonId = dto.ZonId,
                NombreSucursal = dto.NombreSucursal,
                Direccion = dto.Direccion,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                EstadoDel = true
            };

            _context.DireccionCliente.Add(direccion);
            await _context.SaveChangesAsync();

            return new DireccionClienteResponseDTO
            {
                Id = direccion.Id,
                ClId = direccion.ClId,
                ZonId = direccion.ZonId,
                NombreSucursal = direccion.NombreSucursal,
                Direccion = direccion.Direccion,
                Latitud = direccion.Latitud,
                Longitud = direccion.Longitud,
                FechaCreacion = direccion.FechaCreacion,
                EstadoDel = direccion.EstadoDel
            };
        }

        public async Task<DireccionClienteResponseDTO?> ActualizarDireccion(int id, DireccionClienteRequestDTO dto)
        {
            var direccion = await _context.DireccionCliente.FirstOrDefaultAsync(x => x.Id == id && x.EstadoDel);
            if (direccion == null) return null;

            direccion.ClId = dto.ClId;
            direccion.ZonId = dto.ZonId;
            direccion.NombreSucursal = dto.NombreSucursal;
            direccion.Direccion = dto.Direccion;
            direccion.Latitud = dto.Latitud;
            direccion.Longitud = dto.Longitud;

            await _context.SaveChangesAsync();

            return new DireccionClienteResponseDTO
            {
                Id = direccion.Id,
                ClId = direccion.ClId,
                ZonId = direccion.ZonId,
                NombreSucursal = direccion.NombreSucursal,
                Direccion = direccion.Direccion,
                Latitud = direccion.Latitud,
                Longitud = direccion.Longitud,
                FechaCreacion = direccion.FechaCreacion,
                EstadoDel = direccion.EstadoDel
            };
        }

        public async Task<bool> DesactivarDireccion(int id)
        {
            var direccion = await _context.DireccionCliente.FirstOrDefaultAsync(x => x.Id == id && x.EstadoDel);
            if (direccion == null) return false;

            direccion.EstadoDel = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DireccionClienteResponseDTO?> ObtenerPorId(int id)
        {
            var direccion = await _context.DireccionCliente
                .Include(d => d.Cliente)
                .Include(d => d.Zona)
                .FirstOrDefaultAsync(x => x.Id == id && x.EstadoDel);

            if (direccion == null) return null;

            return new DireccionClienteResponseDTO
            {
                Id = direccion.Id,
                ClId = direccion.ClId,
                ZonId = direccion.ZonId,
                NombreSucursal = direccion.NombreSucursal,
                Direccion = direccion.Direccion,
                Latitud = direccion.Latitud,
                Longitud = direccion.Longitud,
                FechaCreacion = direccion.FechaCreacion,
                EstadoDel = direccion.EstadoDel,
                NombreCliente = direccion.Cliente?.NombreCompleto,
                NombreZona = direccion.Zona?.Nombre
            };
        }

        public async Task<List<DireccionClienteResponseDTO>> ObtenerPorCliente(int clId)
        {
            return await _context.DireccionCliente
                .Include(d => d.Zona)
                .Where(x => x.ClId == clId && x.EstadoDel)
                .Select(d => new DireccionClienteResponseDTO
                {
                    Id = d.Id,
                    ClId = d.ClId,
                    ZonId = d.ZonId,
                    NombreSucursal = d.NombreSucursal,
                    Direccion = d.Direccion,
                    Latitud = d.Latitud,
                    Longitud = d.Longitud,
                    FechaCreacion = d.FechaCreacion,
                    EstadoDel = d.EstadoDel,
                    NombreCliente =d.Cliente != null ? d.Cliente.NombreCompleto : null,
                    NombreZona = d.Zona != null ? d.Zona.Nombre : null
                })
                .ToListAsync();
        }

        public async Task<List<DireccionClienteResponseDTO>> ObtenerTodas()
        {
            return await _context.DireccionCliente
                .Include(d => d.Cliente)
                .Include(d => d.Zona)
                .Where(x => x.EstadoDel)
                .Select(d => new DireccionClienteResponseDTO
                {
                    Id = d.Id,
                    ClId = d.ClId,
                    ZonId = d.ZonId,
                    NombreSucursal = d.NombreSucursal,
                    Direccion = d.Direccion,
                    Latitud = d.Latitud,
                    Longitud = d.Longitud,
                    FechaCreacion = d.FechaCreacion,
                    EstadoDel = d.EstadoDel,
                    NombreCliente = d.Cliente.NombreCompleto,
                    NombreZona = d.Zona != null ? d.Zona.Nombre : null
                })
                .ToListAsync();
        }
    }
}
