using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using Desafio.Clientes.Application.Comandos.CriarCliente;
using Desafio.Clientes.Application.Interfaces;
using Desafio.Clientes.Infrastructure.Repositorios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IRepositorioCliente, RepositorioCliente>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CriarClienteHandler).Assembly));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
