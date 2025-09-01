using Desafio.Clientes.Application.DTOs;
using Desafio.Clientes.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.Queries.GetClienteById
{
    public class ObterClientePorIdHandler : IRequestHandler<ObterClientePorIdConsulta, ClienteDto?>
    {
        private readonly IRepositorioCliente _repositorio;

        public ObterClientePorIdHandler(IRepositorioCliente repo)
        {
            _repositorio = repo;
        }

        public async Task<ClienteDto?> Handle(ObterClientePorIdConsulta request, CancellationToken cancellationToken)
        {
            var cliente = await _repositorio.ObterPorIdAsync(request.Id);
            if (cliente == null) return null;

            return new ClienteDto
            {
                Id = cliente.Id,
                NomeFantasia = cliente.NomeFantasia,
                Cnpj = cliente.Cnpj.ToString(),
                Ativo = cliente.Ativo
            };
        }
    }
}
