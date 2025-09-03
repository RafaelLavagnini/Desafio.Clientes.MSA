using Desafio.Clientes.Application.DTOs;
using Desafio.Clientes.Application.Interfaces;
using Desafio.Clientes.Domain.Excecoes;
using Desafio.Clientes.Domain.ObjetosDeValor;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.Comandos.AtualizarCliente
{
    public class AtualizarClienteHandler : IRequestHandler<AtualizarClienteCommand, Unit>
    {
        private readonly IRepositorioCliente _repositorio;
        private readonly ILogger<AtualizarClienteHandler> _logger;

        public AtualizarClienteHandler(IRepositorioCliente repositorio, ILogger<AtualizarClienteHandler> logger)
        {
            _repositorio = repositorio;
            _logger = logger;
        }

        public async Task<Unit> Handle(AtualizarClienteCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Atualizando cliente Id={Id}", request.Id);

            var cliente = await _repositorio.ObterPorIdAsync(request.Id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente não encontrado: Id={Id}", request.Id);
                throw new KeyNotFoundException("Cliente não encontrado.");
            }

            // Atualiza nome (entidade valida invariantes)
            cliente.AtualizarNomeFantasia(request.NomeFantasia);

            // Valida e cria o VO do CNPJ
            var novoCnpj = Cnpj.Criar(request.Cnpj);

            // Se CNPJ diferente e já existe em outro cliente -> erro de duplicidade
            if (cliente.Cnpj.ToString() != novoCnpj.ToString())
            {
                if (await _repositorio.ExisteCnpjAsync(novoCnpj.ToString()))
                {
                    _logger.LogWarning("CNPJ já cadastrado: {Cnpj}", novoCnpj.ToString());
                    throw new ExcecaoDominio("CNPJ já cadastrado.");
                }

                cliente.AtualizarCnpj(novoCnpj);
            }

            if (cliente.Ativo != request.Ativo)
            {
                if (request.Ativo)
                    cliente.Ativar();
                else
                    cliente.Desativar();
            }

            await _repositorio.AtualizarAsync(cliente);

            _logger.LogInformation("Cliente atualizado com sucesso. Id={Id}", cliente.Id);

            return Unit.Value;
        }
    }
}
