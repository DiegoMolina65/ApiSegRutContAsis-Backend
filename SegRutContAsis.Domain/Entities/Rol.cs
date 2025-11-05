using System;
using System.Collections.Generic;

namespace SegRutContAsis.Domain.Entities;

public class Rol
{
    public int rolId { get; set; }
    public DateTime rolFechaCreacion { get; set; } = DateTime.Now;
    public string rolNombre { get; set; }
    public string rolDescripcion { get; set; }
    public bool rolEstadoDel { get; set; }

    // Navegación
    public ICollection<UsuarioRol> UsuarioRoles { get; set; }
}
