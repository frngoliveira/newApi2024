using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FRN.Domain._2._2_Entity
{
    public class Product
    {
        public int cod_produto { get; set; }
        public string? descricao { get; set; }
        public bool situacao_produto { get; set; }
        public DateTime dt_fabricacao { get; set; }
        public DateTime dt_validade { get; set; }
        public int cod_fornecedor { get; set; }

        [NotMapped]
        public string? descricao_fornecedor { get; set; }

        
    }
}
