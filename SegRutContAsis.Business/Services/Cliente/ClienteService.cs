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

        // =========================
        // Obtener todos los clientes
        // =========================
        public async Task<List<ClienteResponseDTO>> ObtenerClientes()
        {
            var clientes = await _context.Cliente
                .Where(c => c.EstadoDel)
                .ToListAsync();

            return clientes.Select(c => new ClienteResponseDTO
            {
                Id = c.Id,
                NombreCompleto = c.NombreCompleto,
                CarnetIdentidad = c.CarnetIdentidad,
                NitCliente = c.NitCliente,
                TipoCliente = c.TipoCliente,
                Telefono = c.Telefono,
            }).ToList();
        }

        // =========================
        // Obtener cliente por ID
        // =========================
        public async Task<ClienteResponseDTO?> ObtenerClientePorId(int id)
        {
            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(c => c.Id == id && c.EstadoDel);

            if (cliente == null) return null;

            return new ClienteResponseDTO
            {
                Id = cliente.Id,
                NombreCompleto = cliente.NombreCompleto,
                CarnetIdentidad = cliente.CarnetIdentidad,
                NitCliente = cliente.NitCliente,
                TipoCliente = cliente.TipoCliente,
                Telefono = cliente.Telefono,
            };
        }

        // =========================
        // Crear cliente
        // =========================
        public async Task<ClienteResponseDTO> CrearCliente(ClienteRequestDTO request)
        {
            bool existe = await _context.Cliente.AnyAsync(c => c.CarnetIdentidad == request.CarnetIdentidad);
            if (existe)
                throw new Exception("Ya existe un cliente con ese carnet de identidad.");

            var cliente = new Cliente
            {
                NombreCompleto = request.NombreCompleto,
                CarnetIdentidad = request.CarnetIdentidad,
                NitCliente = request.NitCliente,
                TipoCliente = request.TipoCliente,
                Telefono = request.Telefono,
            };

            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();


            return new ClienteResponseDTO
            {
                Id = cliente.Id,
                NombreCompleto = cliente.NombreCompleto,
                CarnetIdentidad = cliente.CarnetIdentidad,
                NitCliente = cliente.NitCliente,
                TipoCliente = cliente.TipoCliente,
                Telefono = cliente.Telefono,
            };
        }

        // =========================
        // Actualizar cliente
        // =========================
        public async Task<ClienteResponseDTO?> ActualizarCliente(int id, ClienteRequestDTO request)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null || !cliente.EstadoDel) return null;

            cliente.NombreCompleto = request.NombreCompleto;
            cliente.TipoCliente = request.TipoCliente;
            cliente.NitCliente = request.NitCliente;
            cliente.CarnetIdentidad = request.CarnetIdentidad;
            cliente.Telefono = request.Telefono;

            await _context.SaveChangesAsync();

            return new ClienteResponseDTO
            {
                Id = cliente.Id,
                NombreCompleto = cliente.NombreCompleto,
                CarnetIdentidad = cliente.CarnetIdentidad,
                NitCliente = cliente.NitCliente,
                TipoCliente = cliente.TipoCliente,
                Telefono = cliente.Telefono,
            };
        }

        // =========================
        // Deshabilitar cliente
        // =========================
        public async Task<bool> DeshabilitarCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null || !cliente.EstadoDel)
                return false;

            cliente.EstadoDel = false;
            _context.Cliente.Update(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
