# PrototipoRelatorioRonda

Sistema de gerenciamento de relatórios de ronda e vigilância.

## 📋 Descrição

O **PrototipoRelatorioRonda** é uma API RESTful desenvolvida em ASP.NET Core para gerenciar relatórios de ronda, vigilantes, empresas e voltas de ronda. O sistema permite o controle completo de operações de vigilância com histórico de rondas realizadas.

## 🚀 Tecnologias Utilizadas

- **ASP.NET Core 6.0** - Framework web
- **Entity Framework Core** - ORM para acesso a dados
- **AutoMapper** - Mapeamento entre entidades e DTOs
- **SQL Server** - Banco de dados (LocalDB)
- **Swagger/OpenAPI** - Documentação da API
- **xUnit** - Testes unitários

## 🏗️ Arquitetura

O projeto segue os padrões de design:

- **Repository Pattern** - Abstração do acesso a dados
- **Service Layer Pattern** - Camada de lógica de negócio
- **Dependency Injection** - Inversão de controle
- **DTO Pattern** - Objetos de transferência de dados
- **Soft Delete Pattern** - Exclusão lógica (Ativo/Inativo)

### Estrutura de Pastas

```
PrototipoRelatorioRonda/
├── Controllers/              # API Controllers (100% refatorados)
├── Models/                   # Entidades de domínio
│   ├── DTO/                 # Data Transfer Objects
│   ├── Interface/           # Interfaces de domínio
│   └── Enums/              # Enumerações
├── Data/                    # Camada de Dados
│   ├── Interface/           # Interfaces de Repositório
│   ├── Repositories/        # Implementações de Repositório
│   └── RelatorioRondaContext.cs
├── Services/                # Camada de Serviços (100% completa)
│   ├── Interface/           # Interfaces de Serviço
│   ├── EmpresaService.cs
│   ├── UsuarioService.cs
│   ├── RelatorioRondaService.cs
│   └── VoltaRondaService.cs
├── Profiles/                # Configurações do AutoMapper
├── Middleware/              # Middlewares Customizados
├── Exceptions/              # Exceções Personalizadas
└── Migrations/              # Migrações do Entity Framework
```

## 📦 Entidades Principais

1. **Empresa** - Empresas contratantes dos serviços de vigilância
2. **Usuario** - Vigilantes/usuários do sistema (com hash de senha)
3. **RelatorioRonda** - Relatórios de ronda por data/empresa/vigilante
4. **VoltaRonda** - Voltas realizadas em cada relatório

## 🔧 Como Executar

### Pré-requisitos

- .NET 6.0 SDK ou superior
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
   cd PrototipoRelatorioRonda
   dotnet ef database update
   ```

4. **Execute o projeto:**
   ```bash
   dotnet run
   ```

5. **Acesse a documentação da API:**
   - Swagger UI: `https://localhost:port/swagger`
   - OpenAPI spec: `https://localhost:port/swagger/v1/swagger.json`

## 🧪 Testes

O projeto possui testes unitários utilizando xUnit:

```bash
cd PrototipoRelatorioRonda.Tests
dotnet test
```

## 📊 Status do Projeto

- ✅ **Repository Pattern**: 100% implementado
- ✅ **Service Layer**: 100% implementado
- ✅ **Controller Refactoring**: 100% concluído
- ✅ **Exception Handling**: 100% implementado
- ✅ **Dependency Injection**: 100% configurado
- 🔄 **Testes Unitários**: Em desenvolvimento
- ✅ **Autenticação JWT**: 100% implementado

## 📚 Documentação Adicional

- [README_ESTRUTURA.md](PrototipoRelatorioRonda/README_ESTRUTURA.md) - Documentação detalhada da estrutura do projeto
- [CORRECOES_REALIZADAS.md](CORRECOES_REALIZADAS.md) - Histórico de correções realizadas

## 🤝 Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.
