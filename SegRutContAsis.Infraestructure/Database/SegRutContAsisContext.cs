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
    public DbSet<MarcarLlegadaVisita> MarcarLlegadaVisita { get; set; }
    public DbSet<AsignacionSupervisorVendedor> AsignacionSupervisorVendedor { get; set; }
    public DbSet<Reportes> Reportes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ---------------- Usuario ----------------
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");
            entity.HasKey(u => u.usrId);
            entity.Property(u => u.usrId).HasColumnName("usrId");
            entity.Property(u => u.usrNombreCompleto).HasColumnName("usrNombreCompleto").HasMaxLength(100).IsRequired();
            entity.Property(u => u.usrCorreo).HasColumnName("usrCorreo").HasMaxLength(100).IsRequired();
            entity.Property(u => u.usrTelefono).HasColumnName("usrTelefono").HasMaxLength(20).IsRequired();
            entity.Property(u => u.usrNitEmpleado).HasColumnName("usrNitEmpleado").HasMaxLength(100).IsRequired();
            entity.Property(u => u.usrCarnetIdentidad).HasColumnName("usrCarnetIdentidad").HasMaxLength(100).IsRequired();
            entity.Property(u => u.usrUsuarioLog).HasColumnName("usrUsuarioLog").HasMaxLength(50).IsRequired();
            entity.Property(u => u.usrContrasenaLog).HasColumnName("usrContrasenaLog").HasMaxLength(255).IsRequired();
            entity.Property(u => u.usrEstadoDel).HasColumnName("usrEstadoDel").HasDefaultValue(true);
            entity.Property(u => u.usrFechaCreacion).HasColumnName("usrFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
        });

        // ---------------- Rol ----------------
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol");
            entity.HasKey(r => r.rolId);
            entity.Property(r => r.rolId).HasColumnName("rolId");
            entity.Property(r => r.rolNombre).HasColumnName("rolNombre").HasMaxLength(50).IsRequired();
            entity.Property(r => r.rolDescripcion).HasColumnName("rolDescripcion");
            entity.Property(r => r.rolFechaCreacion).HasColumnName("rolFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(r => r.rolEstadoDel).HasColumnName("rolEstadoDel").HasDefaultValue(true);
        });

        // ---------------- UsuarioRol (muchos a muchos) ----------------
        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.ToTable("UsuarioRol");
            entity.HasKey(ur => new { ur.usrId, ur.rolId });
            entity.HasOne(ur => ur.Usuario).WithMany(u => u.UsuarioRoles).HasForeignKey(ur => ur.usrId);
            entity.HasOne(ur => ur.Rol).WithMany(r => r.UsuarioRoles).HasForeignKey(ur => ur.rolId);
        });

        // ---------------- Administrador ----------------
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.ToTable("Administrador");
            entity.HasKey(a => a.admId);
            entity.Property(a => a.admId).HasColumnName("admId");
            entity.Property(a => a.usrId).HasColumnName("usrId");
            entity.Property(a => a.admFechaCreacion).HasColumnName("admFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(a => a.admEstadoDel).HasColumnName("admEstadoDel").HasDefaultValue(true);
            entity.HasOne(a => a.Usuario).WithOne(u => u.Administrador).HasForeignKey<Administrador>(a => a.usrId);
        });

        // ---------------- Supervisor ----------------
        modelBuilder.Entity<Supervisor>(entity =>
        {
            entity.ToTable("Supervisor");
            entity.HasKey(s => s.supId);
            entity.Property(s => s.supId).HasColumnName("supId");
            entity.Property(s => s.usrId).HasColumnName("usrId");
            entity.Property(s => s.supFechaCreacion).HasColumnName("supFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(s => s.supEstadoDel).HasColumnName("supEstadoDel").HasDefaultValue(true);
            entity.HasOne(s => s.Usuario).WithOne(u => u.Supervisor).HasForeignKey<Supervisor>(s => s.usrId);
        });

        // ---------------- Vendedor ----------------
        modelBuilder.Entity<Vendedor>(entity =>
        {
            entity.ToTable("Vendedor");
            entity.HasKey(v => v.venId);
            entity.Property(v => v.venId).HasColumnName("venId");
            entity.Property(v => v.usrId).HasColumnName("usrId");
            entity.Property(v => v.venFechaCreacion).HasColumnName("venFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(v => v.venEstadoDel).HasColumnName("venEstadoDel").HasDefaultValue(true);
            entity.HasOne(v => v.Usuario).WithOne(u => u.Vendedor).HasForeignKey<Vendedor>(v => v.usrId);
        });

        // ---------------- AsignacionClienteVendedor ----------------
        modelBuilder.Entity<AsignacionClienteVendedor>(entity =>
        {
            entity.ToTable("AsignacionClienteVendedor");
            entity.HasKey(a => a.asgId);
            entity.Property(a => a.asgId).HasColumnName("asgId");
            entity.Property(a => a.supId).HasColumnName("supId");
            entity.Property(a => a.venId).HasColumnName("venId");
            entity.Property(a => a.clId).HasColumnName("clId");
            entity.Property(a => a.asgFechaCreacion).HasColumnName("asgFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(a => a.asgEstadoDel).HasColumnName("asgEstadoDel").HasDefaultValue(true);

            entity.HasOne(a => a.Supervisor).WithMany(s => s.AsignacionesClienteVendedor).HasForeignKey(a => a.supId);
            entity.HasOne(a => a.Vendedor).WithMany(v => v.AsignacionesClienteVendedor).HasForeignKey(a => a.venId);
            entity.HasOne(a => a.Cliente).WithMany(c => c.AsignacionesClienteVendedor).HasForeignKey(a => a.clId);
        });

        // ---------------- Asistencia ----------------
        modelBuilder.Entity<Asistencia>(entity =>
        {
            entity.ToTable("Asistencia");
            entity.HasKey(a => a.asiId);
            entity.Property(a => a.asiId).HasColumnName("asiId");
            entity.Property(a => a.asiFechaCreacion).HasColumnName("asiFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(a => a.venId).HasColumnName("venId");
            entity.Property(a => a.asiHoraEntrada).HasColumnName("asiHoraEntrada").HasColumnType("datetime");
            entity.Property(a => a.asiHoraSalida).HasColumnName("asiHoraSalida").HasColumnType("datetime");
            entity.Property(a => a.asiLatitud).HasColumnName("asiLatitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(a => a.asiLongitud).HasColumnName("asiLongitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(a => a.asiEstadoDel).HasColumnName("asiEstadoDel").HasDefaultValue(true);
            entity.HasOne(a => a.Vendedor).WithMany().HasForeignKey(a => a.venId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------- Zona ----------------
        modelBuilder.Entity<Zona>(entity =>
        {
            entity.ToTable("Zona");
            entity.HasKey(z => z.zonId);
            entity.Property(z => z.zonId).HasColumnName("zonId");
            entity.Property(z => z.zonFechaCreacion).HasColumnName("zonFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(z => z.zonNombre).HasColumnName("zonNombre").HasMaxLength(100).IsRequired();
            entity.Property(z => z.zonDescripcion).HasColumnName("zonDescripcion").IsRequired();
            entity.Property(z => z.zonEstadoDel).HasColumnName("zonEstadoDel").HasDefaultValue(true);
        });

        // ---------------- Cliente ----------------
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");
            entity.HasKey(c => c.clId);
            entity.Property(c => c.clId).HasColumnName("clId");
            entity.Property(c => c.clFechaCreacion).HasColumnName("clFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(c => c.clNombreCompleto).HasColumnName("clNombreCompleto").HasMaxLength(100).IsRequired();
            entity.Property(c => c.clNitCliente).HasColumnName("clNitCliente").HasMaxLength(100).IsRequired();
            entity.Property(c => c.clCarnetIdentidad).HasColumnName("clCarnetIdentidad").HasMaxLength(100).IsRequired();
            entity.Property(c => c.clTipoCliente).HasColumnName("clTipoCliente").HasMaxLength(100);
            entity.Property(c => c.clTelefono).HasColumnName("clTelefono").HasMaxLength(20).IsRequired();
            entity.Property(c => c.clEstadoDel).HasColumnName("clEstadoDel").HasDefaultValue(true);
        });

        // ---------------- DireccionCliente ----------------
        modelBuilder.Entity<DireccionCliente>(entity =>
        {
            entity.ToTable("DireccionCliente");
            entity.HasKey(d => d.dirClId);
            entity.Property(d => d.dirClId).HasColumnName("dirClId");
            entity.Property(d => d.dirClFechaCreacion).HasColumnName("dirClFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(d => d.clId).HasColumnName("clId").IsRequired();
            entity.Property(d => d.zonId).HasColumnName("zonId").IsRequired(false);
            entity.Property(d => d.dirClNombreSucursal).HasColumnName("dirClNombreSucursal").HasMaxLength(100);
            entity.Property(d => d.dirClDireccion).HasColumnName("dirClDireccion").HasMaxLength(255).IsRequired();
            entity.Property(d => d.dirClLatitud).HasColumnName("dirClLatitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(d => d.dirClLongitud).HasColumnName("dirClLongitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(d => d.dirClEstadoDel).HasColumnName("dirClEstadoDel").HasDefaultValue(true);

            entity.HasOne(d => d.Cliente).WithMany(c => c.DireccionesCliente).HasForeignKey(d => d.clId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(d => d.Zona).WithMany().HasForeignKey(d => d.zonId).IsRequired(false);
        });

        // ---------------- Visita ----------------
        modelBuilder.Entity<Visita>(entity =>
        {
            entity.ToTable("Visita");
            entity.HasKey(v => v.visId);
            entity.Property(v => v.visId).HasColumnName("visId");
            entity.Property(v => v.visFechaCreacion).HasColumnName("visFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(v => v.rutId).HasColumnName("rutId");
            entity.Property(v => v.dirClId).HasColumnName("dirClId");
            entity.Property(v => v.visEstadoDel).HasColumnName("visEstadoDel").HasDefaultValue(true);
            entity.Property(v => v.visComentario).HasColumnName("visComentario");

            entity.HasOne(v => v.Ruta).WithMany().HasForeignKey(v => v.rutId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(v => v.DireccionCliente).WithMany(d => d.Visitas).HasForeignKey(v => v.dirClId);
        });

        // ---------------- Evidencia ----------------
        modelBuilder.Entity<Evidencia>(entity =>
        {
            entity.ToTable("Evidencia");
            entity.HasKey(e => e.eviId);
            entity.Property(e => e.eviId).HasColumnName("eviId");
            entity.Property(e => e.eviFechaCreacion).HasColumnName("eviFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(e => e.visId).HasColumnName("visId");
            entity.Property(e => e.eviTipo).HasColumnName("eviTipo").HasMaxLength(255).IsRequired(false);
            entity.Property(e => e.eviObservaciones).HasColumnName("eviObservaciones").IsRequired(false);
            entity.HasOne(e => e.Visita).WithMany(v => v.Evidencias).HasForeignKey(e => e.visId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------- SeguimientoVendedor ----------------
        modelBuilder.Entity<SeguimientoVendedor>(entity =>
        {
            entity.ToTable("SeguimientoVendedor");
            entity.HasKey(s => s.segId);
            entity.Property(s => s.segId).HasColumnName("segId");
            entity.Property(s => s.venId).HasColumnName("venId");
            entity.Property(s => s.segFechaCreacion).HasColumnName("segFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(s => s.segLatitud).HasColumnName("segLatitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(s => s.segLongitud).HasColumnName("segLongitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.HasOne(s => s.Vendedor).WithMany().HasForeignKey(s => s.venId).OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------- Ruta ----------------
        modelBuilder.Entity<Ruta>(entity =>
        {
            entity.ToTable("Ruta");
            entity.HasKey(r => r.rutId);
            entity.Property(r => r.rutId).HasColumnName("rutId");
            entity.Property(r => r.VendedorId)
                .HasColumnName("venId")
                .IsRequired();
            entity.Property(r => r.SupervisorId)
                .HasColumnName("supId");
            entity.Property(r => r.rutFechaEjecucion)
                .HasColumnName("rutFechaEjecucion")
                .HasColumnType("date")
                .IsRequired();
            entity.Property(r => r.rutFechaCreacion)
                .HasColumnName("rutFechaCreacion")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");
            entity.Property(r => r.rutEstadoDel)
                .HasColumnName("rutEstadoDel")
                .HasDefaultValue(true);
            entity.Property(r => r.rutNombre)
                .HasColumnName("rutNombre")
                .HasMaxLength(255)
                .IsRequired();
            entity.Property(r => r.rutComentario)
                .HasColumnName("rutComentario");
            entity.HasOne(r => r.Vendedor)
                .WithMany(v => v.Rutas)
                .HasForeignKey(r => r.VendedorId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(r => r.Supervisor)
                .WithMany(s => s.Rutas)
                .HasForeignKey(r => r.SupervisorId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasIndex(r => new { r.VendedorId, r.rutFechaEjecucion })
                  .IsUnique()
                  .HasDatabaseName("UQ_Ruta_Ven_Fecha");
        });


        // ---------------- MarcarLlegadaVisita ----------------
        modelBuilder.Entity<MarcarLlegadaVisita>(entity =>
        {
            entity.ToTable("MarcarLlegadaVisita");

            entity.HasKey(m => m.mlvId);
            entity.Property(m => m.mlvId).HasColumnName("mlvId");
            entity.Property(m => m.visId).HasColumnName("visId").IsRequired();
            entity.Property(m => m.mlvHora).HasColumnName("mlvHora").HasColumnType("time").IsRequired(false);
            entity.Property(m => m.mlvLatitud).HasColumnName("mlvLatitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(m => m.mlvLongitud).HasColumnName("mlvLongitud").HasColumnType("decimal(10,8)").IsRequired();
            entity.Property(m => m.mlvEstadoDel).HasColumnName("mlvEstadoDel").HasDefaultValue(true);
            entity.Property(m => m.mlvFechaCreacion).HasColumnName("mlvFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.HasOne(m => m.Visita).WithMany(v => v.MarcarLlegadaVisitas).HasForeignKey(m => m.visId).HasConstraintName("FK_MarcarLlegadaVisita").OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------- AsignacionSupervisorVendedor ----------------
        modelBuilder.Entity<AsignacionSupervisorVendedor>(entity =>
        {
            entity.ToTable("AsignacionSupervisorVendedor");entity.HasKey(a => a.asvId);
            entity.Property(a => a.asvId).HasColumnName("asvId");
            entity.Property(a => a.supId).HasColumnName("supId").IsRequired();
            entity.Property(a => a.venId).HasColumnName("venId").IsRequired();
            entity.Property(a => a.asvFechaCreacion).HasColumnName("asvFechaCreacion").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(a => a.asvEstadoDel).HasColumnName("asvEstadoDel").HasDefaultValue(true);
            entity.HasOne(a => a.Supervisor).WithMany(s => s.AsignacionesSupervisorVendedor).HasForeignKey(a => a.supId).HasConstraintName("FK_AsigSupVen_Supervisor").OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(a => a.Vendedor).WithMany(v => v.AsignacionesSupervisorVendedor).HasForeignKey(a => a.venId).HasConstraintName("FK_AsigSupVen_Vendedor").OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(a => new { a.supId, a.venId }).IsUnique().HasDatabaseName("UQ_AsigSupVen");
        });

        // ---------------- Reportes ----------------
        modelBuilder.Entity<Reportes>(entity =>
        {
            entity.ToTable("ReporteActividad");
            entity.HasKey(r => r.repId);

            entity.Property(r => r.repId).HasColumnName("repId");
            entity.Property(r => r.venId).HasColumnName("venId").IsRequired();
            entity.Property(r => r.supId).HasColumnName("supId");
            entity.Property(r => r.clId).HasColumnName("clId");
            entity.Property(r => r.visId).HasColumnName("visId");
            entity.Property(r => r.asiId).HasColumnName("asiId");
            entity.Property(r => r.eviId).HasColumnName("eviId");
            entity.Property(r => r.rutId).HasColumnName("rutId");
            entity.Property(r => r.zonId).HasColumnName("zonId");

            entity.Property(r => r.repFechaCreacion)
                .HasColumnName("repFechaCreacion")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            entity.Property(r => r.repFecha)
                .HasColumnName("repFecha")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(r => r.repTipoActividad)
                .HasColumnName("repTipoActividad")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(r => r.repDescripcion)
                .HasColumnName("repDescripcion");

            entity.Property(r => r.repLatitud)
                .HasColumnName("repLatitud")
                .HasColumnType("decimal(10,8)");

            entity.Property(r => r.repLongitud)
                .HasColumnName("repLongitud")
                .HasColumnType("decimal(10,8)");

            entity.Property(r => r.repEstadoDel)
                .HasColumnName("repEstadoDel")
                .HasDefaultValue(true);

            entity.HasOne(r => r.Vendedor)
                .WithMany()
                .HasForeignKey(r => r.venId)
                .HasConstraintName("FK_ReporteActividad_Vendedor")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Supervisor)
                .WithMany()
                .HasForeignKey(r => r.supId)
                .HasConstraintName("FK_ReporteActividad_Supervisor")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(r => r.Cliente)
                .WithMany()
                .HasForeignKey(r => r.clId)
                .HasConstraintName("FK_ReporteActividad_Cliente")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(r => r.Visita)
                .WithMany()
                .HasForeignKey(r => r.visId)
                .HasConstraintName("FK_ReporteActividad_Visita")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(r => r.Asistencia)
                .WithMany()
                .HasForeignKey(r => r.asiId)
                .HasConstraintName("FK_ReporteActividad_Asistencia")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(r => r.Evidencia)
                .WithMany()
                .HasForeignKey(r => r.eviId)
                .HasConstraintName("FK_ReporteActividad_Evidencia")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(r => r.Ruta)
                .WithMany()
                .HasForeignKey(r => r.rutId)
                .HasConstraintName("FK_ReporteActividad_Ruta")
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(r => r.Zona)
                .WithMany()
                .HasForeignKey(r => r.zonId)
                .HasConstraintName("FK_ReporteActividad_Zona")
                .OnDelete(DeleteBehavior.NoAction);
        });




        base.OnModelCreating(modelBuilder);
    }
}
