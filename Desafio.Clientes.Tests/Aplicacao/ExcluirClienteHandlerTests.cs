using Desafio.Clientes.Application.Comandos.ExcluirCliente;
using Desafio.Clientes.Application.Interfaces;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Tests.Aplicacao
{
    /// <summary>
    /// Testes unitários para o ExcluirClienteHandler.
    /// </summary>
    public class ExcluirClienteHandlerTests
    {
        [Fact(DisplayName = "Chama ExcluirAsync e retornar Unit")]
        public async Task ExcluirAsync()
        {
            var repoMock = new Mock<IRepositorioCliente>();
            var id = Guid.NewGuid();

            repoMock.Setup(r => r.ExcluirAsync(id)).Returns(Task.CompletedTask);

            var handler = new ExcluirClienteHandler(repoMock.Object);

            await handler.Handle(new ExcluirClienteCommand(id), CancellationToken.None);

            repoMock.Verify(r => r.ExcluirAsync(id), Times.Once);
        }
    }
}
