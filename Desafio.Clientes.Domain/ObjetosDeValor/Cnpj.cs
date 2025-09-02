using System;
using System.Linq;
using System.Text.RegularExpressions;
using Desafio.Clientes.Domain.Excecoes;

namespace Desafio.Clientes.Domain.ObjetosDeValor
{
    public sealed class Cnpj : IEquatable<Cnpj>
    {
        public string Numero { get; }

        private Cnpj(string numero)
        {
            Numero = numero;
        }

        public static Cnpj Criar(string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada))
                throw new ExcecaoDominio("CNPJ inválido: valor nulo ou em branco.");

            var digits = Regex.Replace(entrada, "[^0-9]", "");

            if (digits.Length != 14)
                throw new ExcecaoDominio("CNPJ inválido: deve conter 14 dígitos.");

            if (SequenciaMesmoDigito(digits))
                throw new ExcecaoDominio("CNPJ inválido.");

            if (!ValidarDigitosVerificadores(digits))
                throw new ExcecaoDominio("CNPJ inválido.");

            return new Cnpj(digits);
        }

        private static bool SequenciaMesmoDigito(string s) => s.Distinct().Count() == 1;

        private static bool ValidarDigitosVerificadores(string cnpj)
        {
            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string temp = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += (temp[i] - '0') * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;
            temp += digito1.ToString();
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += (temp[i] - '0') * multiplicador2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;
            string digitosVerificadores = cnpj.Substring(12, 2);

            return digitosVerificadores == $"{digito1}{digito2}";
        }

        public override string ToString() => Numero;

        public string ParaFormatado()
        {
            if (string.IsNullOrEmpty(Numero) || Numero.Length != 14) return Numero;
            return $"{Numero.Substring(0, 2)}.{Numero.Substring(2, 3)}.{Numero.Substring(5, 3)}/{Numero.Substring(8, 4)}-{Numero.Substring(12, 2)}";
        }

        public bool Equals(Cnpj? other) => other is not null && Numero == other.Numero;

        public override bool Equals(object? obj) => obj is Cnpj other && Equals(other);

        public override int GetHashCode() => Numero.GetHashCode();

        public static bool operator ==(Cnpj? left, Cnpj? right) => Equals(left, right);

        public static bool operator !=(Cnpj? left, Cnpj? right) => !Equals(left, right);
    }
}
