using FluentValidation;

namespace Desafio.Clientes.Application.Comandos.CriarCliente
{
    /// <summary>
    /// Valida o comando de criação de cliente.
    /// </summary>
    public class CriarClienteCommandValidator : AbstractValidator<CriarClienteCommand>
    {
        public CriarClienteCommandValidator()
        {
            RuleFor(x => x.NomeFantasia)
                .NotEmpty().WithMessage("NomeFantasia é obrigatório.")
                .MaximumLength(200).WithMessage("NomeFantasia deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Cnpj)
                .NotEmpty().WithMessage("CNPJ é obrigatório.")
                .Must(c => !string.IsNullOrWhiteSpace(c) && c.Trim().Length >= 14)
                .WithMessage("CNPJ inválido.");
        }
    }
}
