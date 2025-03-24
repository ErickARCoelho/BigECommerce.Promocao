using BigECommerce.Promocao.Aplicacao.DTOs;

namespace BigECommerce.Promocao.Aplicacao.Interfaces
{
    public interface ICompraServico
    {
        CompraResultado RealizarCompra(Guid produtoId, int quantidade, DateTime dataHoraAtual);
    }
}
