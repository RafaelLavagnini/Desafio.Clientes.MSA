using Desafio.Clientes.Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.Comandos.ExcluirCliente
{
    public class ExcluirClienteHandler : IRequestHandler<ExcluirClienteCommand>
    {
        private readonly IRepositorioCliente _repositorio;

        public ExcluirClienteHandler(IRepositorioCliente repositorio)
        {
            _repositorio = repositorio;
        }

        public Task Handle(ExcluirClienteCommand request, CancellationToken cancellationToken)
        {
            _repositorio.ExcluirAsync(request.Id);
            return Task.CompletedTask;
        }
    }
}
