using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.Visita;
using SegRutContAsis.Business.DTO.Response.Visita;
using SegRutContAsis.Business.Interfaces.Visita;
using SegRutContAsis.Domain.Entities;

public class VisitaService : IVisitaService
{
    private readonly SegRutContAsisContext _context;
    public VisitaService(SegRutContAsisContext context)
    {
        _context = context;
    }

    public async Task<VisitaResponseDTO> CrearVisita(VisitaRequestDTO dto)
    {
        var ruta = await _context.Ruta.FirstOrDefaultAsync(r => r.Id == dto.RutId && r.EstadoDel);
        if (ruta == null) throw new Exception("Ruta no existe o está desactivada.");

        var fecha = dto.VisFecha ?? DateTime.Now;
        int semana = ((fecha.Day - 1) / 7) + 1;

        bool existeDuplicado = await _context.Visita.AnyAsync(v =>
            v.RutId == dto.RutId &&
            v.DirClId == dto.DirClId &&
            v.VisFecha == fecha &&
            v.VisHora == dto.VisHora &&
            v.VisEstadoDel);

        if (existeDuplicado) throw new Exception("Ya existe visita para ese cliente en la misma fecha y hora.");

        var visita = new Visita
        {
            RutId = dto.RutId,
            DirClId = dto.DirClId,
            VisFecha = fecha,
            VisHora = dto.VisHora,
            VisSemanaDelMes = semana,
            VisLatitud = dto.VisLatitud,
            VisLongitud = dto.VisLongitud,
            VisComentario = dto.VisComentario
        };

        _context.Visita.Add(visita);
        await _context.SaveChangesAsync();

        return await ObtenerVisitaId(visita.Id);
    }

    public async Task<VisitaResponseDTO> ActualizarVisita(int id, VisitaRequestDTO dto)
    {
        var visita = await _context.Visita.FindAsync(id);
        if (visita == null || !visita.VisEstadoDel) throw new Exception("Visita no encontrada");

        var fecha = dto.VisFecha ?? DateTime.Now;
        int semana = ((fecha.Day - 1) / 7) + 1;

        bool existeDuplicado = await _context.Visita.AnyAsync(v =>
            v.Id != id &&
            v.RutId == dto.RutId &&
            v.DirClId == dto.DirClId &&
            v.VisFecha == fecha &&
            v.VisHora == dto.VisHora &&
            v.VisEstadoDel);

        if (existeDuplicado) throw new Exception("Ya existe otra visita para este cliente en la misma fecha y hora.");

        visita.RutId = dto.RutId;
        visita.DirClId = dto.DirClId;
        visita.VisFecha = fecha;
        visita.VisHora = dto.VisHora;
        visita.VisSemanaDelMes = semana;
        visita.VisLatitud = dto.VisLatitud;
        visita.VisLongitud = dto.VisLongitud;
        visita.VisComentario = dto.VisComentario;

        await _context.SaveChangesAsync();
        return await ObtenerVisitaId(visita.Id);
    }

    public async Task<bool> DeshabilitarVisita(int id)
    {
        var visita = await _context.Visita.FindAsync(id);
        if (visita == null) return false;

        visita.VisEstadoDel = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<VisitaResponseDTO> ObtenerVisitaId(int id)
    {
        var v = await _context.Visita
            .Include(x => x.DireccionCliente)
                .ThenInclude(d => d.Cliente)
            .Include(x => x.Ruta)
            .Where(x => x.Id == id && x.VisEstadoDel && x.Ruta.EstadoDel)
            .FirstOrDefaultAsync();

        if (v == null) throw new Exception("Visita no encontrada");

        return new VisitaResponseDTO
        {
            Id = v.Id,
            RutId = v.RutId,
            DirClId = v.DirClId,
            VisFechaCreacion = v.VisFechaCreacion,
            VisFecha = v.VisFecha,
            VisHora = v.VisHora,
            VisSemanaDelMes = v.VisSemanaDelMes,
            VisLatitud = v.VisLatitud,
            VisLongitud = v.VisLongitud,
            VisEstadoDel = v.VisEstadoDel,
            VisComentario = v.VisComentario,
            NombreCliente = v.DireccionCliente.Cliente.NombreCompleto,
            NombreSucursal = v.DireccionCliente.NombreSucursal
        };
    }

    public async Task<List<VisitaResponseDTO>> ObtenerTodasVisitas()
    {
        var visitas = await _context.Visita
            .Include(v => v.Ruta)
            .Include(v => v.DireccionCliente)
                .ThenInclude(d => d.Cliente)
            .Where(v => v.VisEstadoDel && v.Ruta.EstadoDel)
            .ToListAsync();

        return visitas.Select(v => new VisitaResponseDTO
        {
            Id = v.Id,
            RutId = v.RutId,
            DirClId = v.DirClId,
            VisFechaCreacion = v.VisFechaCreacion,
            VisFecha = v.VisFecha,
            VisHora = v.VisHora,
            VisSemanaDelMes = v.VisSemanaDelMes,
            VisLatitud = v.VisLatitud,
            VisLongitud = v.VisLongitud,
            VisEstadoDel = v.VisEstadoDel,
            VisComentario = v.VisComentario,
            NombreCliente = v.DireccionCliente.Cliente.NombreCompleto,
            NombreSucursal = v.DireccionCliente.NombreSucursal
        }).ToList();
    }


    public async Task<List<VisitaResponseDTO>> ObtenerVisitasPorRuta(int rutaId) =>
        (await ObtenerTodasVisitas()).Where(v => v.RutId == rutaId).ToList();

    public async Task<List<VisitaResponseDTO>> ObtenerVisitasPorDireccionCliente(int clienteId) =>
        (await ObtenerTodasVisitas()).Where(v => v.DirClId == clienteId).ToList();

    public async Task<List<VisitaResponseDTO>> ObtenerVisitasPorVendedor(int venId)
    {
        var visitas = await _context.Visita
            .Include(v => v.Ruta)               
            .Include(v => v.DireccionCliente)
                .ThenInclude(d => d.Cliente)
            .Where(v => v.VisEstadoDel && v.Ruta.EstadoDel && v.Ruta.VendedorId == venId)
            .ToListAsync();

        return visitas.Select(v => new VisitaResponseDTO
        {
            Id = v.Id,
            RutId = v.RutId,
            DirClId = v.DirClId,
            VisFechaCreacion = v.VisFechaCreacion,
            VisFecha = v.VisFecha,
            VisHora = v.VisHora,
            VisSemanaDelMes = v.VisSemanaDelMes,
            VisLatitud = v.VisLatitud,
            VisLongitud = v.VisLongitud,
            VisEstadoDel = v.VisEstadoDel,
            VisComentario = v.VisComentario,
            NombreCliente = v.DireccionCliente.Cliente.NombreCompleto,
            NombreSucursal = v.DireccionCliente.NombreSucursal
        }).ToList();
    }

    public async Task<List<VisitaResponseDTO>> ObtenerVisitasPorSemana(int venId, int semana)
    {
        var visitas = await _context.Visita
            .Include(v => v.Ruta)
            .Include(v => v.DireccionCliente)
                .ThenInclude(d => d.Cliente)
            .Where(v => v.VisEstadoDel && v.Ruta.EstadoDel && v.Ruta.VendedorId == venId && v.VisSemanaDelMes == semana)
            .ToListAsync();

        return visitas.Select(v => new VisitaResponseDTO
        {
            Id = v.Id,
            RutId = v.RutId,
            DirClId = v.DirClId,
            VisFechaCreacion = v.VisFechaCreacion,
            VisFecha = v.VisFecha,
            VisHora = v.VisHora,
            VisSemanaDelMes = v.VisSemanaDelMes,
            VisLatitud = v.VisLatitud,
            VisLongitud = v.VisLongitud,
            VisEstadoDel = v.VisEstadoDel,
            VisComentario = v.VisComentario,
            NombreCliente = v.DireccionCliente.Cliente.NombreCompleto,
            NombreSucursal = v.DireccionCliente.NombreSucursal
        }).ToList();
    }

}
