using System;
using Desafio.Clientes.Domain.ValueObjects;
using Desafio.Clientes.Domain.Exceptions;

namespace Desafio.Clientes.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; private set; }

        public string NomeFantasia { get; private set; } = default!;

        public Cnpj Cnpj { get; private set; } = default!;

        public bool Ativo { get; private set; }

        private Cliente() { }

        public Cliente(Guid id, string nomeFantasia, Cnpj cnpj, bool ativo = true)
        {
            if (string.IsNullOrWhiteSpace(nomeFantasia))
                throw new ExcecaoDominio("NomeFantasia não pode ser vazio.");

            if (cnpj is null)
                throw new ExcecaoDominio("CNPJ é obrigatório.");

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            NomeFantasia = nomeFantasia.Trim();
            Cnpj = cnpj;
            Ativo = ativo;
        }
        public static Cliente Criar(string nomeFantasia, Cnpj cnpj)
            => new Cliente(Guid.NewGuid(), nomeFantasia, cnpj, true);

        public void Desativar() => Ativo = false;

        public void Ativar() => Ativo = true;

        public void AtualizarNomeFantasia(string novoNome)
        {
            if (string.IsNullOrWhiteSpace(novoNome))
                throw new ExcecaoDominio("NomeFantasia não pode ser vazio.");

            NomeFantasia = novoNome.Trim();
        }

        public void AtualizarCnpj(Cnpj novoCnpj)
        {
            if (novoCnpj is null)
                throw new ExcecaoDominio("CNPJ é obrigatório.");

            Cnpj = novoCnpj;
        }
    }
}
