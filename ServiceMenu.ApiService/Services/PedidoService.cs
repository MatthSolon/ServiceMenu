using ServiceMenu.ApiService.Model;

namespace ServiceMenu.ApiService.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly List<Pedido> _pedidos = new();
        private readonly ICardapioService _cardapioService;
        private int _proximoId = 1;

        public PedidoService(ICardapioService cardapioService)
        {
            _cardapioService = cardapioService;
        }

        public List<Pedido> ObterTodos() => _pedidos;

        public Pedido? ObterPorId(int id) => _pedidos.FirstOrDefault(p => p.Id == id);

        public Pedido Criar(Pedido pedido)
        {
            pedido.Id = _proximoId++;
            pedido.DataCriacao = DateTime.Now;

            PreencherDadosItens(pedido);
            CalcularDesconto(pedido);

            _pedidos.Add(pedido);
            return pedido;
        }

        public Pedido? Atualizar(int id, Pedido pedidoAtualizado)
        {
            var pedidoExistente = ObterPorId(id);
            if (pedidoExistente == null) return null;

            pedidoExistente.Itens = pedidoAtualizado.Itens;
            pedidoExistente.DataCriacao = DateTime.Now;

            PreencherDadosItens(pedidoExistente);
            CalcularDesconto(pedidoExistente);

            return pedidoExistente;
        }

        public bool Remover(int id)
        {
            var pedido = ObterPorId(id);
            if (pedido == null) return false;
            return _pedidos.Remove(pedido);
        }

        private void PreencherDadosItens(Pedido pedido)
        {
            foreach (var item in pedido.Itens)
            {
                var itemCardapio = _cardapioService.ObterPorId(item.ItemCardapioId);
                if (itemCardapio != null)
                {
                    item.Nome = itemCardapio.Nome;
                    item.PrecoUnitario = itemCardapio.Preco;
                    item.Categoria = itemCardapio.Categoria;
                }
            }
        }

        private void CalcularDesconto(Pedido pedido)
        {
            var temPrincipal = pedido.Itens.Any(i => i.Categoria == CategoriaItem.Principal);
            var temAcompanhamento = pedido.Itens.Any(i => i.Categoria == CategoriaItem.Acompanhamento);
            var temBebida = pedido.Itens.Any(i => i.Categoria == CategoriaItem.Bebida);

            pedido.Desconto = (temPrincipal, temAcompanhamento, temBebida) switch
            {
                (true, true, true) => pedido.Subtotal * 0.20m,  // 20%
                (true, false, true) => pedido.Subtotal * 0.15m, // 15%
                (true, true, false) => pedido.Subtotal * 0.10m, // 10%
                _ => 0
            };
        }

        public (bool valido, string mensagem) ValidarPedido(Pedido pedido)
        {
            if (!pedido.Itens.Any(i => i.Categoria == CategoriaItem.Principal))
                return (false, "O pedido deve conter pelo menos um item principal (sanduíche)");

            var categoriasComMaisDeUmItem = pedido.Itens
                .GroupBy(i => i.Categoria)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (categoriasComMaisDeUmItem.Any())
            {
                var categoriasInvalidas = string.Join(", ", categoriasComMaisDeUmItem);
                return (false, $"Máximo 1 item por categoria. Categoria Com mais de 1 Item: {categoriasInvalidas}");
            }

            var idsDuplicados = pedido.Itens
                .GroupBy(i => i.ItemCardapioId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (idsDuplicados.Any())
                return (false, $"Itens duplicados detectados (IDs: {string.Join(", ", idsDuplicados)})");

            return (true, "Pedido Criado");
        }
    }
}
