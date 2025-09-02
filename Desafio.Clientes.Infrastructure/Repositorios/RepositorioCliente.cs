using Desafio.Clientes.Application.Interfaces;
using Desafio.Clientes.Application.DTOs;
using Desafio.Clientes.Domain.Entidades;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Infrastructure.Repositorios
{
    /// <summary>
    /// Implementação do repositório em memória.
    /// </summary>
    public class RepositorioCliente : IRepositorioCliente
    {
        private static readonly ConcurrentDictionary<Guid, Cliente> _store = new();

        public Task AdicionarAsync(Cliente cliente)
        {
            if (!_store.TryAdd(cliente.Id, cliente))
                throw new Exception("Erro ao adicionar cliente no repositório em memória.");

            return Task.CompletedTask;
        }

        public Task<Cliente?> ObterPorIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var cliente);
            return Task.FromResult(cliente);
        }

        public Task<bool> ExisteCnpjAsync(string cnpj)
        {
            var existe = _store.Values.Any(c => c.Cnpj.ToString() == cnpj);
            return Task.FromResult(existe);
        }
    }
}
