using Desafio.Clientes.Application.DTOs;
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
        Task<List<Cliente>> ObterTodosAsync();
        Task ExcluirAsync(Guid id);
        Task AtualizarAsync(Cliente cliente);

    }
}
