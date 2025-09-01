using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Clientes.Application.DTOs
{
    public class ClienteDto
    {
        public Guid Id { get; set; }
        public string NomeFantasia { get; set; } = default!;
        public string Cnpj { get; set; } = default!;
        public bool Ativo { get; set; }
    }
}
