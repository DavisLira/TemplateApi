# 🚀 TemplateApi - .NET 10 Base Project

![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF%20Core-Latest-blue?style=for-the-badge)
![JWT](https://img.shields.io/badge/JWT-Authentication-black?style=for-the-badge)

O **TemplateApi** é um acelerador de desenvolvimento para APIs robustas e escaláveis. Construído com as tecnologias mais recentes do ecossistema Microsoft, este template foca em Clean Architecture, padrões de segurança modernos e alta testabilidade, permitindo que você saia do "zero" para uma API funcional em segundos.

🔗 **Repositório:** [https://github.com/DavisLira/TemplateApi/](https://github.com/DavisLira/TemplateApi/)

---

## 🛠 Tecnologias Principais

- **Framework:** .NET 10 (C#)
- **ORM:** Entity Framework Core (Code First)
- **Autenticação:** JWT (JSON Web Tokens) com Refresh Tokens
- **Mapeamento:** Mapster
- **Validação:** FluentValidation
- **Documentação:** Swagger/OpenAPI
- **Testes:** xUnit, FluentAssertions e Bogus

---

## 🚀 Como Iniciar um Novo Projeto

Este repositório foi configurado como um **dotnet template**. Para utilizá-lo localmente, siga os passos abaixo:

### 1. Instalar o Template
Abra o terminal na raiz do projeto `TemplateApi` e execute:
```bash
dotnet new install ./
```

### 2. Criar sua nova API
Navegue até a pasta onde deseja criar o novo projeto e execute:
```bash
dotnet new new-api -n NomeDaMinhaApi
```
*O comando substituirá automaticamente os namespaces e referências de arquivos para `NomeDaMinhaApi`.*

---

## 🏗 Arquitetura e Camadas

O projeto segue uma separação de responsabilidades rigorosa para facilitar a manutenção e evolução:

| Camada | Responsabilidade | O que contém? |
| :--- | :--- | :--- |
| **Domain** | O "coração" da aplicação. Independente de tecnologia. | Entidades, Enums, Interfaces de Repositório e Regras de Negócio. |
| **Application** | Orquestração de fluxos e lógica de aplicação. | Use Cases, Mapeamentos (DTOs) e Injeção de Dependência da lógica. |
| **Infrastructure** | Implementações externas e acesso a dados. | DbContext, Migrations, Repositórios e Serviços de Terceiros (Criptografia, Email). |
| **Api** | Interface de entrada (Ponto de contato com o usuário). | Controllers, Middlewares, Filters e Configuração de Segurança (JWT). |
| **Communication** | Contratos de comunicação. | Requests e Responses (JSON) compartilhados com o cliente. |
| **Exceptions** | Centralização de erros. | Exceções customizadas e mensagens multi-idioma. |

---

## 💾 Guia Prático: Criando uma Nova Tabela

Para adicionar uma nova entidade ao banco de dados, siga este fluxo:

### 1. Criar a Entidade (Camada Domain)
Crie sua classe em `TemplateApi.Domain/Entities` herdando de `EntitieBase`:

```csharp
public class Product : EntitieBase
{
    public long ProductId { get; set; } 
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

### 2. Mapeamento (Camada Infrastructure)
Crie a configuração em `TemplateApi.Infrastructure/DataAccess/Configurations` para definir regras de banco:

```csharp
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);

        builder.Property(p => p.Price).HasPrecision(18, 2);
        
        builder.Property(p => p.Active)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();
    }
}
```

### 3. Adicionar ao DbContext (Camada Infrastructure)
Abra o `TemplateApiDbContext.cs` e adicione o `DbSet`:

```csharp
public DbSet<Product> Products { get; set; }
```

---

## 🔄 Migrations: Sincronizando o Banco

Com a entidade criada, gere a migration através do terminal. Certifique-se de estar na raiz da solução ou aponte os projetos corretamente:

**Criar Migration:**
```bash
dotnet ef migrations add NomeDaMigration --project src/Backend/TemplateApi.Infrastructure --startup-project src/Backend/TemplateApi.Api
```

**Atualizar Banco de Dados:**
_A api já faz a atualização automáticamente sempre ao inicializar_
```bash
dotnet ef database update --project src/Backend/TemplateApi.Infrastructure --startup-project src/Backend/TemplateApi.Api
```

---

## 🧪 Testes Automatizados

A estrutura de testes é dividida para garantir cobertura total:

- **UseCases.Test:** Testes unitários da lógica de negócio.
- **Validator.Test:** Validação de inputs (FluentValidation).
- **WebApi.Test:** Testes de integração (End-to-End) usando `WebApplicationFactory`.

**Para executar todos os testes:**
```bash
dotnet test
```

*Dica: Utilizamos o padrão **Builder** (em `CommonTestUtilities`) para criar objetos de teste de forma fluida e reutilizável.*

---

## 📂 Organização de Arquivos

Para manter a consistência, siga estes caminhos:

- **Controllers:** `src/Backend/TemplateApi.Api/Controllers/`
- **Lógica de Negócio (UseCases):** `src/Backend/TemplateApi.Application/UseCases/`
- **Configuração de Injeção de Dependência:** `DependencyInjectionExtension.cs` em cada camada.
- **Mensagens de Erro:** `src/Shared/TemplateApi.Exceptions/ResourceMessagesException.resx`
- **Entidades de Banco:** `src/Backend/TemplateApi.Domain/Entities/`

---

## 🛡 Segurança
A API utiliza **JWT** com políticas de autorização.
- Use `[AuthenticatedUser]` para rotas logadas.
- Use `[AuthenticatedAsAdmin]` para rotas restritas a administradores.

---
⭐ **Dica de Sênior:** Sempre que criar um novo serviço, lembre-se de registrá-lo na `DependencyInjectionExtension` da respectiva camada para que o container do .NET consiga resolvê-lo.

**Bons códigos!** 🚀
