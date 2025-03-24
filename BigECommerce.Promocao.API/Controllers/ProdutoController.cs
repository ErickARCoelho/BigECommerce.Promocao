using BigECommerce.Promocao.Aplicacao.Interfaces;
using BigECommerce.Promocao.Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BigECommerce.Promocao.API.Controllers
{
    /// <summary>
    /// Controller responsável por operações relacionadas a produtos.
    /// </summary>
    [ApiController]
    [Route("api/produtos")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Tags("Produtos")]
    public class ProdutoController : ControllerBase
    {
        private readonly IPromocaoServico _promocaoServico;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoController(IPromocaoServico promocaoServico, IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _promocaoServico = promocaoServico;
        }

        /// <summary>
        /// Retorna o preço atual do produto considerando promoções ativas.
        /// </summary>
        /// <remarks>
        /// Caso não haja promoção, será retornado o preço base do produto.
        /// </remarks>
        /// <param name="produtoId">ID do produto</param>
        /// <returns>Preço visualizado na tela</returns>
        [HttpGet("{produtoId}/preco-atual")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ObterPrecoAtual([FromRoute] Guid produtoId)
        {
            try
            {
                var simulacao = _promocaoServico.SimularCompra(produtoId, 1, DateTime.Now);
                return Ok(new
                {
                    produtoId,
                    precoVisualizado = simulacao.PrecoTotalEstimado,
                    dataHora = DateTime.Now
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
        /// <summary>
        /// Apenas para obter o GUID do produto ao subir a aplicação. Como está em memória é apenas para facilitar o teste do negócio.
        /// </summary>
        /// <returns>Lista todos os produtos (levar em consideração somente para facilitar o teste)</returns>

        [HttpGet]
        public IActionResult Listar()
        {
            var produtos = _produtoRepositorio.ObterTodos();
            return Ok(produtos);
        }

    }
}
