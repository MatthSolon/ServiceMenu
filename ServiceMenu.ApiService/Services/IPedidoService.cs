using ServiceMenu.ApiService.Model;

namespace ServiceMenu.ApiService.Services
{
    public interface IPedidoService
    {
        List<Pedido> ObterTodos();
        Pedido? ObterPorId(int id);
        Pedido Criar(Pedido pedido);
        Pedido? Atualizar(int id, Pedido pedido);
        bool Remover(int id);
        (bool valido, string mensagem) ValidarPedido(Pedido pedido);
    }
}
