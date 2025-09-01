using Desafio.Clientes.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.Queries.GetClienteById
{
    public record GetClienteByIdQuery(Guid Id) : IRequest<ClienteDto?>;
}
