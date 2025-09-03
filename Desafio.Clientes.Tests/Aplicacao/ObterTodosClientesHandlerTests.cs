using Desafio.Clientes.Application.Consultas.ObterTodosClientes;
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

namespace Desafio.Clientes.Tests.Aplicacao
{
    /// <summary>
    /// Testes unitários para o ObterTodosClienteHandler.
    /// </summary>
    public class ObterTodosClientesHandlerTests
    {
        [Fact(DisplayName = "Retornar lista de ClienteDto mapeada a partir de entidades")]
        public async Task RetornaListaClienteDto()
        {
            var repoMock = new Mock<IRepositorioCliente>();

            var cnpj1 = Cnpj.Criar("12345678000195");
            var cnpj2 = Cnpj.Criar("04252011000110");

            var cliente1 = Cliente.Criar("Empresa A", cnpj1);
            var cliente2 = Cliente.Criar("Empresa B", cnpj2);

            var lista = new List<Cliente> { cliente1, cliente2 };

            repoMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(lista);

            var handler = new ObterTodosClientesHandler(repoMock.Object);

            var resultado = await handler.Handle(new ObterTodosClientesQuery(), CancellationToken.None);

            resultado.Should().HaveCount(2);

            var ids = resultado.Select(x => x.Id).ToList();
            ids.Should().Contain(cliente1.Id);
            ids.Should().Contain(cliente2.Id);

            var dto1 = resultado.First(x => x.Id == cliente1.Id);
            dto1.NomeFantasia.Should().Be("Empresa A");
            dto1.Cnpj.Should().Be(cnpj1.ToString());

            var dto2 = resultado.First(x => x.Id == cliente2.Id);
            dto2.NomeFantasia.Should().Be("Empresa B");
            dto2.Cnpj.Should().Be(cnpj2.ToString());
        }
    }
}
