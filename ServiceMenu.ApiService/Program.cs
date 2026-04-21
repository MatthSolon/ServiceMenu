using Microsoft.EntityFrameworkCore;
using ServiceMenu.ApiService.Data;
using ServiceMenu.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("https://localhost:7000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Adicionar PostgreSQL via Aspire
builder.AddNpgsqlDbContext<AppDbContext>("goodhamburgerdb");

// Configurar serviços (agora Scoped por causa do DbContext)
builder.Services.AddScoped<ICardapioService, CardapioServiceEf>();
builder.Services.AddScoped<IPedidoService, PedidoServiceEf>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Aplicar migrations automaticamente em desenvolvimento
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated(); // Cria banco e aplica seed
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazor");
app.UseAuthorization();
app.MapControllers();

app.Run();