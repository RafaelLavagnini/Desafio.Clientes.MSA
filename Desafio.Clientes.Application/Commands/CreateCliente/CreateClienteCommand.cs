using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.Commands.CreateCliente
{
    public record CreateClienteCommand(string NomeFantasia, string Cnpj) : IRequest<Guid>;
}
