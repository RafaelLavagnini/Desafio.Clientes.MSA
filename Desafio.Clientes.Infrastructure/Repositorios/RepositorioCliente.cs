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
        public Task<List<Cliente>> ObterTodosAsync()
        {
            var clientes = _store.Values.ToList();
            return Task.FromResult(clientes);
        }

        public Task ExcluirAsync(Guid id)
        {
            _store.TryRemove(id, out _);
            return Task.CompletedTask;
        }

        public Task AtualizarAsync(Cliente cliente)
        {
            if (!_store.ContainsKey(cliente.Id))
                throw new KeyNotFoundException("Cliente não encontrado.");

            _store[cliente.Id] = cliente;
            return Task.CompletedTask;
        }

    }
}
