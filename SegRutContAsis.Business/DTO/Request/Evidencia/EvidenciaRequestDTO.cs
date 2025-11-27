using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace SegRutContAsis.Business.DTO.Request.Evidencia
{
    public class EvidenciaRequestDTO
    {
        public int VisitaId { get; set; }
        public string? eviTipo { get; set; } 
        public string? eviObservaciones { get; set; }

        // Archivo (imagen o PDF)
        public IFormFile? Archivo { get; set; }
    }
}
