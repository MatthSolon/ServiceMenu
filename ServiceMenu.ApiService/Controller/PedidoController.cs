using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceMenu.ApiService.DTO;
using ServiceMenu.ApiService.Model;
using ServiceMenu.ApiService.Services;

namespace ServiceMenu.ApiService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public IActionResult ObterTodos() => Ok(_pedidoService.ObterTodos());

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var pedido = _pedidoService.ObterPorId(id);
            return pedido == null ? NotFound($"Pedido {id} não encontrado") : Ok(pedido);
        }

        [HttpPost]
        public IActionResult Criar([FromBody] CriarPedidoDTO pedidoDTO)
        {
            var pedido = new Pedido
            {
                Itens = pedidoDTO.Itens.Select(i => new PedidoItem
                {
                    ItemCardapioId = i.ItemCardapioId,
                    Quantidade = i.Quantidade
                }).ToList()
            };

            var (valido, mensagem) = _pedidoService.ValidarPedido(pedido);
            if (!valido) return BadRequest(new { mensagem });

            var pedidoCriado = _pedidoService.Criar(pedido);
            return CreatedAtAction(nameof(ObterPorId), new { id = pedidoCriado.Id }, pedidoCriado);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, [FromBody] CriarPedidoDTO pedidoDTO)
        {
            var pedido = new Pedido
            {
                Itens = pedidoDTO.Itens.Select(i => new PedidoItem
                {
                    ItemCardapioId = i.ItemCardapioId,
                    Quantidade = i.Quantidade
                }).ToList()
            };

            var (valido, mensagem) = _pedidoService.ValidarPedido(pedido);
            if (!valido) return BadRequest(new { mensagem });

            var pedidoAtualizado = _pedidoService.Atualizar(id, pedido);
            return pedidoAtualizado == null
                ? NotFound($"Pedido {id} não encontrado")
                : Ok(pedidoAtualizado);
        }

        [HttpDelete("{id}")]
        public IActionResult Remover(int id)
        {
            return _pedidoService.Remover(id)
                ? NoContent()
                : NotFound($"Pedido {id} não encontrado");
        }
    }
}
