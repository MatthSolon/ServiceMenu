using Microsoft.EntityFrameworkCore;
using ServiceMenu.ApiService.Data;
using ServiceMenu.ApiService.Model;

namespace ServiceMenu.ApiService.Services;

public class PedidoServiceEf : IPedidoService
{
    private readonly AppDbContext _context;
    private readonly ICardapioService _cardapioService;
    private readonly ILogger<PedidoServiceEf> _logger;

    public PedidoServiceEf(
        AppDbContext context,
        ICardapioService cardapioService,
        ILogger<PedidoServiceEf> logger)
    {
        _context = context;
        _cardapioService = cardapioService;
        _logger = logger;
    }

    public List<Pedido> ObterTodos()
    {
        try
        {
            return _context.Pedidos
                .Include(p => p.Itens)
                .OrderByDescending(p => p.DataCriacao)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter pedidos");
            return new List<Pedido>();
        }
    }

    public Pedido? ObterPorId(int id)
    {
        try
        {
            return _context.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefault(p => p.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter pedido {Id}", id);
            return null;
        }
    }

    public Pedido Criar(Pedido pedido)
    {
        try
        {
            pedido.DataCriacao = DateTime.UtcNow;

            PreencherDadosItens(pedido);
            CalcularDesconto(pedido);

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            _logger.LogInformation("Pedido #{Id} criado com sucesso. Total: R$ {Total}",
                pedido.Id, pedido.Total);

            return pedido;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pedido");
            throw;
        }
    }

    public Pedido? Atualizar(int id, Pedido pedidoAtualizado)
    {
        try
        {
            var pedidoExistente = _context.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefault(p => p.Id == id);

            if (pedidoExistente == null)
                return null;

            // Remove itens antigos
            _context.ItensPedido.RemoveRange(pedidoExistente.Itens);

            // Adiciona novos itens
            pedidoExistente.Itens = pedidoAtualizado.Itens;
            pedidoExistente.DataCriacao = DateTime.UtcNow;

            PreencherDadosItens(pedidoExistente);
            CalcularDesconto(pedidoExistente);

            _context.SaveChanges();

            _logger.LogInformation("Pedido #{Id} atualizado com sucesso", id);

            return pedidoExistente;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar pedido {Id}", id);
            throw;
        }
    }

    public bool Remover(int id)
    {
        try
        {
            var pedido = _context.Pedidos.Find(id);
            if (pedido == null)
                return false;

            _context.Pedidos.Remove(pedido);
            _context.SaveChanges();

            _logger.LogInformation("Pedido #{Id} removido com sucesso", id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover pedido {Id}", id);
            return false;
        }
    }

    private void PreencherDadosItens(Pedido pedido)
    {
        foreach (var item in pedido.Itens)
        {
            var itemCardapio = _cardapioService.ObterPorId(item.ItemCardapioId);
            if (itemCardapio != null)
            {
                item.Nome = itemCardapio.Nome;
                item.PrecoUnitario = itemCardapio.Preco;
                item.Categoria = itemCardapio.Categoria;
            }
        }
    }

    private void CalcularDesconto(Pedido pedido)
    {
        var temPrincipal = pedido.Itens.Any(i => i.Categoria == CategoriaItem.Principal);
        var temAcompanhamento = pedido.Itens.Any(i => i.Categoria == CategoriaItem.Acompanhamento);
        var temBebida = pedido.Itens.Any(i => i.Categoria == CategoriaItem.Bebida);

        pedido.Desconto = (temPrincipal, temAcompanhamento, temBebida) switch
        {
            (true, true, true) => pedido.Subtotal * 0.20m,
            (true, false, true) => pedido.Subtotal * 0.15m,
            (true, true, false) => pedido.Subtotal * 0.10m,
            _ => 0
        };
    }

    public (bool valido, string mensagem) ValidarPedido(Pedido pedido)
    {
        if (!pedido.Itens.Any(i => i.Categoria == CategoriaItem.Principal))
            return (false, "O pedido deve conter pelo menos um item principal (sanduíche)");

        var categoriasComMaisDeUmItem = pedido.Itens
            .GroupBy(i => i.Categoria)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (categoriasComMaisDeUmItem.Any())
        {
            var categoriasInvalidas = string.Join(", ", categoriasComMaisDeUmItem);
            return (false, $"Máximo 1 item por categoria. Violação: {categoriasInvalidas}");
        }

        var idsDuplicados = pedido.Itens
            .GroupBy(i => i.ItemCardapioId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (idsDuplicados.Any())
            return (false, $"Itens duplicados detectados (IDs: {string.Join(", ", idsDuplicados)})");

        return (true, "Pedido válido");
    }
}