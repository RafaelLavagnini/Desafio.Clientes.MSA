using MediatR;
using System;

namespace Desafio.Clientes.Application.Comandos.ExcluirCliente
{
    public record ExcluirClienteCommand(Guid Id) : IRequest;
}
