using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using SegRutContAsis.Business.Interfaces.AsignacionClienteVendedor;
using SegRutContAsis.Business.Interfaces.AsignacionSupervisorVendedor;
using SegRutContAsis.Business.Interfaces.Asistencia;
using SegRutContAsis.Business.Interfaces.Authentication;
using SegRutContAsis.Business.Interfaces.Cliente;
using SegRutContAsis.Business.Interfaces.DireccionCliente;
using SegRutContAsis.Business.Interfaces.Evidencia;
using SegRutContAsis.Business.Interfaces.MarcarLlegadaVisita;
using SegRutContAsis.Business.Interfaces.Notificacion;
using SegRutContAsis.Business.Interfaces.ReportesActividad;
using SegRutContAsis.Business.Interfaces.Rol;
using SegRutContAsis.Business.Interfaces.Ruta;
using SegRutContAsis.Business.Interfaces.SeguimientoVendedor;
using SegRutContAsis.Business.Interfaces.Visita;
using SegRutContAsis.Business.Interfaces.Zona;
using SegRutContAsis.Business.Services;
using SegRutContAsis.Business.Services.Notificacion;
using SegRutContAsis.Business.Services.Reporte;
using SegRutContAsis.Business.Services.Rol;
using System.Text;
 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configurar CORS con política nombrada solo desarrollo

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

// Aumentar límite de tamaño para uploads grandes (form-data)
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB, ajusta según necesites
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
// Configuración de la base de datos
builder.Services.AddDbContext<SegRutContAsisContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 
// Inyección de dependencias
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IAsistenciaService, AsistenciaService>();
builder.Services.AddScoped<IZonaService, ZonaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IRutaService, RutaService>();
builder.Services.AddScoped<IVisitaService, VisitaService>();
builder.Services.AddScoped<IEvidenciaService, EvidenciaService>();
builder.Services.AddScoped<ISeguimientoVendedorService, SeguimientoVendedorService>();
builder.Services.AddScoped<IDireccionClienteService, DireccionClienteService>();
builder.Services.AddScoped<IAsignacionClienteVendedorService, AsignacionClienteVendedorService>();
builder.Services.AddScoped<IMarcarLlegadaVisitaService,  MarcarLlegadaVisitaService>();
builder.Services.AddScoped<IAsignacionSupervisorVendedorService, AsignacionSupervisorVendedorService>();
builder.Services.AddScoped<IReportesActividadService, ReporteActividadService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
 
// Configuración PDF
QuestPDF.Settings.License = LicenseType.Community;
 
// Configuración de JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; 
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
 
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50 MB
});


var app = builder.Build();
 
// En produccion cors
app.UseCors("AllowFrontend");
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
// En produccion habilitar
app.UseHttpsRedirection();
 
// Estado 
app.MapGet("/", () => "Corriendo...");

// Archivos estáticos (PDF/imagenes)
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")
    ),
    RequestPath = "/Uploads"
});

app.UseAuthentication();

app.UseAuthorization();
 
app.MapControllers();
 
app.Run();
