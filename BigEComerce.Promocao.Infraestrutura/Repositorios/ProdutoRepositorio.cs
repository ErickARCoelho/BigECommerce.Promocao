using BigECommerce.Promocao.Dominio.Entidades;
using BigECommerce.Promocao.Dominio.Factories;
using BigECommerce.Promocao.Dominio.Interfaces;

namespace BigEComerce.Promocao.Infraestrutura.Repositorios
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly List<Produto> _produtos;

        public ProdutoRepositorio()
        {
            _produtos = new List<Produto>
            {
                ProdutoFactory.CriarProduto("iPhone 15", 2500.00m)
            };
        }

        public void Adicionar(Produto produto)
        {
            _produtos.Add(produto);
        }

        public Produto? ObterPorId(Guid id)
        {
            return _produtos.FirstOrDefault(p => p.Id == id);
        }

        public List<Produto> ObterTodos()
        {
            return _produtos.ToList();
        }
    }
}
