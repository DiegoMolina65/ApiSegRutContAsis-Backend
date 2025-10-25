using System;
using System.Collections.Generic;

namespace SegRutContAsis.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public string NombreCompleto { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string NitEmpleado { get; set; }
    public string CarnetIdentidad { get; set; } 
    public string UsuarioLog { get; set; }
    public string ContrasenaLog { get; set; }
    public bool EstadoDel { get; set; }

    // Navegación
    public ICollection<UsuarioRol> UsuarioRoles { get; set; }
    public Administrador Administrador { get; set; }
    public Supervisor Supervisor { get; set; }
    public Vendedor Vendedor { get; set; }
}
