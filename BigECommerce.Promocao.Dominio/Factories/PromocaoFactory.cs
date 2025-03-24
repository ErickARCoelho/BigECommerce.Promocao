namespace BigECommerce.Promocao.Dominio.Factories
{
    public static class PromocaoFactory
    {
        public static Entidades.Promocao CriarPromocao(Guid produtoId, DateTime horaInicial)
        {
            return Entidades.Promocao.Criar(produtoId, horaInicial, 100, 1.00m);
        }
    }
}
