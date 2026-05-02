# PrototipoRelatorioRonda

Sistema de gerenciamento de relatórios de ronda e vigilância.

## 📋 Descrição

O **PrototipoRelatorioRonda** é uma API RESTful desenvolvida em ASP.NET Core para gerenciar relatórios de ronda, vigilantes, empresas e voltas de ronda. O sistema permite o controle completo de operações de vigilância com histórico de rondas realizadas.

## 🚀 Tecnologias Utilizadas

- **ASP.NET Core 10.0** - Framework web
- **Entity Framework Core** - ORM para acesso a dados
- **AutoMapper** - Mapeamento entre entidades e DTOs
- **SQL Server** - Banco de dados (LocalDB)
- **Swagger/OpenAPI** - Documentação da API
- **xUnit** - Testes unitários
- **Moq** - Framework de mock para testes
- **FluentAssertions** - Asserções fluentes para testes

## 🏗️ Arquitetura

O projeto segue **Clean Architecture** (Arquitetura Limpa) com separação em 4 camadas:

- **Domain** - Entidades e interfaces de domínio (sem dependências)
- **Application** - Lógica de aplicação, serviços, DTOs, perfis
- **Infrastructure** - Acesso a dados, repositórios, DbContext
- **API** - Controllers, middleware, configuração

### Padrões de Design

- **Repository Pattern** - Abstração do acesso a dados
- **Unit of Work Pattern** - Gerenciamento de transações
- **Service Layer Pattern** - Camada de lógica de negócio
- **Dependency Injection** - Inversão de controle
- **DTO Pattern** - Objetos de transferência de dados
- **Soft Delete Pattern** - Exclusão lógica (Ativo/Inativo)

### Estrutura de Pastas

```
PrototipoRelatorioRonda/
├── PrototipoRelatorioRonda.Domain/           # Camada de Domínio
│   ├── Entities/                            # Entidades (Empresa, Usuario, RelatorioRonda, VoltaRonda)
│   ├── Interfaces/                           # Interfaces de domínio (IBaseRepository, IAtivavel)
│   ├── Enums/                               # Enumerações (Funcao)
│   └── BaseModel.cs                        # Classe base para entidades
│
├── PrototipoRelatorioRonda.Application/      # Camada de Aplicação
│   ├── Services/                            # Serviços de aplicação
│   ├── Interfaces/                          # Interfaces de serviços e repositórios
│   ├── DTOs/                               # Data Transfer Objects
│   ├── Profiles/                           # Configurações do AutoMapper
│   └── Exceptions/                         # Exceções de aplicação
│
├── PrototipoRelatorioRonda.Infrastructure/  # Camada de Infraestrutura
│   ├── Data/                               # DbContext (RelatorioRondaContext, UnitOfWork)
│   ├── Repositories/                       # Implementações de repositórios
│   └── Migrations/                         # Migrações do Entity Framework
│
├── PrototipoRelatorioRonda.API/             # Camada de Apresentação (API)
│   ├── Controllers/                         # API Controllers
│   ├── Middleware/                         # Middlewares customizados
│   ├── Program.cs                          # Configuração da aplicação
│   └── appsettings.json                    # Configurações
│
└── PrototipoRelatorioRonda.Tests/           # Testes Unitários (33 testes)
```

## 📦 Entidades Principais

1. **Empresa** - Empresas contratantes dos serviços de vigilância
2. **Usuario** - Vigilantes/usuários do sistema (com hash de senha)
3. **RelatorioRonda** - Relatórios de ronda por data/empresa/vigilante
4. **VoltaRonda** - Voltas realizadas em cada relatório

## 🔧 Como Executar

### Pré-requisitos

- .NET 10.0 SDK ou superior
- SQL Server LocalDB (ou instância SQL Server configurada)

### Passos

1. **Clone o repositório:**
   ```bash
   git clone <url-do-repositorio>
   cd PrototipoRelatorioRonda
   ```

2. **Restaure as dependências:**
   ```bash
   dotnet restore
   ```

3. **Aplique as migrações do banco de dados:**
   ```bash
   cd PrototipoRelatorioRonda.Infrastructure
   dotnet ef database update
   ```

4. **Execute o projeto:**
   ```bash
   dotnet run --project PrototipoRelatorioRonda.API
   ```

5. **Acesse a documentação da API:**
   - Swagger UI: `https://localhost:port/swagger`
   - OpenAPI spec: `https://localhost:port/swagger/v1/swagger.json`

## 🧪 Testes

O projeto possui 53 testes unitários utilizando xUnit, Moq e FluentAssertions:

```bash
dotnet test
```

**Cobertura:** 100% dos testes passando (53/53)

## 📊 Status do Projeto

- ✅ **Clean Architecture**: 100% implementada (4 projetos)
- ✅ **Repository Pattern**: 100% implementado
- ✅ **Unit of Work**: 100% implementado
- ✅ **Service Layer**: 100% implementado
- ✅ **Controller Refactoring**: 100% concluído
- ✅ **Exception Handling**: 100% implementado
- ✅ **Dependency Injection**: 100% configurado
- ✅ **Autenticação JWT**: 100% implementado
- ✅ **Testes Unitários**: 100% passando (53/53)
- ✅ **AutoMapper**: 100% configurado (conflito de versão corrigido)
- ✅ **Newtonsoft.Json**: Vulnerabilidade corrigida (v13.0.3)

## 📚 Documentação Adicional

- [CORRECOES_REALIZADAS.md](CORRECOES_REALIZADAS.md) - Histórico de correções realizadas

## 🤝 Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.
