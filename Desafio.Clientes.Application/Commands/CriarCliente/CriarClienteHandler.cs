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

namespace Desafio.Clientes.Application.Commands.CriarCliente
{
    /// <summary>
    /// Manipulador do comando de criação de cliente.
    /// </summary>
    public class CriarClienteHandler : IRequestHandler<CriarClienteCommand, Guid>
    {
        private readonly IRepositorioCliente _repo;

        public CriarClienteHandler(IRepositorioCliente repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CriarClienteCommand request, CancellationToken tokenDeCancelamento)
        {
            var cnpjVo = Cnpj.Criar(request.Cnpj);

            if (await _repo.ExisteCnpjAsync(cnpjVo.ToString()))
                throw new ExcecaoDominio("CNPJ já cadastrado.");

            var cliente = Cliente.Criar(request.NomeFantasia, cnpjVo);

            await _repo.AdicionarAsync(cliente);

            return cliente.Id;
        }
    }
}
