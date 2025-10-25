using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Vendedor
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public bool EstadoDel { get; set; } = true;

        // Relaciones
        public ICollection<Ruta> Rutas { get; set; } = new List<Ruta>();
        public ICollection<AsignacionClienteVendedor> AsignacionesClienteVendedor { get; set; } = new List<AsignacionClienteVendedor>();

    }

}
