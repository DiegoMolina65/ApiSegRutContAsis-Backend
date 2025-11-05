using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Response.Cliente;
using SegRutContAsis.Business.DTO.Response.Rol;
using SegRutContAsis.Business.Interfaces.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services.Rol
{
    public class RolService : IRolService
    {
        private readonly SegRutContAsisContext _context;
        public RolService(SegRutContAsisContext context)
        {
            _context = context;
        }

        // Obtener todos los roles
        public async Task<List<RolResponseDTO>> ObtenerRoles() 
        {
            var roles = await _context.Rol
                .Where(r => r.rolEstadoDel)
                .ToListAsync();

            return roles.Select(r => new RolResponseDTO
            {
                rolId = r.rolId,
                rolNombre = r.rolNombre,
                rolDescripcion = r.rolDescripcion,
            }).ToList();
        }
    }
}