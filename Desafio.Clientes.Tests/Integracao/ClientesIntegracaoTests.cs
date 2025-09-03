using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Desafio.Clientes.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Desafio.Clientes.Tests.Integracao
{
    /// <summary>
    /// Testes de integração dos métodos
    /// </summary>
    public class ClientesIntegracaoTests : IClassFixture<WebApplicationFactory<Programa>>
    {
        private readonly HttpClient _client;

        public ClientesIntegracaoTests(WebApplicationFactory<Programa> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "Fluxo completo")]
        public async Task TesteCompleto()
        {
            var criarPayload = new { nomeFantasia = "Empresa", cnpj = "12.345.678/0001-95" };
            var postResp = await _client.PostAsJsonAsync("/api/clientes", criarPayload);
            postResp.StatusCode.Should().Be(HttpStatusCode.Created);

            var criadoDto = await postResp.Content.ReadFromJsonAsync<ClienteDto>();
            criadoDto.Should().NotBeNull();
            var id = criadoDto!.Id;
            var getTodosResp = await _client.GetAsync("/api/clientes");
            getTodosResp.StatusCode.Should().Be(HttpStatusCode.OK);

            var lista = await getTodosResp.Content.ReadFromJsonAsync<ClienteDto[]>();
            lista.Should().NotBeNull();
            lista!.Select(x => x.Id).Should().Contain(id);

            var atualizarPayload = new { nomeFantasia = "Empresa Atualizada", cnpj = "12.345.678/0001-95", ativo = false };
            var putResp = await _client.PutAsJsonAsync($"/api/clientes/{id}", atualizarPayload);
            putResp.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getPorId = await _client.GetAsync($"/api/clientes/{id}");
            getPorId.StatusCode.Should().Be(HttpStatusCode.OK);
            var dtoDepois = await getPorId.Content.ReadFromJsonAsync<ClienteDto>();
            dtoDepois.Should().NotBeNull();
            dtoDepois!.NomeFantasia.Should().Be("Empresa Atualizada");
            dtoDepois.Ativo.Should().BeFalse();

            var deleteResp = await _client.DeleteAsync($"/api/clientes/{id}");
            deleteResp.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getDepoisDelete = await _client.GetAsync($"/api/clientes/{id}");
            getDepoisDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact(DisplayName = "POST e GET")]
        public async Task PostEGetCriarCliente()
        {
            var payload = new { nomeFantasia = "Empresa", cnpj = "19131243000197" };
            var respostaPost = await _client.PostAsJsonAsync("/api/clientes", payload);

            respostaPost.StatusCode.Should().Be(HttpStatusCode.Created);
            respostaPost.Headers.Location.Should().NotBeNull();

            var dto = await respostaPost.Content.ReadFromJsonAsync<ClienteDto>();
            dto.Should().NotBeNull();
            dto!.NomeFantasia.Should().Be(payload.nomeFantasia);

            var respostaGet = await _client.GetAsync(respostaPost.Headers.Location);
            respostaGet.StatusCode.Should().Be(HttpStatusCode.OK);

            var dtoGet = await respostaGet.Content.ReadFromJsonAsync<ClienteDto>();
            dtoGet!.Id.Should().Be(dto.Id);
        }
    }
}
