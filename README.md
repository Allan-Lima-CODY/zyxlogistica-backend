# 🚚 Logistics / Expedition Management API

API para gestão de expedições, motoristas, caminhões e pedidos, seguindo princípios de arquitetura limpa, DDD e boas práticas corporativas de software.

---

## 🏗️ Arquitetura

A aplicação segue os conceitos de **DDD (Domain-Driven Design)** combinados com **Clean Architecture / Hexagonal**, promovendo:

* Independência da infraestrutura (banco, ORM, etc)
* Domínio rico com regras de negócio encapsuladas
* Casos de uso isolados
* Separação clara de responsabilidades

```
/src
 ├── Application
 │   ├── Commands (CQRS)
 │   ├── Queries (CQRS)
 │   ├── DTOs
 │   └── Mappers (AutoMapper)
 ├── Domain
 │   ├── Entities (Aggregate Roots)
 │   ├── Enums
 │   ├── Repositories (Interfaces)
 │   └── Services / Exceptions
 ├── Infrastructure
 │   ├── Persistence (Dapper / EF optional)
 │   └── Repositories (Implementations)
 └── API
     ├── Controllers
     └── Configurations (DI, Middlewares)
```

---

## 🧠 Padrões e Conceitos

### ✅ DDD — Domain-Driven Design

* **Entidades ricas** com regras de negócio (ex: `ChangeTruck`)
* Invariantes de domínio
* Exceptions de domínio (`InvalidOperationException`, `NotFoundException`, etc)
* Regras aplicadas na entidade, não no controller

Exemplo:

```csharp
public void ChangeTruck(Truck newTruck)
{
    Truck.ToMakeAvailable();
    newTruck.Occupy();
    Truck = newTruck;
}
```

---

### ✅ Clean Architecture / Hexagonal

* Domínio **não depende** de infra
* Repositórios são interfaces no domínio e implementações na infra
* Facilita troca do ORM (Dapper/EF)

---

### ✅ CQRS + MediatR

* Separação de comandos e consultas
* Cada caso de uso = um handler

Exemplo: `UpdateExpeditionCommandHandler`

---

### ✅ AutoMapper

Conversão entre entidades e DTOs usando `ConstructUsingExpression` para construtores com parâmetros:

```csharp
CreateMap<Expedition, ExpeditionDTO.CommandDTO>()
    .ConstructUsing(x => new ExpeditionDTO.CommandDTO(
        x.Id,
        x.Order.Id,
        x.Order.OrderNumber,
        ...
    ));
```

---

### ✅ Dapper

Dapper utilizado para consultas performáticas e controle explícito de SQL.
Repositories expõem apenas operações necessárias.

---

## ⚙️ Principais Tecnologias

| Stack              | Tecnologia                      |
| ------------------ | ------------------------------- |
| Linguagem          | C# .NET 8                       |
| API                | ASP.NET Core Web API            |
| Padrão             | Clean Architecture + DDD + CQRS |
| ORM                | Dapper e EF                     |
| Mensageria Interna | MediatR                         |
| Mapeamento         | AutoMapper                      |
| Auth (opcional)    | JWT                             |
| DB                 | SQL Server / PostgreSQL         |

---

## 📦 Casos de Uso Implementados

* Criar / atualizar expedições
* Alterar caminhão da expedição com validação
* Associar motorista
* Atualizar situação da expedição
* Consultas com Dapper

---

## 🧪 Testes

* Testes de domínio para regras de negócio
* Testes de aplicação para casos de uso

---

## 🚀 Execução

```bash
dotnet restore
dotnet build
dotnet run --project src/API
```

---

## 🌐 Rotas Exemplo

```
GET    /api/expedition/{id}
POST   /api/expedition
PUT    /api/expedition/{id}
```

---

## 📁 Exemplo de Entidade

```csharp
public class Expedition : EntityBase
{
    public Order Order { get; private set; }
    public Driver Driver { get; private set; }
    public Truck Truck { get; private set; }

    public void ChangeTruck(Truck newTruck)
    {
        Truck.ToMakeAvailable();
        newTruck.Occupy();
        Truck = newTruck;
    }
}
```

---

## 📌 Diferenciais

* Domínio limpo e coeso
* Regras de negócio internas às entidades
* Separação de leitura/escrita (CQRS)
* Repositórios com Dapper para performance
* Extensível e testável
