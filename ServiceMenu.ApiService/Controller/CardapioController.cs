using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMenu.ApiService.Model;
using ServiceMenu.ApiService.Services;

namespace ServiceMenu.ApiService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardapioController : ControllerBase
    {
        private readonly ICardapioService _cardapioService;

        public CardapioController(ICardapioService cardapioService)
        {
            _cardapioService = cardapioService;
        }

        [HttpGet]
        public IActionResult ObterCardapio()
        {
            return Ok(_cardapioService.ObterCardapio());
        }

        [HttpGet("agrupado")]
        public IActionResult ObterCardapioAgrupado()
        {
            var agrupado = _cardapioService.ObterCardapio()
                .GroupBy(i => i.Categoria)
                .Select(g => new
                {
                    Categoria = g.Key.ToString(),
                    Itens = g.Select(i => new { i.Id, i.Nome, i.Preco })
                });

            return Ok(agrupado);
        }

        [HttpGet("categoria/{categoria}")]
        public IActionResult ObterPorCategoria(CategoriaItem categoria)
        {
            return Ok(_cardapioService.ObterPorCategoria(categoria));
        }
    }
}
