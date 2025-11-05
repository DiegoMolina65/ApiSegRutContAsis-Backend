using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Domain.Entities
{
    public class Cliente
    {
        public int clId { get; set; }
        public DateTime clFechaCreacion { get; set; } = DateTime.Now;
        public string clNombreCompleto { get; set; } = null!;
        public string clCarnetIdentidad { get; set; } = null!;
        public string clNitCliente { get; set; } = null!;
        public string? clTipoCliente { get; set; }
        public string clTelefono { get; set; } = null!;
        public bool clEstadoDel { get; set; } = true;

        // Relaciones
        public ICollection<AsignacionClienteVendedor> AsignacionesClienteVendedor { get; set; } = new List<AsignacionClienteVendedor>();
        public ICollection<DireccionCliente> DireccionesCliente { get; set; } = new List<DireccionCliente>();
    }
}
