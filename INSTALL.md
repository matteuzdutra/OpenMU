# INSTALL.md — Guia de Instalação do OpenMU

## Status
> 🚧 Em construção — actualizado a cada etapa concluída.

## Pré-requisitos

| Ferramenta | Versão mínima | Verificação |
|---|---|---|
| Windows | 10 ou superior | `winver` |
| Docker Desktop | Última versão | `docker --version` |
| Git | Qualquer | `git --version` |
| GitHub CLI | Qualquer | `gh --version` |

## Estrutura de pastas

```
C:\MUDEV
├── 01-Downloads      — ficheiros baixados (clientes, patches)
├── 02-Installers     — instaladores de ferramentas
├── 03-Server         — dados de runtime do servidor
├── 04-Client         — cliente do jogo MU Online
├── 05-Database       — backups e scripts de banco de dados
├── 06-Tools          — ferramentas auxiliares
├── 07-Backups        — backups gerais
├── 08-Documentation  — documentação externa
└── 09-Source         — código-fonte
    └── OpenMU        — este repositório
```

## Etapas de instalação

### Etapa 1 — Fork e clone ✅
- Fork: https://github.com/matteuzdutra/OpenMU
- Clone em: `C:\MUDEV\09-Source\OpenMU`
- Remote origin: `https://github.com/matteuzdutra/OpenMU.git`
- Remote upstream: `https://github.com/MUnique/OpenMU.git`

### Etapa 2 — Docker (a fazer)
- Instalar Docker Desktop
- Verificar que o Docker está em execução
- Testar: `docker run hello-world`

### Etapa 3 — Primeiro arranque (a fazer)
- Navegar para `deploy/all-in-one`
- Executar `docker compose up -d --no-build`
- Aceder ao AdminPanel em `http://localhost`

## Comandos úteis

```bash
# Actualizar fork com as novidades do projecto original
git pull upstream master
git push origin master

# Iniciar o servidor
cd deploy/all-in-one
docker compose up -d --no-build

# Parar o servidor
docker compose down

# Ver logs
docker compose logs -f
```
