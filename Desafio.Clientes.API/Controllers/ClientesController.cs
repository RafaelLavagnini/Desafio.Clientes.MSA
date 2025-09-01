using Desafio.Clientes.Application.Comandos.CriarCliente;
using Desafio.Clientes.Application.Consultas.ObterClientePorId;
using Desafio.Clientes.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Criar([FromBody] CreateClienteRequest request)
        {
            try
            {
                var id = await _mediator.Send(new CriarClienteCommand(request.NomeFantasia, request.Cnpj));
                return CreatedAtAction(nameof(ObterPorId), new { id }, null);
            }
            catch (ExcecaoDominio ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var cliente = await _mediator.Send(new ObterClientePorIdConsulta(id));
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }
    }

    public class CreateClienteRequest
    {
        public string NomeFantasia { get; set; } = default!;
        public string Cnpj { get; set; } = default!;
    }
}
