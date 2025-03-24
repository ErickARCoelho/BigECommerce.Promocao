using BigECommerce.Promocao.Aplicacao.DTOs;
using BigECommerce.Promocao.Aplicacao.Interfaces;
using BigECommerce.Promocao.Dominio.Interfaces;
using Microsoft.Extensions.Logging;

namespace BigECommerce.Promocao.Aplicacao.Servicos
{
    public class PromocaoServico : IPromocaoServico
    {
        private readonly IPromocaoRepositorio _promocaoRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ILogger<PromocaoServico> _logger;

        public PromocaoServico(IPromocaoRepositorio promocaoRepositorio, IProdutoRepositorio produtoRepositorio, ILogger<PromocaoServico> logger)
        {
            _promocaoRepositorio = promocaoRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _logger = logger;
        }

        /// <summary>
        /// Retorna o preço que deve ser exibido ao cliente na tela, com base na promoção ativa (se houver), simulando o valor final.
        /// </summary>
        public ValidacaoCompraResultado SimularCompra(Guid produtoId, int quantidade, DateTime dataHoraAtual)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade inválida.");

            var produto = _produtoRepositorio.ObterPorId(produtoId)
                ?? throw new ArgumentException("Produto não encontrado.");

            var promocao = _promocaoRepositorio.ObterPromocaoAtiva(produtoId, dataHoraAtual);

            int qtdPromo = 0;
            int qtdNormal = 0;
            decimal total = 0;

            if (promocao != null && promocao.EstaAtiva(dataHoraAtual) && promocao.TemEstoquePromocional())
            {
                int disponiveis = promocao.QuantidadeDisponivel;
                qtdPromo = Math.Min(quantidade, disponiveis);
                qtdNormal = quantidade - qtdPromo;

                total += qtdPromo * promocao.PrecoPromocional;
                total += qtdNormal * produto.PrecoBase;
            }
            else
            {
                qtdNormal = quantidade;
                total = quantidade * produto.PrecoBase;
            }

            return new ValidacaoCompraResultado
            {
                ProdutoId = produtoId,
                QuantidadePromocional = qtdPromo,
                QuantidadeNormal = qtdNormal,
                PrecoTotalEstimado = total,
                DataHoraConsulta = dataHoraAtual
            };
        }
    }
}
