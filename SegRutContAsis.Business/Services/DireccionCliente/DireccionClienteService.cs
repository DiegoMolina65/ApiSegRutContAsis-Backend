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

        // Crear Direccion Cliente 
        public async Task<DireccionClienteResponseDTO> CrearDireccion(DireccionClienteRequestDTO dto)
        {
            var direccion = new DireccionCliente
            {
                clId = dto.clId,
                zonId = dto.zonId,
                dirClNombreSucursal = dto.dirClNombreSucursal,
                dirClDireccion = dto.dirClDireccion,
                dirClLatitud = dto.dirClLatitud,
                dirClLongitud = dto.dirClLongitud,
                dirClEstadoDel = true
            };

            _context.DireccionCliente.Add(direccion);
            await _context.SaveChangesAsync();

            return new DireccionClienteResponseDTO
            {
                dirClId = direccion.dirClId,
                clId = direccion.clId,
                zonId = direccion.zonId,
                dirClNombreSucursal = direccion.dirClNombreSucursal,
                dirClDireccion = direccion.dirClDireccion,
                dirClLatitud = direccion.dirClLatitud,
                dirClLongitud = direccion.dirClLongitud,
                dirClFechaCreacion = direccion.dirClFechaCreacion,
                dirClEstadoDel = direccion.dirClEstadoDel
            };
        }

        // Actualizar Direccion Cliente 
        public async Task<DireccionClienteResponseDTO?> ActualizarDireccion(int id, DireccionClienteRequestDTO dto)
        {
            var direccion = await _context.DireccionCliente.FirstOrDefaultAsync(x => x.dirClId == id && x.dirClEstadoDel);
            if (direccion == null) return null;

            direccion.clId = dto.clId;
            direccion.zonId = dto.zonId;
            direccion.dirClNombreSucursal = dto.dirClNombreSucursal;
            direccion.dirClDireccion = dto.dirClDireccion;
            direccion.dirClLatitud = dto.dirClLatitud;
            direccion.dirClLongitud = dto.dirClLongitud;

            await _context.SaveChangesAsync();

            return new DireccionClienteResponseDTO
            {
                dirClId = direccion.dirClId,
                clId = direccion.clId,
                zonId = direccion.zonId,
                dirClNombreSucursal = direccion.dirClNombreSucursal,
                dirClDireccion = direccion.dirClDireccion,
                dirClLatitud = direccion.dirClLatitud,
                dirClLongitud = direccion.dirClLongitud,
                dirClFechaCreacion = direccion.dirClFechaCreacion,
                dirClEstadoDel = direccion.dirClEstadoDel
            };
        }

        // Desactivar Direccion Cliente 
        public async Task<bool> DesactivarDireccion(int id)
        {
            var direccion = await _context.DireccionCliente.FirstOrDefaultAsync(x => x.dirClId == id && x.dirClEstadoDel);
            if (direccion == null) return false;

            direccion.dirClEstadoDel = false;
            await _context.SaveChangesAsync();
            return true;
        }

        // Obtener Por Id Direccion Cliente 
        public async Task<DireccionClienteResponseDTO?> ObtenerPorId(int id)
        {
            var direccion = await _context.DireccionCliente
                .Include(d => d.Cliente)
                .Include(d => d.Zona)
                .FirstOrDefaultAsync(x => x.dirClId == id && x.dirClEstadoDel);

            if (direccion == null) return null;

            return new DireccionClienteResponseDTO
            {
                dirClId = direccion.dirClId,
                clId = direccion.clId,
                zonId = direccion.zonId,
                dirClNombreSucursal = direccion.dirClNombreSucursal,
                dirClDireccion = direccion.dirClDireccion,
                dirClLatitud = direccion.dirClLatitud,
                dirClLongitud = direccion.dirClLongitud,
                dirClFechaCreacion = direccion.dirClFechaCreacion,
                dirClEstadoDel = direccion.dirClEstadoDel,
                NombreCliente = direccion.Cliente?.clNombreCompleto,
                NombreZona = direccion.Zona?.zonNombre
            };
        }

        // Obtener por Cliente Direccion Cliente 
        public async Task<List<DireccionClienteResponseDTO>> ObtenerPorCliente(int clId)
        {
            return await _context.DireccionCliente
                .Include(d => d.Zona)
                .Where(x => x.clId == clId && x.dirClEstadoDel)
                .Select(d => new DireccionClienteResponseDTO
                {
                    dirClId = d.dirClId,
                    clId = d.clId,
                    zonId = d.zonId,
                    dirClNombreSucursal = d.dirClNombreSucursal,
                    dirClDireccion = d.dirClDireccion,
                    dirClLatitud = d.dirClLatitud,
                    dirClLongitud = d.dirClLongitud,
                    dirClFechaCreacion = d.dirClFechaCreacion,
                    dirClEstadoDel = d.dirClEstadoDel,
                    NombreCliente =d.Cliente != null ? d.Cliente.clNombreCompleto : null,
                    NombreZona = d.Zona != null ? d.Zona.zonNombre : null
                })
                .ToListAsync();
        }

        // Obtener todas Direccion Cliente 
        public async Task<List<DireccionClienteResponseDTO>> ObtenerTodas()
        {
            return await _context.DireccionCliente
                .Include(d => d.Cliente)
                .Include(d => d.Zona)
                .Where(x => x.dirClEstadoDel)
                .Select(d => new DireccionClienteResponseDTO
                {
                    dirClId = d.dirClId,
                    clId = d.clId,
                    zonId = d.zonId,
                    dirClNombreSucursal = d.dirClNombreSucursal,
                    dirClDireccion = d.dirClDireccion,
                    dirClLatitud = d.dirClLatitud,
                    dirClLongitud = d.dirClLongitud,
                    dirClFechaCreacion = d.dirClFechaCreacion,
                    dirClEstadoDel = d.dirClEstadoDel,
                    NombreCliente = d.Cliente.clNombreCompleto,
                    NombreZona = d.Zona != null ? d.Zona.zonNombre : null
                })
                .ToListAsync();
        }
    }
}
