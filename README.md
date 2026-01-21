# Standard Dependencies - Grupo 118

Pacotes NuGet para facilitar a configuração de dependências padrão em aplicações .NET, incluindo OpenTelemetry e Swagger.

## 📦 Pacotes

Este repositório contém dois pacotes NuGet:

- **FinalChallenge.Grupo118.StandardDependencies.Models** - Modelos de configuração
- **FinalChallenge.Grupo118.StandardDependencies.Injection** - Extensões de configuração e injeção de dependências

## 🚀 Instalação

### Via .NET CLI

```bash
dotnet add package FinalChallenge.Grupo118.StandardDependencies.Injection
dotnet add package FinalChallenge.Grupo118.StandardDependencies.Models
```

### Via Package Manager Console

```powershell
Install-Package FinalChallenge.Grupo118.StandardDependencies.Injection
Install-Package FinalChallenge.Grupo118.StandardDependencies.Models
```

### Via PackageReference (arquivo .csproj)

```xml
<ItemGroup>
  <PackageReference Include="FinalChallenge.Grupo118.StandardDependencies.Injection" Version="1.0.0" />
  <PackageReference Include="FinalChallenge.Grupo118.StandardDependencies.Models" Version="1.0.0" />
</ItemGroup>
```

## 📝 Configuração

### 1. Configurar no Program.cs

```csharp
using StandardDependencies.Injection;
using StandardDependencies.Models;

var builder = WebApplication.CreateBuilder(args);

// Leia as configurações do appsettings.json
var swaggerOptions = builder
    .Configuration
    .GetSection(SwaggerOptions.SectionName)
    .Get<SwaggerOptions>();

var openTelemetryOptions = builder
    .Configuration
    .GetSection(OpenTelemetryOptions.SectionName)
    .Get<OpenTelemetryOptions>();

// Configura elementos comuns: Environment Variables, OpenTelemetry e Swagger
builder.ConfigureCommonElements(openTelemetryOptions, swaggerOptions);

var app = builder.Build();

// Configure o middleware do Swagger
app.UseStandarizedSwagger(swaggerOptions);

app.Run();
```

### 2. Adicionar configurações no appsettings.json

```json
{
  "OpenTelemetry": {
    "ServiceName": "minha-api",
    "ServiceVersion": "1.0.0",
    "Url": "http://localhost:4317",
    "Exporters": ["OTLP", "Console"]
  },
  "Swagger": {
    "Version": "v1",
    "Title": "Minha API",
    "Description": "Descrição da minha API",
    "ContactName": "Equipe de Desenvolvimento",
    "ContactUrl": "https://github.com/meu-repositorio"
  }
}
```

## ⚙️ Configurações Detalhadas

> **⚠️ Importante:** As configurações de OpenTelemetry e Swagger devem ser passadas explicitamente como parâmetros para os métodos de extensão. Embora as propriedades individuais tenham valores padrão, os objetos de configuração não podem ser nulos.

### OpenTelemetry

A seção `OpenTelemetry` no `appsettings.json` configura a observabilidade da aplicação.

| Propriedade | Tipo | Obrigatório | Valor Padrão | Descrição |
|-------------|------|-------------|--------------|-----------|
| `ServiceName` | string | ✅ Sim       | `""` (vazio) | Nome do serviço que será exibido no sstema de observabilidade (ex: Jaeger, Grafana) |
| `ServiceVersion` | string | ✅ Sim       | `""` (vazio) | Versão do serviço para rastreamento de mudanças |
| `Url` | string | ✅ Sim       | `http://localhost:4317` | URL do coletor OpenTelemetry (OTLP endpoint) |
| `Exporters` | array | ✅ Sim       | `["OTLP"]` | Lista de exportadores a serem utilizados. Valores possíveis: `OTLP`, `Console` |

#### Valores Possíveis para Exporters

- **`OTLP`**: Exporta telemetria para um coletor OpenTelemetry via gRPC
- **`Console`**: Exporta telemetria diretamente no console (útil para desenvolvimento/debug)

#### Exemplo Completo

```json
{
  "OpenTelemetry": {
    "ServiceName": "tech-challenge-api",
    "ServiceVersion": "1.0.0",
    "Url": "http://otel-collector:4317",
    "Exporters": ["OTLP", "Console"]
  }
}
```

#### Funcionalidades Configuradas Automaticamente

O pacote configura automaticamente as seguintes instrumentações:

**Tracing:**
- ASP.NET Core (requisições HTTP)
- HttpClient (chamadas HTTP externas)
- Npgsql (PostgreSQL)
- SQL Client (SQL Server)
- Redis (StackExchange.Redis)
- Entity Framework Core
- MongoDB

**Metrics:**
- ASP.NET Core
- HttpClient
- Runtime (.NET)
- Process (informações do processo)
- Hosting (Microsoft.AspNetCore.Hosting)
- Kestrel (servidor web)
- HTTP (System.Net.Http)
- DNS (System.Net.NameResolution)

**Logging:**
- Integração com OpenTelemetry
- Inclui TraceId, SpanId, ParentId
- Suporta Baggage e Tags

---

### Swagger

A seção `Swagger` no `appsettings.json` configura a documentação da API.

| Propriedade | Tipo | Obrigatório | Valor Padrão | Descrição |
|-------------|------|-------------|--------------|-----------|
| `Version` | string | ✅ Sim | `v1` | Versão da API exibida na documentação Swagger |
| `Title` | string | ✅ Sim | `API` | Título principal da documentação |
| `Description` | string | ✅ Sim | `API Documentation` | Descrição detalhada da API |
| `ContactName` | string | ✅ Sim | `API Support` | Nome do contato ou equipe responsável |
| `ContactUrl` | string | ✅ Sim | `http://example.com/support` | URL para contato (repositório GitHub, site, etc.) |

#### Exemplo Completo

```json
{
  "Swagger": {
    "Version": "v1",
    "Title": "Tech Challenge - Fast Food API",
    "Description": "API para gerenciamento de pedidos para lanchonete usando conceitos de Clean Architecture.",
    "ContactName": "Grupo 118 - Tech Challenge",
    "ContactUrl": "https://github.com/Grupo-118-Desafio-Final/final-challenge-grupo-118-standard-dependencies"
  }
}
```

---

## 🔧 Configurações Avançadas

### Leitura das Configurações

As configurações devem ser lidas explicitamente do `appsettings.json` e passadas como parâmetros para os métodos de extensão:

```csharp
var swaggerOptions = builder.Configuration
    .GetSection(SwaggerOptions.SectionName)
    .Get<SwaggerOptions>();

var openTelemetryOptions = builder.Configuration
    .GetSection(OpenTelemetryOptions.SectionName)
    .Get<OpenTelemetryOptions>();

builder.ConfigureCommonElements(openTelemetryOptions, swaggerOptions);
```

### Middleware do Swagger

O pacote fornece o método `UseStandarizedSwagger` que deve ser chamado no pipeline da aplicação para configurar o Swagger UI:

```csharp
app.UseStandarizedSwagger(swaggerOptions);
```

Este método configura:
- O endpoint do Swagger JSON em `../swagger/v1/swagger.json`
- A rota do Swagger UI na raiz da aplicação (`/`)
- O título da documentação conforme especificado nas opções

### Personalizando o Swagger

Você pode adicionar configurações personalizadas ao Swagger chamando `AddSwaggerGen` novamente em seu `Program.cs`. As configurações serão mescladas com as configurações do pacote.

```csharp
builder.ConfigureCommonElements(openTelemetryOptions, swaggerOptions);

// Adicionar segurança JWT ao Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                      "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                      "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
```

### Variáveis de Ambiente

O pacote automaticamente adiciona suporte a variáveis de ambiente. Você pode sobrescrever qualquer configuração usando variáveis de ambiente:

```bash
# OpenTelemetry
export OpenTelemetry__ServiceName="minha-api"
export OpenTelemetry__Url="http://otel-collector:4317"
export OpenTelemetry__Exporters__0="OTLP"
export OpenTelemetry__Exporters__1="Console"

# Swagger
export Swagger__Title="Minha API"
export Swagger__Version="v2"
```

---

## 📚 Dependências Incluídas

O pacote `StandardDependencies.Injection` já inclui as seguintes dependências:

**OpenTelemetry Core:**
- OpenTelemetry (1.11.2)
- OpenTelemetry.Extensions.Hosting (1.11.2)

**Exportadores:**
- OpenTelemetry.Exporter.Console (1.11.2)
- OpenTelemetry.Exporter.OpenTelemetryProtocol (1.11.2)

**Instrumentações:**
- OpenTelemetry.Instrumentation.AspNetCore (1.11.1)
- OpenTelemetry.Instrumentation.Http (1.11.1)
- OpenTelemetry.Instrumentation.Runtime (1.11.1)
- OpenTelemetry.Instrumentation.Process (1.11.0-beta.2)
- OpenTelemetry.Instrumentation.EntityFrameworkCore (1.14.0-beta.2)
- OpenTelemetry.Instrumentation.SqlClient (1.11.0-beta.2)
- OpenTelemetry.Instrumentation.StackExchangeRedis (1.11.0-beta.2)
- Npgsql.OpenTelemetry (9.0.3)
- MongoDB.Driver.Core.Extensions.DiagnosticSources (3.0.0)
- MongoDB.Driver.Core.Extensions.OpenTelemetry (1.0.0)

**Swagger:**
- Swashbuckle.AspNetCore (10.1.0)

**Outros:**
- Microsoft.Extensions.Configuration (9.0.4)

---

## 🎯 Exemplo Completo

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "OpenTelemetry": {
    "ServiceName": "tech-challenge-api",
    "ServiceVersion": "1.0.0",
    "Url": "http://localhost:4317",
    "Exporters": ["OTLP", "Console"]
  },
  "Swagger": {
    "Version": "v1",
    "Title": "Tech Challenge - Fast Food API - Fase 3",
    "Description": "API para gerenciamento de pedidos para lanchonete usando conceitos de Clean Architecture.",
    "ContactName": "Grupo 118 - Sabrina Cardoso | Tiago Koch | Tiago Oliveira | Túlio Rezende | Vinícius Nunes",
    "ContactUrl": "https://github.com/Grupo-118-Tech-Challenge-Fiap-11SOAT/tech-challenge-grupo-118-fase-1"
  },
  "AllowedHosts": "*"
}
```

### Program.cs

```csharp
using StandardDependencies.Injection;
using StandardDependencies.Models;

var builder = WebApplication.CreateBuilder(args);

// Leia as configurações do appsettings.json
var swaggerOptions = builder
    .Configuration
    .GetSection(SwaggerOptions.SectionName)
    .Get<SwaggerOptions>();

var openTelemetryOptions = builder
    .Configuration
    .GetSection(OpenTelemetryOptions.SectionName)
    .Get<OpenTelemetryOptions>();

// Configura elementos comuns
builder.ConfigureCommonElements(openTelemetryOptions, swaggerOptions);

// Adiciona seus próprios serviços
builder.Services.AddControllers();

var app = builder.Build();

// Configura o pipeline HTTP
app.UseStandarizedSwagger(swaggerOptions);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

---

## 🐳 Docker Compose para Observabilidade

Para visualizar os dados do OpenTelemetry, você pode usar o docker-compose contigo no repositório [docker-otel-lgtm](https://github.com/grafana/docker-otel-lgtm)

---

## 📖 Recursos Adicionais

- [OpenTelemetry .NET](https://opentelemetry.io/docs/instrumentation/net/)
- [Swagger/OpenAPI](https://swagger.io/specification/)

---

## 👥 Autores

**Grupo 118**
- Sabrina Cardoso
- Tiago Koch
- Tiago Oliveira
- Túlio Rezende
- Vinícius Nunes

---

## 📄 Licença

Este pacote foi desenvolvido para o Hackaton Final - FIAP 11SOAT.

