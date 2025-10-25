using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Domain.Entities;

public class SegRutContAsisContext : DbContext
{
    public SegRutContAsisContext(DbContextOptions<SegRutContAsisContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Rol> Rol { get; set; }
    public DbSet<UsuarioRol> UsuarioRol { get; set; }
    public DbSet<Administrador> Administrador { get; set; }
    public DbSet<Supervisor> Supervisor { get; set; }
    public DbSet<Vendedor> Vendedor { get; set; }
    public DbSet<Asistencia> Asistencia { get; set; }
    public DbSet<Zona> Zona { get; set; }
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<DireccionCliente> DireccionCliente { get; set; }
    public DbSet<Ruta> Ruta { get; set; }
    public DbSet<Visita> Visita { get; set; }
    public DbSet<Evidencia> Evidencia { get; set; }
    public DbSet<SeguimientoVendedor> SeguimientoVendedor { get; set; }
    public DbSet<AsignacionClienteVendedor> AsignacionClienteVendedor { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ---------------- Usuario ----------------
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("usrId");
            entity.Property(u => u.NombreCompleto).HasColumnName("usrNombreCompleto").HasMaxLength(100).IsRequired();
            entity.Property(u => u.Correo).HasColumnName("usrCorreo").HasMaxLength(100).IsRequired();
            entity.Property(u => u.Telefono).HasColumnName("usrTelefono").HasMaxLength(20).IsRequired();
            entity.Property(u => u.NitEmpleado).HasColumnName("usrNitEmpleado").HasMaxLength(100).IsRequired();
            entity.Property(u => u.CarnetIdentidad).HasColumnName("usrCarnetIdentidad").HasMaxLength(100).IsRequired();
            entity.Property(u => u.UsuarioLog).HasColumnName("usrUsuarioLog").HasMaxLength(50).IsRequired();
            entity.Property(u => u.ContrasenaLog).HasColumnName("usrContrasenaLog").HasMaxLength(255).IsRequired();
            entity.Property(u => u.EstadoDel).HasColumnName("usrEstadoDel").HasDefaultValue(true);
            entity.Property(u => u.FechaCreacion).HasColumnName("usrFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
        });

        // ---------------- Rol ----------------
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).HasColumnName("rolId");
            entity.Property(r => r.Nombre).HasColumnName("rolNombre").HasMaxLength(50).IsRequired();
            entity.Property(r => r.Descripcion).HasColumnName("rolDescripcion");
            entity.Property(r => r.FechaCreacion).HasColumnName("rolFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(r => r.EstadoDel).HasColumnName("rolEstadoDel").HasDefaultValue(true);
        });

        // ---------------- UsuarioRol (muchos a muchos) ----------------
        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.ToTable("UsuarioRol");
            entity.HasKey(ur => new { ur.usrID, ur.rolId });
            entity.HasOne(ur => ur.Usuario).WithMany(u => u.UsuarioRoles).HasForeignKey(ur => ur.usrID);
            entity.HasOne(ur => ur.Rol).WithMany(r => r.UsuarioRoles).HasForeignKey(ur => ur.rolId);
        });

        // ---------------- Administrador ----------------
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.ToTable("Administrador");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("admId");
            entity.Property(a => a.UsuarioId).HasColumnName("usrId");
            entity.Property(a => a.FechaCreacion).HasColumnName("admFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(a => a.EstadoDel).HasColumnName("admEstadoDel").HasDefaultValue(true);
            entity.HasOne(a => a.Usuario).WithOne(u => u.Administrador).HasForeignKey<Administrador>(a => a.UsuarioId);
        });

        // ---------------- Supervisor ----------------
        modelBuilder.Entity<Supervisor>(entity =>
        {
            entity.ToTable("Supervisor");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).HasColumnName("supId");
            entity.Property(s => s.UsuarioId).HasColumnName("usrId");
            entity.Property(s => s.FechaCreacion).HasColumnName("supFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(s => s.EstadoDel).HasColumnName("supEstadoDel").HasDefaultValue(true);
            entity.HasOne(s => s.Usuario).WithOne(u => u.Supervisor).HasForeignKey<Supervisor>(s => s.UsuarioId);
        });

        // ---------------- Vendedor ----------------
        modelBuilder.Entity<Vendedor>(entity =>
        {
            entity.ToTable("Vendedor");
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Id).HasColumnName("venId");
            entity.Property(v => v.UsuarioId).HasColumnName("usrId");
            entity.Property(v => v.FechaCreacion).HasColumnName("venFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(v => v.EstadoDel).HasColumnName("venEstadoDel").HasDefaultValue(true);
            entity.HasOne(v => v.Usuario).WithOne(u => u.Vendedor).HasForeignKey<Vendedor>(v => v.UsuarioId);
        });

        // ---------------- AsignacionClienteVendedor ----------------
        modelBuilder.Entity<AsignacionClienteVendedor>(entity =>
        {
            entity.ToTable("AsignacionClienteVendedor");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("asgId");
            entity.Property(a => a.SupId).HasColumnName("supId");
            entity.Property(a => a.VenId).HasColumnName("venId");
            entity.Property(a => a.ClId).HasColumnName("clId");
            entity.Property(a => a.FechaCreacion).HasColumnName("asgFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(a => a.EstadoDel).HasColumnName("asgEstadoDel").HasDefaultValue(true);

            entity.HasOne(a => a.Supervisor).WithMany(s => s.AsignacionesClienteVendedor).HasForeignKey(a => a.SupId);
            entity.HasOne(a => a.Vendedor).WithMany(v => v.AsignacionesClienteVendedor).HasForeignKey(a => a.VenId);
            entity.HasOne(a => a.Cliente).WithMany(c => c.AsignacionesClienteVendedor).HasForeignKey(a => a.ClId);
        });

        // ---------------- Asistencia ----------------
        modelBuilder.Entity<Asistencia>(entity =>
        {
            entity.ToTable("Asistencia");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("asiId");
            entity.Property(a => a.FechaCreacion).HasColumnName("asiFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(a => a.VenId).HasColumnName("venId");
            entity.Property(a => a.HoraEntrada).HasColumnName("asiHoraEntrada").HasColumnType("datetime");
            entity.Property(a => a.HoraSalida).HasColumnName("asiHoraSalida").HasColumnType("datetime");
            entity.Property(a => a.Latitud).HasColumnName("asiLatitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(a => a.Longitud).HasColumnName("asiLongitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(a => a.EstadoDel).HasColumnName("asiEstadoDel").HasDefaultValue(true);
            entity.HasOne(a => a.Vendedor).WithMany().HasForeignKey(a => a.VenId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------- Zona ----------------
        modelBuilder.Entity<Zona>(entity =>
        {
            entity.ToTable("Zona");
            entity.HasKey(z => z.Id);
            entity.Property(z => z.Id).HasColumnName("zonId");
            entity.Property(z => z.FechaCreacion).HasColumnName("zonFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(z => z.Nombre).HasColumnName("zonNombre").HasMaxLength(100).IsRequired();
            entity.Property(z => z.Descripcion).HasColumnName("zonDescripcion").IsRequired();
            entity.Property(z => z.EstadoDel).HasColumnName("zonEstadoDel").HasDefaultValue(true);
        });

        // ---------------- Cliente ----------------
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("clId");
            entity.Property(c => c.FechaCreacion).HasColumnName("clFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(c => c.NombreCompleto).HasColumnName("clNombreCompleto").HasMaxLength(100).IsRequired();
            entity.Property(c => c.NitCliente).HasColumnName("clNitCliente").HasMaxLength(100).IsRequired();
            entity.Property(c => c.CarnetIdentidad).HasColumnName("clCarnetIdentidad").HasMaxLength(100).IsRequired();
            entity.Property(c => c.TipoCliente).HasColumnName("clTipoCliente").HasMaxLength(100);
            entity.Property(c => c.Telefono).HasColumnName("clTelefono").HasMaxLength(20).IsRequired();
            entity.Property(c => c.EstadoDel).HasColumnName("clEstadoDel").HasDefaultValue(true);
        });

        // ---------------- DireccionCliente ----------------
        modelBuilder.Entity<DireccionCliente>(entity =>
        {
            entity.ToTable("DireccionCliente");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Id).HasColumnName("dirClId");
            entity.Property(d => d.FechaCreacion).HasColumnName("dirClFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(d => d.ClId).HasColumnName("clId").IsRequired();
            entity.Property(d => d.ZonId).HasColumnName("zonId").IsRequired(false);
            entity.Property(d => d.NombreSucursal).HasColumnName("dirClNombreSucursal").HasMaxLength(100);
            entity.Property(d => d.Direccion).HasColumnName("dirClDireccion").HasMaxLength(255).IsRequired();
            entity.Property(d => d.Latitud).HasColumnName("dirClLatitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(d => d.Longitud).HasColumnName("dirClLongitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(d => d.EstadoDel).HasColumnName("dirClEstadoDel").HasDefaultValue(true);

            entity.HasOne(d => d.Cliente).WithMany(c => c.DireccionesCliente).HasForeignKey(d => d.ClId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(d => d.Zona).WithMany().HasForeignKey(d => d.ZonId).IsRequired(false);
        });

        // ---------------- Visita ----------------
        modelBuilder.Entity<Visita>(entity =>
        {
            entity.ToTable("Visita");
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Id).HasColumnName("visId");
            entity.Property(v => v.VisFechaCreacion).HasColumnName("visFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(v => v.RutId).HasColumnName("rutId");
            entity.Property(v => v.DirClId).HasColumnName("dirClId");
            entity.Property(v => v.VisFecha).HasColumnName("visFecha").HasColumnType("date");
            entity.Property(v => v.VisHora).HasColumnName("visHora").HasColumnType("time");
            entity.Property(v => v.VisSemanaDelMes).HasColumnName("visSemanaDelMes");
            entity.Property(v => v.VisLatitud).HasColumnName("visLatitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(v => v.VisLongitud).HasColumnName("visLongitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(v => v.VisEstadoDel).HasColumnName("visEstadoDel").HasDefaultValue(true);
            entity.Property(v => v.VisComentario).HasColumnName("visComentario");

            entity.HasOne(v => v.Ruta).WithMany().HasForeignKey(v => v.RutId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(v => v.DireccionCliente).WithMany(d => d.Visitas).HasForeignKey(v => v.DirClId);
        });

        // ---------------- Evidencia ----------------
        modelBuilder.Entity<Evidencia>(entity =>
        {
            entity.ToTable("Evidencia");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("eviId");
            entity.Property(e => e.FechaCreacion).HasColumnName("eviFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(e => e.visId).HasColumnName("visId");
            entity.Property(e => e.Tipo).HasColumnName("eviTipo").HasMaxLength(255).IsRequired(false);
            entity.Property(e => e.Observaciones).HasColumnName("eviObservaciones").IsRequired(false);
            entity.HasOne(e => e.Visita).WithMany(v => v.Evidencias).HasForeignKey(e => e.visId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------- SeguimientoVendedor ----------------
        modelBuilder.Entity<SeguimientoVendedor>(entity =>
        {
            entity.ToTable("SeguimientoVendedor");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).HasColumnName("segId");
            entity.Property(s => s.venId).HasColumnName("venId");
            entity.Property(s => s.FechaCreacion).HasColumnName("segFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(s => s.Latitud).HasColumnName("segLatitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(s => s.Longitud).HasColumnName("segLongitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.HasOne(s => s.Vendedor).WithMany().HasForeignKey(s => s.venId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------- Ruta ----------------
        modelBuilder.Entity<Ruta>(entity =>
        {
            entity.ToTable("Ruta");
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Id).HasColumnName("rutId");
            entity.Property(r => r.VendedorId).HasColumnName("venId");
            entity.Property(r => r.SupervisorId).HasColumnName("supId");
            entity.Property(r => r.FechaCreacion).HasColumnName("rutFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(r => r.EstadoDel).HasColumnName("rutEstadoDel").HasDefaultValue(true);
            entity.Property(r => r.Nombre).HasColumnName("rutNombre").HasMaxLength(20).IsRequired();
            entity.Property(r => r.Comentario).HasColumnName("rutComentario");
            entity.HasOne(r => r.Vendedor).WithMany(v => v.Rutas).HasForeignKey(r => r.VendedorId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(r => r.Supervisor).WithMany(s => s.Rutas).HasForeignKey(r => r.SupervisorId).OnDelete(DeleteBehavior.NoAction);
        });


        base.OnModelCreating(modelBuilder);
    }
}
