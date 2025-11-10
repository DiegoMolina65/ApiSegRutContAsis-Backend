using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.Visita
{
    public class VisitaResponseDTO
    {
        public int visId { get; set; }
        public int rutId { get; set; }
        public int dirClId { get; set; }
        public DateTime visFechaCreacion { get; set; }
        public bool visEstadoDel { get; set; }
        public string? visComentario { get; set; }

        // Información adicional 
        public string? NombreCliente { get; set; }
        public string? NombreSucursalCliente { get; set; }
        public decimal? SucursalLatitud { get; set; }
        public decimal? SucursalLongitud { get; set; }
        public string? NombreZona { get; set; }
        public string? Direccion { get; set; }
        public string? NombreVendedor { get; set; }
    }

}
