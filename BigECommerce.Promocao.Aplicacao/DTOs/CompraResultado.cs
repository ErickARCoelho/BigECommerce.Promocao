namespace BigECommerce.Promocao.Aplicacao.DTOs
{
    public class CompraResultado
    {
        public Guid ProdutoId { get; set; }
        public int QuantidadePromocional { get; set; }
        public int QuantidadeNormal { get; set; }
        public decimal PrecoTotal { get; set; }
        public DateTime DataHora { get; set; }
    }
}
