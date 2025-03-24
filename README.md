# BigECommerce.Promocao

Abaixo relato a ideia que tive para implementar o negócio do projeto, conforme solicitado:
- A fim de teste, implementei a persistência em memória já que o foco estava no negócio.
- Endpoints documentados que são utilizados para a base de teste Produto/Listar já que o Id (Guid) do produto é criado ao iniciar a aplicação
-  Levei em consideração que a cada 1 hora cria-se a promoção então caso não tenha sido consumido a totalidade dos itens em promoção os mesmos, não são mais válidos para a próxima.
- Para não ficar exposto em um endpoint, criei um serviço para criar as promoções do dia, na intenção de que um serviço pode ser pausado,
ou a manipulação dele pode ser conjunta com o banco de dados validando todas as promoções ativas no banco de dados e ativa-las se necessário, então acredito que seja algo com um maior controle sem interromper as vendas.
- O fluxo qual imaginei seria do usuário selecionar o produto (Produto/Listar), fazendo com que o sistema retorne o preço atual para o usuário (Produto/ObterPrecoAtual), validar a compra conforme promoção ativa e quantidade para ter expectativa de valor simulando uma alteração na quantidade pelo usuário (Promoção/ValidarPrecoParaCompra), para assim efetivar e finalizar a compra (Compra/RealizarCompra).
- Levei em consideração também que não havia regra para a quantidade de compra que seria permitida para cada usuário distinto.
- Também imaginei um cenário de múltiplos usuário ao mesmo tempo, tentei implementar essa lógica de validações baseadas na persistência in memoria.