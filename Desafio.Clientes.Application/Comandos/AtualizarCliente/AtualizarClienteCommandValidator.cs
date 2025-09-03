using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.Comandos.AtualizarCliente
{
    public class AtualizarClienteCommandValidator : AbstractValidator<AtualizarClienteCommand>
    {
        public AtualizarClienteCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id do cliente inválido.");

            RuleFor(x => x.NomeFantasia)
                .NotEmpty().WithMessage("NomeFantasia é obrigatório.")
                .MaximumLength(200).WithMessage("NomeFantasia deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Cnpj)
                .NotEmpty().WithMessage("CNPJ é obrigatório.")
                .MinimumLength(14).WithMessage("CNPJ inválido.");
        }
    }
}
