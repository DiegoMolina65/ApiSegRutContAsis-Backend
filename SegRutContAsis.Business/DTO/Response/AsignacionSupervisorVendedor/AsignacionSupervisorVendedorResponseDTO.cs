using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.DTO.Request.AsignacionSupervisorVendedor
{
    public class AsignacionSupervisorVendedorResponseDTO
    {
        public int asvId { get; set; }
        public int supId { get; set; }
        public string NombreSupervisor { get; set; }
        public int venId { get; set; }
        public string NombreVendedor { get; set; }  
        public DateTime asvFechaCreacion { get; set; } = DateTime.Now;
        public bool asvEstadoDel { get; set; } = true;
    }
}
