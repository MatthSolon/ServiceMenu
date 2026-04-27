using ServiceMenu.Web.Models;

namespace ServiceMenu.Web.Services
{
    public class CarrinhoState
    {
        public List<ItemPedidoDTO> Itens { get; private set; } = new();
        public event Action? OnChange;
        private string _errorMessage = string.Empty;
        public string ErrorMessage => _errorMessage;
        public void AdicionarItem(int itemId, string categoria, int quantidade = 1)
        {
            var itemExistente = Itens.FirstOrDefault(i => i.Categoria.ToString() == categoria);

            if (itemExistente != null)
            {
                _errorMessage = "Este item já foi adicionado ao pedido, mas pertence a uma categoria diferente. Remova o item existente para adicionar este.";
            }
            else if (itemExistente?.Categoria == Enum.Parse<CategoriaItem>(categoria.ToString()))
            {
                _errorMessage = "Algum item da mesma categoria já foi adicionado ao pedido, mas pertence a uma categoria diferente. Remova o item existente para adicionar este.";
            }
            else
            {
                Itens.Add(new ItemPedidoDTO { ItemCardapioId = itemId, Quantidade = quantidade, Categoria = Enum.Parse<CategoriaItem>(categoria.ToString()) });
            }


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
        public void ClearErrorMessage()
        {
            _errorMessage = string.Empty;
            NotifyStateChanged();
        }
    }

}
