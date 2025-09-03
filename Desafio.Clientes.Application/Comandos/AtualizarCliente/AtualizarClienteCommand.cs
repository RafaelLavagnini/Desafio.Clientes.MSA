using MediatR;
using System;

namespace Desafio.Clientes.Application.Comandos.AtualizarCliente
{
    public record AtualizarClienteCommand(Guid Id, string NomeFantasia, string Cnpj, bool Ativo) : IRequest<Unit>;
}
