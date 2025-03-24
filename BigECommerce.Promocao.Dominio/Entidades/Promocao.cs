namespace BigECommerce.Promocao.Dominio.Entidades
{
    public class Promocao
    {
        public Guid Id { get; private set; }
        public Guid ProdutoId { get; private set; }
        public DateTime HoraInicial { get; private set; }
        public int QuantidadeTotal { get; private set; }
        public int QuantidadeDisponivel { get; private set; }
        public decimal PrecoPromocional { get; private set; }

        private Promocao(Guid produtoId, DateTime horaInicial, int quantidadeTotal, decimal precoPromocional)
        {
            if (quantidadeTotal <= 0)
                throw new ArgumentException("Não existe produto disponível no estoque.");

            if (precoPromocional < 0)
                throw new ArgumentException("O preço promocional não pode ser negativo.");

            Id = Guid.NewGuid();
            ProdutoId = produtoId;
            HoraInicial = horaInicial;
            QuantidadeTotal = quantidadeTotal;
            QuantidadeDisponivel = quantidadeTotal;
            PrecoPromocional = precoPromocional;
        }

        public static Promocao Criar(Guid produtoId, DateTime horaInicial, int quantidadeTotal, decimal precoPromocional)
        {
            return new Promocao(produtoId, horaInicial, quantidadeTotal, precoPromocional);
        }

        public bool EstaAtiva(DateTime dataHoraAtual)
        {
            return dataHoraAtual >= HoraInicial && dataHoraAtual < HoraInicial.AddHours(1);
        }

        public bool TemEstoquePromocional()
        {
            return QuantidadeDisponivel > 0;
        }

        public bool PodeExibirPrecoPromocional(DateTime dataHoraAtual)
        {
            return EstaAtiva(dataHoraAtual) && TemEstoquePromocional();
        }

        public bool ReservarUnidade()
        {
            if (!TemEstoquePromocional())
                return false;

            QuantidadeDisponivel--;
            return true;
        }

        public void ReporEstoque(int quantidade)
        {
            QuantidadeDisponivel += quantidade;
        }
    }
}
