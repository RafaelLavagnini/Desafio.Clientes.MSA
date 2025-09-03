using Desafio.Clientes.Application.Comandos.AtualizarCliente;
using Desafio.Clientes.Application.Comandos.CriarCliente;
using Desafio.Clientes.Application.Comandos.ExcluirCliente;
using Desafio.Clientes.Application.Consultas.ObterClientePorId;
using Desafio.Clientes.Application.Consultas.ObterTodosClientes;
using Desafio.Clientes.API.Models;
using Desafio.Clientes.Application.DTOs;
using Desafio.Clientes.Domain.Excecoes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Desafio.Clientes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] RequisicaoCriarCliente request)
        {
            var id = await _mediator.Send(new CriarClienteCommand(request.NomeFantasia, request.Cnpj));
            var clienteDto = await _mediator.Send(new ObterClientePorIdConsulta(id));

            return CreatedAtAction(nameof(ObterPorId), new { id = clienteDto!.Id }, clienteDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var cliente = await _mediator.Send(new ObterClientePorIdConsulta(id));
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var clientes = await _mediator.Send(new ObterTodosClientesQuery());
            return Ok(clientes);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                await _mediator.Send(new ExcluirClienteCommand(id));
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] RequisicaoAtualizarCliente request)
        {
            try
            {
                await _mediator.Send(new AtualizarClienteCommand(id, request.NomeFantasia, request.Cnpj, request.Ativo));
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ExcecaoDominio ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

    }
}
