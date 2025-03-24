using BigECommerce.Promocao.Aplicacao.Servicos;
using BigECommerce.Promocao.Dominio.Entidades;
using BigECommerce.Promocao.Dominio.Factories;
using BigECommerce.Promocao.Dominio.Interfaces;
using Moq;

namespace Tests
{
    public class CompraServicoTests
    {
        private readonly Mock<IProdutoRepositorio> _produtoRepoMock;
        private readonly Mock<IPromocaoRepositorio> _promocaoRepoMock;
        private readonly CompraServico _servico;

        public CompraServicoTests()
        {
            _produtoRepoMock = new Mock<IProdutoRepositorio>();
            _promocaoRepoMock = new Mock<IPromocaoRepositorio>();
            _servico = new CompraServico(_produtoRepoMock.Object, _promocaoRepoMock.Object);
        }

        [Fact]
        public void Deve_Comprar_Com_Todo_Estoque_Promocional()
        {
            var produtoId = Guid.NewGuid();
            var produto = ProdutoFactory.CriarProduto("iPhone", 2500m);
            var promocao = Promocao.Criar(produtoId, DateTime.Now, 10, 1m);

            _produtoRepoMock.Setup(r => r.ObterPorId(produtoId)).Returns(produto);
            _promocaoRepoMock.Setup(r => r.ObterPromocaoAtiva(produtoId, It.IsAny<DateTime>())).Returns(promocao);

            var resultado = _servico.RealizarCompra(produtoId, 5, DateTime.Now);

            Assert.Equal(5, resultado.QuantidadePromocional);
            Assert.Equal(0, resultado.QuantidadeNormal);
            Assert.Equal(5m, resultado.PrecoTotal);
        }

        [Fact]
        public void Deve_Comprar_Misturando_Promocional_E_Normal()
        {
            var produtoId = Guid.NewGuid();
            var produto = ProdutoFactory.CriarProduto("iPhone", 2500m);
            var promocao = Promocao.Criar(produtoId, DateTime.Now, 3, 1m);

            _produtoRepoMock.Setup(r => r.ObterPorId(produtoId)).Returns(produto);
            _promocaoRepoMock.Setup(r => r.ObterPromocaoAtiva(produtoId, It.IsAny<DateTime>())).Returns(promocao);

            var resultado = _servico.RealizarCompra(produtoId, 5, DateTime.Now);

            Assert.Equal(3, resultado.QuantidadePromocional);
            Assert.Equal(2, resultado.QuantidadeNormal);
            Assert.Equal((3 * 1m) + (2 * 2500m), resultado.PrecoTotal);
        }

        [Fact]
        public void Deve_Comprar_Com_Todo_Preco_Normal_Se_Promocao_Esta_Inativa()
        {
            var produtoId = Guid.NewGuid();
            var produto = ProdutoFactory.CriarProduto("iPhone", 2500m);
            var promocao = Promocao.Criar(produtoId, DateTime.Now.AddHours(-1), 100, 1m);

            _produtoRepoMock.Setup(r => r.ObterPorId(produtoId)).Returns(produto);
            _promocaoRepoMock.Setup(r => r.ObterPromocaoAtiva(produtoId, It.IsAny<DateTime>())).Returns(promocao);

            var resultado = _servico.RealizarCompra(produtoId, 2, DateTime.Now);

            Assert.Equal(0, resultado.QuantidadePromocional);
            Assert.Equal(2, resultado.QuantidadeNormal);
            Assert.Equal(5000m, resultado.PrecoTotal);
        }

        [Fact]
        public void Deve_Comprar_Todo_Normal_Se_Nao_Ha_Promocao()
        {
            var produtoId = Guid.NewGuid();
            var produto = ProdutoFactory.CriarProduto("iPhone", 2500m);

            _produtoRepoMock.Setup(r => r.ObterPorId(produtoId)).Returns(produto);
            _promocaoRepoMock.Setup(r => r.ObterPromocaoAtiva(produtoId, It.IsAny<DateTime>())).Returns((Promocao?)null);

            var resultado = _servico.RealizarCompra(produtoId, 2, DateTime.Now);

            Assert.Equal(0, resultado.QuantidadePromocional);
            Assert.Equal(2, resultado.QuantidadeNormal);
            Assert.Equal(5000m, resultado.PrecoTotal);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Se_Quantidade_Invalida()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                _servico.RealizarCompra(Guid.NewGuid(), 0, DateTime.Now));

            Assert.Equal("Quantidade inválida.", ex.Message);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Se_Produto_Nao_Encontrado()
        {
            _produtoRepoMock.Setup(r => r.ObterPorId(It.IsAny<Guid>())).Returns((Produto?)null);

            var ex = Assert.Throws<ArgumentException>(() =>
                _servico.RealizarCompra(Guid.NewGuid(), 1, DateTime.Now));

            Assert.Equal("Produto não encontrado.", ex.Message);
        }
    }
}
