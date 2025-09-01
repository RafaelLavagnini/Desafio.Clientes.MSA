using Desafio.Clientes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.Interfaces
{
    public interface IClienteRepository
    {
        Task AddAsync(Cliente cliente);
        Task<Cliente?> GetByIdAsync(Guid id);
        Task<bool> ExistsByCnpjAsync(string cnpj);
    }
}
