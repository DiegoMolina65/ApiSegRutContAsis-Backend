using System;
using System.Collections.Generic;

namespace SegRutContAsis.Domain.Entities;

public class Rol
{
    public int Id { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public bool EstadoDel { get; set; }

    // Navegación
    public ICollection<UsuarioRol> UsuarioRoles { get; set; }
}
