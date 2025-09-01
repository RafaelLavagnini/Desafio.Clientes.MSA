using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.Commands.CriarCliente
{
    /// <summary>
    /// Comando para criar cliente — retorna Guid do cliente criado.
    /// </summary>
    public record CriarClienteCommand(string NomeFantasia, string Cnpj) : IRequest<Guid>;
}
