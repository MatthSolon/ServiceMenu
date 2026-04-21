using ServiceMenu.Web;
using ServiceMenu.Web.Services;
using ServiceMenu.Web.Components;
//using ServiceMenu.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Configurar HttpClient para API
builder.Services.AddHttpClient<ApiClient>(client =>
{
    // Aspire injeta automaticamente a URL da API
    client.BaseAddress = new Uri("https+http://api");
});

// Serviços Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Serviço para estado do carrinho (criaremos a seguir)
builder.Services.AddScoped<CarrinhoState>();

var app = builder.Build();

app.MapDefaultEndpoints();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();