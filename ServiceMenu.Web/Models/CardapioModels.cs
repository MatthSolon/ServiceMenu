namespace ServiceMenu.Web.Models
{
    public class CardapioModels
    {
    }
    public enum CategoriaItem
    {
        Principal = 1,
        Acompanhamento = 2,
        Bebida = 3
    }

    public class ItemCardapio
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public CategoriaItem Categoria { get; set; }
    }

    public class CardapioAgrupado
    {
        public string Categoria { get; set; } = string.Empty;
        public List<ItemCardapioResumido> Itens { get; set; } = new();
    }

    public class ItemCardapioResumido
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public CategoriaItem Categoria { get; set; }
    }
}
