using Desafio.Clientes.Application.Comandos.AtualizarCliente;
using Desafio.Clientes.Application.Interfaces;
using Desafio.Clientes.Domain.Entidades;
using Desafio.Clientes.Domain.ObjetosDeValor;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Tests.Aplicacao
{
    /// <summary>
    /// Testes unitários para o AtualizarClienteHandler.
    /// </summary>
    public class AtualizarClienteHandlerTests
    {
        [Fact(DisplayName = "Atualizar Ativo e NomeFantasia")]
        public async Task AtualizaAtivoENome()
        {
            var repoMock = new Mock<IRepositorioCliente>();
            var loggerMock = new Mock<ILogger<AtualizarClienteHandler>>();

            var cnpj = Cnpj.Criar("12345678000195");
            var cliente = Cliente.Criar("Empresa Teste", cnpj);
            var id = cliente.Id;

            repoMock.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync(cliente);
            repoMock.Setup(r => r.ExisteCnpjAsync(It.IsAny<string>())).ReturnsAsync(false);
            repoMock.Setup(r => r.AtualizarAsync(It.IsAny<Cliente>())).Returns(Task.CompletedTask);

            var handler = new AtualizarClienteHandler(repoMock.Object, loggerMock.Object);

            var comando = new AtualizarClienteCommand(id, "Empresa Teste Atualizada", "12345678000195", false);

            var result = await handler.Handle(comando, CancellationToken.None);

            result.Should().Be(Unit.Value);
            cliente.NomeFantasia.Should().Be("Empresa Teste Atualizada");
            cliente.Ativo.Should().BeFalse();
            cliente.Id.Should().Be(id);
            repoMock.Verify(r => r.AtualizarAsync(It.Is<Cliente>(c => c.Id == id && c.NomeFantasia == "Empresa Teste Atualizada" && c.Ativo == false)), Times.Once);
        }

        [Fact(DisplayName = "KeyNotFoundException para cliente que não existe")]
        public async Task ClienteNaoExiste()
        {
            var repoMock = new Mock<IRepositorioCliente>();
            var loggerMock = new Mock<ILogger<AtualizarClienteHandler>>();
            var id = Guid.NewGuid();

            repoMock.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync((Cliente?)null);

            var handler = new AtualizarClienteHandler(repoMock.Object, loggerMock.Object);
            var comando = new AtualizarClienteCommand(id, "Nome", "12345678000195", true);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(comando, CancellationToken.None));
            repoMock.Verify(r => r.AtualizarAsync(It.IsAny<Cliente>()), Times.Never);
        }
    }
}
