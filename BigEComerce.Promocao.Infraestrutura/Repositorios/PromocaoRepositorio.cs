using BigECommerce.Promocao.Dominio.Interfaces;

namespace BigEComerce.Promocao.Infraestrutura.Repositorios
{
    public class PromocaoRepositorio : IPromocaoRepositorio
    {
        private readonly List<BigECommerce.Promocao.Dominio.Entidades.Promocao> _promocoes = new();

        public void Adicionar(BigECommerce.Promocao.Dominio.Entidades.Promocao promocao)
        {
            _promocoes.Add(promocao);
        }

        public BigECommerce.Promocao.Dominio.Entidades.Promocao? ObterPromocaoAtiva(Guid produtoId, DateTime dataHoraAtual)
        {
            return _promocoes
                .Where(p => p.ProdutoId == produtoId && p.EstaAtiva(dataHoraAtual))
                .OrderByDescending(p => p.HoraInicial)
                .FirstOrDefault();
        }

        public List<BigECommerce.Promocao.Dominio.Entidades.Promocao> ObterTodasPorProduto(Guid produtoId)
        {
            return _promocoes
                .Where(p => p.ProdutoId == produtoId)
                .OrderByDescending(p => p.HoraInicial)
                .ToList();
        }
    }
}
