using ServiceMenu.Web.Models;

namespace ServiceMenu.Web.Services
{
    public class CarrinhoState
    {
        public List<ItemPedidoDTO> Itens { get; private set; } = new();
        public event Action? OnChange;

        public void AdicionarItem(int itemId, int quantidade = 1)
        {
            var itemExistente = Itens.FirstOrDefault(i => i.ItemCardapioId == itemId);

            if (itemExistente != null)
                itemExistente.Quantidade += quantidade;
            else
                Itens.Add(new ItemPedidoDTO { ItemCardapioId = itemId, Quantidade = quantidade });

            NotifyStateChanged();
        }

        public void RemoverItem(int itemId)
        {
            Itens.RemoveAll(i => i.ItemCardapioId == itemId);
            NotifyStateChanged();
        }

        public void AtualizarQuantidade(int itemId, int quantidade)
        {
            var item = Itens.FirstOrDefault(i => i.ItemCardapioId == itemId);
            if (item != null)
            {
                item.Quantidade = quantidade;
                NotifyStateChanged();
            }
        }

        public void LimparCarrinho()
        {
            Itens.Clear();
            NotifyStateChanged();
        }

        public int TotalItens => Itens.Sum(i => i.Quantidade);

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
