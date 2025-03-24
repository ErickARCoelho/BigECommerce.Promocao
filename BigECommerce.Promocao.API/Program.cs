
using BigECommerce.Promocao.Aplicacao.Servicos;
using BigECommerce.Promocao.Dominio.Interfaces;
using BigEComerce.Promocao.Infraestrutura.Repositorios;
using BigECommerce.Promocao.API.Servicos;
using BigECommerce.Promocao.Aplicacao.Interfaces;
using System.Reflection;

namespace BigECommerce.Promocao.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            builder.Services.AddScoped<IPromocaoServico, PromocaoServico>();
            builder.Services.AddScoped<ICompraServico, CompraServico>();
            builder.Services.AddSingleton<IProdutoRepositorio, ProdutoRepositorio>();
            builder.Services.AddSingleton<IPromocaoRepositorio, PromocaoRepositorio>();

            builder.Services.AddHostedService<PromocaoAgendadorServico>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
