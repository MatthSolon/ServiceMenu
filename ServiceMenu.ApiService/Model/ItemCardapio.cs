namespace ServiceMenu.ApiService.Model
{
    public class ItemCardapio
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public CategoriaItem Categoria { get; set; }
    }
}
