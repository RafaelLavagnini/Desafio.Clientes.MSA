using Desafio.Clientes.Application.Interfaces;
using Desafio.Clientes.Domain.Entities;
using Desafio.Clientes.Domain.Exceptions;
using Desafio.Clientes.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.Commands.CreateCliente
{
    public class CreateClienteHandler : IRequestHandler<CreateClienteCommand, Guid>
    {
        private readonly IClienteRepository _repo;

        public CreateClienteHandler(IClienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
        {
            var cnpjVo = Cnpj.Criar(request.Cnpj);

            if (await _repo.ExistsByCnpjAsync(cnpjVo.ToString()))
                throw new ExcecaoDominio("CNPJ já cadastrado.");

            var cliente = Cliente.Criar(request.NomeFantasia, cnpjVo);

            await _repo.AddAsync(cliente);

            return cliente.Id;
        }
    }
}
