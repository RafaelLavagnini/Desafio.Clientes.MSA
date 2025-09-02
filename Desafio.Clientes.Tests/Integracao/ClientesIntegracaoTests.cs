using Desafio.Clientes.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Desafio.Clientes.Tests.Integracao
{
    /// <summary>
    /// Testes de integração para o fluxo HTTP completo:
    /// </summary>
    public class ClientesIntegracaoTests : IClassFixture<WebApplicationFactory<Programa>>
    {
        private readonly HttpClient _clienteHttp;

        public ClientesIntegracaoTests(WebApplicationFactory<Programa> factory)
        {
            _clienteHttp = factory.CreateClient();
        }

        [Fact(DisplayName = "POST /api/clientes cria cliente e GET retorna o DTO")]
        public async Task PostEGetCriaClienteERetornaDto()
        {
            var payload = new { nomeFantasia = "Empresa Integra", cnpj = "12.345.678/0001-95" };
            var respostaPost = await _clienteHttp.PostAsJsonAsync("/api/clientes", payload);

            respostaPost.StatusCode.Should().Be(HttpStatusCode.Created);
            respostaPost.Headers.Location.Should().NotBeNull();

            var dto = await respostaPost.Content.ReadFromJsonAsync<ClienteDto>();
            dto.Should().NotBeNull();
            dto!.NomeFantasia.Should().Be("Empresa Integra");

            var respostaGet = await _clienteHttp.GetAsync(respostaPost.Headers.Location);
            respostaGet.StatusCode.Should().Be(HttpStatusCode.OK);

            var dtoGet = await respostaGet.Content.ReadFromJsonAsync<ClienteDto>();
            dtoGet!.Id.Should().Be(dto.Id);
        }

        [Fact(DisplayName = "POST CNPJ inválido")]
        public async Task PostInvalido()
        {
            var payload = new { nomeFantasia = "Empresa", cnpj = "111" };
            var resposta = await _clienteHttp.PostAsJsonAsync("/api/clientes", payload);

            resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            resposta.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");

            var corpo = await resposta.Content.ReadAsStringAsync();

            using var doc = System.Text.Json.JsonDocument.Parse(corpo);
            var root = doc.RootElement;
            var detail = root.GetProperty("detail").GetString();

            detail.Should().Contain("CNPJ inválido");
        }

        [Fact(DisplayName = "GET inexistente")]
        public async Task GetInexistente()
        {
            var resposta = await _clienteHttp.GetAsync($"/api/clientes/{System.Guid.NewGuid()}");
            resposta.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
