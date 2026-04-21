namespace ServiceMenu.ApiService.DTO
{
    public class PedidoDTO
    {
    }
    public class ItemPedidoDTO
    {
        public int ItemCardapioId { get; set; }
        public int Quantidade { get; set; } = 1;
    }

    public class CriarPedidoDTO
    {
        public List<ItemPedidoDTO> Itens { get; set; } = new();
    }
}
