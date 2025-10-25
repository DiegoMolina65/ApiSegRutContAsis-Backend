using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string NombreCompleto { get; set; } = null!;
        public string CarnetIdentidad { get; set; } = null!;
        public string NitCliente { get; set; } = null!;
        public string? TipoCliente { get; set; }
        public string Telefono { get; set; } = null!;
        public bool EstadoDel { get; set; } = true;

        // Relaciones
        public ICollection<AsignacionClienteVendedor> AsignacionesClienteVendedor { get; set; } = new List<AsignacionClienteVendedor>();
        public ICollection<DireccionCliente> DireccionesCliente { get; set; } = new List<DireccionCliente>();
    }
}
