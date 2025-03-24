namespace BigECommerce.Promocao.Dominio.Interfaces
{
    public interface IPromocaoRepositorio
    {
        void Adicionar(Entidades.Promocao promocao);
        Entidades.Promocao? ObterPromocaoAtiva(Guid produtoId, DateTime dataHoraAtual);
        List<Entidades.Promocao> ObterTodasPorProduto(Guid produtoId);
    }
}
