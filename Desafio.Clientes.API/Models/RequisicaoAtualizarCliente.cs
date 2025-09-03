namespace Desafio.Clientes.API.Models
{
    public class RequisicaoAtualizarCliente
    {
        public string NomeFantasia { get; set; } = default!;
        public string Cnpj { get; set; } = default!;
        public bool Ativo { get; set; } = true;
    }
}
