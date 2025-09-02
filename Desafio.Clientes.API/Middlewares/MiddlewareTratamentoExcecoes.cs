using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Desafio.Clientes.Domain.Excecoes;

namespace Desafio.Clientes.API.Middlewares
{
    /// <summary>
    /// Middleware responsável por transformar ExcecaoDominio em 400 BadRequest
    /// </summary>
    public class MiddlewareTratamentoExcecoes
    {
        private readonly RequestDelegate _next;

        public MiddlewareTratamentoExcecoes(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ExcecaoDominio ex)
            {
                httpContext.Response.ContentType = "application/problem+json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var problem = new
                {
                    type = "https://example.com/probs/validacao-dominio",
                    title = "Erro de validação de domínio",
                    status = 400,
                    detail = ex.Message
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
            catch (Exception)
            {
                httpContext.Response.ContentType = "application/problem+json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problem = new
                {
                    type = "https://example.com/probs/internal",
                    title = "Erro interno",
                    status = 500,
                    detail = "Ocorreu um erro inesperado."
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}
