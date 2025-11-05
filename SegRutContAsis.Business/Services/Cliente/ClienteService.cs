using Microsoft.EntityFrameworkCore;
using SegRutContAsis.Business.DTO.Request.Cliente;
using SegRutContAsis.Business.DTO.Response.Cliente;
using SegRutContAsis.Business.Interfaces.Cliente;
using SegRutContAsis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegRutContAsis.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly SegRutContAsisContext _context;
        public ClienteService(SegRutContAsisContext context)
        {
            _context = context;
        }

        // Obtener todos los clientes
        public async Task<List<ClienteResponseDTO>> ObtenerClientes()
        {
            var clientes = await _context.Cliente
                .Where(c => c.clEstadoDel)
                .ToListAsync();

            return clientes.Select(c => new ClienteResponseDTO
            {
                clId = c.clId,
                clNombreCompleto = c.clNombreCompleto,
                clCarnetIdentidad = c.clCarnetIdentidad,
                clNitCliente = c.clNitCliente,
                clTipoCliente = c.clTipoCliente,
                clTelefono = c.clTelefono,
            }).ToList();
        }

        // Obtener cliente por ID
        public async Task<ClienteResponseDTO?> ObtenerClientePorId(int id)
        {
            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(c => c.clId == id && c.clEstadoDel);

            if (cliente == null) return null;

            return new ClienteResponseDTO
            {
                clId = cliente.clId,
                clNombreCompleto = cliente.clNombreCompleto,
                clCarnetIdentidad = cliente.clCarnetIdentidad,
                clNitCliente = cliente.clNitCliente,
                clTipoCliente = cliente.clTipoCliente,
                clTelefono = cliente.clTelefono,
            };
        }

        // Crear cliente
        public async Task<ClienteResponseDTO> CrearCliente(ClienteRequestDTO request)
        {
            bool existe = await _context.Cliente.AnyAsync(c => c.clCarnetIdentidad == request.clCarnetIdentidad);
            if (existe)
                throw new Exception("Ya existe un cliente con ese carnet de identidad.");

            var cliente = new Cliente
            {
                clNombreCompleto = request.clNombreCompleto,
                clCarnetIdentidad = request.clCarnetIdentidad,
                clNitCliente = request.clNitCliente,
                clTipoCliente = request.clTipoCliente,
                clTelefono = request.clTelefono,
            };

            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();


            return new ClienteResponseDTO
            {
                clId = cliente.clId,
                clNombreCompleto = cliente.clNombreCompleto,
                clCarnetIdentidad = cliente.clCarnetIdentidad,
                clNitCliente = cliente.clNitCliente,
                clTipoCliente = cliente.clTipoCliente,
                clTelefono = cliente.clTelefono,
            };
        }

        // Actualizar cliente
        public async Task<ClienteResponseDTO?> ActualizarCliente(int id, ClienteRequestDTO request)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null || !cliente.clEstadoDel) return null;

            cliente.clNombreCompleto = request.clNombreCompleto;
            cliente.clTipoCliente = request.clTipoCliente;
            cliente.clNitCliente = request.clNitCliente;
            cliente.clCarnetIdentidad = request.clCarnetIdentidad;
            cliente.clTelefono = request.clTelefono;

            await _context.SaveChangesAsync();

            return new ClienteResponseDTO
            {
                clId = cliente.clId,
                clNombreCompleto = cliente.clNombreCompleto,
                clCarnetIdentidad = cliente.clCarnetIdentidad,
                clNitCliente = cliente.clNitCliente,
                clTipoCliente = cliente.clTipoCliente,
                clTelefono = cliente.clTelefono,
            };
        }

        // Deshabilitar cliente
        public async Task<bool> DeshabilitarCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null || !cliente.clEstadoDel)
                return false;

            cliente.clEstadoDel = false;
            _context.Cliente.Update(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
