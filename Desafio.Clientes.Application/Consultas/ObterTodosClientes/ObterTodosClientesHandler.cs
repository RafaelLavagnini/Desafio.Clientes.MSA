using Desafio.Clientes.Application.DTOs;
using Desafio.Clientes.Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.Consultas.ObterTodosClientes
{
    public class ObterTodosClientesHandler : IRequestHandler<ObterTodosClientesQuery, List<ClienteDto>>
    {
        private readonly IRepositorioCliente _repositorio;

        public ObterTodosClientesHandler(IRepositorioCliente repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<ClienteDto>> Handle(ObterTodosClientesQuery request, CancellationToken cancellationToken)
        {
            var clientes = await _repositorio.ObterTodosAsync();
            var clientesDto = new List<ClienteDto>();
            foreach (var cliente in clientes)
            {
                clientesDto.Add(new ClienteDto
                {
                    Id = cliente.Id,
                    NomeFantasia = cliente.NomeFantasia,
                    Cnpj = cliente.Cnpj.ToString(),
                    Ativo = cliente.Ativo
                });
            }
            return clientesDto;
        }

    }
}
