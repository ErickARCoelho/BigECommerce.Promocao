namespace BigECommerce.Promocao.Aplicacao.DTOs
{
    public class ValidacaoCompraResultado
    {
        public Guid ProdutoId { get; set; }
        public int QuantidadePromocional { get; set; }
        public int QuantidadeNormal { get; set; }
        public decimal PrecoTotalEstimado { get; set; }
        public DateTime DataHoraConsulta { get; set; }
    }
}
