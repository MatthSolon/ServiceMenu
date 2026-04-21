using ServiceMenu.ApiService.Model;

namespace ServiceMenu.ApiService.Services
{
    public interface ICardapioService
    {
        List<ItemCardapio> ObterCardapio();
        ItemCardapio? ObterPorId(int id);
        List<ItemCardapio> ObterPorCategoria(CategoriaItem categoria);
    }
}
