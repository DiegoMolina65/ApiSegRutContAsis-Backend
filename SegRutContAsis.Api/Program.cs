using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SegRutContAsis.Business.Interfaces;
using SegRutContAsis.Business.Interfaces.AsignacionClienteVendedor;
using SegRutContAsis.Business.Interfaces.Asistencia;
using SegRutContAsis.Business.Interfaces.Authentication;
using SegRutContAsis.Business.Interfaces.Cliente;
using SegRutContAsis.Business.Interfaces.DireccionCliente;
using SegRutContAsis.Business.Interfaces.Evidencia;
using SegRutContAsis.Business.Interfaces.Rol;
using SegRutContAsis.Business.Interfaces.Ruta;
using SegRutContAsis.Business.Interfaces.SeguimientoVendedor;
using SegRutContAsis.Business.Interfaces.Visita;
using SegRutContAsis.Business.Interfaces.Zona;
using SegRutContAsis.Business.Services;
using SegRutContAsis.Business.Services.Rol;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configurar CORS con pol�tica nombrada solo desarrollo
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",    // tu front (http)
                "https://localhost:5173"    // por si usas https en dev
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); 
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuraci�n de la base de datos
builder.Services.AddDbContext<SegRutContAsisContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyecci�n de dependencias
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

// Configuraci�n de JWT
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

var app = builder.Build();

// En produccion quitar
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// En produccion habilitar
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
