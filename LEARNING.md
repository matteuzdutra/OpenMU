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

## Próxima sessão
- Docker Desktop: verificar instalação e primeiro arranque do servidor
