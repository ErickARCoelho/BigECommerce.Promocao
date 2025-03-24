using BigECommerce.Promocao.Dominio.Entidades;

namespace Tests
{
    public class PromocaoTests
    {
        [Fact]
        public void Criar_Promocao_Deve_Ser_Valida()
        {
            var produtoId = Guid.NewGuid();
            var horaAtual = DateTime.Now;
            var promocao = Promocao.Criar(produtoId, horaAtual, 100, 1.00m);

            Assert.NotNull(promocao);
            Assert.Equal(produtoId, promocao.ProdutoId);
            Assert.Equal(100, promocao.QuantidadeDisponivel);
            Assert.Equal(1.00m, promocao.PrecoPromocional);
        }

        [Fact]
        public void Promocao_Deve_Permitir_Reserva_Quando_Tem_Estoque()
        {
            var promocao = Promocao.Criar(Guid.NewGuid(), DateTime.Now, 2, 1.00m);

            var reservado = promocao.ReservarUnidade();

            Assert.True(reservado);
            Assert.Equal(1, promocao.QuantidadeDisponivel);
        }

        [Fact]
        public void Promocao_Nao_Deve_Permitir_Criar_Com_Estoque_Zerado()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                Promocao.Criar(Guid.NewGuid(), DateTime.Now, 0, 1.00m));

            Assert.Equal("Não existe produto disponível no estoque.", ex.Message);
        }

        [Fact]
        public void Promocao_Deve_Estar_Ativa_Se_Estiver_Na_Mesma_Hora()
        {
            var horaInicial = new DateTime(2025, 03, 23, 10, 0, 0);
            var horaConsulta = new DateTime(2025, 03, 23, 10, 30, 0);

            var promocao = Promocao.Criar(Guid.NewGuid(), horaInicial, 100, 1.00m);

            var ativa = promocao.EstaAtiva(horaConsulta);

            Assert.True(ativa);
        }

        [Fact]
        public void Promocao_Deve_Estar_Inativa_Se_Estiver_Em_Hora_Diferente()
        {
            var horaInicial = new DateTime(2025, 03, 23, 10, 0, 0);
            var horaConsulta = new DateTime(2025, 03, 23, 11, 0, 0);

            var promocao = Promocao.Criar(Guid.NewGuid(), horaInicial, 100, 1.00m);

            var ativa = promocao.EstaAtiva(horaConsulta);

            Assert.False(ativa);
        }

        [Fact]
        public void Promocao_De_Hora_Anterior_Deve_Estar_Inativa()
        {
            var horaAgora = new DateTime(2025, 03, 23, 11, 15, 0);
            var horaCriacaoPromocao = new DateTime(2025, 03, 23, 10, 0, 0);

            var promocao = Promocao.Criar(Guid.NewGuid(), horaCriacaoPromocao, 100, 1.00m);

            var ativa = promocao.EstaAtiva(horaAgora);

            Assert.False(ativa);
        }
    }
}
