using BigECommerce.Promocao.Aplicacao.Interfaces;
using BigECommerce.Promocao.Aplicacao.Servicos;
using BigECommerce.Promocao.Dominio.Entidades;
using BigECommerce.Promocao.Dominio.Factories;
using BigECommerce.Promocao.Dominio.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests
{
    public class PromocaoServicoTests
    {
        private readonly Mock<IProdutoRepositorio> _produtoRepoMock;
        private readonly Mock<IPromocaoRepositorio> _promocaoRepoMock;
        private readonly IPromocaoServico _servico;
        private readonly Mock<ILogger<PromocaoServico>> _loggerMock;


        public PromocaoServicoTests()
        {
            _loggerMock = new Mock<ILogger<PromocaoServico>>();
            _produtoRepoMock = new Mock<IProdutoRepositorio>();
            _promocaoRepoMock = new Mock<IPromocaoRepositorio>();
            _servico = new PromocaoServico(_promocaoRepoMock.Object, _produtoRepoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Deve_Simular_Tudo_Promocional()
        {
            var produto = ProdutoFactory.CriarProduto("iPhone", 2500m);
            var promocao = Promocao.Criar(produto.Id, DateTime.Now, 10, 1m);

            _produtoRepoMock.Setup(r => r.ObterPorId(produto.Id)).Returns(produto);
            _promocaoRepoMock.Setup(r => r.ObterPromocaoAtiva(produto.Id, It.IsAny<DateTime>())).Returns(promocao);

            var resultado = _servico.SimularCompra(produto.Id, 5, DateTime.Now);

            Assert.Equal(5, resultado.QuantidadePromocional);
            Assert.Equal(0, resultado.QuantidadeNormal);
            Assert.Equal(5m, resultado.PrecoTotalEstimado);
        }

        [Fact]
        public void Deve_Simular_Misto_Promocional_E_Normal()
        {
            var produto = ProdutoFactory.CriarProduto("iPhone", 2500m);
            var promocao = Promocao.Criar(produto.Id, DateTime.Now, 3, 1m);

            _produtoRepoMock.Setup(r => r.ObterPorId(produto.Id)).Returns(produto);
            _promocaoRepoMock.Setup(r => r.ObterPromocaoAtiva(produto.Id, It.IsAny<DateTime>())).Returns(promocao);

            var resultado = _servico.SimularCompra(produto.Id, 5, DateTime.Now);

            Assert.Equal(3, resultado.QuantidadePromocional);
            Assert.Equal(2, resultado.QuantidadeNormal);
            Assert.Equal(3 * 1m + 2 * 2500m, resultado.PrecoTotalEstimado);
        }

        [Fact]
        public void Deve_Simular_Tudo_Preco_Cheio_Se_Nao_Houver_Promocao()
        {
            var produto = ProdutoFactory.CriarProduto("iPhone", 2500m);

            _produtoRepoMock.Setup(r => r.ObterPorId(produto.Id)).Returns(produto);
            _promocaoRepoMock.Setup(r => r.ObterPromocaoAtiva(produto.Id, It.IsAny<DateTime>())).Returns((Promocao?)null);

            var resultado = _servico.SimularCompra(produto.Id, 2, DateTime.Now);

            Assert.Equal(0, resultado.QuantidadePromocional);
            Assert.Equal(2, resultado.QuantidadeNormal);
            Assert.Equal(5000m, resultado.PrecoTotalEstimado);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Se_Produto_Nao_Encontrado()
        {
            _produtoRepoMock.Setup(r => r.ObterPorId(It.IsAny<Guid>())).Returns((Produto?)null);

            var ex = Assert.Throws<ArgumentException>(() =>
                _servico.SimularCompra(Guid.NewGuid(), 1, DateTime.Now));

            Assert.Equal("Produto não encontrado.", ex.Message);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Se_Quantidade_Invalida()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                _servico.SimularCompra(Guid.NewGuid(), 0, DateTime.Now));

            Assert.Equal("Quantidade inválida.", ex.Message);
        }
    }
}
