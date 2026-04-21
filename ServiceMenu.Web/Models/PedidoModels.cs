namespace ServiceMenu.Web.Models
{
    public class PedidoModels
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

    public class PedidoItem
    {
        public int Id { get; set; }
        public int ItemCardapioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public CategoriaItem Categoria { get; set; }
    }

    public class Pedido
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public List<PedidoItem> Itens { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
    }

    public class PedidoResumo
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public int QuantidadeItens { get; set; }
        public decimal Total { get; set; }
    }
}
