# ROADMAP.md — Plano do Projecto

## Fase 1 — Setup e primeiro arranque
- [x] Análise de viabilidade (Railway vs local)
- [x] Fork do repositório: https://github.com/matteuzdutra/OpenMU
- [x] Clone em C:\MUDEV\09-Source\OpenMU
- [x] Criação da estrutura de documentação
- [ ] Verificar Docker Desktop instalado e em execução
- [ ] Primeiro `docker compose up` com a imagem oficial
- [ ] Aceder ao AdminPanel em http://localhost
- [ ] Inicializar a base de dados (Season 6)
- [ ] Conectar o cliente MU ao servidor local

## Fase 2 — Exploração da arquitectura
- [ ] Ler e entender o projecto Startup
- [ ] Ler e entender o projecto Network
- [ ] Ler e entender o projecto GameLogic
- [ ] Ler e entender o projecto Persistence
- [ ] Ler e entender o projecto GameServer
- [ ] Ler e entender o AdminPanel (Blazor)
- [ ] Mapear o fluxo completo de uma conexão de cliente

## Fase 3 — Primeira modificação
- [ ] Criar uma conta de teste personalizada
- [ ] Modificar uma mensagem do sistema
- [ ] Entender como adicionar um comando GM

## Fase 4 — Build a partir do código-fonte
- [ ] Instalar .NET 10 SDK
- [ ] Instalar Visual Studio 2026
- [ ] Build completo da solução
- [ ] Debug com breakpoint activo

## Fase 5 — Deploy em produção
- [ ] Escolher VPS (Hetzner / DigitalOcean)
- [ ] Configurar servidor Linux
- [ ] Deploy com docker compose
- [ ] Configurar domínio e SSL para AdminPanel
- [ ] Configurar backup automático do PostgreSQL
