using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Response.MarcarLlegadaVisita
{
    public class MarcarLlegadaVisitaResponseDTO
    {
        public int mlvId { get; set; }
        public int visId { get; set; }
        public TimeSpan? mlvHora { get; set; }
        public decimal mlvLatitud { get; set; }
        public decimal mlvLongitud { get; set; }
        public bool mlvEstadoDel { get; set; } = true;
        public DateTime mlvFechaCreacion { get; set; } = DateTime.Now;

        // Datos adicionales
        public string? NombreCliente { get; set; }
        public string? NombreSucursalCliente { get; set; }
        public decimal? SucursalLatitud { get; set; }      
        public decimal? SucursalLongitud { get; set; }
        public string? NombreVendedor { get; set; }
        public string? UsuarioLogVendedor { get; set; }
        public string? TelefonoVendedor { get; set; }

    }
}


