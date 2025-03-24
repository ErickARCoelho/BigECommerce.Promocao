using BigECommerce.Promocao.Dominio.Entidades;

namespace BigECommerce.Promocao.Dominio.Interfaces
{
    public interface IProdutoRepositorio
    {
        Produto? ObterPorId(Guid id);
        void Adicionar(Produto produto);
        List<Produto> ObterTodos();
    }
}
