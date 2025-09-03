using Desafio.Clientes.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Desafio.Clientes.Application.Consultas.ObterTodosClientes
{
    public record ObterTodosClientesQuery : IRequest<List<ClienteDto>>;
}
