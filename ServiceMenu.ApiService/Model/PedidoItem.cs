namespace ServiceMenu.ApiService.Model
{
    public class PedidoItem
    {
        public int Id { get; set; }
        public int ItemCardapioId { get; set; }
        public string Nome { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public CategoriaItem Categoria { get; set; }
}
}
