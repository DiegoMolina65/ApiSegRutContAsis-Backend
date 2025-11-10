using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.Visita;
using SegRutContAsis.Business.DTO.Response.Visita;
using SegRutContAsis.Business.Interfaces.Visita;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class VisitaService : IVisitaService
{
    private readonly SegRutContAsisContext _context;
    public VisitaService(SegRutContAsisContext context)
    {
        _context = context;
    }

    // Crear Visita
    public async Task<VisitaResponseDTO> CrearVisita(VisitaRequestDTO dto)
    {
        var ruta = await _context.Ruta.FirstOrDefaultAsync(r => r.rutId == dto.rutId && r.rutEstadoDel);
        if (ruta == null) throw new Exception("Ruta no existe o está desactivada.");

        bool existeDuplicado = await _context.Visita.AnyAsync(v =>
            v.rutId == dto.rutId &&
            v.dirClId == dto.dirClId &&
            v.visEstadoDel);

        if (existeDuplicado)
            throw new Exception("Ya existe una visita registrada para este cliente en la ruta.");

        var visita = new Visita
        {
            rutId = dto.rutId,
            dirClId = dto.dirClId,
            visComentario = dto.visComentario
        };

        _context.Visita.Add(visita);
        await _context.SaveChangesAsync();

        return await ObtenerVisitaId(visita.visId);
    }

    // Actualizar Visita
    public async Task<VisitaResponseDTO> ActualizarVisita(int id, VisitaRequestDTO dto)
    {
        var visita = await _context.Visita.FindAsync(id);
        if (visita == null || !visita.visEstadoDel)
            throw new Exception("Visita no encontrada");

        bool existeDuplicado = await _context.Visita.AnyAsync(v =>
            v.visId != id &&
            v.rutId == dto.rutId &&
            v.dirClId == dto.dirClId &&
            v.visEstadoDel);

        if (existeDuplicado)
            throw new Exception("Ya existe otra visita registrada para este cliente en la misma ruta.");

        visita.rutId = dto.rutId;
        visita.dirClId = dto.dirClId;
        visita.visComentario = dto.visComentario;

        await _context.SaveChangesAsync();
        return await ObtenerVisitaId(visita.visId);
    }

    // Deshabilitar Visita
    public async Task<bool> DeshabilitarVisita(int id)
    {
        var visita = await _context.Visita.FindAsync(id);
        if (visita == null) return false;

        visita.visEstadoDel = false;
        await _context.SaveChangesAsync();
        return true;
    }

    // Obtener Visita por Id
    public async Task<VisitaResponseDTO> ObtenerVisitaId(int id)
    {
        var v = await _context.Visita
            .Include(x => x.DireccionCliente)
                .ThenInclude(d => d.Cliente)
            .Include(x => x.Ruta)
            .Where(x => x.visId == id && x.visEstadoDel && x.Ruta.rutEstadoDel)
            .FirstOrDefaultAsync();

        if (v == null) throw new Exception("Visita no encontrada");

        return new VisitaResponseDTO
        {
            visId = v.visId,
            rutId = v.rutId,
            dirClId = v.dirClId,
            visFechaCreacion = v.visFechaCreacion,
            visEstadoDel = v.visEstadoDel,
            visComentario = v.visComentario,
            NombreCliente = v.DireccionCliente.Cliente?.clNombreCompleto,
            NombreSucursalCliente = v.DireccionCliente?.dirClNombreSucursal,
            NombreVendedor = v.Ruta?.Vendedor?.Usuario?.usrNombreCompleto
        };
    }

    // Obtener Todas las Visitas
    public async Task<List<VisitaResponseDTO>> ObtenerTodasVisitas()
    {
        var visitas = await _context.Visita
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Vendedor)
                    .ThenInclude(ven => ven.Usuario)
            .Include(v => v.DireccionCliente)
                .ThenInclude(d => d.Cliente)
            .Where(v => v.visEstadoDel && v.Ruta.rutEstadoDel)
            .ToListAsync();

        return visitas.Select(v => new VisitaResponseDTO
        {
            visId = v.visId,
            rutId = v.rutId,
            dirClId = v.dirClId,
            visFechaCreacion = v.visFechaCreacion,
            visEstadoDel = v.visEstadoDel,
            visComentario = v.visComentario,
            NombreCliente = v.DireccionCliente?.Cliente?.clNombreCompleto,
            NombreSucursalCliente = v.DireccionCliente?.dirClNombreSucursal,
            SucursalLatitud = v.DireccionCliente?.dirClLatitud,
            SucursalLongitud = v.DireccionCliente?.dirClLongitud,
            NombreZona = v.DireccionCliente?.Zona?.zonNombre,
            Direccion = v.DireccionCliente?.dirClDireccion,
            NombreVendedor = v.Ruta?.Vendedor?.Usuario?.usrNombreCompleto
        }).ToList();
    }

    // Obtener visitas filtradas
    public async Task<List<VisitaResponseDTO>> ObtenerVisitasPorRuta(int rutaId) =>
        (await ObtenerTodasVisitas()).Where(v => v.rutId == rutaId).ToList();

    public async Task<List<VisitaResponseDTO>> ObtenerVisitasPorDireccionCliente(int clienteId) =>
        (await ObtenerTodasVisitas()).Where(v => v.dirClId == clienteId).ToList();

    public async Task<List<VisitaResponseDTO>> ObtenerVisitasPorVendedor(int venId)
    {
        var visitas = await _context.Visita
            .Include(v => v.Ruta)
            .Include(v => v.DireccionCliente)
                .ThenInclude(d => d.Cliente)
            .Where(v => v.visEstadoDel && v.Ruta.rutEstadoDel && v.Ruta.VendedorId == venId)
            .ToListAsync();

        return visitas.Select(v => new VisitaResponseDTO
        {
            visId = v.visId,
            rutId = v.rutId,
            dirClId = v.dirClId,
            visFechaCreacion = v.visFechaCreacion,
            visEstadoDel = v.visEstadoDel,
            visComentario = v.visComentario,
            NombreCliente = v.DireccionCliente.Cliente?.clNombreCompleto,
            NombreSucursalCliente = v.DireccionCliente?.dirClNombreSucursal,
            SucursalLatitud = v.DireccionCliente?.dirClLatitud,
            SucursalLongitud = v.DireccionCliente?.dirClLongitud,
            NombreZona = v.DireccionCliente?.Zona?.zonNombre,
            Direccion = v.DireccionCliente?.dirClDireccion,
            NombreVendedor = v.Ruta?.Vendedor?.Usuario?.usrNombreCompleto
        }).ToList();
    }
}
