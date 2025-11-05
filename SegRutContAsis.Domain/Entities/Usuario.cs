using System;
using System.Collections.Generic;

namespace SegRutContAsis.Domain.Entities;

public class Usuario
{
    public int usrId { get; set; }
    public DateTime usrFechaCreacion { get; set; } = DateTime.Now;
    public string usrNombreCompleto { get; set; }
    public string usrCorreo { get; set; }
    public string usrTelefono { get; set; }
    public string usrNitEmpleado { get; set; }
    public string usrCarnetIdentidad { get; set; } 
    public string usrUsuarioLog { get; set; }
    public string usrContrasenaLog { get; set; }
    public bool usrEstadoDel { get; set; }

    // Navegación
    public ICollection<UsuarioRol> UsuarioRoles { get; set; }
    public Administrador Administrador { get; set; }
    public Supervisor Supervisor { get; set; }
    public Vendedor Vendedor { get; set; }
}
