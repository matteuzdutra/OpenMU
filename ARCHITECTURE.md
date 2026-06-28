# ARCHITECTURE.md — Arquitectura do OpenMU

## Visão geral

O OpenMU é um servidor de MU Online escrito em C# / .NET 10.
Suporta dois modos de deployment: **all-in-one** e **distributed**.

Para este projecto utilizamos o modo **all-in-one**.

## Modos de deployment

### All-in-one (utilizado neste projecto)
Todos os subsistemas correm num único processo/contêiner Docker.
Recomendado para servidores pequenos e para desenvolvimento.

```
[nginx:80]
    │ HTTP
    ▼
[openmu-startup]
    ├── ConnectServer   → TCP 44405 / 44406
    ├── LoginServer     → interno
    ├── GameServer 0    → TCP 55901, 55902
    ├── GameServer 1    → TCP 55903, 55904
    ├── GameServer 2    → TCP 55905, 55906
    ├── ChatServer      → TCP 55980
    ├── FriendServer    → interno
    ├── GuildServer     → interno
    └── AdminPanel      → HTTP 8080 (via nginx)
         │
         ▼
    [PostgreSQL:5432]
```

### Distributed (não utilizado — actualmente quebrado e sem suporte)
Cada subsistema é um contêiner separado com sidecar Dapr.
Inclui: RabbitMQ, Redis, Prometheus, Grafana, Loki, MinIO, Zipkin.

## Portas

| Porta | Protocolo | Serviço | Descrição |
|---|---|---|---|
| 80 | HTTP | nginx | Painel de administração |
| 44405 | TCP | ConnectServer | Cliente original Season 6 |
| 44406 | TCP | ConnectServer | Cliente open-source |
| 55901-55902 | TCP | GameServer 0 | Canais de jogo |
| 55903-55904 | TCP | GameServer 1 | Canais de jogo |
| 55905-55906 | TCP | GameServer 2 | Canais de jogo |
| 55980 | TCP | ChatServer | Mensagens privadas |
| 5432 | TCP | PostgreSQL | Base de dados (interno) |

## Fluxo de conexão do cliente

```
CLIENTE MU
    │
    │ TCP → 44405
    ▼
ConnectServer
    │ responde com IP + porta do GameServer
    │
    │ TCP → 55901
    ▼
GameServer
    │ autenticação via LoginServer (interno)
    │ carrega conta e personagem do PostgreSQL
    ↕ pacotes TCP contínuos durante o jogo
```

## Stack tecnológica

| Camada | Tecnologia |
|---|---|
| Linguagem | C# |
| Framework | .NET 10 |
| ORM | Entity Framework Core |
| Base de dados | PostgreSQL |
| Web (AdminPanel) | Blazor Server + Kestrel |
| Containerização | Docker |
| Proxy HTTP | nginx |
| Protocolo de rede | TCP customizado (SimpleModulus) |

## Projectos principais (src/)

| Projecto | Namespace | Função |
|---|---|---|
| Startup | MUnique.OpenMU.Startup | Ponto de entrada — junta todos os componentes |
| GameServer | MUnique.OpenMU.GameServer | Lógica de jogo, handlers de pacotes, views |
| GameLogic | MUnique.OpenMU.GameLogic | Regras do jogo, acções do jogador (sem conhecimento de rede) |
| ConnectServer | MUnique.OpenMU.ConnectServer | Lista de servidores disponíveis para o cliente |
| Network | MUnique.OpenMU.Network | Protocolo TCP, conexão, pacotes |
| Persistence | MUnique.OpenMU.Persistence | Repositórios, contextos, acesso a dados |
| DataModel | MUnique.OpenMU.DataModel | Entidades (Account, Character, Item, etc.) |
| Web/AdminPanel | MUnique.OpenMU.Web.AdminPanel | Interface de administração (Blazor) |

## Padrões de arquitectura

- **Clean Architecture** — GameLogic não conhece a rede nem a base de dados
- **Repository Pattern** — acesso a dados via interfaces de repositório
- **Plugin System** — handlers e views são plugins activáveis pelo AdminPanel
- **Dependency Injection** — componentes são injectados via .NET DI
- **Domain-Driven Design** — entidades ricas com comportamento próprio
