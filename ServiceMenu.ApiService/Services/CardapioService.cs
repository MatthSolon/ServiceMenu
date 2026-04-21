using ServiceMenu.ApiService.Model;

namespace ServiceMenu.ApiService.Services
{
    public class CardapioService : ICardapioService

    {
        private readonly List<ItemCardapio> _cardapio = new()
        {
            // Principais
            new()
            {
                Id = 1,
                Nome = "X Burger",
                Preco = 5.00m,
                Categoria = CategoriaItem.Principal,
            },
            new()
            {
                Id = 2,
                Nome = "X Egg",
                Preco = 4.50m,
                Categoria = CategoriaItem.Principal,
            },
            new()
            {
                Id = 3,
                Nome = "X Bacon",
                Preco = 7.00m,
                Categoria = CategoriaItem.Principal,
            },

            // Acompanhamentos
            new()
            {
                Id = 4,
                Nome = "Batata frita",
                Preco = 2.00m,
                Categoria = CategoriaItem.Acompanhamento,
            },
            new()
            {
                Id = 6,
                Nome = "Onion rings",
                Preco = 3.00m,
                Categoria = CategoriaItem.Acompanhamento,
            },

            // Bebidas
            new()
            {
                Id = 5,
                Nome = "Refrigerante",
                Preco = 2.50m,
                Categoria = CategoriaItem.Bebida,
            },

        };
        public List<ItemCardapio> ObterCardapio() => _cardapio;

        public ItemCardapio? ObterPorId(int id) =>
            _cardapio.FirstOrDefault(c => c.Id == id);

        public List<ItemCardapio> ObterPorCategoria(CategoriaItem categoria) =>
            _cardapio.Where(c => c.Categoria == categoria).ToList();
    }
}
