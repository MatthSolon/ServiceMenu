namespace ServiceMenu.ApiService.Model
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public List<PedidoItem> Itens { get; set; }
        public decimal Subtotal => Itens.Sum(i => i.PrecoUnitario * i.Quantidade);
        public decimal Desconto { get; set; }
        public decimal Total => Subtotal - Desconto;
    }
}
