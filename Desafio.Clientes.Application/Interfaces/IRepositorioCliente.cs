using Desafio.Clientes.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.Interfaces
{
    public interface IRepositorioCliente
    {
        Task AdicionarAsync(Cliente cliente);
        Task<Cliente?> ObterPorIdAsync(Guid id);
        Task<bool> ExisteCnpjAsync(string cnpj);
    }
}
