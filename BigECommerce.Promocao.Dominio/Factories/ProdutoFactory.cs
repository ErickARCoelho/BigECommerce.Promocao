using BigECommerce.Promocao.Dominio.Entidades;

namespace BigECommerce.Promocao.Dominio.Factories
{
    public class ProdutoFactory
    {
        public static Produto CriarProduto(string nome, decimal precoBase)
        {
            return Produto.Criar(Guid.NewGuid(), nome, precoBase);
        }
    }
}
