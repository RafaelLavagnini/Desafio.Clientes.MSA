using Desafio.Clientes.Application.Consultas.ObterClientePorId;
using Desafio.Clientes.Application.Interfaces;
using Desafio.Clientes.Domain.Entidades;
using Desafio.Clientes.Domain.ObjetosDeValor;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Tests.Applicacao
{
    public class ObterClientePorIdTests
    {
        [Fact(DisplayName = "Retornar ClienteDto")]
        public async Task RetornarCliente()
        {
            var repositorioMock = new Mock<IRepositorioCliente>();
            var cnpj = Cnpj.Criar("12.345.678/0001-95");
            var cliente = Cliente.Criar("Empresa Teste", cnpj);
            repositorioMock.Setup(r => r.ObterPorIdAsync(cliente.Id)).ReturnsAsync(cliente);
            var manipulador = new ObterClientePorIdHandler(repositorioMock.Object);

            var dto = await manipulador.Handle(new ObterClientePorIdConsulta(cliente.Id), CancellationToken.None);

            dto.Should().NotBeNull();
            dto!.Id.Should().Be(cliente.Id);
            dto.NomeFantasia.Should().Be(cliente.NomeFantasia);
            dto.Cnpj.Should().Be(cliente.Cnpj.ToString());
            dto.Ativo.Should().Be(cliente.Ativo);
        }

        [Fact(DisplayName = "Retornar ID nulo")]
        public async Task RetornarIdNulo()
        {
            var repositorioMock = new Mock<IRepositorioCliente>();
            repositorioMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entidades.Cliente?)null);
            var manipulador = new ObterClientePorIdHandler(repositorioMock.Object);

            var dto = await manipulador.Handle(new ObterClientePorIdConsulta(Guid.NewGuid()), CancellationToken.None);

            dto.Should().BeNull();
        }
    }
}
