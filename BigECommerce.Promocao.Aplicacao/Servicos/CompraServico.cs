using BigECommerce.Promocao.Aplicacao.DTOs;
using BigECommerce.Promocao.Aplicacao.Interfaces;
using BigECommerce.Promocao.Dominio.Interfaces;

namespace BigECommerce.Promocao.Aplicacao.Servicos
{
    public class CompraServico : ICompraServico
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IPromocaoRepositorio _promocaoRepositorio;

        public CompraServico(IProdutoRepositorio produtoRepositorio, IPromocaoRepositorio promocaoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _promocaoRepositorio = promocaoRepositorio;
        }

        public CompraResultado RealizarCompra(Guid produtoId, int quantidade, DateTime dataHoraAtual)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade inválida.");

            var produto = _produtoRepositorio.ObterPorId(produtoId)
                ?? throw new ArgumentException("Produto não encontrado.");

            var promocao = _promocaoRepositorio.ObterPromocaoAtiva(produtoId, dataHoraAtual);

            int quantidadePromocional = 0;
            int quantidadeNormal = 0;
            decimal precoTotal = 0;

            if (promocao != null && promocao.EstaAtiva(dataHoraAtual) && promocao.TemEstoquePromocional())
            {
                for (int i = 0; i < quantidade; i++)
                {
                    if (promocao.TemEstoquePromocional())
                    {
                        promocao.ReservarUnidade();
                        precoTotal += promocao.PrecoPromocional;
                        quantidadePromocional++;
                    }
                    else
                    {
                        precoTotal += produto.PrecoBase;
                        quantidadeNormal++;
                    }
                }
            }
            else
            {
                precoTotal = produto.PrecoBase * quantidade;
                quantidadeNormal = quantidade;
            }

            return new CompraResultado
            {
                ProdutoId = produtoId,
                QuantidadePromocional = quantidadePromocional,
                QuantidadeNormal = quantidadeNormal,
                PrecoTotal = precoTotal,
                DataHora = DateTime.Now
            };
        }

    }
}
