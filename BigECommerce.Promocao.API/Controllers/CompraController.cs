using BigECommerce.Promocao.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BigECommerce.Promocao.API.Controllers
{
    [ApiController]
    [Route("api/compras")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Tags("Compras")]
    public class CompraController : ControllerBase
    {
        private readonly ICompraServico _compraServico;

        public CompraController(ICompraServico compraServico)
        {
            _compraServico = compraServico;
        }

        /// <summary>
        /// Realiza a compra de um produto aplicando a política promocional da hora atual.
        /// Se houver unidades promocionais disponíveis, elas serão aplicadas primeiro.
        /// O restante, se houver, será cobrado com o preço normal do produto.
        /// </summary>
        /// <param name="produtoId">Identificador do produto a ser comprado.</param>
        /// <param name="quantidade">Quantidade total de unidades que o cliente deseja comprar.</param>
        /// <returns>Informações detalhadas da compra, incluindo o preço total e o quanto foi promocional.</returns>

        [HttpPost("{produtoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RealizarCompra([FromRoute] Guid produtoId, [FromQuery] int quantidade)
        {
            try
            {
                var resultado = _compraServico.RealizarCompra(produtoId, quantidade, DateTime.Now);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
