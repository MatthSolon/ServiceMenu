using ServiceMenu.Web.Models;

namespace ServiceMenu.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // Cardápio
        public async Task<List<ItemCardapio>> GetCardapioAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ItemCardapio>>("api/cardapio")
                       ?? new List<ItemCardapio>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar cardápio");
                return new List<ItemCardapio>();
            }
        }

        public async Task<List<CardapioAgrupado>> GetCardapioAgrupadoAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<CardapioAgrupado>>("api/cardapio/agrupado")
                       ?? new List<CardapioAgrupado>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar cardápio agrupado");
                return new List<CardapioAgrupado>();
            }
        }

        // Pedidos
        public async Task<List<Pedido>> GetPedidosAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Pedido>>("api/pedidos")
                       ?? new List<Pedido>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar pedidos");
                return new List<Pedido>();
            }
        }

        public async Task<Pedido?> GetPedidoAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Pedido>($"api/pedidos/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar pedido {Id}", id);
                return null;
            }
        }

        public async Task<Pedido?> CriarPedidoAsync(CriarPedidoDTO pedido)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/pedidos", pedido);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<Pedido>();

                var error = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Erro ao criar pedido: {Error}", error);
                throw new Exception($"Erro ao criar pedido: {error}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar pedido");
                throw;
            }
        }

        public async Task<bool> RemoverPedidoAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/pedidos/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover pedido {Id}", id);
                return false;
            }
        }
    }
}
