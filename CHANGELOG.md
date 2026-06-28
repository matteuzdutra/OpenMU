# CHANGELOG.md — Histórico de alterações

Formato baseado em [Keep a Changelog](https://keepachangelog.com/).
Versões seguem [Conventional Commits](https://www.conventionalcommits.org/).

---

## [Unreleased]

### Added
- Fork do repositório MUnique/OpenMU para matteuzdutra/OpenMU
- Ficheiros de documentação: INSTALL.md, ARCHITECTURE.md, ROADMAP.md, CHANGELOG.md, LEARNING.md
- Estrutura de pastas C:\MUDEV criada localmente
- Docker Desktop + WSL 2 instalados e configurados
- Servidor OpenMU a correr localmente via `docker compose up`
- Cliente MuMain (Main.exe 6.4MB) configurado e funcional
- Conta `matteuz` criada com personagem `Magia` (Wizard)
- Login bem sucedido no servidor local

### Fixed
- `IpResolverType` alterado de `Public_Name` para `Loopback_Name` no AdminPanel → corrigiu o "disconnected from server"
- `Filter.bmd` substituído pela versão Season6 (20004 bytes) — versão MuMain causava "file corrupted"
- `GameGuard.csr` renomeado para `.bak` para desactivar anti-cheat
- Porta do Connect Server: MuMain usa 44406 por defeito (correcto para o OpenMU)
