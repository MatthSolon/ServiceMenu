using Microsoft.Extensions.Hosting;
var builder = DistributedApplication.CreateBuilder(args);

// Adicionar PostgreSQL
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .AddDatabase("goodhamburgerdb");

// Adicionar API com referência ao banco
var api = builder.AddProject<Projects.ServiceMenu_ApiService>("api")
    .WithExternalHttpEndpoints()
    .WithReference(postgres)
    .WaitFor(postgres);

// Adicionar Frontend com referência à API
var web = builder.AddProject<Projects.ServiceMenu_Web>("web")
    .WithExternalHttpEndpoints()
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();