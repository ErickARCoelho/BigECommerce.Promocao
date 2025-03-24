using BigECommerce.Promocao.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BigECommerce.Promocao.API.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações relacionadas às promoções.
    /// </summary>
    [ApiController]
    [Route("api/promocoes")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Tags("Promoções")]
    public class PromocaoController : ControllerBase
    {
        private readonly IPromocaoServico _promocaoServico;

        public PromocaoController(IPromocaoServico promocaoServico)
        {
            _promocaoServico = promocaoServico;
        }

        /// <summary>
        /// Valida o preço final no momento da compra.
        /// </summary>
        /// <remarks>
        /// Se houver estoque promocional, reserva a unidade.
        /// Caso contrário, retorna o preço cheio.
        /// </remarks>
        /// <param name="produtoId">ID do produto</param>
        /// <param name="quantidade">Quantidade a ser adquirida</param>
        /// <returns>Preço a ser cobrado</returns>
        [HttpPost("produto/{produtoId}/validar-compra")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ValidarPrecoParaCompra(Guid produtoId, [FromQuery] int quantidade)
        {
            try
            {
                var resultado = _promocaoServico.SimularCompra(produtoId, quantidade, DateTime.Now);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
