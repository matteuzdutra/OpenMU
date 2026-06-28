# LEARNING.md — Diário de aprendizagem

Registo dos conceitos aprendidos durante o projecto.
Actualizado a cada sessão de desenvolvimento.

---

## Sessão 1 — Análise e setup inicial

### Git — Remotes
Um "remote" é um repositório Git alojado noutro lugar (GitHub, GitLab, etc.).
Por convenção:
- `origin` — o teu fork (onde fazes push)
- `upstream` — o projecto original (de onde puxas actualizações)

```bash
git pull upstream master   # puxar novidades do projecto original
git push origin master     # enviar as tuas alterações para o teu fork
```

### Fork vs Clone
- **Fork**: cópia de um repositório para a tua conta GitHub. Mantém ligação ao original.
- **Clone**: cópia local de um repositório (fork ou não) para a tua máquina.

Fluxo normal:
```
MUnique/OpenMU (original)
    │
    │ fork
    ▼
matteuzdutra/OpenMU (teu fork no GitHub)
    │
    │ clone
    ▼
C:\MUDEV\09-Source\OpenMU (tua máquina local)
```

### Arquitectura All-in-one vs Distributed
O OpenMU pode correr como um único processo (all-in-one) ou como vários processos
separados que comunicam via Dapr (distributed).

Para aprendizagem e servidores pequenos, o all-in-one é sempre a escolha certa.

### TCP vs HTTP
- **HTTP**: protocolo de texto, baseado em pedido-resposta. Usado para o AdminPanel.
- **TCP**: protocolo de transporte de bytes raw. Usado pelo cliente MU para comunicar
  com o servidor de jogo. Mais rápido e eficiente para jogos em tempo real.

### Portas
Uma "porta" é um número (0-65535) que identifica um serviço específico numa máquina.
O cliente MU sabe que deve conectar na porta 44405 porque é o padrão do protocolo.
O servidor escuta nessa porta e aguarda conexões.

### Docker e Contêineres
Um contêiner Docker é um processo isolado que corre a sua própria cópia de um sistema
operativo mínimo, com o software necessário já instalado. É como uma máquina virtual
mas muito mais leve e rápido de iniciar.

O `docker-compose.yml` descreve quais contêineres devem correr juntos e como se
relacionam entre si (portas, volumes, variáveis de ambiente, dependências).

### Entity Framework Core
ORM (Object-Relational Mapper) que permite trabalhar com a base de dados PostgreSQL
usando classes C# em vez de SQL puro. As classes C# mapeiam para tabelas no PostgreSQL.

### Clean Architecture (observada no OpenMU)
O GameLogic não conhece:
- Como os dados chegam (rede, interface gráfica, testes)
- Onde os dados são guardados (PostgreSQL, memória, outro banco)

Esta separação permite:
- Testar a lógica do jogo sem base de dados
- Trocar de base de dados sem mudar a lógica
- Suportar múltiplos protocolos de cliente

---

---

## Sessão 2 — Cliente e primeira ligação

### ServerList.bmd — O que contém realmente
O ficheiro `Data\Local\ServerList.bmd` está encriptado com XOR (chave `FC CF AB` repetida).
Após decifrar, contém apenas **nomes de grupos de servidores** e metadata de display (posição, sequência, flags PvP).
**Não contém IPs nem portas.** O IP real do connect server vem do código (`config.ini` ou defaults hard-coded).

### Fluxo de ligação do cliente MuMain
```
Main.exe arranca
    → lê config.ini → [CONNECTION SETTINGS] ServerIP / ServerPort
    → se não existe, usa default: 127.127.127.127:44406
    → conecta ao Connect Server (porta 44406)
    → Connect Server responde com lista de Game Servers + IPs
    → cliente conecta ao Game Server escolhido
```

### IpResolverType — Configuração crítica
O OpenMU tem uma definição de como reporta o seu IP aos clientes:
- `IpResolverType_Public_Name` → usa o IP público da máquina (ex: 89.115.187.246) — causa "disconnected" em jogo local
- `IpResolverType_Loopback_Name` → usa `127.0.0.1` — correcto para jogar na mesma máquina
- `IpResolverType_Custom_Name` → IP personalizado definido em "Custom IP / Hostname"

**Para servidor local, usar sempre `Loopback_Name`.**
Configuração em: AdminPanel → Configuration → System.

### GameGuard — Anti-cheat
O MU Online original usa GameGuard (npgmup.dll, GameGuard.des, GameGuard.csr) para proteger contra cheats.
Para servidores privados/desenvolvimento, deve ser desactivado renomeando os ficheiros para `.bak`.
O MuMain open-source não precisa de GameGuard — foi reescrito sem essa dependência.

### Filter.bmd — Versões incompatíveis
O ficheiro `Data\Local\Filter.bmd` existe em duas versões:
- **MuMain zip**: 20005 bytes → causa erro "file corrupted" no cliente patched
- **Season6 zip**: 20004 bytes → versão correcta

Sempre usar o Filter.bmd do zip Season6.

### Portas do OpenMU
| Serviço | Porta | Protocolo |
|---------|-------|-----------|
| Connect Server (Season6) | 44405 | TCP |
| Connect Server (MuMain) | 44406 | TCP |
| Game Server 0 | 55901, 55902 | TCP |
| Game Server 1 | 55903, 55904 | TCP |
| Game Server 2 | 55905, 55906 | TCP |
| Chat Server | 55980 | TCP |
| AdminPanel | 80 | HTTP |

### Base de dados PostgreSQL
O OpenMU usa PostgreSQL com schema `data`. Exemplos de queries úteis:
```sql
-- Ver personagens de uma conta
SELECT a."LoginName", c."Name", c."Experience"
FROM data."Account" a
JOIN data."Character" c ON c."AccountId" = a."Id"
WHERE a."LoginName" = 'matteuz';
```
As tabelas e colunas usam PascalCase com aspas duplas em PostgreSQL.

## Próxima sessão
- Explorar a arquitectura do OpenMU (projecto Startup, Network, GameLogic)
- Aprender a adicionar itens via AdminPanel
- Explorar comandos GM
