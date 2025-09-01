using System;

namespace Desafio.Clientes.Domain.Exceptions
{
    public class ExcecaoDominio : Exception
    {
        public ExcecaoDominio(string mensagem) : base(mensagem) { }

        public ExcecaoDominio(string mensagem, Exception innerException) : base(mensagem, innerException) { }
    }
}