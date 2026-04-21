using Microsoft.EntityFrameworkCore;
using ServiceMenu.ApiService.Model;

namespace ServiceMenu.ApiService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoItem> ItensPedido { get; set; }
    public DbSet<ItemCardapio> Cardapio { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração de Pedido
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DataCriacao).IsRequired();
            entity.Property(e => e.Desconto).HasPrecision(18, 2);
            entity.HasMany(e => e.Itens)
                  .WithOne()
                  .HasForeignKey("PedidoId")
                  .OnDelete(DeleteBehavior.Cascade);

            // Ignorar propriedades calculadas
            entity.Ignore(e => e.Subtotal);
            entity.Ignore(e => e.Total);
        });

        // Configuração de ItemPedido
        modelBuilder.Entity<PedidoItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PrecoUnitario).HasPrecision(18, 2);
            entity.Property(e => e.Categoria)
                  .HasConversion<string>()
                  .HasMaxLength(50);
            entity.Property(e => e.Quantidade).IsRequired();
        });

        // Configuração de ItemCardapio (dados seed)
        modelBuilder.Entity<ItemCardapio>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Preco).HasPrecision(18, 2);
            entity.Property(e => e.Categoria)
                  .HasConversion<string>()
                  .HasMaxLength(50);
        });

        // Seed inicial do cardápio
        SeedCardapio(modelBuilder);
    }

    private void SeedCardapio(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemCardapio>().HasData(
            // Principais
            new ItemCardapio
            {
                Id = 1,
                Nome = "X Burger",
                Preco = 5.00m,
                Categoria = CategoriaItem.Principal,
            },
            new ItemCardapio
            {
                Id = 2,
                Nome = "X Egg",
                Preco = 4.50m,
                Categoria = CategoriaItem.Principal,
            },
            new ItemCardapio
            {
                Id = 3,
                Nome = "X Bacon",
                Preco = 7.00m,
                Categoria = CategoriaItem.Principal,
            },

            // Acompanhamentos
            new ItemCardapio
            {
                Id = 4,
                Nome = "Batata frita",
                Preco = 2.00m,
                Categoria = CategoriaItem.Acompanhamento,
            },
            new ItemCardapio
            {
                Id = 6,
                Nome = "Onion rings",
                Preco = 3.00m,
                Categoria = CategoriaItem.Acompanhamento,
            },

            // Bebidas
            new ItemCardapio
            {
                Id = 5,
                Nome = "Refrigerante",
                Preco = 2.50m,
                Categoria = CategoriaItem.Bebida,
            },
            new ItemCardapio
            {
                Id = 7,
                Nome = "Suco natural",
                Preco = 4.00m,
                Categoria = CategoriaItem.Bebida,
            }
        );
    }
}