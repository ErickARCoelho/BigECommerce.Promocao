
using BigECommerce.Promocao.Aplicacao.DTOs;

namespace BigECommerce.Promocao.Aplicacao.Interfaces
{
    public interface IPromocaoServico
    {
        ValidacaoCompraResultado SimularCompra(Guid produtoId, int quantidade, DateTime dataHoraAtual);
    }
}
