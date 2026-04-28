O projeto segue a arquitetura de microsserviços orquestrada pelo .NET Aspire, composta por 4 projetos:
```
ServiceMenu/
├── ServiceMenu.AppHost/          # Orquestrador .NET Aspire
├── ServiceMenu.ApiService/       # API REST (backend)
├── ServiceMenu.Web/              # Frontend Blazor Server
└── ServiceMenu.ServiceDefaults/  # Configurações e extensões compartilhadas
```
* Tecnologias

| Camada | Tecnologia |
|---|---|
| Runtime | .NET 10 |
| Orquestração | .NET Aspire 13.2.2 |
| Frontend | Blazor Server (Interactive Server Mode) |
| Backend | ASP.NET Core Web API |
| ORM | Entity Framework Core 10 |
| Banco de Dados | PostgreSQL (via Aspire.Npgsql) |
| Documentação API | Swagger / Swashbuckle 10 |
| UI | Bootstrap 5 + Bootstrap Icons |

---
* Endpoints da API

- Cardápio

| Método | Rota | Descrição |
| GET | `/api/cardapio` | Lista todos os itens |
| GET | `/api/cardapio/agrupado` | Lista itens agrupados por categoria |
| GET | `/api/cardapio/categoria/{categoria}` | Filtra itens por categoria |

- Pedidos

| Método | Rota | Descrição |
| GET | `/api/pedidos` | Lista todos os pedidos |
| GET | `/api/pedidos/{id}` | Busca pedido por ID |
| POST | `/api/pedidos` | Cria novo pedido |
| PUT | `/api/pedidos/{id}` | Atualiza pedido existente |
| DELETE | `/api/pedidos/{id}` | Remove pedido |

* Como Executar

- Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (para o PostgreSQL via Aspire)
- (Opcional) [.NET Aspire workload](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling)

```bash
# 1. Clone o repositório
git clone <url-do-repositorio>
cd ServiceMenu

# 2. Instale o workload do Aspire (se necessário)
dotnet workload install aspire

# 3. Execute o AppHost (orquestra todos os serviços)
dotnet run --project ServiceMenu.AppHost

# 4. Acesse o dashboard do Aspire para ver as URLs dos serviços
# Por padrão: https://localhost:15888
```

O Aspire irá subir automaticamente:
- O banco PostgreSQL via Docker
- O PgAdmin para gerenciamento do banco
- A API REST
- O frontend Blazor
