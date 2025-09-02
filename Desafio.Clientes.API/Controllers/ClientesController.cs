using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Desafio.Clientes.Application.Comandos.CriarCliente;
using Desafio.Clientes.Application.Consultas.ObterClientePorId;
using Desafio.Clientes.Application.DTOs;

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
    }

    public class RequisicaoCriarCliente
    {
        public string NomeFantasia { get; set; } = default!;
        public string Cnpj { get; set; } = default!;
    }
}
