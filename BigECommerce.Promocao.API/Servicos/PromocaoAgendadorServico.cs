using BigECommerce.Promocao.Dominio.Interfaces;

namespace BigECommerce.Promocao.API.Servicos
{
    public class PromocaoAgendadorServico : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _verificacaoIntervalo = TimeSpan.FromMinutes(1);
        private DateTime _ultimaPromocaoRegistrada = DateTime.MinValue;
        public PromocaoAgendadorServico(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var agora = DateTime.Now;
                var horaAtual = new DateTime(agora.Year, agora.Month, agora.Day, agora.Hour, 0, 0);

                if (_ultimaPromocaoRegistrada != horaAtual)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var produtoRepo = scope.ServiceProvider.GetRequiredService<IProdutoRepositorio>();
                        var promocaoRepo = scope.ServiceProvider.GetRequiredService<IPromocaoRepositorio>();

                        var produto = produtoRepo.ObterTodos().FirstOrDefault();
                        if (produto != null)
                        {
                            var promocao = Dominio.Entidades.Promocao.Criar(produto.Id, horaAtual, 100, 1.00m);
                            promocaoRepo.Adicionar(promocao);
                            _ultimaPromocaoRegistrada = horaAtual;
                        }
                    }
                }

                await Task.Delay(_verificacaoIntervalo, stoppingToken);
            }
        }
    }
}
