using Microsoft.EntityFrameworkCore;
using ServiceMenu.ApiService.Data;
using ServiceMenu.ApiService.Model;

namespace ServiceMenu.ApiService.Services;

public class CardapioServiceEf : ICardapioService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CardapioServiceEf> _logger;

    public CardapioServiceEf(AppDbContext context, ILogger<CardapioServiceEf> logger)
    {
        _context = context;
        _logger = logger;
    }

    public List<ItemCardapio> ObterCardapio()
    {
        try
        {
            return _context.Cardapio
                .OrderBy(i => i.Categoria)
                .ThenBy(i => i.Nome)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter cardápio");
            return new List<ItemCardapio>();
        }
    }

    public ItemCardapio? ObterPorId(int id)
    {
        try
        {
            return _context.Cardapio.Find(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter item {Id} do cardápio", id);
            return null;
        }
    }

    public List<ItemCardapio> ObterPorCategoria(CategoriaItem categoria)
    {
        try
        {
            return _context.Cardapio
                .Where(i => i.Categoria == categoria)
                .OrderBy(i => i.Nome)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter itens da categoria {Categoria}", categoria);
            return new List<ItemCardapio>();
        }
    }
}