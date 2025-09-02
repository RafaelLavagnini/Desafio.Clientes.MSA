using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Desafio.Clientes.Application.Interfaces;
using Desafio.Clientes.Domain.Entidades;
using Desafio.Clientes.Domain.Excecoes;
using Desafio.Clientes.Domain.ObjetosDeValor;

namespace Desafio.Clientes.Application.Comandos.CriarCliente
{
    /// <summary>
    /// Manipulador do comando de criação de cliente.
    /// </summary>
    public class CriarClienteHandler : IRequestHandler<CriarClienteCommand, Guid>
    {
        private readonly IRepositorioCliente _repo;
        private readonly ILogger<CriarClienteHandler> _logger;

        public CriarClienteHandler(IRepositorioCliente repo, ILogger<CriarClienteHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Guid> Handle(CriarClienteCommand solicita, CancellationToken tokenDeCancelamento)
        {
            _logger.LogInformation("Iniciando criação de cliente: {Nome}", solicita.NomeFantasia);

            var cnpj = Cnpj.Criar(solicita.Cnpj);

            if (await _repo.ExisteCnpjAsync(cnpj.ToString()))
            {
                _logger.LogWarning("Tentativa de criar cliente com CNPJ já existente: {Cnpj}", cnpj.ToString());
                throw new ExcecaoDominio("CNPJ já cadastrado.");
            }

            var cliente = Cliente.Criar(solicita.NomeFantasia, cnpj);

            await _repo.AdicionarAsync(cliente);

            _logger.LogInformation("Cliente criado com sucesso. Id={Id} Cnpj={Cnpj}", cliente.Id, cliente.Cnpj.ToString());

            return cliente.Id;
        }
    }
}
