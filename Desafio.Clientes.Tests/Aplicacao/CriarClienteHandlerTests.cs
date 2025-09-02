using System;
using System.Threading;
using System.Threading.Tasks;
using Desafio.Clientes.Application.Comandos.CriarCliente;
using Desafio.Clientes.Application.Interfaces;
using Desafio.Clientes.Domain.Excecoes;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Desafio.Clientes.Tests.Applicacao
{
    /// <summary>
    /// Testes unitários para o CriarClienteHandler.
    /// </summary>
    public class CriarClienteHandlerTests
    {
        [Fact(DisplayName = "Criar Cliente")]
        public async Task CriarCliente()
        {
            var repositorioMock = new Mock<IRepositorioCliente>();
            repositorioMock.Setup(r => r.ExisteCnpjAsync(It.IsAny<string>())).ReturnsAsync(false);
            repositorioMock.Setup(r => r.AdicionarAsync(It.IsAny<Domain.Entidades.Cliente>())).Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<CriarClienteHandler>>();

            var manipulador = new CriarClienteHandler(repositorioMock.Object, loggerMock.Object);
            var comando = new CriarClienteCommand("Empresa X", "12.345.678/0001-95");

            var id = await manipulador.Handle(comando, CancellationToken.None);

            id.Should().NotBe(Guid.Empty);
            repositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Domain.Entidades.Cliente>()), Times.Once);
        }

        [Fact(DisplayName = "Validar CNPJ Cadastrado")]
        public async Task ValidarCNPJCadastrado()
        {
            var repositorioMock = new Mock<IRepositorioCliente>();
            repositorioMock.Setup(r => r.ExisteCnpjAsync(It.IsAny<string>())).ReturnsAsync(true);

            var loggerMock = new Mock<ILogger<CriarClienteHandler>>();

            var manipulador = new CriarClienteHandler(repositorioMock.Object, loggerMock.Object);
            var comando = new CriarClienteCommand("Empresa Y", "12.345.678/0001-95");

            await Assert.ThrowsAsync<ExcecaoDominio>(() => manipulador.Handle(comando, CancellationToken.None));
            repositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Domain.Entidades.Cliente>()), Times.Never);
        }

        [Fact(DisplayName = "Validar Nome Fantasia")]
        public async Task ValidarNomeFantasia()
        {
            var repositorioMock = new Mock<IRepositorioCliente>();
            repositorioMock.Setup(r => r.ExisteCnpjAsync(It.IsAny<string>())).ReturnsAsync(false);

            var loggerMock = new Mock<ILogger<CriarClienteHandler>>();

            var manipulador = new CriarClienteHandler(repositorioMock.Object, loggerMock.Object);
            var comando = new CriarClienteCommand("", "12.345.678/0001-95");

            await Assert.ThrowsAsync<ExcecaoDominio>(() => manipulador.Handle(comando, CancellationToken.None));
            repositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Domain.Entidades.Cliente>()), Times.Never);
        }
    }
}
