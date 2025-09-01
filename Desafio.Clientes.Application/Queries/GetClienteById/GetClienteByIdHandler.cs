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
    public class GetClienteByIdHandler : IRequestHandler<GetClienteByIdQuery, ClienteDto?>
    {
        private readonly IClienteRepository _repo;

        public GetClienteByIdHandler(IClienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<ClienteDto?> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _repo.GetByIdAsync(request.Id);
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
