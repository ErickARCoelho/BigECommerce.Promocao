namespace BigECommerce.Promocao.Dominio.Entidades
{
    public class Produto
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public decimal PrecoBase { get; private set; }

        private Produto(Guid id, string nome, decimal precoBase)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("O ID do produto não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do produto é obrigatório.");

            if (precoBase <= 0)
                throw new ArgumentException("O preço base deve ser maior que zero.");

            Id = id;
            Nome = nome;
            PrecoBase = precoBase;
        }

        public void AlterarPreco(decimal novoPreco)
        {
            if (novoPreco <= 0)
                throw new ArgumentException("O preço deve ser maior que zero.");

            PrecoBase = novoPreco;
        }

        internal static Produto Criar(Guid id, string nome, decimal precoBase)
        {
            return new Produto(id, nome, precoBase);
        }
    }
}
