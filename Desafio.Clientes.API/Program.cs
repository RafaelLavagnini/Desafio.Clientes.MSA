using Desafio.Clientes.API.Middlewares;
using Desafio.Clientes.Application.Behaviors;
using Desafio.Clientes.Application.Comandos.CriarCliente;
using Desafio.Clientes.Application.Consultas.ObterClientePorId;
using Desafio.Clientes.Application.Interfaces;
using Desafio.Clientes.Infrastructure.Repositorios;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IRepositorioCliente, RepositorioCliente>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CriarClienteHandler).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(CriarClienteCommandValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<MiddlewareTratamentoExcecoes>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Programa { }
